using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs;

public class ChatHub:Hub
{
    public async Task GetNickName(string nickName)
    {
        Client client = new Client
        {
            ConnectionId = Context.ConnectionId,
            NickName = nickName
        };
        ClientSource.Clients.Add(client);
        await Clients.Others.SendAsync("clientJoined", nickName);
        await Clients.All.SendAsync("clients", ClientSource.Clients);
    }
    public async Task SendMessageAllClient(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
    public async Task SendMessageClientAsync(string message, string clientName)
    {
        clientName = clientName.Trim();
        
        Client client =  ClientSource.Clients.FirstOrDefault(x => x.NickName == clientName.Replace(" {çevrimiçi}",""));
        Client senderClient = ClientSource.Clients.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
        var newmessage = new Message
        {
            Text = message,
            Sender = client.ConnectionId,
            Receiver = clientName,
            Date = DateTime.Now
        };
        ClientSource.messageHistory.Add(newmessage);
        await Clients.Client(client.ConnectionId).SendAsync("receiveclientmessage", message,senderClient.NickName);
    }
    public override async Task OnConnectedAsync()
    {
        await Clients.Others.SendAsync("ReceiveConnectionId",Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {  
        // ConnectionId'ye göre client'ı bul ve listeden sil
        var disconnectedClient = ClientSource.Clients.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
        if (disconnectedClient != null)
        {
            ClientSource.Clients.Remove(disconnectedClient);
        
            // Diğer kullanıcılara bildir
            await Clients.Others.SendAsync("clientDisconnected", disconnectedClient.NickName);
        
            // Güncel client listesini herkese gönder
            await Clients.All.SendAsync("clients", ClientSource.Clients);
        }
    }
}