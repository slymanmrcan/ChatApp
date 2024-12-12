namespace ChatApp.Models;

public class Message
{
    public string Text { get; set; }
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public DateTime Date { get; set; }
    
}