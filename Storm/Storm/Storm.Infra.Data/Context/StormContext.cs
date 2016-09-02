using MongoDB.Driver;
using System;

namespace Storm.Infra.Data.Context
{
    public class StormContext 
    {
        public readonly IMongoDatabase context;

        public StormContext()
        {
            var client = new MongoClient("mongodb://user:123@ds040089.mlab.com:40089/heroesofthestorm");
            context = client.GetDatabase("heroesofthestorm");
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
