# EasyEasy.Client

## How to use

### Install with Nuget
```
Install-Package EasyEasy.Client
```

### Initialize

```csharp YOUR_APP_KEY
var client = new Client("YOUR_APP_KEY");
```

### Examples

```csharp

class Cat
{
    // Id property is required
	public string Id { get; set; }

	public string Name { get; set; }

	public double Age { get; set; }

	public IEnumerable<string> Interests { get; set; }
}

/////////////////

var cat = new Cat()
	{
		Name = "Sam",
		Age = 1.5,
		Interests = new string[] { "play", "eat" }
	};

// add new object
var id = await client.AddAsync(cat);

// get object by id
cat = await client.GetOne<Cat>(id);

// update
cat.Age = 1.7;
await client.UpdateAsync(cat);

// get filtered array of objects
var cats = await client.GetAsync<Cat>(new { }); // return all cats
cats = await client.GetAsync<Cat>(new { age=1.5 }); // 1.5 years old cats
cats = await client.GetAsync<Cat>(new { age_gt=1.0 }); // cats older than 1 year
cats = await client.GetAsync<Cat>(new { name_like="Sa*" }); // wildcard

// paging
cats = await client.GetAsync<Cat>(new { _start = 10, _count = 10 });

//Learn more about filtering operators at: http://easyeasy.io/docs#/operators

//delete
await client.DeleteAsync<Cat>(cat.Id)
```

# Licence
ISC
