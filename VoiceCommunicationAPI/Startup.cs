using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SignalR;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSignalR(); // Add SignalR services
        services.AddControllers(); // Add support for controllers if needed
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage(); // Use developer exception page in development
        }
        else
        {
            app.UseExceptionHandler("/Home/Error"); // Use a global error handler in production
            app.UseHsts(); // Add HTTP Strict Transport Security
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles(); // Serve static files

        app.UseRouting();

        app.UseAuthorization(); // If using authorization

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // Map controller routes if any
            endpoints.MapHub<VoiceHub>("/voiceHub"); // Map the SignalR hub
        });
    }
}
