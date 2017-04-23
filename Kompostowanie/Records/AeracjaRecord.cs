using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace Kompostowanie.Records
{
    public class AeracjaRecord
    {
        public AeracjaRecord()
        {
            AeracjaEntries = new List<AeracjaEntryRecord>();
        }

        public virtual int Id { get; protected set; }
        public virtual DoswiadczenieRecord Doswiadczenie { get; set; }
        public virtual string Nazwa { get; set; }
        public virtual IList<AeracjaEntryRecord> AeracjaEntries { get; set; }
    }

    public class AeracjaMap : ClassMap<AeracjaRecord>
    {
        public AeracjaMap()
        {
            Id(s => s.Id);
            References(s => s.Doswiadczenie).Not.Nullable();
            Map(s => s.Nazwa).Not.Nullable();
            HasMany(d => d.AeracjaEntries).KeyColumn("Aeracja_id").Inverse();
        }
    }

    public interface IAeracjaRepository : IRepository<AeracjaRecord>
    {
    }

    public class AeracjaRepository : Repository<AeracjaRecord>, IAeracjaRepository
    {
        public AeracjaRepository(IUnitOfWork uow) : base(uow)
        {
        }
    }
}