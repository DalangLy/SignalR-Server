using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalR_Server.Hubs
{
    public class AnotherSignalRTestHub : Hub
    {
        public void AnotherHello()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SignalRTestHub>();
            context.Clients.All.anotherSayHello("Another Say Hello");
        }
    }
}