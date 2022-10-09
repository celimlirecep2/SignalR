using Microsoft.EntityFrameworkCore;
using SignalRApp.API.Hubs;
using SignalRApp.API.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//services
builder.Services.AddSignalR();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("https://localhost:7093/MyHub").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});
//builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("MyLocalDbSQLite")));
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyLocalDbSQLite")));



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//middleware
app.UseRouting();
app.UseCors("CorsPolicy");//maphub dan önce çaðýrýlmalý

app.UseAuthorization();

app.MapControllers();
//*
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<MyHub>("/MyHub");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.Run();
