namespace BazaMvp
{
    using BazaMvp.Model;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public class IFModel : DbContext
    {
        public IFModel()
            : base("name=IFModel")
        {
        }

        public virtual DbSet<InputBase> InputBases { get; set; }
        public virtual DbSet<CryteriaBase> Cryteria { get; set; }
        public virtual DbSet<InputFile> InputFiles { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<VisaImportRule> VisaImportRules { get; set; }
        public virtual DbSet<CurrencyCode> CurrencyCodes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InputBase>().Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            modelBuilder.Entity<VisaRecord>().HasKey(x => x.Id).Map(m =>
                {
                    m.MapInheritedProperties();
                    m.ToTable("VisaRecords");
                });
            modelBuilder.Entity<MasterCardRecord>().HasKey(x => x.Id).Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("MasterCardRecords");
            });
        }
    }
}