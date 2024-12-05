using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomApi.Models
{
    public class DeliveryCharge
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [MaxLength(555)]
        public string Description { get; set; }

        public override string ToString() => $"Delivery Charge of {Price}";
    }
}
