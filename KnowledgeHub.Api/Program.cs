using KnowledgeHub.Api.Configurations.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMyCores();
builder.Services.AddMyAuthentication(builder.Configuration);
builder.Services.AddEntityFramework(builder.Configuration);
builder.Services.AddMyRateLimiting();
builder.Services.Addrepositories();
builder.Services.AddServices();
builder.Services.AddPasswordHasher();

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

app.MapGet("/", () => Results.Content("""
    <!DOCTYPE html>
    <html lang="pt-br">
    <head>
        <meta charset="UTF-8" />
        <title>Notium API</title>
        <style>
            body {
                font-family: 'Segoe UI', Tahoma, sans-serif;
                background: #0f0f1a;
                color: #e4e4e7;
                display: flex;
                align-items: center;
                justify-content: center;
                height: 100vh;
                margin: 0;
            }
            .card {
                background: #1a1a2e;
                border: 1px solid #2e2e4d;
                border-radius: 12px;
                padding: 2.5rem 3rem;
                text-align: center;
                box-shadow: 0 8px 24px rgba(0,0,0,0.4);
            }
            h1 {
                margin: 0 0 0.5rem 0;
                font-size: 1.8rem;
                color: #a78bfa;
            }
            .status {
                display: inline-block;
                background: #16a34a33;
                color: #4ade80;
                padding: 0.25rem 0.75rem;
                border-radius: 999px;
                font-size: 0.85rem;
                font-weight: 600;
                margin-bottom: 1rem;
            }
            p {
                margin: 0.3rem 0;
                color: #a1a1aa;
            }
            a {
                color: #a78bfa;
                text-decoration: none;
            }
            a:hover {
                text-decoration: underline;
            }
            .footer {
                margin-top: 1.5rem;
                font-size: 0.8rem;
                color: #6b7280;
            }
        </style>
    </head>
    <body>
        <div class="card">
            <div class="status">● Online</div>
            <h1>Notium API</h1>
            <p>API de gestão de conhecimento pessoal — notas, categorias e tags organizadas em um só lugar.</p>
            <div class="footer">
                by Christiano CJ · <a href="https://github.com/christianocj/Notium">github.com/christianocj/Notium</a>
            </div>
        </div>
    </body>
    </html>
    """, "text/html"));

app.MapControllers();

app.Run();
