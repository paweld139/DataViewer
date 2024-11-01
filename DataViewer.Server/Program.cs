
using DataViewer.DAL;
using Microsoft.EntityFrameworkCore;

namespace DataViewer.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddNpgsql<DataViewerContext>(connectionString);

            builder.Services.AddScoped<DataViewerSeeder>();

            var app = builder.Build();

            using var scope = app.Services.CreateScope();

            var seeder = scope.ServiceProvider.GetRequiredService<DataViewerSeeder>();

            seeder.Migrate().Wait();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
