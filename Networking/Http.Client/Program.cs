using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Http.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string responseText = null;
            var request = WebRequest.Create(@"http://www.fizyka.umk.pl/~bigman/tmp/wersja.txt");
            request.Credentials = CredentialCache.DefaultCredentials;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
            }
        }
    }
}
