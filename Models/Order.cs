using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomApi.Models
{
    public class Order
    {
        public enum OrderStatus
        {
            Pending,
            Canceled,
            Completed
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool Seen { get; set; } = false;

        [ForeignKey("DeliveryChargeId")]
        public int? DeliveryChargeId { get; set; }
        public virtual DeliveryCharge DeliveryCharge { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        public override string ToString() => $"Order #{Id}: {Name}";

        public decimal GetTotalCost() => CalculateTotalCost();

        private decimal CalculateTotalCost()
        {
            decimal totalCost = 0;
            foreach (var item in CartItems)
            {
                totalCost += item.TotalCost();
            }
            return totalCost;
        }

        public decimal GetGrandTotal()
        {
            var totalCost = GetTotalCost();
            if (DeliveryCharge != null)
            {
                totalCost += DeliveryCharge.Price;
            }
            return totalCost;
        }
    }
}