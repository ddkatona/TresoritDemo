using System.Runtime.Serialization;

namespace TresoritDemo
{
    [DataContract]
    public class CommentJson
    {
        [DataMember]
        public string CommentID { get; set; }
        [DataMember]
        public string Product { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public string DateTime { get; set; }

        public CommentJson(CommentDB le)
        {
            CommentID = le.RowKey;
            Product = le.PartitionKey;
            Username = le.Username;
            Comment = le.Comment;
            DateTime = le.Timestamp.ToString();
        }
    }
}
