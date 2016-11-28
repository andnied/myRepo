using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Common.Structures
{
    public class ApiCollection<T> : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }

        public ApiCollection(IEnumerable<T> src)
        {
            Items = src;
        }
    }
}
