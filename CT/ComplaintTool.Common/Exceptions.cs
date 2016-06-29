using System;

namespace ComplaintTool.Common
{
    [Serializable]
    public class ComplaintException : Exception
    {
        public ComplaintException() { }
        public ComplaintException(string message) : base(message) { }
        public ComplaintException(string message, Exception inner) : base(message, inner) { }
        protected ComplaintException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class ComplaintInvalidOperationException : ComplaintException
    {
        public ComplaintInvalidOperationException() { }
        public ComplaintInvalidOperationException(string message) : base(message) { }
        public ComplaintInvalidOperationException(string message, Exception inner) : base(message, inner) { }
        protected ComplaintInvalidOperationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class DirectoryNotExistException : Exception
    {
        public string Path { get; set; }

        public DirectoryNotExistException(string path) { Path = path; }
        public DirectoryNotExistException(string path, string message) : base(message) { Path = path; }
        public DirectoryNotExistException(string path, string message, Exception inner) : base(message, inner) { Path = path; }

        protected DirectoryNotExistException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class ComplaintConfigLoadingException : ComplaintException
    {
        public ComplaintConfigLoadingException(string configParam, Exception inner) 
            : base(string.Format("Error during loading parameters: {0}", configParam), inner) { }
    }
}
