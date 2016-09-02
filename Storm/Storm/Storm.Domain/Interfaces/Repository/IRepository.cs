using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Storm.Domain.Interfaces.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity obj);

        TEntity GetById(ObjectId id);

        IEnumerable<TEntity> ListAll();

        void Update(TEntity obj);

        void Remove(ObjectId id);

        IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> predicate);

        TEntity GetByName(string name);
    }

}
