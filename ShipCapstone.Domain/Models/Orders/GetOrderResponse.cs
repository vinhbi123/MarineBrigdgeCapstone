using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipCapstone.Domain.Enums;


namespace ShipCapstone.Domain.Models.Orders
{
    public class GetOrderResponse
    {
        public Guid Id { get; set; }
        public Guid? ShipId { get; set; }
        public string? ShipName { get; set; }
        public Guid? BoatyardId { get; set; }
        public string? BoatyardName { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public string? Phone {get; set;}
        public decimal TotalAmount { get; set; }
        public EOrderStatus Status { get; set; }
        public List<GetOrderItemsResponse>? OrderItems { get; set; }
    }
    
    public class GetOrderItemsResponse
    {
        public Guid Id { get; set; }
        public Guid? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public Guid ProductVariantId { get; set; }
        public string? ProductVariantName { get; set; }
        public string? ProductOptionName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
