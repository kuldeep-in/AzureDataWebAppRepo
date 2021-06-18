
namespace AppCustomerDemo.Models
{
    using Newtonsoft.Json;
    using System;

    public class Order
    {
        [JsonProperty(PropertyName = "orderId")]
        public Int64 OrderId { get; set; }

        [JsonProperty(PropertyName = "orderDesc")]
        public string OrderDesc { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public Int64 Quantity { get; set; }

        [JsonProperty(PropertyName = "orderValue")]
        public Int64 OrderValue { get; set; }
    }
}
