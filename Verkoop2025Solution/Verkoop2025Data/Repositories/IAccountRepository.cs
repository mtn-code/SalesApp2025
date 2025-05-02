using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verkoop2025Data.Models;

namespace Verkoop2025Data.Repositories;
public interface IAccountRepository
{
    Task<PersoneelslidAccount?> GetUserAccountByEmailAsync(string email);
    Task<Personeelslid?> GetUserByUserAccountIdAsync(int id);
    Task<string?> GetHashedPasswordByIdAsync(int id);
    Task<PersoneelslidAccount?> ChangeUserAccountPasswordAsync(int id, string newPassword);
}
