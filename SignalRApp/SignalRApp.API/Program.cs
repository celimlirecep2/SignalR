using SignalRApp.API.Hubs;

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

//app.MapHub<MyHub>("/chatHub");


app.MapControllers();
//middleware
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapHub<MyHub>("/MyHub");
//});

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<MyHub>("/MyHub");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.Run();
