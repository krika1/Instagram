﻿using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.DTOs.Requests;

namespace Meta.Instagram.Infrastructure.Services
{
    public interface IAccountService
    {
        Task<AccountContract> PostAccountAsync(CreateAccountRequest request);
        Task ChangeAccountPasswordAsync(ChangeAccountPasswordRequest request);
        Task DeleteAccountAsync(string accountId);
        Task<AccountContract> UpdateAccountAsync(string accountId, ChanageAccountRequest request);
        Task<AccountContract> GetAccountAsync(string accountId);
    }
}
