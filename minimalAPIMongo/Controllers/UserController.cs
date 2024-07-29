using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using MongoDB.Driver;

namespace minimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Armazena os dados de acesso da collection
        /// </summary>
        private readonly IMongoCollection<User> _user;

        /// <summary>
        /// Construtor que recebe como dependência o obj da classe MongoDbService
        /// </summary>
        /// <param name="mongoDbService"></param>
        public UserController(MongoDbService mongoDbService)
        {
            // Obtem a coleção "user"
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                var users = await _user.Find(FilterDefinition<User>.Empty).ToListAsync();
                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(u => u.Id, id);
                var user = await _user.Find(filter).FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound("User not found");
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] User user)
        {
            try
            {
                await _user.InsertOneAsync(user);
                return StatusCode(201, user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> Update(string id, [FromBody] User updatedUser)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(u => u.Id, id);
                var result = await _user.ReplaceOneAsync(filter, updatedUser);

                if (result.MatchedCount == 0)
                {
                    return NotFound("User not found");
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(u => u.Id, id);
                var result = await _user.DeleteOneAsync(filter);

                if (result.DeletedCount == 0)
                {
                    return NotFound("User not found");
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

