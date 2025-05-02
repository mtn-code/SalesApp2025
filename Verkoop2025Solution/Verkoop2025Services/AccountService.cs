using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verkoop2025Data.Models;
using Verkoop2025Data.Repositories;
using BCrypt;
using Microsoft.AspNetCore.Http;
using System.Reflection.Metadata;

namespace Verkoop2025Services;
public class AccountService
{
    private readonly IAccountRepository accountRepository;
    public AccountService(IAccountRepository accountRepository)
    {
        this.accountRepository = accountRepository;
    }
    public async Task<Personeelslid?> AuthenticateUserAsync(string email, string password)
    {
        PersoneelslidAccount? userAccount = await accountRepository.GetUserAccountByEmailAsync(email);
        if (userAccount == null || !BCrypt.Net.BCrypt.Verify(password, userAccount.Paswoord))
            return null;

        Personeelslid? user = await accountRepository.GetUserByUserAccountIdAsync(userAccount.PersoneelslidAccountId);
        if (user?.SecurityGroepen.Any(g => g.Naam == "Cwebsite") == true)
            return user;

        return null;
    }
    public async Task<PersoneelslidAccount> GetPersoneelslidAccountByEmail(string email)
    {
        PersoneelslidAccount? userAccount = await accountRepository.GetUserAccountByEmailAsync(email);
        if (userAccount == null)
            return null!;

        return userAccount;

    }
    public async Task<bool> VerifyOldPassword(int accountId, string oldPassword)
    {
        string? hashedPassword = await accountRepository.GetHashedPasswordByIdAsync(accountId);
        return hashedPassword != null && BCrypt.Net.BCrypt.Verify(oldPassword, hashedPassword);
    }
    
    public async Task<PersoneelslidAccount> ChangePassword(int accountId, string newPassword)
    {
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
        var updatedAccount = await accountRepository.ChangeUserAccountPasswordAsync(accountId, hashedPassword);
        if (updatedAccount == null)
        {
            throw new Exception("Fout bij het bijwerken van het wachtwoord.");

        }
        return updatedAccount;

    }
}
