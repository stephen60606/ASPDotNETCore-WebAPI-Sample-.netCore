using System;
using Microsoft.EntityFrameworkCore.Storage;
using URF.Core.Abstractions;

namespace Entities
{

    public interface IDemoUnitOfWork : IUnitOfWork
    {
        IDbContextTransaction Transaction { get; }

        void BeginTransaction();

        bool Commit();

        void Rollback();
    }

}

