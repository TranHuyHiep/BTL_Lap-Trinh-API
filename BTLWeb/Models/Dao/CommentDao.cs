using BTLWeb.Models.ViewModels;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
namespace BTLWeb.Models.Dao
{
    public class CommentDao
    {
        QlbanMayAnhContext db = new QlbanMayAnhContext();   
        public Boolean Insert(Comment entity)
        {
            db.TComments.Add(entity);
            db.SaveChanges();
            return true;
        }
        public List<Comment>  ListComment(long parentid , string productID) {
            return db.TComments.Where(x =>  x.ProductID == productID).ToList();
        }
        //public List<CommentViewModal> commentViewModals(long parentid, string productID)
        //{
        //    var model = (from a in db.TComments
        //                 where a.ParentID == parentid && a.ProductID == productID
        //                 select new
        //                 {
        //                     ID = a.ID,
        //                     CommentMsg = a.CommentMsg,
        //                     CommentDate = a.CommentDate,
        //                     ProductID = a.ProductID,
        //                     UserId = a.UserId,
        //                     FullName = a.FullName,
        //                     ParentID = a.ParentID
        //                 }
        //                 ).AsEnumerable().Select(x => new CommentViewModal()
        //                 {
        //                     ID = x.ID,
        //                     CommentMsg = x.CommentMsg,
        //                     CommentDate = x.CommentDate,
        //                     ProductID = x.ProductID,
        //                     UserId = x.UserId,
        //                     FullName = x.FullName,
        //                     ParentID = x.ParentID
        //                 });
        //    return model.OrderByDescending(y => y.ID).ToList();
        //}
    }
}
