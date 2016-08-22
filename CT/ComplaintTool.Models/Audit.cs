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
    
    public partial class Audit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Audit()
        {
            this.FilesStageNotifications = new HashSet<FilesStageNotification>();
        }
    
        public long AuditId { get; set; }
        public string CaseId { get; set; }
        public Nullable<long> StageId { get; set; }
        public Nullable<System.Guid> stream_id { get; set; }
        public Nullable<System.DateTime> IncomingDate { get; set; }
        public Nullable<System.Guid> ProcessKey { get; set; }
        public string Description { get; set; }
        public Nullable<int> Status { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public System.DateTimeOffset InsertDate { get; set; }
        public string InsertUser { get; set; }
    
        public virtual Complaint Complaint { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FilesStageNotification> FilesStageNotifications { get; set; }
    }
}