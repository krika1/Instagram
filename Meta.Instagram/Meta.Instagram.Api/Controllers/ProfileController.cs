using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.DTOs.Requests;
using Meta.Instagram.Infrastructure.Exceptions.Errors;
using Meta.Instagram.Infrastructure.Exceptions;
using Meta.Instagram.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Meta.Instagram.Infrastructure.Services;

namespace Meta.Instagram.Api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    [ApiVersion("1.0")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost, Route("profiles/{profileId}/follow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> FollowProfileAsync([BindRequired, FromRoute] string profileId, [BindRequired, FromForm] FollowRequest request)
        {
            try
            {
                await _profileService.FollowProfileAsync(profileId, request).ConfigureAwait(false);

                return Ok();
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

        [HttpPut, Route("profiles/{profileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProfileContract>> UpdateProfileAsync([BindRequired, FromRoute] string profileId, [BindRequired, FromForm] ChangeProfileRequest request)
        {
            try
            {
                var updatedProfile = await _profileService.UpdateProfileAsync(profileId, request).ConfigureAwait(false);

                return Ok(updatedProfile);
            }
            catch (NotFoundException ex)
            {
                return ObjectResultConverter.ToNotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return ObjectResultConverter.ToBadRequestException(ex.Message);
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

        [HttpGet, Route("profiles/{profileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProfileContract>> GetProfileAsync([BindRequired, FromRoute] string profileId)
        {
            try
            {
                var profile = await _profileService.GetProfileAsync(profileId).ConfigureAwait(false);

                return Ok(profile);
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
    }
}
