namespace api.Services
{
    public interface ITokenService
    {
        string GenerateToken(string userId, IDictionary<string, string>? additionalClaims = null);
    }
}