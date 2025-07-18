namespace HungrAPI.Configuration;

public class JwtConfiguration
{
    public required string Secret { get; set; }
    
    public required string Audience { get; set; }
    
    public required string Issuer { get; set; }

    public required int Clockskew { get; set; }
}