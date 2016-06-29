using ComplaintTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Processing._3DSecure.SecurityProviders
{
    public interface I3dSecureProvider
    {
        bool? Get3dSecure(Complaint complaint, string correctValueObject);
    }
}
