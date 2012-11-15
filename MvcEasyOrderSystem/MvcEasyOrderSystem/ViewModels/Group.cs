using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.ViewModels
{
    public class Group<K,T>
    {
        public K Key { get; set; }
        public T Value { get; set; }
        public T Id { get; set; }
        public IEnumerable<T>Values { get; set; }
    }
}