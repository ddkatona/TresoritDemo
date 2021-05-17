using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace TresoritDemo
{
    class Service : IService
    {
        private TableQueries tableQueries = new TableQueries();

        public List<CommentJson> GetComments(string product, int limit)
        {
            // Fetching comments from database
            if (limit < 1) limit = 1;
            Task<List<CommentDB>> task = tableQueries.GetComments(product, limit);
            task.Wait();
            List<CommentDB> dbLinks = task.Result;
            if (dbLinks.Count == 0) throw new WebFaultException<string>("Product doesn't exist!", HttpStatusCode.NotFound);
            return dbLinks.ConvertAll(le => new CommentJson(le));
        }

        public void PostComment(CommentJson comment)
        {
            if (comment.Comment == null) throw new WebFaultException<string>("You must specify a comment!", HttpStatusCode.BadRequest);
            if (comment.Comment.Length > 500) throw new WebFaultException<string>("Comment must exist and can be no longer than 500 characters!", HttpStatusCode.BadRequest);
            string expectedID = tableQueries.GetLastCommentID(comment.Product);
            if(expectedID != null && !expectedID.Equals(comment.CommentID)) throw new WebFaultException<string>("CommentID doesn't match last comment's ID!", HttpStatusCode.Forbidden);
            tableQueries.AutoInsert(comment.Product, comment.Comment, comment.Username);
        }
    }
}
