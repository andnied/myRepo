using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Common.Structures
{
    public class ApiCollection<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }

        public ApiCollection()
        { }

        public ApiCollection(IEnumerable<T> src)
        {
            Items = src;
        }
    }

    public class ApiCollection
    {
        public IEnumerable Items { get; set; }
        public int TotalCount { get; set; }

        public ApiCollection()
        { }

        public ApiCollection(IEnumerable src)
        {
            Items = src;
        }
    }
}
