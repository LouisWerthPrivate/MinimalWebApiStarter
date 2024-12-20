using Scalar.AspNetCore;

namespace APITemplate.WebApplicationExtentions
{
    public static class WebApplicationStartupExtentions
    {
        public static WebApplication ConfigureApplication(this WebApplication app)
        {
            if (app.Configuration.GetValue<bool>("MapOpenApi", false) == true)
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options => 
                {
                    options.WithTheme(ScalarTheme.BluePlanet)
                    .WithTitle("AlertCore API Reference");
                });
            }

            app.UseHttpsRedirection();

            return app;
        }
    }
}
