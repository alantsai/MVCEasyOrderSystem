using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.ViewModels
{
    /// <summary>
    /// 主要用來做Linq Group By的時候想傳入View的時候用到的ViewModel
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class Group<K,T>
    {
        public K Key { get; set; }
        public T Value { get; set; }
        public T Id { get; set; }
        public IEnumerable<T>Values { get; set; }
    }
}
