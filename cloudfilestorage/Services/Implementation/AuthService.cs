using Amazon.Util.Internal;
using cloudfilestorage.Helpers;
using cloudfilestorage.Models;
using cloudfilestorage.Repositories.Interface;
using cloudfilestorage.Services.Interface;

namespace cloudfilestorage.Services.Implementation;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IFileStorageService _fileStorageService;
    public AuthService(IAuthRepository authRepository, IFileStorageService fileStorageService)
    {
        _authRepository = authRepository;
        _fileStorageService = fileStorageService;
    }

    public async Task Register(string login, string password)
    {
        var encryptedPassword = HashPasswordHelper.HashPassword(password);
        
        _authRepository.Register(login, encryptedPassword);
        var user = _authRepository.FindByLoginAndPass(login, encryptedPassword);
        _fileStorageService.CreateUsersBucket(user.Result.ID);
        
    }

    public string Login(string login, string password)
    {
        return _authRepository.Login(login, password).Result;
    }

    public string? Authenticate(string login, string password)
    {
        var encryptedPassword = HashPasswordHelper.HashPassword(password);

        var user = _authRepository.FindByLoginAndPass(login, encryptedPassword).Result;
        {
            if (user.Password == encryptedPassword)
            {
                return user.Login;
                
            }
        }

        return null;
    }

    public async Task<User> GetUser(string login, string password)
    {
        return await _authRepository.FindByLoginAndPass(login, password);
    }
}