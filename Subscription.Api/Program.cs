using Polly;
using Subscription.Api.Service;
using Subscription.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add Circuit Breaker
var httpRetryPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
    .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));
builder.Services.AddHttpClient("Book.Api", c => { c.BaseAddress = new Uri("http://localhost:5028"); });
builder.Services.AddSingleton<IAsyncPolicy<HttpResponseMessage>>(httpRetryPolicy);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ISqlHelper, SqlHelper>();
builder.Services.AddTransient<ISubscription, SubscriptionService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
