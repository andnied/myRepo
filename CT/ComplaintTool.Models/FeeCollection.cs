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
    
    public partial class FeeCollection
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FeeCollection()
        {
            this.FeeCollectionExtracts = new HashSet<FeeCollectionExtract>();
        }
    
        public long FeeCollectionId { get; set; }
        public string CaseId { get; set; }
        public string StageCode { get; set; }
        public bool IsAutomatic { get; set; }
        public long ValueId { get; set; }
        public long RecordsId { get; set; }
        public long StageId { get; set; }
        public bool DocumentationIndicator { get; set; }
        public Nullable<bool> IsReversal { get; set; }
        public int Status { get; set; }
        public System.DateTimeOffset InsertDate { get; set; }
        public string InsertUser { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeeCollectionExtract> FeeCollectionExtracts { get; set; }
    }
}