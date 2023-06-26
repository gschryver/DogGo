using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Dog
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a name for the dog.")]
        [MaxLength(35, ErrorMessage = "The name cannot exceed 35 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please select an owner.")]
        [DisplayName("Owner")]
        public int OwnerId { get; set; }
        [Required]
        public string Breed { get; set; }
        [Required]
        public string Notes { get; set; }
        [Required]
        [Display(Name="Dog's Image URL")]
        [DataType(DataType.ImageUrl, ErrorMessage = "Please enter a valid Image URL.")]
        public string ImageUrl { get; set; }
        public Owner Owner { get; set; }
    }
}
