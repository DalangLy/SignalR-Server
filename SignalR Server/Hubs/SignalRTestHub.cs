using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalR_Server.Hubs
{
    public class SignalRTestHub : Hub
    {
        public void Hello()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SignalRTestHub>();
            context.Clients.All.sayHello("Hello");
        }
    }
}