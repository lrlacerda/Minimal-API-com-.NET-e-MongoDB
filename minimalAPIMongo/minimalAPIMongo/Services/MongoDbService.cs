using MongoDB.Driver;

namespace minimalAPIMongo.Services
{
    public class MongoDbService
    {
        /// <summary>
        /// Armazena a configuração da aplicação
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Armazena uma referência ao MongoDb
        /// </summary>
        private readonly IMongoDatabase _database;

        /// <summary>
        /// recebe a config da aplicação como parâmetro
        /// </summary>
        /// <param name="configuration">objeto configuration</param>
        public MongoDbService(IConfiguration configuration)
        {
            //atribui a config recebida em _configuration
            _configuration = configuration;

            //obtem a string de conexão através do _configuration
            var connectionString = _configuration.GetConnectionString("DbConnection");

            //cria um objeto MongoUrl que recebe como parâmetro a string de conexão
            var mongoUrl = MongoUrl.Create(connectionString);

            //cria um MongoCliente para se conectra ao MongoDb
            var mongoClient = new MongoClient(mongoUrl);

            //obtem a referência ao banco com o nome especificado na string de conexão
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        /// <summary>
        /// Propriedade para acessar o banco de dados
        /// Retorna a referência ao bd
        /// </summary>
        public IMongoDatabase GetDatabase => _database;
    }
}
