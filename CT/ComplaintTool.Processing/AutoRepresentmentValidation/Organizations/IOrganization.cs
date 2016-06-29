using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common.Enum;
using ComplaintTool.Models;
using Model=ComplaintTool.Processing.AutoRepresentmentValidation.Representments;


namespace ComplaintTool.Processing.AutoRepresentmentValidation.Organizations
{
    public interface IOrganization
    {
        Model.Representment CheckRules(Complaint complaint);
    }
}
