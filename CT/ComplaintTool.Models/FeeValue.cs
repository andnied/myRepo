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
    
    public partial class FeeValue
    {
        public int FeeValueId { get; set; }
        public string OrganizationId { get; set; }
        public int PickupTypeId { get; set; }
        public bool PrizeApproved { get; set; }
        public decimal PrizeValue { get; set; }
        public string PrizeValueCurrencyCode { get; set; }
        public decimal FeeValue1 { get; set; }
        public string FeeValueCurrencyCode { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public System.DateTimeOffset InsertDate { get; set; }
        public string InsertUser { get; set; }
    }
}
