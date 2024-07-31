using API_Minimal_Mongo.Domains;
using API_Minimal_Mongo.Services;
using API_Minimal_Mongo.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace API_Minimal_Mongo.Controllers
{
    /// <summary>
    /// Controlador para gerenciar pedidos (orders).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrderController : ControllerBase
    {
        // Referência para a coleção de pedidos no MongoDB
        private readonly IMongoCollection<Order> _order;
        // Referência para a coleção de clientes no MongoDB
        private readonly IMongoCollection<Client> _client;
        // Referência para a coleção de produtos no MongoDB
        private readonly IMongoCollection<Product> _product;

        /// <summary>
        /// Construtor que inicializa as coleções de pedidos, clientes e produtos.
        /// </summary>
        /// <param name="mongoDbService">Serviço MongoDb.</param>
        public OrderController(MongoDbService mongoDbService)
        {
            // Inicializa a coleção de pedidos
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
            // Inicializa a coleção de clientes
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            // Inicializa a coleção de produtos
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        /// <summary>
        /// Obtém a lista de todos os pedidos.
        /// </summary>
        /// <returns>Lista de pedidos.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get()
        {
            // Busca todos os pedidos na coleção
            var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();
            // Retorna a lista de pedidos com status 200 OK se encontrar, ou 404 NotFound se não encontrar
            return orders is not null ? Ok(orders) : NotFound();
        }

        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        /// <param name="orderViewModel">Modelo de visualização do pedido a ser criado.</param>
        /// <returns>Status da operação.</returns>
        [HttpPost]
        public async Task<ActionResult<Order>> Create([FromBody] OrderViewModel orderViewModel)
        {
            try
            {
                // Cria um novo objeto Order a partir do ViewModel
                var order = new Order
                {
                    Id = orderViewModel.Id,
                    OrderDate = orderViewModel.OrderDate,
                    Status = orderViewModel.Status,
                    ClientId = orderViewModel.ClientId,
                    ProductId = orderViewModel.ProductId,
                    AdditionalAttributes = orderViewModel.AdditionalAttributes!
                };
                // Busca o cliente correspondente no banco de dados
                var client = await _client.Find(c => c.Id == orderViewModel.ClientId).FirstOrDefaultAsync();

                // Se o cliente não for encontrado, retorna 404 NotFound
                if (client == null)
                {
                    return NotFound();
                }

                // Atribui o cliente ao pedido
                order.Client = client;

                // Insere o novo pedido na coleção
                await _order.InsertOneAsync(order);

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
        /// Atualiza um pedido existente.
        /// </summary>
        /// <param name="Id">ID do pedido a ser atualizado.</param>
        /// <param name="order">Objeto pedido com as informações atualizadas.</param>
        /// <returns>Status da operação.</returns>
        [HttpPut]
        public async Task<ActionResult<Order>> Edit(string Id, Order order)
        {
            try
            {
                // Cria um filtro para encontrar o pedido pelo ID
                var filter = Builders<Order>.Filter.Eq(x => x.Id, order.Id);
                // Substitui o pedido existente pelo pedido atualizado
                await _order.ReplaceOneAsync(filter, order);
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
        /// Exclui um pedido pelo ID.
        /// </summary>
        /// <param name="id">ID do pedido a ser excluído.</param>
        /// <returns>Status da operação.</returns>
        [HttpDelete]
        public async Task<ActionResult<Order>> Delete(string id)
        {
            // Cria um filtro para encontrar o pedido pelo ID
            var filter = Builders<Order>.Filter.Eq(x => x.Id, id);

            // Se o filtro for nulo, retorna 404 NotFound
            if (filter is null)
            {
                return NotFound();
            }

            // Exclui o pedido da coleção
            await _order.DeleteOneAsync(filter);

            // Retorna status 204 NoContent em caso de sucesso
            return NoContent();
        }

        /// <summary>
        /// Obtém um pedido pelo ID.
        /// </summary>
        /// <param name="id">ID do pedido.</param>
        /// <returns>Objeto pedido.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(string id)
        {
            // Busca um pedido pelo ID na coleção
            var order = await _order.Find(x => x.Id == id).FirstOrDefaultAsync();
            // Retorna o pedido encontrado com status 200 OK, ou status 404 NotFound se não encontrado
            return order is not null ? Ok(order) : NotFound();
        }
    }
}
