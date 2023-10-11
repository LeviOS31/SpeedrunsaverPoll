using Interfaces.DTO;
using MongoDB.Driver;

namespace Database
{
    public class PollDal
    {
        private readonly IMongoCollection<PollDTO> _collection;
        public PollDal()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("Speedruns");
            _collection = database.GetCollection<PollDTO>("polls");
        }

        public async Task<List<PollDTO>> GetAllPolls()
        {
            return await _collection.Find(poll => true).ToListAsync();
        }

        public async Task<PollDTO> GetPollById(string id)
        {
            return await _collection.Find(poll => poll.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreatePoll(PollDTO poll)
        {
            await _collection.InsertOneAsync(poll);
        }

        public async Task UpdatePoll(string id, PollDTO poll)
        {
            await _collection.ReplaceOneAsync(poll => poll.Id == id, poll);
        }

        public async Task DeletePoll(string id)
        {
            await _collection.DeleteOneAsync(poll => poll.Id == id);
        }
    }
}
