using Application.Common;
using EsteroidesToDo.Application;
using Infrastructure;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Users.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsersContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
       .AddEntityFrameworkStores<UsersContext>()
       .AddDefaultTokenProviders();

builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.AddScoped<GlobalExceptionHandlingMiddleware>();

builder.Services.AddControllers();

builder.Services.AddAuthorization(Options => {
    Options.AddPolicy("ActiveUser", policy => policy.RequireClaim("IsActive", "true"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedRoles(roleManager);
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
