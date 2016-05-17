using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.Utils
{
    public class VisaRecordAttribute : Attribute
    {
        public int Position { get; set; }
    }

    public class ConfigurationViewAttribute : Attribute
    {
        public ConfigurationViewAttribute(string name)
        {
            Name = name;
        }

        public ConfigurationViewAttribute(string name, Type type)
        {
            Name = name;
            PropertyType = type;
        }

        public string Name { get; set; }
        public Type PropertyType { get; set; }
    }
}
