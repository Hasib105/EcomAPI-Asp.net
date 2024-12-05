using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomApi.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Slug { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;

        public string CategoryImage { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public override string ToString() => Name;

        public void GenerateSlug()
        {
            Slug = $"{Slugify(Name)}-{Created:yyyy-MM-dd-HH-mm-ss}";
        }

        private bool SlugExists(string slug)
        {
            // Implement your logic to check if slug exists in the database
            return false;
        }

        private string Slugify(string name)
        {
            // Implement your slugify logic here
            return name.ToLower().Replace(" ", "-");
        }
    }
}