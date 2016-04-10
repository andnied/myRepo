using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSolution
{
    public class MeAttribute : Attribute
    {
        public MeAttribute(int val)
        {
            Console.WriteLine(val);
        }

        public int IntProperty { get; set; }
        public string StringProperty { get; set; }
    }

    // constructor here, optionally can set other props
    [Me(20, IntProperty = 10)]
    class Program
    {
        static void Main(string[] args)
        {
            // need to invoke this so the attribute is actually triggered
            typeof(Program).GetCustomAttributes(false);
        }
    }
}
