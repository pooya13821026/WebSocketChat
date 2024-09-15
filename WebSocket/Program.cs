using WebSocket.WebSocketSetting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseWebSockets(WebSocketSetting.GetOptions());
app.Use(async (context, next) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        if (context.Request.Path == "/wsChat")
        {
            var ws = new WebSocketClass();
            await ws.ListenAccept(context);
        }
    }
    else
    {
        await next();
    }
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();