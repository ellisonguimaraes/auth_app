using System;
using System.Text.Json;
using AuthApp.Application.Models;
using AuthApp.Infra.CrossCutting.Resources;
using AuthApp.Services.Exceptions;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace AuthApp.API.Middlewares;

public class GlobalExceptionMiddleware
{
    #region Constants
    private const string CONTENT_TYPE = "application/json";
    private const string TRACE_ID_NAME = "TraceId";
    private const string ENDPOINT_NAME = "Endpoint";
    #endregion

    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Exception interceptor method
    /// </summary>
    /// <param name="context">Http context</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException e)
        {
            var response = Responses.WithError(StatusCodes.Status400BadRequest, e.Message);

            _logger.LogError(e, $"[{context.Request.Method} {context.Request.Path}] {ErrorCodeResource.BUSINESS_FAILURE}: {e.Message}, {TRACE_ID_NAME}: {response.TraceId}");

            await BuildResponseAsync(context, response.StatusCode, JsonSerializer.Serialize(response), CONTENT_TYPE);
        }
        catch (AuthException e)
        {
            var response = Responses.WithError(StatusCodes.Status400BadRequest, e.Message);

            _logger.LogError(e, $"[{context.Request.Method} {context.Request.Path}] {e.Message}, {TRACE_ID_NAME}: {response.TraceId}");

            await BuildResponseAsync(context, response.StatusCode, JsonSerializer.Serialize(response), CONTENT_TYPE);
        }
        catch (ValidationException e)
        {
            var response = Responses.WithError(StatusCodes.Status400BadRequest, e.Errors.Select(error => error.ErrorMessage));

            _logger.LogError(e, $"[{context.Request.Method} {context.Request.Path}] {ErrorCodeResource.DATA_VALIDATION_FAILURE}, {TRACE_ID_NAME}: {response.TraceId}");

            await BuildResponseAsync(context, response.StatusCode, JsonSerializer.Serialize(response), CONTENT_TYPE);
        }
        catch (SecurityTokenException e)
        {
            var response = Responses.WithError(StatusCodes.Status400BadRequest, ErrorCodeResource.ERROR_GENERATING_TOKENS);

            _logger.LogError(e, $"[{context.Request.Method} {context.Request.Path}] {ErrorCodeResource.ERROR_GENERATING_TOKENS_MESSAGE}, {TRACE_ID_NAME}: {response.TraceId}");

            await BuildResponseAsync(context, response.StatusCode, JsonSerializer.Serialize(response), CONTENT_TYPE);
        }
        catch (Exception e)
        {
            var response = Responses.WithError(StatusCodes.Status400BadRequest, ErrorCodeResource.UNEXPECTED_ERROR_OCURRED);

            _logger.LogError(e, $"[{context.Request.Method} {context.Request.Path}] {ErrorCodeResource.UNEXPECTED_ERROR_OCURRED}, {TRACE_ID_NAME}: {response.TraceId}");

            await BuildResponseAsync(context, response.StatusCode, JsonSerializer.Serialize(response), CONTENT_TYPE);
        }
    }

    /// <summary>
    /// Build HTTP response
    /// </summary>
    /// <param name="context">Context</param>
    /// <param name="statusCodes">Status code</param>
    /// <param name="body">Response body</param>
    /// <param name="contentType">Content type</param>
    private async Task BuildResponseAsync(HttpContext context, int statusCodes, string body, string contentType)
    {
        context.Response.Clear();
        context.Response.StatusCode = statusCodes;
        context.Response.ContentType = contentType;
        await context.Response.WriteAsync(body);
    }
}
