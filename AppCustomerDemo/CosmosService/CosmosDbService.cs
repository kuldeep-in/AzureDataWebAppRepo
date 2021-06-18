
namespace AppCustomerDemo.CosmosService
{
    using Microsoft.Azure.Cosmos;
    using AppCustomerDemo.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(Item item)
        {
            await this._container.CreateItemAsync<Item>(item, new PartitionKey(item.Category));
        }

        public async Task DeleteItemAsync(string id)
        {
            string partitionKey = await this.GetPartitionKey(id);
            await this._container.DeleteItemAsync<Item>(id, new PartitionKey(partitionKey));
        }

        public async Task<Item> GetItemAsync(string id)
        {
            try
            {
                string partitionKey = await this.GetPartitionKey(id);
                ItemResponse<Item> response = await this._container.ReadItemAsync<Item>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Item>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Item>(new QueryDefinition(queryString));
            List<Item> results = new List<Item>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, Item item)
        {
            await this._container.UpsertItemAsync<Item>(item, new PartitionKey(item.Category));
        }

        public async Task<string> GetPartitionKey(string id)
        {
            string queryString = string.Format("SELECT * FROM c where c.id = '{0}'", id);
            var result = this._container.GetItemQueryIterator<Item>(new QueryDefinition(queryString));
            List<Item> results = new List<Item>();
            while (result.HasMoreResults && results.Count() == 0)
            {
                var response = await result.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results.FirstOrDefault().Category;
        }
    }
}