using ComplaintTool.Common;
using System;

namespace ComplaintTool.Postilion.Incoming
{
    public abstract class PostilionIncomingBase : IComplaintProcess
    {
        #region IComplaintProcess

        public string OrganizationId
        {
            get { throw new NotImplementedException(); }
        }

        public string ProcessName
        {
            get
            {
                return Globals.PostilionIncomingService;
            }
        }

        #region Abstract

        public abstract string ProcessFilePath
        {
            get;
        }

        public abstract string FilePath
        {
            get;
        }

        #endregion

        #endregion

        public static PostilionIncomingBase GetService(string type, string filePath = null, long fileId = 0)
        {
            switch (type)
            {
                case "response":
                    return new PostilionResponseFiles(filePath);
                case "processing":
                    return new PostilionProcessingFiles(fileId);
            }

            return null;
        }

        public abstract long Process();
    }
}
