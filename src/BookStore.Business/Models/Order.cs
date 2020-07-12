using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Business.Models
{
    public class Order
    {
        public long Id { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public int TotalItemCount { get; set; }
    }
}
