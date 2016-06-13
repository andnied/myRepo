using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ftp.Client
{
    public class FtpClient
    {
        private readonly string _host;
        private readonly string _userName;
        private readonly string _password;
        private readonly bool _isAnonymous = false;
        
        public FtpClient(string host)
        {
            _host = host;
            _isAnonymous = true;
        }

        public FtpClient(string host, string userName, string password)
        {
            _host = host;
            _userName = userName;
            _password = password;
        }

        public ArrayList GetDirectories()
        {
            ArrayList dirs = new ArrayList();
            var request = (FtpWebRequest)WebRequest.Create(_host.Replace(_host, "ftp://" + _host));
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = _isAnonymous ? CredentialCache.DefaultCredentials : new NetworkCredential(_userName, _password);
            request.KeepAlive = false;

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string dir;
                        while ((dir = reader.ReadLine()) != null)
                        {
                            dirs.Add(dir);
                        }
                    }
                }
            }

            return dirs;
        }
    }
}
