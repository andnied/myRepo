using System;

namespace ComplaintTool.Common.Extensions
{
    public static class GuidExtensions
    {
        public static bool IsEmpty(this Guid value)
        {
            return value == null || value == Guid.Empty;
        }
    }
}
