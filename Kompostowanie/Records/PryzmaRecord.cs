using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;

namespace Kompostowanie.Records
{
    public class PryzmaRecord
    {
        public PryzmaRecord()
        {
            Skladniki = new List<SkladnikRecord>();
        }

        public virtual int Id { get; protected set; }
        public virtual DoswiadczenieRecord Doswiadczenie { get; set; }
        public virtual string Nazwa { get; set; }
        public virtual decimal Wilgotnosc { get; set; }
        public virtual decimal UdzialSuma { get; set; }
        public virtual decimal IloscWSmSuma { get; set; }
        public virtual decimal MasaSwiezaSuma { get; set; }
        public virtual decimal MasaCSuma { get; set; }
        public virtual decimal MasaNSuma { get; set; }
        public virtual decimal Cn { get; set; }
        public virtual decimal SuchaMasa { get; set; }
        public virtual decimal WilgotnoscOstateczna { get; set; }

        public virtual IList<SkladnikRecord> Skladniki { get; set; }
        public virtual IList<PryzmaStartRecord> Starts { get; set; }
    }

    public class PryzmaMap : ClassMap<PryzmaRecord>
    {
        public PryzmaMap()
        {
            Id(d => d.Id);
            References(d => d.Doswiadczenie).Not.Nullable();
            Map(d => d.Nazwa).Length(255).Not.Nullable();
            Map(d => d.Wilgotnosc).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.UdzialSuma).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.IloscWSmSuma).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.MasaSwiezaSuma).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.MasaCSuma).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.MasaNSuma).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.Cn).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.SuchaMasa).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.WilgotnoscOstateczna).Precision(10).Scale(2).Not.Nullable();
            HasMany(d => d.Skladniki).KeyColumn("Pryzma_id").Inverse();
            HasMany(d => d.Starts).KeyColumn("Pryzma_id").Inverse();
        }
    }

    public interface IPryzmaRepository : IRepository<PryzmaRecord>
    {
        IQueryable<PryzmaRecord> QueryByDoswiadczenieId(int id);
    }

    public class PryzmaRepository : Repository<PryzmaRecord>, IPryzmaRepository
    {
        public PryzmaRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public IQueryable<PryzmaRecord> QueryByDoswiadczenieId(int id)
        {
            return Query()
                .Where(p => p.Doswiadczenie.Id == id);
        }
    }
}