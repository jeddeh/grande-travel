using GrandeTravel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandeTravel.Service
{
    public interface IOrderService
    {
        Result<Order> GetOrderById(int id);
        Result<IEnumerable<Order>> GetAllOrders();
        Result<IEnumerable<Order>> GetOrdersByCustomerId(int customerId);
        Result<Order> AddOrder(Order order);
        ResultEnum UpdateOrder(Order order);
    }
}
