
namespace APITemplate.EndpointFilters
{
    public class AuditLoggingFilter(ILogger<AuditLoggingFilter> _logger) : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            _logger.LogInformation("Request: {path}", context.HttpContext.Request.Path);
            var res = await next(context);

            //log the request and response

            return res;
        }
    }
}
