using MongoDB.Driver;

namespace WebGame.Domain
{
    public class MongoGameRepository : IGameRepository
    {
        private readonly IMongoCollection<GameEntity> gameCollection;

        public MongoGameRepository(IMongoDatabase database)
        {
            gameCollection = database.GetCollection<GameEntity>("games");
        }

        public GameEntity Create(GameEntity game)
        {
            gameCollection.InsertOne(game);
            return game;
        }

        public GameEntity ReadById(string gameId)
        {
            return gameCollection.Find(g => g.Id == gameId).Single();
        }

        public void Update(GameEntity game)
        {
            gameCollection.ReplaceOne(g => g.Id == game.Id, game);
        }
    }
}