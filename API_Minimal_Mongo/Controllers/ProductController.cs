using API_Minimal_Mongo.Domains;
using API_Minimal_Mongo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace API_Minimal_Mongo.Controllers
{
    /// <summary>
    /// Controlador para gerenciar produtos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        // Referência para a coleção de produtos no MongoDB
        private readonly IMongoCollection<Product> _product;

        /// <summary>
        /// Construtor que recebe como dependência o objeto da classe MongoDbService.
        /// </summary>
        /// <param name="mongoDbService">Objeto da classe MongoDbService.</param>
        public ProductController(MongoDbService mongoDbService)
        {
            // Obtém a coleção "product" do banco de dados
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        /// <summary>
        /// Obtém a lista de todos os produtos.
        /// </summary>
        /// <returns>Lista de produtos.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                // Busca todos os produtos na coleção
                var products = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();
                // Retorna a lista de produtos com status 200 OK
                return Ok(products);
            }
            catch (Exception e)
            {
                // Em caso de erro, retorna uma mensagem de erro com status 400 BadRequest
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        /// <param name="product">Objeto produto a ser criado.</param>
        /// <returns>Status da operação.</returns>
        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            try
            {
                // Insere um novo produto na coleção
                await _product.InsertOneAsync(product);
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
        /// Atualiza um produto existente.
        /// </summary>
        /// <param name="Id">ID do produto a ser atualizado.</param>
        /// <param name="product">Objeto produto com as informações atualizadas.</param>
        /// <returns>Status da operação.</returns>
        [HttpPut]
        public async Task<ActionResult<Product>> Edit(string Id, Product product)
        {
            try
            {
                // Cria um filtro para encontrar o produto pelo ID
                var filter = Builders<Product>.Filter.Eq(x => x.Id, product.Id);
                // Substitui o produto existente pelo produto atualizado
                await _product.ReplaceOneAsync(filter, product);
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
        /// Obtém um produto pelo ID.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>Objeto produto.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(string id)
        {
            try
            {
                // Busca um produto pelo ID na coleção
                var product = await _product.Find(x => x.Id == id).FirstOrDefaultAsync();
                // Retorna o produto encontrado com status 200 OK, ou status 404 NotFound se não encontrado
                return product is not null ? Ok(product) : NotFound();
            }
            catch (Exception)
            {
                // Em caso de erro, lança a exceção
                throw;
            }
        }

        /// <summary>
        /// Exclui um produto pelo ID.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>Status da operação.</returns>
        [HttpDelete]
        public async Task<ActionResult<Product>> Delete(string id)
        {
            // Cria um filtro para encontrar o produto pelo ID
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);

            // Se o filtro for nulo, retorna 404 NotFound
            if (filter is null)
            {
                return NotFound();
            }

            // Exclui o produto da coleção
            await _product.DeleteOneAsync(filter);

            // Retorna status 200 OK em caso de sucesso
            return Ok();
        }
    }
}
