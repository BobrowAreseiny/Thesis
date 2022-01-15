using System;

namespace Thesis.Data.Model
{
    public class OrderData
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; }

        public string Counterparty { get; set; }

        public string Status { get; set; }

        public DateTime? DateOfShipment { get; set; }

        public int OrderConstructionCount { get; set; }

        public int OrderConstructionMade { get; set; }

        public OrderData(UserOrder order, int count, int made)
        {
            Id = order.Id;
            OrderNumber = order.OrderNumber;
            Counterparty = order.Counterparty.Name;
            DateOfShipment = order.DateOfShipment;
            Status = order.Status;
            OrderConstructionCount = count;
            OrderConstructionMade = made;

            if (OrderConstructionMade == OrderConstructionCount)
            {
                Status = "Готов";
            }
        }
    }
}
