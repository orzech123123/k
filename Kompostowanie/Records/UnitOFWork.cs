using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Kompostowanie.Records
{
    public interface IUnitOfWork : IDisposable
    {
        ISession Session { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private static ISessionFactory sessionFactory;
        private ITransaction transaction;

        public ISession Session { get; }

        public static void Initialize()
        {
            if (sessionFactory != null)
                return;

            sessionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(cs => cs.FromConnectionStringWithKey("KompostowanieDatabase")))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Repository<int>>())
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                    .BuildSessionFactory();
        }

        static UnitOfWork()
        {
            Initialize();
        }

        public UnitOfWork()
        {
            Session = sessionFactory.OpenSession();
        }

        public void BeginTransaction()
        {
            transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                // commit transaction if there is one active
                if (transaction != null && transaction.IsActive)
                    transaction.Commit();
            }
            catch
            {
                // rollback if there was an exception
                if (transaction != null && transaction.IsActive)
                    transaction.Rollback();

                throw;
            }
            finally
            {
                //Session.Dispose();
            }
        }

        public void Rollback()
        {
            try
            {
                if (transaction != null && transaction.IsActive)
                    transaction.Rollback();
            }
            finally
            {
                //Session.Dispose();
            }
        }

        public void Dispose()
        {
            Session.Dispose();
        }
    }
}