namespace ComplaintTool.Accounting.Currency
{
    public class PKOExchange
    {
        public string Country { get; set; }
        public string ISO { get; set; }
        public string NumericCode { get; set; }
        public int Conversion { get; set; }
        public decimal Purchase { get; set; }
        public decimal Sale { get; set; }
        public decimal Spread { get; set; }
    }
}
