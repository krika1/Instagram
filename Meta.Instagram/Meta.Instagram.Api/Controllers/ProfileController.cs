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

        [HttpPost, Route("profiles/{profileId}/followers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> FollowProfileAsync([BindRequired, FromRoute] string profileId, [BindRequired, FromBody] FollowRequest request)
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
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.FollowProfileErrorTitle);
            }
            catch (Exception ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.FollowProfileErrorTitle);
            }
        }

        [HttpPost, Route("profiles/{profileId}/picture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UploadPictureAsync([BindRequired, FromRoute] string profileId, [BindRequired, FromForm] UploadPictureRequest request)
        {
            try
            {
                await _profileService.UploadPictureAsync(profileId, request).ConfigureAwait(false);

                return Ok();
            }
            catch (NotFoundException ex)
            {
                return ObjectResultConverter.ToNotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.UploadProfileErrorTitle);
            }
            catch (Exception ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.UploadProfileErrorTitle);
            }
        }

        [HttpDelete, Route("profiles/{profileId}/followers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UnFollowProfileAsync([BindRequired, FromRoute] string profileId, [BindRequired, FromBody] FollowRequest request)
        {
            try
            {
                await _profileService.UnFollowProfileAsync(profileId, request).ConfigureAwait(false);

                return Ok();
            }
            catch (NotFoundException ex)
            {
                return ObjectResultConverter.ToNotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.UnfollowProfileErrorTitle);
            }
            catch (Exception ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.UnfollowProfileErrorTitle);
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
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.UpdateProfileErrorTitle);
            }
            catch (Exception ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.UpdateProfileErrorTitle);
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
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.GetProfileErrorTitle);
            }
            catch (Exception ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.GetProfileErrorTitle);
            }
        }

        [HttpGet, Route("profiles/{profileId}/followers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProfileContract>> GetProfileFollowersAsync([BindRequired, FromRoute] string profileId)
        {
            try
            {
                var followers = await _profileService.GetProfileFollowersAsync(profileId).ConfigureAwait(false);

                return Ok(followers);
            }
            catch (NotFoundException ex)
            {
                return ObjectResultConverter.ToNotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.GetProfileErrorTitle);
            }
            catch (Exception ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.GetProfileErrorTitle);
            }
        }

        [HttpGet, Route("profiles/{profileId}/following")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProfileContract>> GetProfileFollowingAsync([BindRequired, FromRoute] string profileId)
        {
            try
            {
                var following = await _profileService.GetProfileFollowingAsync(profileId).ConfigureAwait(false);

                return Ok(following);
            }
            catch (NotFoundException ex)
            {
                return ObjectResultConverter.ToNotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.GetProfileErrorTitle);
            }
            catch (Exception ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.GetProfileErrorTitle);
            }
        }
    }
}
