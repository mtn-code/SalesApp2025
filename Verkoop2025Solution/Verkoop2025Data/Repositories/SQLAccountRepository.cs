using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verkoop2025Data.Models;



namespace Verkoop2025Data.Repositories;
public class SQLAccountRepository : IAccountRepository
{
    private readonly PrulariaComContext context;
    public SQLAccountRepository(PrulariaComContext context)
    {
        this.context = context;
    }

    public async Task<PersoneelslidAccount?> GetUserAccountByEmailAsync(string email)
    {
        var account = await context.PersoneelslidAccounts
            .Where(p => p.Emailadres == email)
            .FirstOrDefaultAsync();
        return account;
    }

    public async Task<Personeelslid?> GetUserByUserAccountIdAsync(int id)
    {
        var user = await context.Personeelsleden
            .Include(p => p.SecurityGroepen)
            .Where(p => p.PersoneelslidAccountId == id)
            .FirstOrDefaultAsync();
        return user;

    }
    public async Task<string?> GetHashedPasswordByIdAsync(int id)
    {
        var userAccount = await context.PersoneelslidAccounts.FindAsync(id);
        return userAccount?.Paswoord;
    }

    public async Task<PersoneelslidAccount?> ChangeUserAccountPasswordAsync(int id, string newPassword)
    {
        var userAccount = await context.PersoneelslidAccounts.Where(p => p.PersoneelslidAccountId == id).FirstOrDefaultAsync();

        if (userAccount == null)
        {
            return null;
        }

        userAccount.Paswoord = newPassword;
        await context.SaveChangesAsync();

        return userAccount;
    }
}
