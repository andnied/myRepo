//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComplaintTool.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CountryCode
    {
        public int CountryCodeId { get; set; }
        public string OrganizationId { get; set; }
        public string CountryNameENG { get; set; }
        public string CountryNamePL { get; set; }
        public string CountryCode_ISO_2AN { get; set; }
        public string CountryCode_ISO_3AN { get; set; }
        public string CountryCode_ISO_3N { get; set; }
        public string BIN { get; set; }
        public string ICA { get; set; }
        public Nullable<bool> RcUsage { get; set; }
        public Nullable<bool> GflUsage { get; set; }
        public System.DateTimeOffset InsertDate { get; set; }
        public string InsertUser { get; set; }
    }
}
