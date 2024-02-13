using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Product_API_Version_6.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Sku { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        //This Property is a foregin key and it describe the relationship between the two entities.
        /*
        Product(*) >----------------> Category(1)
        */

        [Required]
        public int CategoryId { get; set; }

        [JsonIgnore]
        public virtual Category? Category { get; set; }
    }
}