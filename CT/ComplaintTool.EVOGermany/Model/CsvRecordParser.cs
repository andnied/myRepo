using ComplaintTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.EVOGermany.Model
{
    public class CsvRecordParser:IRecordParser
    {
        private LinkedList<string> _exportedColumns = new LinkedList<string>();

        public LinkedList<string> ExportedColumns
        {
            get
            {
                return _exportedColumns;
            }
            set
            {
                _exportedColumns = value;
            }
        }

        public string GetLine<T>(T record)
        {
            StringBuilder sb = new StringBuilder();
            var type = typeof(T);

            foreach(var columnName in _exportedColumns)
            {
                var propInfo = type.GetProperty(columnName);

                object data = propInfo.GetValue(record);

                if (propInfo.PropertyType == typeof(DateTime?))
                {
                    var objectDate = (DateTime?)data;
                    sb.Append(objectDate.HasValue ? objectDate.Value.ToString("dd.MM.yyyy") + "|" : "|");
                    continue;
                }

                var objectString = data!=null?data.ToString():string.Empty;
                objectString = objectString.Replace("\r",string.Empty).Replace("\n",string.Empty);
                sb.Append(String.IsNullOrWhiteSpace(objectString) ? "|" : objectString + "|");
            }

            var result = sb.ToString();
            return result.Length>0?result.Substring(0,result.Length-1):string.Empty;
        }
    }
}
