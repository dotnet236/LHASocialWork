using System;

namespace LHASocialWork.Entities.Enumerations
{
    public class SizeAttribute : Attribute
    {
        public ImageSize ImageSize { get; private set;}

        public SizeAttribute(double width, double length)
        {
            ImageSize = new ImageSize(width, length);
        }
    }
}