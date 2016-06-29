namespace ComplaintTool.Postilion.Outgoing.Model
{
    public class ValidatingItem
    {
        public class VItem
        {
            public string Name { get; set; }
            public string Value { get; set; }

            public VItem(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }

        public VItem RecordItem { get; set; }
        public VItem ComparedItem { get; set; }

        public ValidatingItem(VItem recordItem, VItem comparedItem)
        {
            RecordItem = recordItem;
            ComparedItem = comparedItem;
        }
    }
}
