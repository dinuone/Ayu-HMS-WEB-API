using System.Text.Json.Serialization;

namespace BookInventory.DTOs.Request.Product
{
    public class CreateProductRequest
    {
        [JsonPropertyName("bar_code")]
        public string BarCode { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        
    }
}
