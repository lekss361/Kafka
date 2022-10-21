using API.Controllers;
using KafkaFlow;
using System.Diagnostics;

namespace API;

public class ErrorHandlingMiddleware : IMessageMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _log;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> log)
    {
        _log = log;
    }

    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        var c = context;
         await next(context).ConfigureAwait(true);
        
        
    }
}