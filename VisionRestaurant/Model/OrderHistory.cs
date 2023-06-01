using System.ComponentModel.DataAnnotations;

namespace VisionRestaurant.Model
{
    public class OrderHistory
    {
        [Key, Required]
        public int OrderNo { get; set; }
        [Required]
        public string Email { get; set; }

    }
}
