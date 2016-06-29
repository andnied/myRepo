using ComplaintTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Processing._3DSecure.SecurityProviders
{
    public interface ISecurityDataProvider
    {
        bool Secure3d { get; }
        void LoadData(Complaint complaint);
        ComplaintRecord Fill(ComplaintRecord complaintRecord);
    }
}
