using Database;
using Interfaces.DTO;
using Microsoft.AspNetCore.Mvc;

namespace PollAPI.Controllers;

[ApiController]
public class PollController : Controller
{
    private readonly ILogger<PollController> _logger;
    private readonly PollDal DB;

    public PollController(ILogger<PollController> logger)
    {
        _logger = logger;
        DB = new PollDal();
    }

    [HttpGet]
    [Route("Polls/Getall")]
    public async Task<List<PollDTO>> GetAllPolls()
    {
        return await DB.GetAllPolls();
    }

    [HttpGet]
    [Route("Polls/Get")]
    public async Task<PollDTO> GetPollById(string id)
    {
        return await DB.GetPollById(id);
    }

    [HttpPost]
    [Route("Polls/Create")]
    public async Task CreatePoll([FromBody]PollDTO poll)
    {
        await DB.CreatePoll(poll);
    }

    [HttpPost]
    [Route("Polls/Update")]
    public async Task UpdatePoll(string id, [FromBody] PollDTO poll)
    {
        await DB.UpdatePoll(id, poll);
    }

    [HttpPost]
    [Route("Polls/Delete")]
    public async Task DeletePoll(string id)
    {
        await DB.DeletePoll(id);
    }
} 

