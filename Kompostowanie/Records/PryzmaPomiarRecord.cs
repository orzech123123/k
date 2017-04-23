using System.ComponentModel;
using System.Linq;
using FluentNHibernate.Mapping;

namespace Kompostowanie.Records
{
    public class PryzmaPomiarRecord
    {
        public virtual int Id { get; set; }
        public virtual DoswiadczeniePomiarRecord DoswiadczeniePomiar { get; set; }
        public virtual PryzmaRecord Pryzma { get; set; }
        public virtual PryzmaPomiarTyp Typ { get; set; }
        public virtual decimal Value { get; set; }
    }

    public enum PryzmaPomiarTyp
    {
        [Description("Temperatura [C]")]
        Temperatura,
        [Description("O2 [v/v %]")]
        O2,
        [Description("NH3 [ppm]")]
        Nh3,
        [Description("CO2 [v/v %]")]
        Co2,
        [Description("CH4 [v/v %]")]
        Ch4,
        [Description("H2S [ppm]")]
        H2S,

        [Description("Stan liczników")]
        StanLicznikow,
        [Description("Przepływ [l/min]")]
        Przeplyw,
    }

    public class PryzmaPomiarMap : ClassMap<PryzmaPomiarRecord>
    {
        public PryzmaPomiarMap()
        {
            Id(p => p.Id);
            References(p => p.DoswiadczeniePomiar).Not.Nullable();
            References(p => p.Pryzma).Not.Nullable();
            Map(p => p.Typ).Not.Nullable();
            Map(p => p.Value).Precision(10).Scale(2).Not.Nullable();
        }
    }

    public interface IPryzmaPomiarRepository : IRepository<PryzmaPomiarRecord>
    {
        IQueryable<PryzmaPomiarRecord> QueryByDoswiadczenieAndTyp(int doswiadczenieId, PryzmaPomiarTyp typ);
        PryzmaPomiarRecord GetByPryzmaAndTyp(int pryzmaId, int doswiadczeniePomiarId, PryzmaPomiarTyp typ);
    }

    public class PryzmaPomiarRepository : Repository<PryzmaPomiarRecord>, IPryzmaPomiarRepository
    {
        public PryzmaPomiarRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public IQueryable<PryzmaPomiarRecord> QueryByDoswiadczenieAndTyp(int doswiadczenieId, PryzmaPomiarTyp typ)
        {
            return Query()
                .Where(p => p.DoswiadczeniePomiar.Doswiadczenie.Id == doswiadczenieId)
                .Where(p => p.Typ == typ);
        }

        public PryzmaPomiarRecord GetByPryzmaAndTyp(int pryzmaId, int doswiadczeniePomiarId, PryzmaPomiarTyp typ)
        {
            return Query()
                .Where(p => p.Pryzma.Id == pryzmaId)
                .Where(p => p.DoswiadczeniePomiar.Id == doswiadczeniePomiarId)
                .SingleOrDefault(p => p.Typ == typ);
        }
    }
}