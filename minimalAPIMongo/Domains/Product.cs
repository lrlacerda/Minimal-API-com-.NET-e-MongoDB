using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPIMongo.Domains
{
    public class Product
    {
        //define que está propiedade é Id do objeto
        [BsonId] 
        //define o nome do campo no MongoDb como "_id e o tipo como"ObjectId 
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        //adiciona um dicionário para atributos adicionais
        [BsonElement("additionalAttributes")]
        public Dictionary<string, string> AdditionalAttributes { get; set; }

        /// <summary>
        /// Ao ser instânciado um objeto da classe Product, o atributo AdditionAttributes já virá com um novo dicionário e portanto habilitado para adicionar + atributos
        /// </summary>
        public Product() {

            
            AdditionalAttributes = new Dictionary<string, string>(); 
        }
    }
}
