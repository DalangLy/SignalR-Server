using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SignalR_Server.Hubs
{
    [Authorize] //must be authorize to get current logged in user info
    public class SignalRTestHub : Hub
    {
        public override Task OnConnected()
        {
            if (Context.User.Identity.IsAuthenticated)//check if logged in or not
            {
                string userEmail = Context.User.Identity.Name;//must be authorize to get username
                string connectionId = Context.ConnectionId;

                //add connection id and logged email to idictionary for easy finding connection id by email
                SignalRConnectedUserModel.allConnectedUsers.Add(connectionId, userEmail);
            }
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            //remove disconnected user from idictionary
            SignalRConnectedUserModel.allConnectedUsers.Remove(Context.ConnectionId);

            this.BroadcastConnectionUsers();

            return base.OnDisconnected(stopCalled);
        }
        public override Task OnReconnected()
        {
            //no action yet
            return base.OnReconnected();
        }


        //invoke function (for client side calling)
        public string getCurrentConnectedModel()//this function will invoke from client when they start connect to signalr server to get thier own connection id and email
        {
            this.BroadcastConnectionUsers();

            return this.convertIDictionaryToJsonString(SignalRConnectedUserModel.allConnectedUsers);
        }
        //invoke function (for client side calling)
        public void sendMessage(string targetConnectiondId, string senderMessage)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SignalRTestHub>();
            context.Clients.Client(targetConnectiondId).listenMessage(senderMessage);
        }

        //broadcast hello message
        public void Hello()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SignalRTestHub>();
            context.Clients.All.sayHello("Hello");
        }
        public void BroadcastConnectionUsers()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SignalRTestHub>();
            context.Clients.All.allConnectedUsers(this.convertIDictionaryToJsonString(SignalRConnectedUserModel.allConnectedUsers));
        }

        private string convertIDictionaryToJsonString(IDictionary<string, string> idcitionaryData)
        {
            List<ConnectedUserModel> cum = new List<ConnectedUserModel>();//create new list of model to store connected users

            foreach (KeyValuePair<string, string> connectedUser in idcitionaryData)
            {
                //loop through idictionary of connected user to pass to list
                cum.Add(new ConnectedUserModel(connectedUser.Key, connectedUser.Value));
            }

            //convert list of model to json string
            var json = JsonConvert.SerializeObject(cum, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
            return json;
        }
    }

    class ConnectedUserModel
    {
        public string connectionId { get; set; }
        public string userEmail { set; get; }

        public ConnectedUserModel(string key, string value)
        {
            this.connectionId = key;
            this.userEmail = value;
        }
    }

    public static class SignalRConnectedUserModel
    {
        public static IDictionary<string, string> allConnectedUsers = new Dictionary<string, string>();
    }
}