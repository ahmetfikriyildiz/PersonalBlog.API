using PersonalBlog.API.DTOs.Commons;
using PersonalBlog.API.Exceptions;
using System.Net;
using System.Text.Json;

namespace PersonalBlog.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = exception switch
            {
                NotFoundException notFound => new ErrorResponseDto
                {
                    StatusCode = notFound.StatusCode,
                    ErrorCode = notFound.ErrorCode,
                    Message = notFound.Message
                },
                BadRequestException badRequest => new ErrorResponseDto
                {
                    StatusCode = badRequest.StatusCode,
                    ErrorCode = badRequest.ErrorCode,
                    Message = badRequest.Message
                },
                ValidationException validation => new ErrorResponseDto
                {
                    StatusCode = validation.StatusCode,
                    ErrorCode = validation.ErrorCode,
                    Message = validation.Message,
                    Errors = validation.Errors
                },
                ConflictException conflict => new ErrorResponseDto
                {
                    StatusCode = conflict.StatusCode,
                    ErrorCode = conflict.ErrorCode,
                    Message = conflict.Message
                },
                _ => new ErrorResponseDto
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorCode = "INTERNAL_SERVER_ERROR",
                    Message = exception.Message,
                    Details = exception.StackTrace
                }
            };

            response.StatusCode = errorResponse.StatusCode;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(errorResponse, options);
            return response.WriteAsync(json);
        }
    }
}
