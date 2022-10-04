using partner_aluro.Core.Repositories;
using System;
using System.Collections.Generic;

namespace partner_aluro.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
        public int OrderTotal { get; set; }
        public DateTime OrderPlaced { get; set; }

        public string? MessageToOrder { get; set; }

        public string? UserID { get; set; }
        public StanZamowienia StanZamowienia { get; set; }
    
    }
    public enum StanZamowienia
    {
        Nowe,
        Zrealizowane
    }
}