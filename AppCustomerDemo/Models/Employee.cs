
namespace AppCustomerDemo.Models
{
    using Microsoft.WindowsAzure.Storage.Table;
    using Newtonsoft.Json;

    public class Employee : TableEntity
    {

        [JsonProperty(PropertyName = "employeeid")]
        public int Employeeid { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

    }
}