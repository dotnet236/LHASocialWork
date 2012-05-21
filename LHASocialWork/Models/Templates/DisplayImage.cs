using System;
using System.IO;
using LHASocialWork.Core;
using LHASocialWork.Entities.Enumerations;

namespace LHASocialWork.Models.Templates
{
    public class DisplayImage
    {
        public DisplayImage(Guid fileKey, string title)
        {
            FileKey = fileKey;
            Title = title;
        }

        public DisplayImage(Guid fileKey, string title, ImageSizes imageSize)
        {
            FileKey = fileKey;
            Title = title;
            ImageSize = imageSize;
        }

        public Guid FileKey { get; set; }
        public ImageSizes ImageSize { get; set; }
        public string Title { get; set; }

        public string WebPath
        {
            get
            {
                var directoryName = FileKey.ToString().Replace("-", "");
                var fileName = Path.ChangeExtension(Bootstrapper.LocalImageName, "png");
                return Path.Combine(Bootstrapper.WebImagesPath, directoryName, ImageSize.ToString(), fileName).Replace("\\", "/");
            }
        }
    }
}