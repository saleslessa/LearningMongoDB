using Storm.Domain.Entities;
using Storm.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using System.Linq.Expressions;
using MongoDB.Driver;
using Storm.Infra.Data.Context;

namespace Storm.Infra.Data.Repository
{
    public class PlayerRepository : IRepository<Player>, IPlayerRepository
    {
        protected StormContext db;
        protected IMongoCollection<Player> dbSet;

        public PlayerRepository(StormContext database, Player entity)
        {
            db = database;
            dbSet = db.context.GetCollection<Player>(entity.ToString());
        }

        public void Add(Player obj)
        {
            dbSet.InsertOne(obj);
        }

        public Player GetById(ObjectId id)
        {
            var filter = Builders<Player>.Filter.Eq("PlayerId", id);
            return dbSet.Find(filter).FirstOrDefault();
        }

        public Player GetByName(string name)
        {
            var filter = Builders<Player>.Filter.Eq("PlayerName", name);
            return dbSet.Find(filter).FirstOrDefault();
        }

        public IEnumerable<Player> ListAll()
        {
            var filter = Builders<Player>.Filter.Empty;
            return dbSet.Find(filter).ToList();
        }

        public void Remove(ObjectId id)
        {
            var remove = Builders<Player>.Filter.Eq("PlayerId", id);
            dbSet.DeleteOne(remove);
        }

        public IEnumerable<Player> Search(Expression<Func<Player, bool>> predicate)
        {
            var filter = Builders<Player>.Filter.Where(predicate);
            return dbSet.Find(filter).ToList();
        }

        public void Update(Player obj)
        {
            var player = Builders<Player>.Filter.Eq("PlayerId", obj.PlayerId);
            dbSet.ReplaceOne(player, obj);
        }
    }
}
