var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços ao contêiner de DI (injeção de dependência)
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<Service<Usuario>>(sp => 
    new Service<Usuario>(Path.Combine(Directory.GetCurrentDirectory(), "db",))
);

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
