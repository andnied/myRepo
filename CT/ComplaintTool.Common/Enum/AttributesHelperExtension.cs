using System.ComponentModel;

namespace ComplaintTool.Common.Enum
{
    /// <summary>
    /// Attrubutes Helper Extension static class
    /// </summary>
    public static class AttributesHelperExtension
    {
        /// <summary>
        /// To the description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToDescription(this System.Enum value)
        {
            var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da.Length > 0 ? da[0].Description : value.ToString();
        }
    }
}
