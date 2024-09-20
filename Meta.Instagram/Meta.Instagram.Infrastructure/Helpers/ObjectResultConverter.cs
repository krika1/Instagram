using Meta.Instagram.Infrastructure.DTOs;
using Meta.Instagram.Infrastructure.Exceptions.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta.Instagram.Infrastructure.Helpers
{
    public static class ObjectResultConverter
    {
        public static ObjectResult ToNotFound(string message)
        {
            var errorContract = new ErrorContract
            {
                Details = message,
                Title = ErrorTitles.ResourceNotFoundTitle,
                StatusCode = StatusCodes.Status404NotFound
            };

            return new ObjectResult(errorContract)
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        public static ObjectResult ToInternalException(string message, string title)
        {
            var errorContract = new ErrorContract
            {
                Details = message,
                Title = title,
                StatusCode = StatusCodes.Status500InternalServerError
            };

            return new ObjectResult(errorContract)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
