using System;
using System.Linq;

namespace LHASocialWork.Entities.Enumerations
{
    public enum ImageSizes
    {
        [Size(40, 80)]
        W40xL80,
        [Size(44,44)]
        W44xL44,
        [Size(80, 40)]
        W80xL40,
        [Size(87,87)]
        W87xL87,
        [Size(100, 200)]
        W100xL200,
        [Size(180, 200)]
        W180xL200,
        [Size(200, 100)]
        W200xL100,

    }

    public static class ImageSizesExtensions
    {
        public static ImageSize Size(this ImageSizes size)
        {
            var imageSizesType = Enum.GetUnderlyingType(typeof(ImageSizes));
            var sizeAttributeType = typeof(SizeAttribute);
            var field = typeof(ImageSizes).GetFields().FirstOrDefault(x => x.GetCustomAttributes(true).Any() && ((ImageSizes)Convert.ChangeType(x.GetValue(null), imageSizesType)) == size);
            var sizeAttribute = field.GetCustomAttributes(true).FirstOrDefault(y => y.GetType() == sizeAttributeType) as SizeAttribute;
            return sizeAttribute != null ? sizeAttribute.ImageSize : new ImageSize(0, 0);
        }
    }
}
