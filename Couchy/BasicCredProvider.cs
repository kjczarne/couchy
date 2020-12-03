namespace Couchy
{
    public class BasicCredProvider : ICredProvider
    {
        private string _username;
        public string Username { get => _username; }
        private string _password;
        public string Password { get => _password; }

        public BasicCredProvider(
            string username,
            string password
        ) {
            _username = username;
            _password = password;    
        }
    }
}