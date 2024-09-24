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
    public class PictureController : ControllerBase
    {
        private readonly IPictureService _pictureService;

        public PictureController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        [HttpGet, Route("pictures/{pictureId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PictureContract>> GetPictureAsync([BindRequired, FromRoute] string pictureId)
        {
            try
            {
                var picture = await _pictureService.GetPictureAsync(pictureId).ConfigureAwait(false);

                return Ok(picture);
            }
            catch (NotFoundException ex)
            {
                return ObjectResultConverter.ToNotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.GetPictureErrorTitle);
            }
            catch (Exception ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.GetPictureErrorTitle);
            }
        }

        [HttpPut, Route("pictures/{pictureId}/likes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PictureContract>> LikePictureAsync([BindRequired, FromRoute] string pictureId, [BindRequired, FromBody] LikeRequest request)
        {
            try
            {
                var picture = await _pictureService.LikePictureAsync(pictureId, request).ConfigureAwait(false);

                return Ok(picture);
            }
            catch (NotFoundException ex)
            {
                return ObjectResultConverter.ToNotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.LikePictureErrorTitle);
            }
            catch (Exception ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.LikePictureErrorTitle);
            }
        }

        [HttpDelete, Route("pictures/{pictureId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeletePictureAsync([BindRequired, FromRoute] string pictureId)
        {
            try
            {
                await _pictureService.DeletePictureAsync(pictureId).ConfigureAwait(false);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return ObjectResultConverter.ToNotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.DeletePictureErrorTitle);
            }
            catch (Exception ex)
            {
                return ObjectResultConverter.ToInternalException(ex.Message, ErrorTitles.DeletePictureErrorTitle);
            }
        }
    }
}
