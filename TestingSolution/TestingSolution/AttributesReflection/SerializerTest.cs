using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestingSolution.AttributesReflection
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataMember]
        public int Age { get; set; }
    }

    public class MySerializer
    {
        private Type _dataType;

        public MySerializer(Type dataType)
        {
            _dataType = dataType;
            if (!(_dataType.IsDefined(typeof(DataContractAttribute), false)))
                throw new Exception("");
        }

        public void WriteObject(Stream stream, object graph)
        {
            if (!stream.CanWrite)
                throw new Exception("");

            if (graph.GetType() != _dataType)
                throw new Exception("");

            var props = graph.GetType().GetProperties().Where(p => p.IsDefined(typeof(DataMemberAttribute), false)).ToList();
            if (!props.Any())
                return;

            var writer = new StreamWriter(stream);
            writer.WriteLine("<" + _dataType.Name + ">");
            props.ForEach(p => writer.WriteLine("\t<" + p.Name + ">" + p.GetValue(graph) + "</" + p.Name + ">"));
            writer.WriteLine("</" + _dataType.Name + ">");
            writer.Flush();
        }
    }

    public class SerializerTest
    {
        public static void TestSerializer()
        {
            var person = new Person
            {
                FirstName = "FristName",
                LastName = "LastName",
                Age = 25
            };

            //var serializer = new DataContractSerializer(person.GetType());
            var serializer = new MySerializer(person.GetType());

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, person);
                stream.Seek(0, SeekOrigin.Begin);
                Console.WriteLine(XElement.Parse(Encoding.ASCII.GetString(stream.GetBuffer()).Replace("\0", "")));
            }
        }
    }
}
