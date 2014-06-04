using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;
using GrandeTravel.Manager;
using System.Data.Entity;

namespace GrandeTravel.Service.Implementation
{
    internal class OrderService : IOrderService
    {
        // Fields
        private IManager<Order> manager;

        // Constructors
        public OrderService(IManager<Order> manager)
        {
            this.manager = manager;
        }

        // Methods
        #region Add Order

        public Result<Order> AddOrder(Order order)
        {
            Result<Order> result = new Result<Order>();

            try
            {
                result.Data = manager.Create(order);
                result.Status = ResultEnum.Success;
            }
            catch
            {
                result.Status = ResultEnum.Fail;
            }

            return result;
        }

        #endregion

        #region Update Order

        public ResultEnum UpdateOrder(Order order)
        {
            try
            {
                // Get new voucher code
                if (order.Paid && order.VoucherCode == 0)
                {
                    order.VoucherCode = manager.Get(p => true).Select(p => p.VoucherCode).Max() + 1;
                }

                manager.Update(order);
                return ResultEnum.Success;
            }
            catch
            {
                return ResultEnum.Fail;
            }
        }

        #endregion

        #region Get Order By Id

        public Result<Order> GetOrderById(int id)
        {
            Result<Order> result = new Result<Order>();
            try
            {
                result.Data = manager.EagerGet((p => p.OrderId == id), "Package").First();
                result.Status = ResultEnum.Success;
            }
            catch (Exception)
            {
                result.Status = ResultEnum.Fail;
            }

            return result;
        }

        #endregion

        #region Get All Orders

        public Result<IEnumerable<Order>> GetAllOrders()
        {
            Result<IEnumerable<Order>> result = new Result<IEnumerable<Order>>();

            try
            {
                result.Data = manager.Get(p => true).AsEnumerable<Order>();
                result.Status = ResultEnum.Success;
            }
            catch (Exception)
            {
                result.Status = ResultEnum.Fail;
            }

            return result;
        }

        #endregion

        #region Get Orders By Customer Id

        public Result<IEnumerable<Order>> GetOrdersByCustomerId(int customerId)
        {
            Result<IEnumerable<Order>> result = new Result<IEnumerable<Order>>();

            try
            {
                result.Data = manager.EagerGet(p => p.CustomerId == customerId, "Package").OrderBy(p => p.DateBooked)
                .AsEnumerable<Order>();

                result.Status = ResultEnum.Success;
            }
            catch (Exception)
            {
                result.Status = ResultEnum.Fail;
            }

            return result;
        }

        #endregion

        #region Get Feedback By Package Id

        public Result<IEnumerable<string>> GetFeedbackByPackageId(int packageId)
        {
            Result<IEnumerable<string>> result = new Result<IEnumerable<string>>();

            try
            {
                result.Data = manager.Get(p => p.PackageId == packageId).Where(p => p.Feedback != null).Select(p => p.Feedback)
                .AsEnumerable<string>();

                result.Status = ResultEnum.Success;
            }
            catch (Exception)
            {
                result.Status = ResultEnum.Fail;
            }

            return result;
        }

        #endregion
    }
}