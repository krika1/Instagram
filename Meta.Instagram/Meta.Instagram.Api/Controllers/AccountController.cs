using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.DTOs.Requests;
using Meta.Instagram.Infrastructure.Exceptions;
using Meta.Instagram.Infrastructure.Exceptions.Errors;
using Meta.Instagram.Infrastructure.Helpers;
using Meta.Instagram.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Meta.Instagram.Api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    [ApiVersion("1.0")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet, Route("accounts/{accountId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountContract>> GetAccountAsync([BindRequired, FromRoute] string accountId)
        {
            try
            {
                var account = await _accountService.GetAccountAsync(accountId).ConfigureAwait(false);

                return Ok(account);
            }
            catch (NotFoundException ex)
            {
                return ObjectResultConverter.ToNotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.GetAccountFailedTitle);
            }
            catch (Exception ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.GetAccountFailedTitle);
            }
        }

        [HttpPost, Route("accounts")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountContract>> PostAccountAsync([BindRequired, FromBody] CreateAccountRequest request)
        {
            try
            {
                var createdAccount = await _accountService.PostAccountAsync(request).ConfigureAwait(false);

                return Created(createdAccount.AccountId, createdAccount);
            }
            catch (AuthenticationException ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.CreateAccountFailedTitle);
            }
            catch (DatabaseException ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.CreateAccountFailedTitle);
            }
            catch (Exception ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.CreateAccountFailedTitle);
            }
        }

        [HttpPut, Route("accounts/change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ChangeAccountPasswordAsync([BindRequired, FromBody] ChangeAccountPasswordRequest request)
        {
            try
            {
                await _accountService.ChangeAccountPasswordAsync(request).ConfigureAwait(false);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return ObjectResultConverter.ToNotFound(ex.Message);
            }
            catch (AuthenticationException ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.ChangeAccountPasswordFailedTitle);
            }
            catch (DatabaseException ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.ChangeAccountPasswordFailedTitle);
            }
            catch (Exception ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.ChangeAccountPasswordFailedTitle);
            }
        }

        [HttpPut, Route("accounts/{accountId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountContract>> UpdateAccountAsync([BindRequired, FromRoute] string accountId, [BindRequired, FromBody] ChanageAccountRequest request)
        {
            try
            {
                var updatedAccount = await _accountService.UpdateAccountAsync(accountId, request).ConfigureAwait(false);

                return Ok(updatedAccount);
            }
            catch (NotFoundException ex)
            {
                return ObjectResultConverter.ToNotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.UpdateAccountFailedTitle);
            }
            catch (Exception ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.UpdateAccountFailedTitle);
            }
        }

        [HttpDelete, Route("accounts/{accountId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAccountAsync([BindRequired, FromRoute] string accountId)
        {
            try
            {
                await _accountService.DeleteAccountAsync(accountId).ConfigureAwait(false);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return ObjectResultConverter.ToNotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.DeleteAccountFailedTitle);
            }
            catch (Exception ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.DeleteAccountFailedTitle);
            }
        }
    }
}
