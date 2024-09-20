using AutoMapper;
using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.DTOs.Requests;
using Meta.Instagram.Infrastructure.Entities;
using Meta.Instagram.Infrastructure.Exceptions;
using Meta.Instagram.Infrastructure.Exceptions.Errors;
using Meta.Instagram.Infrastructure.Repositories;
using Meta.Instagram.Infrastructure.Services;

namespace Meta.Instagram.Bussines.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository,
            IAuthenticationService authenticationService,
            IMapper mapper,
            IProfileRepository profileRepository)
        {
            _accountRepository = accountRepository;
            _authenticationService = authenticationService;
            _mapper = mapper;
            _profileRepository=profileRepository;
        }

        public async Task ChangeAccountPasswordAsync(ChangeAccountPasswordRequest request)
        {
            try
            {
                var account = await GetAccount(request.AccountId!).ConfigureAwait(false);

                await _authenticationService.ChangeAuth0UserPasswordAsync(account.ExternalId!, request.NewPassword!);
                account.UpdatedAt = DateTime.Now;

                await _accountRepository.UpdateAccountAsync(account);
            }
            catch (AuthenticationException ex)
            {
                throw new AuthenticationException(ex.Message);
            }
        }

        public async Task DeleteAccountAsync(string accountId)
        {
            var account = await GetAccount(accountId).ConfigureAwait(false);

            await _authenticationService.DeleteAuth0UserAsync(account.ExternalId!).ConfigureAwait(false);

            await _accountRepository.DeleteAccountAsync(account).ConfigureAwait(false);
        }

        public async Task<AccountContract> GetAccountAsync(string accountId)
        {
            var account = await GetAccount(accountId).ConfigureAwait(false);

            return _mapper.Map<AccountContract>(account);
        }

        public async Task<AccountContract> PostAccountAsync(CreateAccountRequest request)
        {
            try
            {
                var auth0User = await _authenticationService.CreateAuth0UserAsync(request).ConfigureAwait(false);

                var domainAccount = _mapper.Map<Account>(auth0User);
                domainAccount.Phone = request.Phone;

                var createdAccount = await _accountRepository.CreateAccountAsync(domainAccount).ConfigureAwait(false);

                var profile = _mapper.Map<Infrastructure.Entities.Profile>(createdAccount);
                profile.AccountId = createdAccount.AccountId;

                await _profileRepository.CreateProfileAsync(profile).ConfigureAwait(false);

                return _mapper.Map<AccountContract>(createdAccount);
            }
            catch (AuthenticationException ex)
            {
                throw new AuthenticationException(ex.Message);
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex.Message);
            }
        }

        public async Task<AccountContract> UpdateAccountAsync(string accountId, ChanageAccountRequest request)
        {
            var account = await GetAccount(accountId).ConfigureAwait(false);

            account.UpdatedAt = DateTime.Now;
            account.Phone = request.PhoneNumber;

            await _accountRepository.UpdateAccountAsync(account);

            var updatedAccount = await GetAccount(accountId).ConfigureAwait(false);

            return _mapper.Map<AccountContract>(updatedAccount);
        }

        private async Task<Account> GetAccount(string accountId)
        {
            return await _accountRepository.GetAccountAsync(accountId).ConfigureAwait(false)
                    ?? throw new NotFoundException(ErrorMessages.AccountNotFoundErrorMessage);
        }
    }
}
