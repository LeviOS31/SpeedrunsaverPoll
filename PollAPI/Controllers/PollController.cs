using Database;
using Interfaces.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;

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
    public async Task CreatePoll([FromBody]PollCreation pollcreator)
    {
        try
        {
            string APIURI = "http://localhost:5099";
            
            using (var client = new HttpClient())
            {
                var userauth = new
                {
                    id = pollcreator.UserAuth.id,
                    username = pollcreator.UserAuth.username,
                    password = "",
                    country = "",
                    email = "",
                    admin = pollcreator.UserAuth.admin
                };
                var JSON = new StringContent(JsonSerializer.Serialize(userauth), Encoding.UTF8, "application/json");

                client.BaseAddress = new Uri(APIURI);

                var Response = await client.PostAsync("/User/Checks", JSON);

                if (!Response.IsSuccessStatusCode)
                {
                    throw new Exception("User is not authorized to create polls.");
                }

            }

            await DB.CreatePoll(pollcreator.Poll);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create poll. ", ex);
        }
    }

    [HttpPost]
    [Route("Polls/Addvote")]
    public async Task AddVote(string id, int option)
    {
        PollDTO poll = await DB.GetPollById(id);
        try
        {
            poll.votes[option]++;
            await DB.UpdatePoll(id, poll);
        }
        catch (Exception ex)
        {
            throw new Exception("cant add vote to non existing option. ", ex);
        }
    }

    [HttpPost]
    [Route("Polls/Setactive")]
    public async Task SetActive(string id,[FromBody] bool active)
    {
        PollDTO poll = await DB.GetPollById(id);
        poll.active = active;
        await DB.UpdatePoll(id, poll);
    }

    [HttpPost]
    [Route("Polls/Setvisible")]
    public async Task SetVisible(string id, [FromBody] bool visible)
    {
        PollDTO poll = await DB.GetPollById(id);
        poll.visible = visible;
        await DB.UpdatePoll(id, poll);
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

