using System;
using System.Collections.Generic;
using System.Text;

namespace EasyEasy.Client
{
    class CollectionResponse<T> where T: class
    {
        public IEnumerable<T> items { get; set; }

        public int total { get; set; }
    }
}
