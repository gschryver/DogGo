using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Walker
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the walker's name.")]
        [MaxLength(50, ErrorMessage = "The name cannot exceed 50 characters.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select a neighborhood.")]
        [Display(Name = "Neighborhood")]
        public int NeighborhoodId { get; set; }

        [Display(Name = "Image URL")]
        [DataType(DataType.ImageUrl, ErrorMessage = "Please enter a valid image URL.")]
        public string ImageUrl { get; set; }

        public Neighborhood Neighborhood { get; set; }
    }
}
