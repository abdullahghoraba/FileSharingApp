using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Hubs
{
    // this is mean we can't send message with out login 
    // [Authorize]
    public class NotificationHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            Context.Items.Add(Context.UserIdentifier, Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Context.Items.Remove(Context.UserIdentifier);
            return base.OnDisconnectedAsync(exception);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
