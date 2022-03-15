namespace Passman
{
    public class Password
    {
        public int Id { get; set; }
        public string Website { get; set; }
        public string Username { get; set; } = string.Empty;
        public string SecretPassword { get; set; } = string.Empty;

        protected Password() { }
        private Password(string site, string username, string password)
        {
            Website = site;
            Username = username;
            SecretPassword = password;
        }

        public static Password Create(string site, string username, string password)
        {
            return new(site, username, password);
        }
    }
}