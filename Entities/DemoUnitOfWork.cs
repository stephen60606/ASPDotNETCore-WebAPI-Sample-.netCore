using Entities.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using URF.Core.Abstractions;
using URF.Core.EF;

namespace Entities
{
    public class DemoUnitOfWork : UnitOfWork, IDemoUnitOfWork, IUnitOfWork
    {
        private DemoDbContext dbContext;

        public IDbContextTransaction Transaction { get; set; }

        public DemoUnitOfWork(DemoDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual void BeginTransaction()
        {
            if (Context.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
            {
                Context.Database.OpenConnection();
            }
            Transaction = Context.Database.BeginTransaction();
        }

        public virtual bool Commit()
        {
            Transaction.Commit();
            return true;
        }

        public virtual void Rollback()
        {
            Transaction.Rollback();
        }
    }
}

