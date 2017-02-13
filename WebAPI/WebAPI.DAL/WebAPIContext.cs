namespace WebAPI.DAL
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
        }

        public virtual DbSet<Value> Values { get; set; }
        public virtual DbSet<Child> Children { get; set; }
    }
}