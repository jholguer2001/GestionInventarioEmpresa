using CurrentUserService = MyApp.Business.Services.Implementations.CurrentUserService;
using ICurrentUserService = MyApp.Business.Services.Interfaces.ICurrentUserService;

namespace MyApp.Presentation;
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Configura todos los servicios de inyección de dependencias
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Configurar Entity Framework
        services.AddEntityFrameworkServices(configuration);

        // 2. Configurar Authentication & Authorization
        services.AddAuthenticationServices();

        // 3. Configurar Data Access Layer
        services.AddDataAccessServices();

        // 4. Configurar Business Layer
        services.AddBusinessServices();

        // 5. Configurar Infrastructure Services
        services.AddInfrastructureServices();

        return services;
    }

    /// <summary>
    /// Configura Entity Framework y base de datos
    /// </summary>
    private static IServiceCollection AddEntityFrameworkServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            // El interceptor se añade automáticamente a través de DI
        });

        // Interceptor para auditoría automática
        services.AddScoped<AuditInterceptor>();

        return services;
    }

    /// <summary>
    /// Configura autenticación y autorización
    /// </summary>
    private static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Administrator"));
            options.AddPolicy("AdminOrOperator", policy =>
                policy.RequireRole("Administrator", "Operator"));
            options.AddPolicy("AuthenticatedUsers", policy =>
                policy.RequireAuthenticatedUser());
        });

        return services;
    }

    /// <summary>
    /// Configura la capa de acceso a datos (Repositories y UoW)
    /// </summary>
    private static IServiceCollection AddDataAccessServices(this IServiceCollection services)
    {
        // Unit of Work Pattern
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repository Pattern - Todos los repositorios
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();

        // Repositorio genérico (si se necesita)
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }

    /// <summary>
    /// Configura la capa de lógica de negocio (Services)
    /// </summary>
    private static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        // Core Business Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<ILoanService, LoanService>();

        // Utility Services
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<IReportService, ReportService>();

        // Current User Context
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }

    /// <summary>
    /// Configura servicios de infraestructura
    /// </summary>
    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // HTTP Context Access
        services.AddHttpContextAccessor();

        // CSRF Protection
        services.AddAntiforgery(options =>
        {
            options.HeaderName = "RequestVerificationToken";
            options.SuppressXFrameOptionsHeader = false;
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.Cookie.SameSite = SameSiteMode.Strict;
        });

        // Memory Cache (si se necesita)
        services.AddMemoryCache();

        // Health Checks (opcional pero recomendado)
        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        return services;
    }

    /// <summary>
    /// Configura servicios adicionales para desarrollo
    /// </summary>
    public static IServiceCollection AddDevelopmentServices(this IServiceCollection services)
    {
        // Servicios específicos para desarrollo

        // Logging más detallado en desarrollo
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
            builder.SetMinimumLevel(LogLevel.Debug);
        });


        return services;
    }

    /// <summary>
    /// Configura servicios adicionales para producción
    /// </summary>
    public static IServiceCollection AddProductionServices(this IServiceCollection services)
    {
        // Servicios específicos para producción
        // Por ejemplo: Redis Cache, Application Insights, etc.

        return services;
    }
}