using API_Minimal_Mongo.Domains;
using API_Minimal_Mongo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace API_Minimal_Mongo.Controllers
{
    /// <summary>
    /// Controlador para gerenciar usuários.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        // Referência para a coleção de usuários no MongoDB
        private readonly IMongoCollection<User> _user;

        /// <summary>
        /// Construtor que recebe como dependência o objeto da classe MongoDbService.
        /// </summary>
        /// <param name="mongoDbService">Objeto da classe MongoDbService.</param>
        public UserController(MongoDbService mongoDbService)
        {
            // Inicializa a coleção de usuários a partir do banco de dados obtido através do serviço
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        /// <summary>
        /// Obtém a lista de todos os usuários.
        /// </summary>
        /// <returns>Lista de usuários.</returns>
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                // Outro jeito para listar todos: "(_ => true)" é uma regra de validação para listar todos
                var users = await _user.Find(_ => true).ToListAsync();
                // Retorna a lista de usuários com status 200 OK
                return Ok(users);
            }
            catch (Exception e)
            {
                // Em caso de erro, retorna uma mensagem de erro com status 400 BadRequest
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="user">Objeto usuário a ser criado.</param>
        /// <returns>Status da operação.</returns>
        [HttpPost]
        public async Task<ActionResult<User>> Create(User user)
        {
            try
            {
                // Insere um novo usuário na coleção
                await _user.InsertOneAsync(user);
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
        /// Atualiza um usuário existente.
        /// </summary>
        /// <param name="Id">ID do usuário a ser atualizado.</param>
        /// <param name="user">Objeto usuário com as informações atualizadas.</param>
        /// <returns>Status da operação.</returns>
        [HttpPut]
        public async Task<ActionResult<User>> Edit(string Id, User user)
        {
            try
            {
                // Cria um filtro para encontrar o usuário pelo ID
                var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);
                // Substitui o usuário existente pelo usuário atualizado
                await _user.ReplaceOneAsync(filter, user);
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
        /// Exclui um usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <returns>Status da operação.</returns>
        [HttpDelete]
        public async Task<ActionResult<User>> Delete(string id)
        {
            // Cria um filtro para encontrar o usuário pelo ID
            var filter = Builders<User>.Filter.Eq(x => x.Id, id);

            // Se o filtro for nulo, retorna 404 NotFound
            if (filter is null)
            {
                return NotFound();
            }

            // Exclui o usuário da coleção
            await _user.DeleteOneAsync(filter);

            // Retorna status 204 NoContent em caso de sucesso
            return NoContent();
        }

        /// <summary>
        /// Obtém um usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <returns>Objeto usuário.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            try
            {
                // Busca um usuário pelo ID na coleção
                var user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();
                // Retorna o usuário encontrado com status 200 OK, ou status 404 NotFound se não encontrado
                return user is not null ? Ok(user) : NotFound();
            }
            catch (Exception)
            {
                // Em caso de erro, lança a exceção
                throw;
            }
        }
    }
}
