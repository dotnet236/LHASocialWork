using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using LHASocialWork.Core.Extensions;
using LHASocialWork.Entities.Enumerations;

namespace LHASocialWork.Core.DataAnnotations
{
    public class ImageFileAttribute : ValidationAttribute
    {
        public ImageFileAttribute()
        {
            ValidateCollections = true;
        }

        public new bool IsValid(object value)
        {
            var image = value as HttpPostedFile;
            if(image != null)
            {
                if(EnumExtensions.Contains<ImageExtensions>(Path.GetExtension(image.FileName)))
                    return true;
                ErrorMessage = "Upload file must be an image.";
            }else
                ErrorMessage = "File is required.";

            return false;
        }

        public bool ValidateCollections { get; set; }
    }
}