using ChatApp.Models;

namespace ChatApp.Data;

public static class ClientSource
{
    public static List<Client> Clients { get; } = new List<Client>();
    public static List<Message> messageHistory { get; } = new List<Message>();
}