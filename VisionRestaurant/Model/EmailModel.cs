using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace VisionRestaurant.Model
{
    public class EmailModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public string Subject { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string Body { get; set; }
    }
}
