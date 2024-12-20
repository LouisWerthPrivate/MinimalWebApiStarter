namespace APITemplate.Authentication
{

    public class ApiSecurityOptions
    {
        public string? AuthenticationScheme { get; set; }
        public Jwtbeareer? JwtBeareer { get; set; }
        public Apikey? ApiKey { get; set; }
    }

    public class Jwtbeareer
    {
        public string? Authority { get; set; }
        public string? Audience { get; set; }
        public bool RequireHttpsMetadata { get; set; }
    }

    public class Apikey
    {
        public string? Key { get; set; }
        public string? Header { get; set; }
    }

}
