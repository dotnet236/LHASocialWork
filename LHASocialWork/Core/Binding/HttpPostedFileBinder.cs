using System.Web;
using System.Web.Mvc;

namespace LHASocialWork.Core.Binding
{
    public class HttpPostedFileBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var postedFiles = ((HttpPostedFileBase[])bindingContext.ValueProvider.GetValue(bindingContext.ModelName).RawValue);
            if(postedFiles == null || postedFiles.Length == 0 || postedFiles[0] == null) return null;
            var postedFile = postedFiles[0];

            return new Models.Shared.HttpPostedFile
                       {
                           ContentLength = postedFile.ContentLength,
                           ContentType = postedFile.ContentType,
                           FileName = postedFile.FileName,
                           InputStream = postedFile.InputStream
                       };
        }
    }
}