using System;
using System.Diagnostics;
using System.IO;

namespace ComplaintTool.Shell.Tests
{
    static class TestHelper
    {
        public const string TestDatabaseServer = "W-CHARGEBACK";
        public const string TestDatabaseName = "Complaint";
        public const bool TestDatabaseIntegratedSecurity = true;

        public const string Tracy = "Tracy";
        public const string Radiant = "Radiant";

        public static readonly string GlobalFolder = GetPath(@"Tests\ComplaintTool\");
        public static readonly string InFolder = GetPath(@"Tests\ComplaintTool\_In");
        public static readonly string OutFolder =  GetPath(@"Tests\ComplaintTool\_Out");

        private static string GetPath(string path)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), path);
        }

        public static string GetUniqueDestinationFolder()
        {
            return Path.Combine(GlobalFolder, DateTime.Now.ToFileTime().ToString());
        }

        public static string GetClassDestinationFolder()
        {
            return Path.Combine(GlobalFolder, new StackFrame(1).GetMethod().DeclaringType.Name);
        }

        public static string GetMethodDestinationFolder()
        {
            return Path.Combine(GlobalFolder, new StackFrame(1).GetMethod().Name);
        }

        public static string GetConnectionString()
        {
            return ComplaintTool.Common.Config.ComplaintConfig.Instance.GetEntityConnectionString();
        }
    }
}
