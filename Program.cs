using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Reflection;

namespace Practice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureServices(builder.Configuration, builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(IConfiguration config, IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(conf => conf.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAutoMapper(Assembly.GetEntryAssembly());

            services.AddDbContext<TimeManagementDBContext>(opt => opt.UseSqlServer(config["ConnectionStrings:DBConnection"]));
        }
    }
}
