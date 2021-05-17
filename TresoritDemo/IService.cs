using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace TresoritDemo
{
    [ServiceContract]
    interface IService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        List<CommentJson> GetComments(string product, int limit);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json)]
        void PostComment(CommentJson comment);
    }
}
