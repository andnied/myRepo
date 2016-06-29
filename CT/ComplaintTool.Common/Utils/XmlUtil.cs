using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ComplaintTool.Common.Utils
{
    public static class XmlUtil
    {
        public static string Serialize<T>(T obj)
        {
            using (var writer = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        public static string SerializeToFile<T>(T obj, string fileName)
        {
            using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj);
            }
            return File.ReadAllText(fileName, Encoding.UTF8);
        }

        public static T Deserialize<T>(string xmlContent, bool throwExceptions = true)
        {
            using (var reader = new StringReader(xmlContent))
            {
                var serializer = new XmlSerializer(typeof(T));
                if (throwExceptions) serializer.SetExceptions();
                return (T)serializer.Deserialize(reader);
            }
        }

        public static T DeserializeFromFile<T>(string fileName, bool throwExceptions = true)
        {
            using (var reader = new StreamReader(fileName, Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(T));
                if (throwExceptions) serializer.SetExceptions();
                return (T)serializer.Deserialize(reader);
            }
        }

        private static void SetExceptions(this XmlSerializer serializer)
        {
            serializer.UnknownAttribute += (sender, args) => { throw new Exception("UnknownAttribute: " + args.ExpectedAttributes); };
            serializer.UnknownElement += (sender, args) => { throw new Exception("UnknownElement: " + args.ExpectedElements); };
            serializer.UnknownNode += (sender, args) => { throw new Exception("UnknownNode: " + args.Name); };
            serializer.UnreferencedObject += (sender, args) => { throw new Exception("UnreferencedObject"); };
        }
    }
}
