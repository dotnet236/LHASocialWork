using System;
using System.IO;
using LHASocialWork.Entities.Enumerations;

namespace LHASocialWork.Services.Image
{
    public interface IImageService
    {
        Entities.Image DefaultUserProfileImage { get; }
        Entities.Image DefaultEventFlyerImage { get; }
        MemoryStream ResizeImage(Stream image, ImageSizes size);
        Entities.Image GetImageByFileKey(Guid fileKey);
        Entities.Image SaveImage(Entities.Image defaultUserProfileImage);
    }
}
