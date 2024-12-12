namespace ChatApp.Abstract;

public interface IChatHub
{
    Task Clients(List<string> clients);
    Task UserJoined(string userId);
    Task UserLeft(string userId);
    Task UserReceived(string userId, string message);
    Task ReceivedMessage(string message);
    Task SendMessage(string message);
    Task GetNickName(string userId);
}