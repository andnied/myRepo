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
    
    public partial class ComplaintAutomaticEvent
    {
        public long AutomaticEventsId { get; set; }
        public string AutomaticProcess { get; set; }
        public System.Guid AutomaticKey { get; set; }
        public string CaseId { get; set; }
        public long ValueId { get; set; }
        public long RecordsId { get; set; }
        public long StageId { get; set; }
        public Nullable<long> ProcessNumericValue { get; set; }
        public string ProcessStringValue { get; set; }
        public System.DateTimeOffset InsertDate { get; set; }
        public string InsertUser { get; set; }
    
        public virtual Complaint Complaint { get; set; }
    }
}
