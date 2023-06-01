using System.ComponentModel.DataAnnotations;

namespace VisionRestaurant.Model
{
    public class CheckoutItems
    {
        [Key, Required]
        public int ID { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }

    }
}
