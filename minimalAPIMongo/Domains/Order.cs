using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPIMongo.Domains
{
    public class Order
    {
        // Define que esta propriedade é o Id do objeto
        [BsonId]
        // Define o nome do campo no MongoDb como "_id" e o tipo como "ObjectId"
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        // Referência aos produtos do pedido
        [BsonElement("products")]
        public List<string> ProductIds { get; set; }

        // Referência ao cliente que fez o pedido
        [BsonElement("clientId"), BsonRepresentation(BsonType.ObjectId)]
        public string ClientId { get; set; }

        /// <summary>
        /// Ao ser instanciado um objeto da classe Order, o atributo ProductIds já virá com uma nova lista e, portanto, habilitado para adicionar mais produtos
        /// </summary>
        public Order()
        {
            ProductIds = new List<string>();
        }
    }
}

