﻿using Amazon.Util.Internal;
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

    public void Register(string login, string password)
    {
        _authRepository.Register(login, password);
        _fileStorageService.CreateUsersBucket(GetUser(login, password).ID);
        
    }

    public string Login(string login, string password)
    {
        return _authRepository.Login(login, password);
    }

    public string? Authenticate(string login, string password)
    {
        foreach (User user in _authRepository.FindByLoginAndPass(login, password))
        {
            if (user.Password == password)
            {
                return user.Login;
                
            }
        }

        return null;
    }

    public User GetUser(string login, string password)
    {
        return _authRepository.FindByLoginAndPass(login, password).FirstOrDefault();
    }
}