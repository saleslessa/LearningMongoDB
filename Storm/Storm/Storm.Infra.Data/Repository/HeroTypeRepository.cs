using Storm.Domain.Entities;
using Storm.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;
using Storm.Infra.Data.Context;
using MongoDB.Bson;

namespace Storm.Infra.Data.Repository
{
    public class HeroTypeRepository : IRepository<HeroType>, IHeroTypeRepository
    {
        protected StormContext db;
        protected IMongoCollection<HeroType> dbSet;

        public HeroTypeRepository(StormContext database, HeroType entity)
        {
            db = database;
            dbSet = db.context.GetCollection<HeroType>(entity.ToString());
        }

        public void Add(HeroType obj)
        {
            dbSet.InsertOne(obj);
        }

        public void Dispose()
        {
            db.Dispose();
            GC.SuppressFinalize(this);
        }

        public HeroType GetById(ObjectId id)
        {
            var filter = Builders<HeroType>.Filter.Eq("HeroTypeId", id);
            return dbSet.Find(filter).FirstOrDefault();
        }

        public HeroType GetByName(string name)
        {
            var filter = Builders<HeroType>.Filter.Eq("HeroTypeName", name);
            return dbSet.Find(filter).FirstOrDefault();
        }

        public IEnumerable<HeroType> ListAll()
        {
            var filter = Builders<HeroType>.Filter.Empty;
            return dbSet.Find(filter).ToList();
        }

        public void Remove(ObjectId id)
        {
            var heroType = Builders<HeroType>.Filter.Eq("HeroTypeId", id);
            var update = Builders<HeroType>.Update.Set("HeroTypeDeleted", true);
            dbSet.UpdateOne(heroType, update);
        }

        public IEnumerable<HeroType> Search(Expression<Func<HeroType, bool>> predicate)
        {
            var filter = Builders<HeroType>.Filter.Where(predicate);
            return dbSet.Find(filter).ToList();
        }

        public void Update(HeroType obj)
        {
            var heroType = Builders<HeroType>.Filter.Eq("HeroTypeId", obj.HeroTypeId);
            dbSet.ReplaceOne(heroType, obj);
        }
    }
}
