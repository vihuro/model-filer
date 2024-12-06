namespace ModelFilter.Application.Interface
{
    public interface IJwtService
    {
        (string acessToken, string refreshToken) Token(string name, string userName, string userId);
    }
}
