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
    
    public partial class ExchangeFixedVISA
    {
        public long ExchangeFixedId { get; set; }
        public string CurrencyCode1 { get; set; }
        public string CurrencyCode2 { get; set; }
        public decimal Exchange1To2 { get; set; }
        public decimal Exchange2To1 { get; set; }
        public decimal Settlement1To2 { get; set; }
        public decimal Settlement2To1 { get; set; }
        public Nullable<decimal> Buy1to2 { get; set; }
        public Nullable<decimal> Sell2To1 { get; set; }
        public System.DateTime ExchangeDate { get; set; }
        public System.DateTime InsertDate { get; set; }
        public string InsertUser { get; set; }
    }
}
