using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using FluentNHibernate.Mapping;
using Newtonsoft.Json.Serialization;

namespace Kompostowanie.Records
{
    public class SkladnikRecord
    {
        public virtual int Id { get; protected set; }
        public virtual PryzmaRecord Pryzma { get; set; }
        public virtual Skladnik Skladnik { get; set; }
        public virtual decimal SuchaMasa { get; set; }
        public virtual decimal Udzial { get; set; }
        public virtual decimal IloscWSm { get; set; }
        public virtual decimal MasaSwieza { get; set; }
        public virtual decimal ZawartoscC { get; set; }
        public virtual decimal ZawartoscN { get; set; }
        public virtual decimal MasaC { get; set; }
        public virtual decimal MasaN { get; set; }
        public virtual decimal Cn { get; set; }
    }

    public enum Skladnik
    {
        [Description("Słoma")]
        Sloma,
        [Description("Trociny")]
        Trociny,
        [Description("Osady ściekowe")]
        OsadySciekowe,
        [Description("Obornik kurzy")]
        ObornikKurzy,
        [Description("Obornik bydlęcy")]
        ObornikBydlecy,
        [Description("Obornik świński")]
        ObornikSwinski,
        [Description("Ferment")]
        Ferment
    }

    public class SkladnikMap : ClassMap<SkladnikRecord>
    {
        public SkladnikMap()
        {
            Id(d => d.Id);
            References(d => d.Pryzma).Not.Nullable();
            Map(d => d.Skladnik).Length(32).Not.Nullable();
            Map(d => d.SuchaMasa).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.Udzial).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.IloscWSm).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.MasaSwieza).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.ZawartoscC).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.ZawartoscN).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.MasaC).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.MasaN).Precision(10).Scale(2).Not.Nullable();
            Map(d => d.Cn).Precision(10).Scale(2).Not.Nullable();
        }
    }

    public interface ISkladnikRepository : IRepository<SkladnikRecord>
    {
        IQueryable<SkladnikRecord> QueryByPryzma(int id);
    }

    public class SkladnikRepository : Repository<SkladnikRecord>, ISkladnikRepository
    {
        public SkladnikRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public IQueryable<SkladnikRecord> QueryByPryzma(int id)
        {
            return Query()
                .Where(s => s.Pryzma.Id == id);
        }
    }
}