using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class OrderDao
    {
        OnlShopCNPM db = null;
        public OrderDao()
        {
            db = new OnlShopCNPM();
        }
        public Guid Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;
        }
        public Order FindByID(Guid orderID)
        {
            return db.Orders.Find(orderID);
        }
        public bool ChangeStatusFirst(Guid orderID)
        {
            var order = db.Orders.Find(orderID);
            if(order.Status == "Order")
            {
                order.Status = "Shipped";
                if(!order.ChangeStatus.Value)
                {
                    order.ChangeStatus = true;
                    order.ChangeDate = DateTime.Now;
                    var inc = new UserDao().ModifyMissing(order.CustomerID, 1);
                }
            }
            db.SaveChanges();
            return true;
        }
        public bool ChangeStatusSecond(Guid orderID)
        {
            var order = db.Orders.Find(orderID);
            if (order.Status == "Shipped")
            {
                order.Status = "Complete";
                order.ShippedDate = DateTime.Now;
                if (!order.ChangeStatus.Value)
                {
                    order.ChangeStatus = true;
                    order.ChangeDate = DateTime.Now;
                    var inc = new UserDao().ModifyMissing(order.CustomerID, 1);
                }
            }
            db.SaveChanges();
            return true;
        }
        public List<Order> ListByUser(Guid userID)
        {
            var model = db.Orders.Where(x => x.CustomerID == userID).OrderBy(x => x.ChangeDate).ToList();
            return model;
        }
        public List<Order> ListCompleteOrderByCustomer(Guid userID)
        {
            var model = db.Orders.Where(x => x.CustomerID == userID &&  x.Status == "Complete").OrderBy(x => x.ChangeDate).ToList();
            return model;
        }
        public bool SwitchStatus(Guid orderID)
        {
            var order = db.Orders.Find(orderID);
            if (order.ChangeStatus.Value)
            {
                order.ChangeStatus = false;
                var inc = new UserDao().ModifyMissing(order.CustomerID, 2);
            }
            db.SaveChanges();
            return true;
        }
    }
}
