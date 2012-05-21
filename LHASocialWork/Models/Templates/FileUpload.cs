using System;

namespace LHASocialWork.Models.Templates
{
    public class FileUpload : EditorTemplate
    {
        public FileUpload() { PropertyName = "File"; FileChanged = false; }
        public bool FileChanged { get; set;  }
    }

    public class ImageFileUpload : FileUpload
    {
        public DisplayImage Image { get; set; }
    }
}