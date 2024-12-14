using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EcomApi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "CategoryId is required.")]
        public int CategoryId { get; set; }



        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
        public string Name { get; set; }

        [MaxLength(200, ErrorMessage = "Slug cannot exceed 200 characters.")]
        public string Slug { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Summary is required.")]
        [MaxLength(300, ErrorMessage = "Summary cannot exceed 300 characters.")]
        public string Summary { get; set; }
        
        [Required(ErrorMessage = "Price is required.")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public bool Available { get; set; } = true;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public bool Featured { get; set; } = false;
        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        public override string ToString() => Name;

        public void GenerateSlug()
        {
            Slug = $"{Slugify(Name)}-{Created:yyyy-MM-dd-HH-mm-ss}";
        }

        private string Slugify(string name)
        {
            return name
                .ToLower()
                .Replace(" ", "-")
                .Replace("--", "-")
                .Trim('-');
        }
    }
}
