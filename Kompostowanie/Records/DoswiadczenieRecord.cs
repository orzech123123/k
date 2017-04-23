using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace Kompostowanie.Records
{
    public class DoswiadczenieRecord
    {
        public DoswiadczenieRecord()
        {
            Pryzmy = new List<PryzmaRecord>();
            Pomiary = new List<DoswiadczeniePomiarRecord>();
            Aeracje = new List<AeracjaRecord>();
        }

        public virtual int Id { get; protected set; }
        public virtual string Nazwa { get; set; }
        public virtual string Cel { get; set; }
        public virtual DateTime Data { get; set; }

        public virtual bool Started { get; set; }
        public virtual bool Stopped { get; set; }
        public virtual DateTime? DataStart { get; set; }
        public virtual DateTime? DataStop { get; set; }

        public virtual IList<PryzmaRecord> Pryzmy { get; set; }
        public virtual IList<AeracjaRecord> Aeracje { get; set; }
        public virtual IList<DoswiadczeniePomiarRecord> Pomiary { get; set; }
    }

    public class DoswiadczenieMap : ClassMap<DoswiadczenieRecord>
    {
        public DoswiadczenieMap()
        {
            Id(d => d.Id);
            Map(d => d.Nazwa).Length(255).Not.Nullable();
            Map(d => d.Cel).Length(1024).Not.Nullable();
            Map(d => d.Data).Not.Nullable();
            Map(d => d.Started).Not.Nullable();
            Map(d => d.Stopped).Not.Nullable();
            Map(d => d.DataStart).Nullable();
            Map(d => d.DataStop).Nullable();
            HasMany(d => d.Pryzmy).KeyColumn("Doswiadczenie_id").Inverse();
            HasMany(d => d.Pomiary).KeyColumn("Doswiadczenie_id").Inverse();
            HasMany(d => d.Aeracje).KeyColumn("Doswiadczenie_id").Inverse();
        }
    }

    public interface IDoswiadczenieRepository : IRepository<DoswiadczenieRecord>
    {
        
    }

    public class DoswiadczenieRepository : Repository<DoswiadczenieRecord>, IDoswiadczenieRepository
    {
        public DoswiadczenieRepository(IUnitOfWork uow) : base(uow)
        {
        }
    }
}