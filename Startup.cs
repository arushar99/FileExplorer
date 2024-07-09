namespace FileBrowserApi{
    public class Startup {
        
            public Startup(IConfiguration configuration)
            {
                Configuration = configuration;
            }

            public IConfiguration Configuration { get; }

            public void ConfigureServices(IServiceCollection services)
            {
                services.AddControllers();
                //services.AddStaticFiles();

                services.AddSingleton(sp => Configuration["HomeDirectory"]);
            }

            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
        }


    
}