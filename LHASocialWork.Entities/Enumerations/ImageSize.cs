namespace LHASocialWork.Entities.Enumerations
{
    public class ImageSize
    {
        public double Height { get; set;}
        public double Width { get; set; }
        public double Length { get; set; }

        public ImageSize(double width, double height)
        {
            Width = width;
            Height = height;
        }
    }
}