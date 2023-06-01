using System.ComponentModel.DataAnnotations;

namespace VisionRestaurant.Data
{
    public class BasketItem
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public int BasketID { get; set; }
        [Required]
        public int Quantity { get; set; }

    }
}
