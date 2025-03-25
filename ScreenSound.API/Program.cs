using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ScreenSound.API.Endpoints;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Dados.Modelos;
using ScreenSound.Shared.Modelos.Modelos;
using System.Data.SqlTypes;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ScreenSoundContext>((options) => 
{
    options
           .UseSqlServer(builder.Configuration["ConnectionStrings:ScreenSoundDB"]) // aqui eu passo minhas string de configuração da variavel de ambiente 
           .UseLazyLoadingProxies();
});

builder.Services.AddIdentityApiEndpoints<PessoaComAcesso>().AddEntityFrameworkStores<ScreenSoundContext>(); // builder para o identity

builder.Services.AddAuthorization(); 

builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>();
builder.Services.AddTransient<DAL<Musica>>();
builder.Services.AddTransient<DAL<Genero>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddCors(options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins([builder.Configuration["BackendUrl"] ?? "https://localhost:7089",
            builder.Configuration["FrontendUrl"] ?? "https://localhost:7015"])
            .AllowAnyMethod()
            .SetIsOriginAllowed(pol => true)
            .AllowAnyHeader()
            .AllowCredentials()));

var app = builder.Build();
app.UseCors("wasm");

app.UseStaticFiles();//aqui falo pra minha api que minha aplicação vai usar arquivos estaticos 
app.UseAuthorization(); //etapa de tratamento da requisição HTTP para os endpoints da aplicação

//endpoint de negocio 
app.AddEndPointsArtistas();
app.AddEndPointsMusicas();
app.AddEndPointGeneros();

app.MapGroup("auth").MapIdentityApi<PessoaComAcesso>().WithTags("Autorização"); //agrupa vario endpoints em cima de uma roda 

app.MapPost("auth/logout", async ([FromServices] SignInManager<PessoaComAcesso> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
}).RequireAuthorization().WithTags("Autorização");

app.UseSwagger();
app.UseSwaggerUI();



app.Run();
