using System.ComponentModel.DataAnnotations;

namespace VisionRestaurant.Model
{
    public class OrderItem
    {
        [Required]
        public int OrderNo { get; set; }
        [Required]
        public int ID { get; set; }
        [Required]
        public int Quantity { get; set; }

    }
}
