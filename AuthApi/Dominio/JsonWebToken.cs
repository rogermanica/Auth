namespace AuthApi.Dominio
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; } = "bearer";
        public long ExpiresIn { get; set; }
    }
}