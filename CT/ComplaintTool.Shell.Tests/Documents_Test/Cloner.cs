using ComplaintTool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Shell.Tests.Documents_Test
{
    public static class Cloner
    {
        public static T Clone<T>(this T source)
        {
            RemoveReferences(source);
            var obj = new DataContractSerializer(typeof(T));
            using (var stream = new System.IO.MemoryStream())
            {
                obj.WriteObject(stream, source);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                return (T)obj.ReadObject(stream);
            }
        }

        private static void RemoveReferences<T>(T source)
        {
            if (source is ComplaintStage)
                (source as ComplaintStage).Complaint = null;
        }
    }
}
