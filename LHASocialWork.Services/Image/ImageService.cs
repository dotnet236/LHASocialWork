using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Repositories;
using LHASocialWork.Repositories.Criteria;
using LHASocialWork.Services.Base;

namespace LHASocialWork.Services.Image
{
    public class ImageService : BaseService, IImageService
    {
        public static readonly Guid DefaultUserProfileImageFileKey = new Guid("673f27c9bd95452ebf32453b25a85f3a");
        public static readonly Guid DefaultEventFlyerFileKey = new Guid("673f27c9bd95452ebf32453b25a85f3b");
        public Entities.Image DefaultUserProfileImage
        {
            get { return GetImageByFileKey(DefaultUserProfileImageFileKey); }
        }
        public Entities.Image DefaultEventFlyerImage
        {
            get { return GetImageByFileKey(DefaultEventFlyerFileKey); }
        }

        public ImageService(IBaseRepository baseRepository) : base(baseRepository)
        {
        }

        public MemoryStream ResizeImage(Stream imageStream, ImageSizes size)
        {
            var newHeight = size.Size().Height;
            var newWidth = size.Size().Width;

            var oldImage = System.Drawing.Image.FromStream(imageStream);
            double oldHeight = oldImage.Height;
            double oldWidth = oldImage.Width;

            var widthChange = Math.Abs(newWidth - oldWidth);
            var heightChange = Math.Abs(newHeight - oldHeight);

            var greatestChangeDimension = (widthChange > heightChange) ? Dimensions.Width : Dimensions.Height;

            if(greatestChangeDimension == Dimensions.Width)
            {
                var ratio  = (1/(oldWidth/oldHeight));
                newHeight = newWidth * ratio;
            }
            if(greatestChangeDimension == Dimensions.Height)
            {
                var ratio = (1/(oldWidth/oldHeight));
                newWidth = newHeight * ratio;
            }

            var newImage = new Bitmap(oldImage, new Size((int) Math.Floor(newWidth), (int) Math.Floor(newHeight)));
            var newImageMemoryStream = new MemoryStream();
            newImage.Save(newImageMemoryStream, ImageFormat.Png);

            return newImageMemoryStream;
        }

        public Entities.Image GetImageByFileKey(Guid fileKey)
        {
            var searchCriteria = new ImageSearchCriteria
                                     {
                                         WithFileyKeys = new[] {fileKey}
                                     };
            return Repository.List<Entities.Image>(searchCriteria.BuildCriteria()).FirstOrDefault();
        }

        public Entities.Image SaveImage(Entities.Image image)
        {
            return ValidateAndSave(image);
        }
    }
}