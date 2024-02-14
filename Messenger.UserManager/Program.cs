using Messenger.UserManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUserManager(builder.Configuration);

builder.Build().UseUserManager().Run();
