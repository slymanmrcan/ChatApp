using ChatApp.Abstract;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs;

public class HubwithInterfaces:Hub<IChatHub>
{
    static List<string> clients = new List<string>();
    
    public async Task SendMessage( string message)
    {
        // await Clients.All.SendAsync("ReceiveMessage", message);
        await Clients.All.ReceivedMessage(message);
    }
    public override async Task OnConnectedAsync()
    {
        clients.Add(Context.ConnectionId);
        // await Clients.All.SendAsync("clients", clients);
        await Clients.All.Clients(clients);
        // await Clients.All.SendAsync("UserJoined",Context.ConnectionId);
        await Clients.All.UserJoined(Context.ConnectionId);
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        clients.Remove(Context.ConnectionId);
        // await Clients.All.SendAsync("clients", clients);
        await Clients.All.Clients(clients);

        // await  Clients.All.SendAsync("UserLeft", Context.ConnectionId);
        await Clients.All.UserLeft(Context.ConnectionId);
    }
}