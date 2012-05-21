using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Services.Image;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Drawing = System.Drawing;

namespace LHASocialWork.Tests.Services
{
    [TestClass]
    public class ImageServiceTest : BaseServiceTest
    {
        private ImageService _imageService;

        private void SetupResizeImage()
        {
            _imageService = new ImageService(MockBaseRepository.Object);
        }

        [TestMethod]
        public void ResizeImage()
        {
            SetupResizeImage();

            #region OldWidth > NewWidth & OldWidth > OldHeight

            var image = ResizeImage(200, 100, ImageSizes.W80xL40);
            Assert.IsTrue(image.Width == 80 && image.Height == 40);

            #endregion

            #region OldWidth > NewWidth & OldWidth < OldHeight

            image = ResizeImage(200, 100, ImageSizes.W40xL80);
            Assert.IsTrue(image.Width == 40 && image.Height == 20);

            #endregion

            #region OldWidth < NewWidth & OldWidth > OldHeight

            image = ResizeImage(80, 40, ImageSizes.W200xL100);
            Assert.IsTrue(image.Width == 200 && image.Height == 100);

            #endregion

            #region OldWidth < NewWidth & OldWidth < OldHeight

            image = ResizeImage(80, 40, ImageSizes.W100xL200);
            Assert.IsTrue(image.Width == 100 && image.Height == 200);

            #endregion
        }

        private Image ResizeImage(int oldWidth, int oldHeight, ImageSizes imageSize)
        {
            var image = new Bitmap(oldWidth, oldHeight);
            var memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Png);
            memoryStream = _imageService.ResizeImage(memoryStream, imageSize);
            return System.Drawing.Image.FromStream(memoryStream);
        }
    }
}
