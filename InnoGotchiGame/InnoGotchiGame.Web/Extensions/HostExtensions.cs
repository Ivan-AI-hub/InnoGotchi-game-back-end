using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Web.Initializers.Models;
using InnoGotchiGame.Web.Initializers;
using Microsoft.Extensions.Options;
using LoggerService;

namespace InnoGotchiGame.Web.Extensions
{
    public static class HostExtensions
    {
        public static void InitializePetView(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var pictureManager = services.GetRequiredService<PictureManager>();
                    var options = services.GetRequiredService<IOptions<BasePetViewInitializeModel>>();
                    BasePetViewInitializer.InvokeAsync(pictureManager, options).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILoggerManager>();
                    logger.LogError($"An error occurred when filling pets view./n{ex}");
                }
            }
        }
    }
}
