using Microsoft.AspNetCore.SignalR;

namespace SignalRTest
{
    public class LocationTrackerHub : Hub
    {
        public void Send(string repName, double latitude, double longitude)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.InvokeAsync("broadcastLocation", repName, latitude, longitude);
        }
    }
}
