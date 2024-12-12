using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs;

public class ChatHubBusiness
{
    readonly IHubContext<ChatHub> _hubContext;

    public ChatHubBusiness(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }
    // public async Task GetNickName(string nickName)
    // {
    //     Client client = new Client
    //     {
    //         ConnectionId = Context.ConnectionId,
    //         NickName = nickName
    //     };
    //     ClientSource.Clients.Add(client);
    //     Clients.Others.SendAsync("clientJoined", nickName);
    // }
}