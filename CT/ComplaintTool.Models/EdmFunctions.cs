using System;
using System.Data.Entity;

namespace ComplaintTool.Models
{
    public static class EdmFunctions
    {
        [DbFunction("ComplaintTool.DataAccess.Model", "ConvertToInt32")]
        public static int ConvertToInt32(string value)
        {
            throw new InvalidOperationException("Only valid when used as part of a LINQ2Entities query.");
        }
        [DbFunction("ComplaintTool.DataAccess.Model", "ConvertToDecimal")]
        public static decimal ConvertToDecimal(string value)
        {
            throw new InvalidOperationException("Only valid when used as part of a LINQ2Entities query.");
        }
    }
}
