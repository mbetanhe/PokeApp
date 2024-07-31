namespace PokeApp.Core.Application.Responses
{
    public class AuthenticationResponse
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string Id { get; set; }
        public string Token { get; set; }
    }
}
