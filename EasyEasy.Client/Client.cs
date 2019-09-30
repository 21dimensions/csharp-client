using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Serialization;
using System.Globalization;

namespace EasyEasy.Client
{
    public class Client
    {
        private string _rootUrl;
        private string _key;
        private HttpClient _http;

        private string GetUrl(string entityName) => _rootUrl + "/" + entityName;

        private string GetObjectId(object obj)
        {
            var idProp = obj.GetType().GetProperties().FirstOrDefault(p => p.Name == "id" || p.Name == "Id");

            if (idProp == null)
                throw new Exception("object shoud have \"id\" or \"Id\" property");

            var id = idProp.GetValue(obj)?.ToString();

            if (id == null || string.IsNullOrEmpty(id))
                throw new Exception("id property value shoud be not null or empty string");

            return id;
        }

        private readonly JsonSerializerSettings _serializationSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public Client(string rootUrl, string key)
        {
            _rootUrl = rootUrl + "/rest";
            _key = key;
            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("Authorization", "Key " + _key);
            _http.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
        }

        public Client(string key) : this("http://easyeasy.io/v1", key)
        {

        }

        public async Task<string> AddAsync<T>(T obj, string entityName)
        {
            var response = await _http.PostAsync(GetUrl(entityName), GetContent(obj));
            response.EnsureSuccessStatusCode();

            var resultStr = await response.Content.ReadAsStringAsync();
            dynamic resultObj = JsonConvert.DeserializeObject<dynamic>(resultStr, _serializationSetting);

            return resultObj.id;
        }

        public async Task<string> AddAsync<T>(T obj) => await AddAsync(obj, typeof(T).Name.ToLower());

        public async Task UpdateAsync<T>(T obj, string entityName)
        {
            var response = await _http.PutAsync(GetUrl(entityName) + "/" + GetObjectId(obj), GetContent(obj));
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync<T>(T obj) => await UpdateAsync(obj, typeof(T).Name.ToLower());

        public async Task<ItemsCollection<T>> GetAsync<T>(string entityName, object query) where T:class
        {
            var filteringStr = String.Join("&",
                query.GetType().GetProperties().Select(p => char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1) + "=" + ConvertToString(p.GetValue(query)).ToString()));

            var responseStr = await _http.GetStringAsync(GetUrl(entityName) + "?" + filteringStr);

            var collectionResponse = JsonConvert.DeserializeObject<CollectionResponse<T>>(responseStr, _serializationSetting);

            return new ItemsCollection<T>(collectionResponse.items, collectionResponse.total);
        }

        public async Task<ItemsCollection<T>> GetAsync<T>(object query) where T:class
            => await GetAsync<T>(typeof(T).Name.ToLower(), query);

        public async Task<T> GetOneAsync<T>(string entityName, string id)
        {
            var responseStr = await _http.GetStringAsync(GetUrl(entityName) + "/" + id);

            return JsonConvert.DeserializeObject<T>(responseStr, _serializationSetting);
        }

        public async Task<T> GetOneAsync<T>(string id) => await GetOneAsync<T>(typeof(T).Name.ToLower(), id);

        public async Task DeleteAsync(string entityName, string id)
        {
            var response = await _http.DeleteAsync(GetUrl(entityName) + "/" + id);
        }

        public async Task DeleteAsync<T>(string id) where T : class
            => await DeleteAsync(typeof(T).Name.ToLower(), id);

        private HttpContent GetContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj, _serializationSetting), Encoding.UTF8, "application/json");
        }

        private string ConvertToString(object obj)
        {
            if (obj is IFormattable)
                return (obj as IFormattable).ToString(null, CultureInfo.InvariantCulture);
            else
                return obj.ToString();
        }
    }
}
