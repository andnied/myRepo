using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Processing.AutoRepresentmentValidation.Validating
{
    public class ValidatingItem
    {
        public class VItem
        {
            public string Name { get; set; }
            public string Value { get; set; }

            public VItem(string name, string value)
            {
                this.Name = name;
                this.Value = value;
            }
        }

        public VItem RecordItem { get; set; }
        public VItem ComparedItem { get; set; }

        public ValidatingItem(VItem recordItem, VItem comparedItem)
        {
            RecordItem = recordItem;
            ComparedItem = comparedItem;
        }
    }
}
