using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ReviewDao
    {
        OnlShopCNPM db = null;
        public ReviewDao()
        {
            db = new OnlShopCNPM();
        }
        public bool AddReview(Review rew)
        {
            db.Reviews.Add(rew);
            db.SaveChanges();
            return true;
        }
        public List<Review> ListReviewByProductCode(int productCode)
        {
            return db.Reviews.Where(x => x.ProductCode == productCode).OrderBy(x => x.CreatedDate).ToList();
        }
        public List<Review> ListReview()
        {
            return db.Reviews.OrderByDescending(x => x.CreatedDate).Take(3).ToList();
        }
    }
}
