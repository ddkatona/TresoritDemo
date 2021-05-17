using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace TresoritDemo
{
    public class TableQueries
    {
        private CloudTable commentTable;

        public TableQueries()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            commentTable = tableClient.GetTableReference("Comments");
        }

        // Initialize database with dummy data
        public void DummyInit()
        {
            bool tableCreatedNow = commentTable.CreateIfNotExists();
            if (tableCreatedNow)
            {
                TableQueries tableQueries = new TableQueries();
                tableQueries.AutoInsert("Laptop", "It has great battery life.", "Adam");
                tableQueries.AutoInsert("Laptop", "Poor build quality", "John");
                tableQueries.AutoInsert("Laptop", "At 2.5kg it's way to heavy to carry around", "Madeline");
                tableQueries.AutoInsert("Lightsaber", "It's heating up too much", "Mia");
                tableQueries.AutoInsert("Apple", "Too sour :(", "Sally");
                tableQueries.AutoInsert("Apple", "Tasty", "Ben");
                tableQueries.AutoInsert("Bicycle", "ideal for long trips", "Anna");
                tableQueries.AutoInsert("Bicycle", "After a month of daily use, the gear shif broke. Don't buy it!", "Abraham");
            }            
        }

        private async Task<bool> InsertComment(string _product, string _commentID, string _comment, string _username)
        {
            CommentDB dbLink = new CommentDB(_product, _commentID, _comment, _username);
            TableOperation insertOperation = TableOperation.InsertOrMerge(dbLink);
            try
            {
                await commentTable.ExecuteAsync(insertOperation);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void AutoInsert(string _product, string _comment, string username)
        {
            string newID = Guid.NewGuid().ToString();
            Task<bool> bLinkCreated = InsertComment(_product, newID, _comment, username);
            bLinkCreated.Wait();
        }

        public async Task<List<CommentDB>> GetComments(string product, int limit)
        {
            TableQuery<CommentDB> query = new TableQuery<CommentDB>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, product));

            query.TakeCount = 5;

            TableContinuationToken token = null;
            List<CommentDB> comments = new List<CommentDB>();
            do
            {
                TableQuerySegment<CommentDB> resultSegment = await commentTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                comments.AddRange(resultSegment.Results);
                /*foreach (CommentDB entity in resultSegment.Results)
                {
                    comments.Add(entity);
                }*/
            } while (token != null);
            comments.Sort((cmt1, cmt2) => cmt1.Timestamp < cmt2.Timestamp ? 1 : -1);
            //Console.WriteLine(comments.Count);
            return comments.GetRange(0, Math.Min(limit, comments.Count));
        }

        public string GetLastCommentID(string product)
        {
            Task<List<CommentDB>> task = GetComments(product, 1);
            task.Wait();
            List<CommentDB> comments = task.Result;
            if (comments.Count == 0) return null;
            return comments[0].RowKey;
        }

    }
}
