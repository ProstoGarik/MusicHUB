namespace WebApplication1
{
    public interface IChatClient
    {
        Task RecieveMessage(string message);
    }
}
