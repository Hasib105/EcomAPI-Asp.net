using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomApi.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public int Quantity { get; set; } = 1;

        public decimal TotalCost() => Quantity * Product.Price;

        public override string ToString() => $"{Product.Name} - Quantity: {Quantity}";
    }
}