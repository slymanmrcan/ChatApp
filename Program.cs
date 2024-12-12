using ChatApp.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .SetIsOriginAllowed(x=>true) 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Kimlik bilgilerine izin ver
    });
});
builder.Services.AddSignalR();
builder.Services.AddScoped<ChatHubBusiness>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chat}/{action=ChatScreen}/{id?}");
    
app.MapHub<ChatHub>("/chatHub");

app.Run();
