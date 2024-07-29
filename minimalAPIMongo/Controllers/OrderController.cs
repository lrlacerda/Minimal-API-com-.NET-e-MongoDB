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
    public class OrderController : ControllerBase
    {
        /// <summary>
        /// Armazena os dados de acesso da collection
        /// </summary>
        private readonly IMongoCollection<Order> _order;

        /// <summary>
        /// Construtor que recebe como dependência o obj da classe MongoDbService
        /// </summary>
        /// <param name="mongoDbService"></param>
        public OrderController(MongoDbService mongoDbService)
        {
            // Obtem a coleção "order"
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get()
        {
            try
            {
                var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();
                return Ok(orders);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Order>> GetById(string id)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(o => o.Id, id);
                var order = await _order.Find(filter).FirstOrDefaultAsync();

                if (order == null)
                {
                    return NotFound("Order not found");
                }

                return Ok(order);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Order order)
        {
            try
            {
                await _order.InsertOneAsync(order);
                return StatusCode(201, order);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> Update(string id, [FromBody] Order updatedOrder)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(o => o.Id, id);
                var result = await _order.ReplaceOneAsync(filter, updatedOrder);

                if (result.MatchedCount == 0)
                {
                    return NotFound("Order not found");
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
                var filter = Builders<Order>.Filter.Eq(o => o.Id, id);
                var result = await _order.DeleteOneAsync(filter);

                if (result.DeletedCount == 0)
                {
                    return NotFound("Order not found");
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

