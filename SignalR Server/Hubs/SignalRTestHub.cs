using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace SignalR_Server.Hubs
{
    [Authorize] //must be authorize to get user name
    public class SignalRTestHub : Hub
    {
        public override Task OnConnected()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                string userName = Context.User.Identity.Name;//must be authorize to get username
                string connectionId = Context.ConnectionId;

                ConnectedUser.allConnectedUsers.Add(connectionId, userName);
            }
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            ConnectedUser.allConnectedUsers.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
        public override Task OnReconnected()
        {
            return base.OnReconnected();
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
            context.Clients.All.allConnectedUsers(ConnectedUser.allConnectedUsers);
        }
    }

    public static class ConnectedUser
    {
        public static IDictionary<string, string> allConnectedUsers = new Dictionary<string, string>();
    }
}