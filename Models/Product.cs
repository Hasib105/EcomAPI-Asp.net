using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomApi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Slug { get; set; }

        public string Description { get; set; }
        [Required]
        [MaxLength(300)]
        public string Summary { get; set; }
        
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
            return name.ToLower().Replace(" ", "-");
        }
    }
}