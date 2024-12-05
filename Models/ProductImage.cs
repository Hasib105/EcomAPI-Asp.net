using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomApi.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public string Image { get; set; }
        [MaxLength(255)]
        public string AltText { get; set; }

        public override string ToString() => $"{Product.Name} - Image {Id}";
    }
}