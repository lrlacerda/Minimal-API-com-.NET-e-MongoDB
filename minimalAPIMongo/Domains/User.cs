using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPIMongo.Domains
{
    public class User
    {
        // Define que esta propriedade é o Id do objeto
        [BsonId]
        // Define o nome do campo no MongoDb como "_id" e o tipo como "ObjectId"
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        // Adiciona um dicionário para atributos adicionais
        public Dictionary<string, string> AdditionalAttributes { get; set; }

        /// <summary>
        /// Ao ser instanciado um objeto da classe User, o atributo AdditionalAttributes já virá com um novo dicionário e, portanto, habilitado para adicionar mais atributos
        /// </summary>
        public User()
        {
            AdditionalAttributes = new Dictionary<string, string>();
        }
    }
}

