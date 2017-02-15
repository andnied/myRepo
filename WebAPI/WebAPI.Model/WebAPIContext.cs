namespace WebAPI.Model
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class WebAPIContext : DbContext
    {
        public WebAPIContext()
            : base("name=WebAPIContext")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public virtual DbSet<Value> Values { get; set; }
        public virtual DbSet<Child> Children { get; set; }
    }
}