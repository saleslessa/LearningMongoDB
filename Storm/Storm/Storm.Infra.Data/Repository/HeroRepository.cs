using Storm.Domain.Entities;
using Storm.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver;
using Storm.Infra.Data.Context;
using MongoDB.Bson;

namespace Storm.Infra.Data.Repository
{
    public class HeroRepository : IRepository<Hero>, IHeroRepository
    {
        protected StormContext db;
        protected IMongoCollection<Hero> dbSet;

        public HeroRepository(StormContext database, Hero entity)
        {
            db = database;
            dbSet = db.context.GetCollection<Hero>(entity.ToString());

        }

        public void Add(Hero obj)
        {
            dbSet.InsertOne(obj);
        }

        public Hero GetById(ObjectId id)
        {
            var filter = Builders<Hero>.Filter.Eq("HeroId", id);
            return dbSet.Find(filter).FirstOrDefault();
        }

        public IEnumerable<Hero> ListAll()
        {
            var filter = Builders<Hero>.Filter.Eq("HeroDeleted", false);
            return dbSet.Find(filter).ToList();
        }

        public void Remove(ObjectId id)
        {
            var hero = Builders<Hero>.Filter.Eq("HeroId", id);
            var update = Builders<Hero>.Update.Set("HeroDeleted", true);
            dbSet.UpdateOne(hero, update);
        }

        public IEnumerable<Hero> Search(Expression<Func<Hero, bool>> predicate)
        {
            var filter = Builders<Hero>.Filter.Where(predicate);
            return dbSet.Find(filter).ToList();
        }

        public void Update(Hero obj)
        {
            var hero = Builders<Hero>.Filter.Eq("HeroId", obj.HeroId);
            dbSet.ReplaceOne(hero, obj);
        }

        public Hero GetByName(string name)
        {
            var filter = Builders<Hero>.Filter.Eq("HeroName", name);
            return dbSet.Find(filter).FirstOrDefault();
        }
    }
}
