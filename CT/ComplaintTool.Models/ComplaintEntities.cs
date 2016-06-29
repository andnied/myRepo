namespace ComplaintTool.Models
{
    public partial class ComplaintEntities
    {
        public ComplaintEntities(string cs)
            : base(cs)
        {
            Database.CommandTimeout = Database.Connection.ConnectionTimeout;
            Configure();
        }

        private void Configure()
        {
            // wylaczone niepotrzebne rzeczy ze wzgledow wydajnosciowych
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }
    }
}
