using Messenger.MessageManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMessageManager(builder.Configuration);

builder.Build().UseMessageManager().Run();