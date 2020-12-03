namespace Couchy
{
    public interface ICredProvider
    {
        string Username { get; }
        string Password { get; }
    }
}