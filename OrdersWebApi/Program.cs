using FirebaseAdmin;
using FluentValidation.AspNetCore;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using OrdersWebApi.Features.CompetitiveOrdering;
using OrdersWebApi.Features.Firebase;
using OrdersWebApi.Features.Geolocation;
using OrdersWebApi.Features.Hub;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.
    AddControllers().
    AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.Load("OrdersShared")));
builder.Services.AddDbContext<OrdersContext>(options => 
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSignalR();
builder.Services.AddScoped<GeolocationService>();
builder.Services.AddSingleton<FirebaseService>();
builder.Services.AddHostedService<OrderProcessingService>();
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ordersweb-firebase-adminsdk-fbsvc-85e7e48744.json"))
});

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

if (!builder.Environment.IsDevelopment())
    builder.WebHost.UseUrls("https://127.0.0.1:" + builder.Configuration.GetValue<string>("HttpsPort"),
        "http://127.0.0.1:" + builder.Configuration.GetValue<string>("HttpPort"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrdersContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");
app.MapHub<UserHub>("/userhub");

app.Run();
