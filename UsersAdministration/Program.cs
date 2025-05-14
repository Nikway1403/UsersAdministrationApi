using UsersAdministration.Database.InMemoryDb;
using UsersAdministration.Extensions;
using UsersAdministration.Middlewares;
using UsersAdministration.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddMyServices(builder.Configuration);

WebApplication app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    IInMemRepository repo = scope.ServiceProvider.GetRequiredService<IInMemRepository>();

    repo.CreateUser(new User
    {
        Guid = Guid.NewGuid(),
        Login = "admin",
        Password = "superAdmin",
        Name = "Admin",
        IsAdmin = true,
        Gender = 1,
        Birthday = DateTime.UtcNow.AddYears(-22),
        CreatedBy = "System",
        CreatedOn = DateTime.UtcNow,
    });
}



app.UseMiddleware<ExceptionsHandlerMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();