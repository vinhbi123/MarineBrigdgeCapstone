using ShipCapstone.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipCapstone.Domain.Models.Orders
{
    public class CreateOrderResponse
    {
        public Guid Id { get; set; }
        public Guid ShipId { get; set; }
        public decimal TotalAmount { get; set; }
        public EOrderStatus Status { get; set; }
    }
}
