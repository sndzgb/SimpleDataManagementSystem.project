namespace SimpleDataManagementSystem.Backend.WebAPI.Options
{
    public class JwtOptions
    {
        /// <summary>
        /// Section name.
        /// </summary>
        public const string Jwt = "JwtOptions";

        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int ExpiresInMinutes { get; set; }
    }
}
