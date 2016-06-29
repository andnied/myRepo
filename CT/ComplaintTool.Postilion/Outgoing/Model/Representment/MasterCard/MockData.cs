using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ComplaintTool.Postilion.Outgoing.Model.Representment.MasterCard
{
    public class MockData
    {
        //219AdjustmentComponent3766
        public string AdjustmentComponentHeader
        {
            get
            {
                if (AdjustmentComponent != null)
                {
                    var adjustmentComponentToXml = Utils.RegexReplace(AdjustmentComponentToXml()); 
                    return @"219AdjustmentComponent" +
                           adjustmentComponentToXml.Length.ToString(CultureInfo.InvariantCulture).Length.ToString(CultureInfo.InvariantCulture) +
                           adjustmentComponentToXml.Length.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new Exception("Object AdjustmentComponent is null.");
                }
            }
         }
        //AdjustmentComponent
        public AdjustmentComponent AdjustmentComponent { get; set; }
        //13Ipm3204
        public string IpmHeader
        {
            get
            {
                if (Ipm != null)
                {
                    var ipmToXml = Utils.RegexReplace(IpmToXml());
                    return @"13Ipm" +
                           ipmToXml.Length.ToString(CultureInfo.InvariantCulture).Length.ToString(CultureInfo.InvariantCulture) +
                           ipmToXml.Length.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new Exception("Object Ipm is null.");
                }
            }
        }
        //Ipm
        public Ipm Ipm { get; set; }
        //16CaseID220ESPLCB20150327400002
        private string _caseID;
        public string CaseID {
            get { return @"16CaseID220" + _caseID; }
            set { _caseID = value; } }

        public byte[] ToArray()
        {
            string toValue = ToString();
            var utf8Encoding = new UTF8Encoding(false);
            byte[] value = utf8Encoding.GetBytes(toValue.ToCharArray());
            return value;
        }

        public override string ToString()
        {
            return AdjustmentComponentHeader
                + Utils.RegexReplace(AdjustmentComponentToXml()) 
                + IpmHeader
                + Utils.RegexReplace(IpmToXml()) 
                + CaseID;
        }

        public string ToBase64String()
        {
            return Convert.ToBase64String(this.ToArray(), Base64FormattingOptions.None);
        }

        private string AdjustmentComponentToXml()
        {
            if (AdjustmentComponent != null)
            {
                string serializeXml = string.Empty;
                if (Serialize<AdjustmentComponent>(AdjustmentComponent, ref serializeXml))
                {
                    return serializeXml;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        private string IpmToXml()
        {
            string serializeXml = string.Empty;
            if (Serialize<Ipm>(Ipm, ref serializeXml))
            {
                return serializeXml;
            }
            else
            {
                return null;
            } 
        }

        public static bool Serialize<T>(T value, ref string serializeXml)
        {
            if (value == null)
            {
                return false;
            }
            try
            {
                XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
                MemoryStream memoryStream = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(memoryStream, new UTF8Encoding(false));
                var xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add("","");
                xmlserializer.Serialize(writer, value, xmlSerializerNamespaces);
                memoryStream.Position = 0;
                var streamReader = new StreamReader(memoryStream);
                serializeXml = streamReader.ReadToEnd();
                writer.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }



}
