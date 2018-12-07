namespace Common
{
    public class IdentitySetting
    {
        public string IdentityServerUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecrets { get; set; }
        public string Scopes { get; set; }
        public string SignInScheme { get; set; }
        public string AuthenticationScheme { get; set; }
        public string ApiName { get; set; }
        public bool RequireHttpsMetadata { get; set; }
        public string ResponseType { get; set; }

        public IdentitySetting()
        {
            ResponseType = "code id_token";
            RequireHttpsMetadata = false;
            SignInScheme = "Cookies";
            AuthenticationScheme = "Bearer";
        }
    }
}
