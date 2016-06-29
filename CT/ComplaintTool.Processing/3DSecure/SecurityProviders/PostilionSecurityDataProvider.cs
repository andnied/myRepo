using ComplaintTool.Common.Config;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Processing.Postilion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Processing._3DSecure.SecurityProviders
{
    public class PostilionSecurityDataProvider:ISecurityDataProvider
    {
        private bool _secure3d;
        private PostilionData _data=null;

        private const string CorrectValueObject = "Y";

        public bool Secure3d{ get{ return _secure3d; } }

        private readonly PostilionRepo _repo;

        public PostilionSecurityDataProvider(PostilionRepo repo)
        {
            _repo = repo;
        }

        public void LoadData(Complaint complaint)
        {
            var dataProvider=new PostilionDataProvider(_repo);
            _data=dataProvider.GetData(complaint);
            _secure3d = Mc3dSecureProvider.Get3DSecure(_data.PrevUcafData, _data.PrevMessageType, CorrectValueObject);
        }

        public ComplaintRecord Fill(ComplaintRecord complaintRecord)
        {
            complaintRecord.StructuredDataReq = _data.StructuredDataReq;
            complaintRecord.DatetimeRsp = _data.DatetimeRsp;
            complaintRecord.MessageType = _data.MessageType;
            complaintRecord.PrevMessageType = _data.PrevMessageType;
            complaintRecord.PrevUcafData = _data.PrevUcafData;
            return complaintRecord;
        }
    }
}
