using API_Minimal_Mongo.Domains;
using API_Minimal_Mongo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace API_Minimal_Mongo.Controllers
{
    /// <summary>
    /// Controlador para gerenciar clientes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ClientController : ControllerBase
    {
        // Referência para a coleção de clientes no MongoDB
        private readonly IMongoCollection<Client> _client;
        // Referência para a coleção de usuários no MongoDB
        private readonly IMongoCollection<User> _user;

        /// <summary>
        /// Construtor que inicializa as coleções de cliente e usuário.
        /// </summary>
        /// <param name="mongoDbService">Serviço MongoDb.</param>
        public ClientController(MongoDbService mongoDbService)
        {
            // Inicializa a coleção de clientes
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            // Inicializa a coleção de usuários
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        /// <summary>
        /// Obtém a lista de todos os clientes.
        /// </summary>
        /// <returns>Lista de clientes.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Client>>> Get()
        {
            try
            {
                // Busca todos os clientes na coleção
                var clients = await _client.Find(_ => true).ToListAsync();
                // Retorna a lista de clientes com status 200 OK
                return Ok(clients);
            }
            catch (Exception e)
            {
                // Em caso de erro, retorna uma mensagem de erro com status 400 BadRequest
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        /// <param name="client">Objeto cliente a ser criado.</param>
        /// <returns>Status da operação.</returns>
        [HttpPost]
        public async Task<ActionResult<Client>> Create(Client client)
        {
            try
            {
                // Insere um novo cliente na coleção
                await _client.InsertOneAsync(client);
                // Retorna status 200 OK em caso de sucesso
                return Ok();
            }
            catch (Exception e)
            {
                // Em caso de erro, retorna uma mensagem de erro com status 400 BadRequest
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Atualiza um cliente existente.
        /// </summary>
        /// <param name="client">Objeto cliente com as informações atualizadas.</param>
        /// <returns>Status da operação.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Client>> Atualizar(Client client)
        {
            try
            {
                // Cria um filtro para encontrar o cliente pelo ID
                var filter = Builders<Client>.Filter.Eq(x => x.Id, client.Id);
                // Substitui o cliente existente pelo cliente atualizado
                await _client.ReplaceOneAsync(filter, client);
                // Retorna status 200 OK em caso de sucesso
                return Ok();
            }
            catch (Exception)
            {
                // Em caso de erro, lança a exceção
                throw;
            }
        }

        /// <summary>
        /// Obtém um cliente pelo ID.
        /// </summary>
        /// <param name="id">ID do cliente.</param>
        /// <returns>Objeto cliente.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetById(string id)
        {
            try
            {
                // Busca um cliente pelo ID na coleção
                var client = await _client.Find(x => x.Id == id).FirstOrDefaultAsync();
                // Retorna o cliente encontrado com status 200 OK, ou status 400 BadRequest se não encontrado
                return client is not null ? Ok(client) : BadRequest();
            }
            catch (Exception)
            {
                // Em caso de erro, lança a exceção
                throw;
            }
        }

        /// <summary>
        /// Exclui um cliente pelo ID.
        /// </summary>
        /// <param name="id">ID do cliente.</param>
        /// <returns>Status da operação.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> Delete(string id)
        {
            try
            {
                // Cria um filtro para encontrar o cliente pelo ID
                var filter = Builders<Client>.Filter.Eq(x => x.Id, id);
                // Exclui o cliente da coleção
                await _client.DeleteOneAsync(filter);
                // Retorna status 204 NoContent em caso de sucesso
                return NoContent();
            }
            catch (Exception)
            {
                // Em caso de erro, lança a exceção
                throw;
            }
        }
    }
}
