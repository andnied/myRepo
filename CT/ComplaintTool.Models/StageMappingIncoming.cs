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
    
    public partial class StageMappingIncoming
    {
        public int StageMappingId { get; set; }
        public Nullable<int> StageId { get; set; }
        public string StageCode { get; set; }
        public string Description { get; set; }
        public string PTCode { get; set; }
        public string MC_MTI { get; set; }
        public string MC_FunctionCode { get; set; }
        public string MC_MessageReversalIndicator { get; set; }
        public string VISA_TransactionCode { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTimeOffset> InsertDate { get; set; }
        public string InsertUser { get; set; }
    }
}
