var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HotpServer.Storage.Implementations.ApplicationContext>();
builder.Services.AddScoped<HotpServer.Storage.IDataLayer, HotpServer.Storage.Implementations.DataLayer>();
builder.Services.AddScoped<HotpServer.Services.IAuthenticationService, HotpServer.Services.Implementations.AuthenticationService>();
builder.Services.AddScoped<HotpServer.Services.ITwoFactorAuthService, HotpServer.Services.Implementations.TwoFactorAuthService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
