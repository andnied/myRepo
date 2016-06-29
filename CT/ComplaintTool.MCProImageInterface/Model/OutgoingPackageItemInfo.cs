using System;
using System.Collections.Generic;
using ComplaintTool.Models;

namespace ComplaintTool.MCProImageInterface.Model
{
    public class OutgoingPackageItemInfo
    {
        public string DirectoryPath { get; set; }
        public string DirectoryName { get; set; }
        public string EndpointName { get; set; }
        public List<OutgoingPackageItem> Items { get; set; }
        public Guid ProcessKey { get; set; }
    }
}
