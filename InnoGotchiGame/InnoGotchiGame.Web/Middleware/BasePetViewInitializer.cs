using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;

namespace InnoGotchiGame.Web.Middleware
{
    public class BasePetViewInitializer
    {
        private RequestDelegate _next;
        private bool _isInitialize = false;

        public BasePetViewInitializer(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, PictureManager manager)
        { 
            if (!_isInitialize)
            {
                var filter = new PictureFiltrator()
                {
                    Description = "petView-nose"
                };
                if ((await manager.GetAllAsync(filter)).Any())
                {
                    for (int i = 1; i < 6; i++)
                    {
                        await AddImageToDb($"wwwroot/Resourses/Bodies/body{i}.svg", "petView-body", manager);
                    }
                    for (int i = 1; i < 7; i++)
                    {
                        await AddImageToDb($"wwwroot/Resourses/Eyes/eyes{i}.svg", "petView-eyes", manager);
                    }
                    for (int i = 1; i < 6; i++)
                    {
                        await AddImageToDb($"wwwroot/Resourses/Mouths/mouth{i}.svg", "petView-mouth", manager);
                    }
                    for (int i = 1; i < 7; i++)
                    {
                        await AddImageToDb($"wwwroot/Resourses/Noses/nose{i}.svg", "petView-nose", manager);
                    }
                }
                _isInitialize = true;
            }
            await _next.Invoke(httpContext);
        }

        private async Task AddImageToDb(string path, string description, PictureManager manager)
        {
            var picture = new PictureDTO()
            {
                Name = Guid.NewGuid().ToString(),
                Description = description,
                Format = "svg",
                Image = File.ReadAllBytes(path).ToArray()
            };
            await manager.AddAsync(picture);
        }
    }
}
