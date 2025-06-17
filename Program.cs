using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using lxcn_movie_web_app.Data;
using lxcn_movie_web_app.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity services for authentication and authorization
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    // Configure password requirements
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Configure application cookies for custom access denied handling
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Error/AccessDenied";
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
});

// Add custom services
builder.Services.AddScoped<IUserRoleService, UserRoleService>();

// Add MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Initialize roles and admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Initialize roles first
        await RoleSeeder.SeedRolesAndAdmin(services);

        // Then seed movie data
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Add status code pages for 404 handling - use the new route name
app.UseStatusCodePagesWithReExecute("/Error/PageNotFound");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Configure default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor Pages for Identity
app.MapRazorPages();

app.Run();