using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Common.SearchParams
{
    public class BaseSearchParams
    {
        private string _sort;
        private int? _page;
        private int? _items;

        public string Sort
        {
            get { return _sort ?? "Id"; }
            set { _sort = value; }
        }

        public int? Page
        {
            get { return _page ?? 1; }
            set { _page = value; }
        }

        public int? Items
        {
            get { return _items ?? 50; }
            set { _items = value; }
        }

        public string Fields { get; set; }
    }
}
