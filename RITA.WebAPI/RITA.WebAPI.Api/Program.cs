using RockLib.Logging;
using RockLib.Logging.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogger()
    .AddConsoleLogProvider(new CoreLogFormatter());

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddTransient<IBooksHelper, BooksHelper>();
//builder.Services.AddTransient<IBookRepository, BookRepository>();

//builder.Services.AddDbContext<BookContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi3(c => { c.DocExpansion = "List"; });
}

//app.UseAuthorization();

app.MapControllers();

app.Run();
