using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Web.Initializers.Models;
using Microsoft.Extensions.Options;

namespace InnoGotchiGame.Web.Initializers
{
    public static class BasePetViewInitializer
    {
        public static async Task InvokeAsync(PictureManager pictureManager, IOptions<BasePetViewInitializeModel> options)
        {
                var filter = new PictureFiltrator()
                {
                    Description = "petView-nose"
                };
                if (!(await pictureManager.GetAllAsync(filter)).Any())
                {
                    var dataModel = options.Value;
                    foreach(var bodyLink in dataModel.BodiesLinks)
                    {
                        await AddImageToDb(bodyLink, "petView-body", pictureManager);
                    }
                    foreach (var eyesLink in dataModel.EyesLinks)
                    {
                        await AddImageToDb(eyesLink, "petView-eyes", pictureManager);
                    }
                    foreach (var mounthLink in dataModel.MouthsLinks)
                    {
                        await AddImageToDb(mounthLink, "petView-mouth", pictureManager);
                    }
                    foreach (var noseLink in dataModel.NosesLinks)
                    {
                        await AddImageToDb(noseLink, "petView-nose", pictureManager);
                    }
                }
        }

        private static async Task AddImageToDb(string path, string description, PictureManager manager)
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
