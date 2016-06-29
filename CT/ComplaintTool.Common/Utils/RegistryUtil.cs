using System;
using System.Linq;
using ComplaintTool.Common.Extensions;
using Microsoft.Win32;

namespace ComplaintTool.Common.Utils
{
    public static class RegistryUtil
    {
        public static void SetObject<T>(T obj, string name)
        {
            Guard.ThrowIf<ArgumentNullException>(obj == null, "registry object");
            Guard.ThrowIf<ArgumentNullException>(name.IsEmpty(), "registry object name");

            var registryKey = GetOrCreateComplaintRegistryKey(name);
            foreach (var prop in obj.GetType().GetProperties().Where(x => x.CanRead))
            {
                registryKey.SetValue(prop.Name, prop.GetValue(obj) ?? "");
            }
        }

        public static T GetObject<T>(string name) where T : new()
        {
            Guard.ThrowIf<ArgumentNullException>(name.IsEmpty(), "registry object name");

            var obj = new T();
            var registryKey = GetOrCreateComplaintRegistryKey(name);
            foreach (var prop in obj.GetType().GetProperties().Where(x => x.CanWrite))
            {
                var value = registryKey.GetValue(prop.Name);
                if (value != null)
                {
                    prop.SetValue(obj, System.Convert.ChangeType(value, prop.PropertyType));
                }
            }
            return obj;
        }

        private static RegistryKey GetOrCreateComplaintRegistryKey(string name)
        {
            RegistryKey localMachine = null;
            if (Environment.Is64BitOperatingSystem)
                localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
            else
                localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);

            var softwareKey = localMachine.OpenSubKey("Software", true);

            var eserviceKey = softwareKey.OpenSubKey("eService", true);
            if (eserviceKey == null)
                eserviceKey = softwareKey.CreateSubKey("eService");

            var ctKey = eserviceKey.OpenSubKey("ComplaintTool", true);
            if (ctKey == null)
                ctKey = eserviceKey.CreateSubKey("ComplaintTool");

            var key = ctKey.OpenSubKey(name, true);
            if (key == null)
                key = ctKey.CreateSubKey(name);

            return key;
        }
    }
}
