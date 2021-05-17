using Microsoft.WindowsAzure.Storage.Table;

namespace TresoritDemo
{
    public class CommentDB : TableEntity
    {
        public CommentDB(string product, string commentID, string comment, string username)
        {
            PartitionKey = product;
            RowKey = commentID;
            Comment = comment;
            Username = username;
        }

        public CommentDB() { }

        public string Username { get; set; }

        public string Comment { get; set; }
    }
}
