using Interfaces.DTO;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Database
{
    public class PollDal
    {
        private readonly IMongoCollection<PollDTO> _collection;
        public PollDal()
        {
            try
            {
                var clientSettings = MongoClientSettings.FromConnectionString("mongodb+srv://PollsAPI:GkpwsTu9GSLGgVW@cluster0.1hrsxov.mongodb.net/?retryWrites=true&w=majority");
                clientSettings.ConnectTimeout = TimeSpan.FromSeconds(30);
                var client = new MongoClient(clientSettings);
                var database = client.GetDatabase("Speedruns");
                _collection = database.GetCollection<PollDTO>("polls");
            }
            catch (Exception ex)
            {
                // Handle the connection error, such as logging the error or throwing a custom exception.
                throw new Exception("Failed to connect to the database.", ex);
            }
        }
        private async Task<bool> IsConnected()
        {
            try
            {
                // Try to ping the server to check if it's responsive
                await _collection.Database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<PollDTO>> GetAllPolls()
        {
            if (! await IsConnected())
            {
                throw new Exception("Failed to connect to the database.");
            }
            return await _collection.Find(poll => true).ToListAsync();
        }

        public async Task<PollDTO> GetPollById(string id)
        {
            if (!await IsConnected())
            {
                throw new Exception("Failed to connect to the database.");
            }
            return await _collection.Find(poll => poll.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreatePoll(PollDTO poll)
        {
            if (!await IsConnected())
            {
                throw new Exception("Failed to connect to the database.");
            }
            await _collection.InsertOneAsync(poll);
        }

        public async Task UpdatePoll(string id, PollDTO poll)
        {
            if (!await IsConnected())
            {
                throw new Exception("Failed to connect to the database.");
            }
            await _collection.ReplaceOneAsync(poll => poll.Id == id, poll);
        }

        public async Task DeletePoll(string id)
        {
            if (!await IsConnected())
            {
                throw new Exception("Failed to connect to the database.");
            }
            await _collection.DeleteOneAsync(poll => poll.Id == id);
        }
    }
}
