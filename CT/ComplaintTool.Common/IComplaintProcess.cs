namespace ComplaintTool.Common
{
    public interface IComplaintProcess
    {
        string OrganizationId { get; }
        string ProcessName { get; }
        string ProcessFilePath { get; }
        string FilePath { get; }
    }
}
