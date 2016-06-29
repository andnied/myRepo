using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;

namespace ComplaintTool.Processing.Postilion
{
    public class PostilionDataProvider
    {
        private readonly PostilionRepo _repo;

        public PostilionDataProvider(PostilionRepo repo)
        {
            _repo = repo;
        }

        public PostilionData GetData(Complaint complaint)
        {

            var panFirst6 = complaint.PANMask.Substring(0, 6);
            var panLast4 = complaint.PANMask.Substring(12, 4);
            var data = _repo.GetPostilionData(complaint.ARN, panFirst6, panLast4);

            if (data == null)
                throw new Exception(string.Format("Cannot find Postilion data for case {0} with ARN {1}", complaint.CaseId, complaint.ARN));

            return data;
        }
    }
}
