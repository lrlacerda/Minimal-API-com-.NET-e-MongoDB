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
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// Armazena os dados de acesso da collection
        /// </summary>
        private readonly IMongoCollection<Product> _product;
        /// <summary>
        /// Construtor que recebe como dependência o obj da classe MongoDbService
        /// </summary>
        /// <param name="mongoDbService"></param>
        public ProductController(MongoDbService mongoDbService)
        {
            //obtem a colection "product"
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                var products = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();
                return Ok(products);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Product>> GetById(string id)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
                var product = await _product.Find(filter).FirstOrDefaultAsync();

                if (product == null)
                {
                    return NotFound("Product not found");
                }

                return Ok(product);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Product product)
        {
            try
            {
                await _product.InsertOneAsync(product);
                return StatusCode(201, product);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> Update(string id, [FromBody] Product updatedProduct)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
                var result = await _product.ReplaceOneAsync(filter, updatedProduct);

                if (result.MatchedCount == 0)
                {
                    return NotFound("Product not found");
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
                var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
                var result = await _product.DeleteOneAsync(filter);

                if (result.DeletedCount == 0)
                {
                    return NotFound("Product not found");
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
