using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestingSolution.AttributesReflection
{
    public class ReflectCollections
    {
        public static void TestReflectingCollections()
        {
            //var mscorlib = Assembly.Load("mscorlib");
            //var collections = mscorlib.GetTypes()
            //    .Where(t => t.Namespace != null && t.Namespace.Contains("Collections"))
            //    .Select(t => t.Name);
            //var interfaces = mscorlib.GetTypes()
            //    .Where(t => t.GetInterfaces().Count() > 0 && t.GetInterfaces()
            //        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>))))
            //    .Select(t => t.Name);

            var an = new AssemblyName("ComplaintTool.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1bbab36f94aeb06e, processorArchitecture = AMD64");
            var complaintToolCommon = Assembly.Load(an);

            var complaintConfigInstance = complaintToolCommon.GetType("ComplaintTool.Common.Config.ComplaintConfig").GetProperty("Instance").GetValue(null);
            var complaintConfig = complaintToolCommon.GetType("ComplaintTool.Common.Config.ComplaintConfig").GetProperty("Conf").GetValue(complaintConfigInstance);
            //var registryConfig = complaintToolCommon.GetType("ComplaintTool.Common.Config.RegistryConfig");
            //var conf = Activator.CreateInstance(registryConfig);
            //registryConfig.GetProperty("ServerName").SetValue(conf, "server name");
            //registryConfig.GetProperty("DatabaseName").SetValue(conf, "db name");
            //registryConfig.GetProperty("IntegratedSecurity").SetValue(conf, false);

            //if (true)
            //{
            //    registryConfig.GetProperty("UserID").SetValue(conf, "user name");
            //    var encryption = complaintToolCommon.GetType("ComplaintTool.Common.Utils.Encryption");
            //    var pass = encryption.GetMethod("Encrypt", BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).Invoke(encryption, new object[] { "abc" });
            //    registryConfig.GetProperty("Password").SetValue(conf, pass);
            //}

            //var complaintConfig = complaintToolCommon.GetType("ComplaintTool.Common.Config.ComplaintConfig");
            //complaintConfig.GetMethod("SetConfig", BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).Invoke(complaintConfig, new object[] { conf, Type.Missing });
        }
    }
}
