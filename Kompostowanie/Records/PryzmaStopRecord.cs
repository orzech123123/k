using System;
using System.Linq;
using FluentNHibernate.Mapping;

namespace Kompostowanie.Records
{
    public class PryzmaStopRecord
    {
        public virtual int Id { get; protected set; }
        public virtual PryzmaRecord Pryzma { get; set; }
        public virtual decimal IleZebranoNaProbki { get; set; }
        public virtual decimal WysokoscOdBrzegu { get; set; }
        public virtual decimal Przeplyw { get; set; }
        public virtual decimal Ph1 { get; set; }
        public virtual decimal Ph2 { get; set; }
        public virtual decimal Konduktywnosc1 { get; set; }
        public virtual decimal Konduktywnosc2 { get; set; }
    }

    public class PryzmaStopMap : ClassMap<PryzmaStopRecord>
    {
        public PryzmaStopMap()
        {
            Id(s => s.Id);
            References(s => s.Pryzma).Not.Nullable();
            Map(s => s.WysokoscOdBrzegu).Precision(10).Scale(2).Not.Nullable();
            Map(s => s.Przeplyw).Precision(10).Scale(2).Not.Nullable();
            Map(s => s.Ph1).Precision(10).Scale(2).Not.Nullable();
            Map(s => s.Ph2).Precision(10).Scale(2).Not.Nullable();
            Map(s => s.Konduktywnosc1).Precision(10).Scale(4).Not.Nullable();
            Map(s => s.Konduktywnosc2).Precision(10).Scale(4).Not.Nullable();
            Map(s => s.IleZebranoNaProbki).Precision(10).Scale(2).Not.Nullable();
        }
    }

    public interface IPryzmaStopRepository : IRepository<PryzmaStopRecord>
    {
        PryzmaStopRecord GetByPryzma(int id);
    }

    public class PryzmaStopRepository : Repository<PryzmaStopRecord>, IPryzmaStopRepository
    {
        public PryzmaStopRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public PryzmaStopRecord GetByPryzma(int id)
        {
            return Query()
                .SingleOrDefault(p => p.Pryzma.Id == id);
        }
    }
}