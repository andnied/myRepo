using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSolution.AttributesReflection
{
    [AttributeUsage(AttributeTargets.Class)] // only classes can be decorated with this attribute
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
    public class Base { }
    public class Derived : Base { }

    public class AttributesTest
    {
        public static void TestAttributes()
        {
            // need to invoke this so the attribute is actually triggered
            typeof(Derived).GetCustomAttributes(true); // true = gets all attributes from the base class
            var isDefined = typeof(Derived).IsDefined(typeof(MeAttribute), true);
            Console.WriteLine("Is Defined: " + isDefined);
        }
    }
}
