using COL.XMEN.Core.Configs;
using COL.XMEN.Core.Domain;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace COL.XMEN.Infraestructure.CosmosDB
{
    public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : Entity
    {
        private bool _disposed = false;
        private Container _container;
        private readonly CosmosClient _cosmosClient;
        private const string QueryString = "SELECT * FROM c";

        public Repository(IOptions<CosmosDBConfig> cosmosDbOptions, IConfiguration configuration)
        {
            Contract.Requires(cosmosDbOptions != null);
            Contract.Requires(cosmosDbOptions.Value != null);
            Contract.Requires(configuration != null);
            var config = cosmosDbOptions.Value;
            var cosmosDbKey = configuration["cosmosdbkey"];

            var cosmosClientOptions = new CosmosClientOptions() { ConnectionMode = ConnectionMode.Gateway };
            _cosmosClient = new CosmosClient(config.Account, cosmosDbKey, cosmosClientOptions);
            Initialize(_cosmosClient, config).Wait();
        }

        public Repository(Container container)
        {
            _container = container;
        }

        private async Task Initialize(CosmosClient cosmosClient, CosmosDBConfig configuration)
        {
            DatabaseResponse databaseResponse = await cosmosClient
                                   .CreateDatabaseIfNotExistsAsync(configuration.DatabaseName)
                                   .ConfigureAwait(false);
            _container = await databaseResponse.Database.CreateContainerIfNotExistsAsync(configuration.Containers[typeof(TEntity).Name], "/id")
                                   .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>> Where(string conditional)
        {
            var query = _container.GetItemQueryIterator<TEntity>(new QueryDefinition(QueryString + (!string.IsNullOrEmpty(conditional) ? $" WHERE c.{ conditional }" : "")));
            List<TEntity> results = new List<TEntity>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync().ConfigureAwait(false);
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task<TEntity> AddAsync(TEntity item)
        {
            return await _container.CreateItemAsync<TEntity>(item, new PartitionKey(item?.Id.ToString())).ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _cosmosClient?.Dispose();
            }

            _disposed = true;
        }
    }
}
