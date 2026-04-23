namespace QLBH.API.Services
{
    public interface ITokenService
    {
        string CreateToken(string id, string username, string name, string role);
    }
}
