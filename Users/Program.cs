using Application.Common;
using EsteroidesToDo.Application;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsersContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
       //.AddEntityFrameworkStores<UsersContext>()
       .AddDefaultTokenProviders();


builder.Services.AddApplicationServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedRoles(roleManager);
}

app.MapGet("/", () => "Hello World!");

app.Run();
