using UsersAdministration.Exceptions;

namespace UsersAdministration.Middlewares;

public class ExceptionsHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionsHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException e)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(e.Message);
        }
        catch (ForbiddenException e)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync(e.Message);
        }
        catch (NotFoundException e)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(e.Message);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Server error");
        }
    }
}