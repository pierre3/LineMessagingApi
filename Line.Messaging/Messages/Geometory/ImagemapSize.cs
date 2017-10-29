namespace Line.Messaging
{

    public class ImagemapSize
    {
        public static ImagemapSize RichMenuLong { get; } = new ImagemapSize(2500, 1686);
        public static ImagemapSize RichMenuShort { get; } = new ImagemapSize(2500, 843);

        public int Width { get; }

        public int Height { get; }

        public ImagemapSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
    
