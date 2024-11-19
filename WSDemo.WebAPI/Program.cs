using Microsoft.EntityFrameworkCore;
using WSDemo.Domain;
using WSDemo.RepositoryLayer;
using WSDemo.ServiceLayer;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString
    , x => x.MigrationsAssembly("WSDemo.SQLiteDB")
    ));


builder.Services.AddScoped<IFolderItemsService, FolderItemsService>();
builder.Services.AddScoped<IFolderItemsRepository, FolderItemsRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapping));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbInitializer.InitializeAsync(context);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while creating the DB.");
    throw;
}


app.Run();

