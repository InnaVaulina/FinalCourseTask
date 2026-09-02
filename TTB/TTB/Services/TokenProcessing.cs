namespace TTB.Services
{
    public delegate void TokenProcessingStateChangedHandler(string? token);
    public class TokenProcessing
    {
        public event TokenProcessingStateChangedHandler? TokenChanged;

        private const string tokenKey = "authToken";
        public string TokenKey { get { return tokenKey; } }

        public void SetToken(string token)
        {
            TokenChanged?.Invoke(token);
        }

        public void RemoveToken()
        {
            TokenChanged?.Invoke(null);
        }
    }
}
