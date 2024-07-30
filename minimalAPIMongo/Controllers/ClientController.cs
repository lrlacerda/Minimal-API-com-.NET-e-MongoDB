using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace minimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ClientController : ControllerBase
    {
        /// <summary>
        /// Armazena os dados de acesso da collection
        /// </summary>
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<User> _user;

        /// <summary>
        /// Construtor que recebe como dependência o obj da classe MongoDbService
        /// </summary>
        /// <param name="mongoDbService"></param>
        public ClientController(MongoDbService mongoDbService)
        {
            // Obtem a coleção "client"
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        [HttpGet]
        public async Task<ActionResult<List<Client>>> Get()
        {
            try
            {
                var clients = await _client.Find(FilterDefinition<Client>.Empty).ToListAsync();
                return Ok(clients);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Client>> GetById(string id)
        {
            try
            {
                var filter = Builders<Client>.Filter.Eq(c => c.Id, id);
                var client = await _client.Find(filter).FirstOrDefaultAsync();

                if (client == null)
                {
                    return NotFound("Client not found");
                }

                return Ok(client);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Client>> Create([FromBody] Client client)
        {
            try
            {
                if (client == null)
                {
                    return BadRequest("Client data is null.");
                }

                // Verifica se o UserId fornecido existe
                var userFilter = Builders<User>.Filter.Eq(u => u.Id, client.UserId);
                var userExists = await _user.Find(userFilter).AnyAsync();

                if (!userExists)
                {
                    return BadRequest("The specified UserId does not exist.");
                }

                // Define um novo Id gerado pelo MongoDB se necessário
                client.Id = ObjectId.GenerateNewId().ToString();

                await _client.InsertOneAsync(client);
                return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
            }
            catch (Exception e)
            {
                return BadRequest($"An error occurred: {e.Message}");
            }
        }


        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> Update(string id, [FromBody] Client updatedClient)
        {
            try
            {
                var filter = Builders<Client>.Filter.Eq(c => c.Id, id);
                var existingClient = await _client.Find(filter).FirstOrDefaultAsync();

                if (existingClient == null)
                {
                    return NotFound("Client not found");
                }

                // Verifica se o UserId fornecido existe
                var userFilter = Builders<User>.Filter.Eq(u => u.Id, updatedClient.UserId);
                var userExists = await _user.Find(userFilter).AnyAsync();

                if (!userExists)
                {
                    return BadRequest("The specified UserId does not exist.");
                }

                // Atualiza o cliente
                var updateResult = await _client.ReplaceOneAsync(filter, updatedClient);

                if (updateResult.MatchedCount == 0)
                {
                    return NotFound("Client not found");
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
                var filter = Builders<Client>.Filter.Eq(c => c.Id, id);
                var result = await _client.DeleteOneAsync(filter);

                if (result.DeletedCount == 0)
                {
                    return NotFound("Client not found");
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

