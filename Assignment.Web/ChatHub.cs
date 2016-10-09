using System;
using System.Globalization;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Assignment.Web
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        public void SendMessage(string login, string message)
        {
            Clients.All.broadcastMessage(login, message, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss",
                CultureInfo.InvariantCulture));
        }
    }
}
