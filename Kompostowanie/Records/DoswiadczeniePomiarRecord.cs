using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace Kompostowanie.Records
{
    public class DoswiadczeniePomiarRecord
    {
        public DoswiadczeniePomiarRecord()
        {
            Pomiary = new List<PryzmaPomiarRecord>();
        }

        public virtual int Id { get; set; }
        public virtual DoswiadczenieRecord Doswiadczenie { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual int Dzien { get; set; }
        public virtual decimal Godzina { get; set; }
        public virtual decimal CzasComputed { get; set; }
        public virtual decimal DzienComputed { get; set; }

        public virtual IList<PryzmaPomiarRecord> Pomiary{ get; set; }
    }

    public class DoswiadczeniePomiarMap : ClassMap<DoswiadczeniePomiarRecord>
    {
        public DoswiadczeniePomiarMap()
        {
            Id(p => p.Id);
            References(p => p.Doswiadczenie).Not.Nullable();
            Map(p => p.Data).Not.Nullable();
            Map(p => p.Dzien).Not.Nullable();
            Map(p => p.Godzina).Precision(10).Scale(2).Not.Nullable();
            Map(p => p.CzasComputed).Precision(10).Scale(2).Not.Nullable();
            Map(p => p.DzienComputed).Precision(10).Scale(2).Not.Nullable();
            HasMany(p => p.Pomiary).KeyColumn("DoswiadczeniePomiar_id").Inverse();
        }
    }

    public interface IDoswiadczeniePomiarRepository : IRepository<DoswiadczeniePomiarRecord>
    {
    }

    public class DoswiadczeniePomiarRepository : Repository<DoswiadczeniePomiarRecord>, IDoswiadczeniePomiarRepository
    {
        public DoswiadczeniePomiarRepository(IUnitOfWork uow) : base(uow)
        {
        }
    }
}