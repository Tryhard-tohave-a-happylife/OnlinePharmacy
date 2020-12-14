using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class OrderDetailDao
    {
        OnlShopCNPM db = null;
        public OrderDetailDao()
        {
            db = new OnlShopCNPM();
        }
        public bool Insert(OrderDetail detail)
        {
            try
            {
                db.OrderDetails.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }    
        public List<OrderDetail> ListByID(Guid orderId)
        {
            var model = db.OrderDetails.Where(x => x.OrderID == orderId).ToList();
            return model;
        }
    }
}
