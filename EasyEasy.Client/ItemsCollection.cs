using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace EasyEasy.Client
{
    public class ItemsCollection<T> : IEnumerable<T>
    {
        private IEnumerable<T> _internalCollection;

        public int TotalCount { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            return _internalCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internalCollection.GetEnumerator();
        }

        public ItemsCollection(IEnumerable<T> items, int totalCount)
        {
            _internalCollection = items;
            TotalCount = totalCount;
        }
    }
}
