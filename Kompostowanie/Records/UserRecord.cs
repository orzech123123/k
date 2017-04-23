using System.Linq;
using FluentNHibernate.Mapping;

namespace Kompostowanie.Records
{
    public class UserRecord
    {
        public virtual int Id { get; protected set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
    }

    public class UserMap : ClassMap<UserRecord>
    {
        public UserMap()
        {
            Id(u => u.Id);
            Map(u => u.Username).Length(255).Not.Nullable();
            Map(u => u.Password).Length(255).Not.Nullable();
        }
    }

    public interface IUserRepository : IRepository<UserRecord>
    {
        UserRecord GetByUsername(string username);
    }

    public class UserRepository : Repository<UserRecord>, IUserRepository
    {
        public UserRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public UserRecord GetByUsername(string username)
        {
            username = string.IsNullOrEmpty(username) ? username : username.ToLower();

            return Query().SingleOrDefault(u => u.Username.ToLower() == username);
        }
    }
}