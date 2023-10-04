using RedditFun.Services.Domain.Models;
using RedditFun.Services.Services;
using RedditFun.WorkerService;

internal class Program
{
	private static void Main(string[] args)
	{
		CreateHostBuilder(args).Run();
	}

	internal static WebApplication CreateHostBuilder(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();
		
		builder.Host.ConfigureServices((hostContext, services) =>
		{
			IConfiguration configuration = hostContext.Configuration;
			RedditOptions options = configuration.GetSection("RedditConfig").Get<RedditOptions>();
			GlobalVariables.RedditOptions = options;
			
			GlobalVariables.WorkerThread = new Worker(options);

			services.AddHostedService<Worker>(sp => GlobalVariables.WorkerThread); 
		});
		builder.Host.UseWindowsService();
		
		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseAuthorization();

		app.MapControllers();

		return app;
	}
}