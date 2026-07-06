using KnowledgeHub.Api.Configurations.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMyCores();
builder.Services.AddMyAuthentication(builder.Configuration);
builder.Services.AddEntityFramework(builder.Configuration);
builder.Services.AddMyRateLimiting();
builder.Services.Addrepositories();
builder.Services.AddServices();

builder.Services.AddSwaggerGen();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
// CORS PERSONALIZADO
app.UseCors("AllowBlazor");
app.UseCors("AllowBlazor");

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();


app.UseHttpsRedirection();

app.MapControllers();

app.Run();
