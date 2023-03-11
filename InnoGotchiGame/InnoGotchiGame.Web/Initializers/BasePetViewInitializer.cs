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
                await AddImagesToDb(dataModel.BodiesLinks, "petView-body", pictureManager);
                await AddImagesToDb(dataModel.EyesLinks, "petView-eyes", pictureManager);
                await AddImagesToDb(dataModel.MouthsLinks, "petView-mouth", pictureManager);
                await AddImagesToDb(dataModel.NosesLinks, "petView-nose", pictureManager);
            }
        }

        private static async Task AddImagesToDb(IList<string> paths, string description, PictureManager manager)
        {
            foreach (var path in paths)
            {
                await AddImageToDb(path, description, manager);
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
