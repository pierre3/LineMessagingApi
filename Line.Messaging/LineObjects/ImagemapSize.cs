namespace Line.Messaging
{
    /// <summary>
    /// Image size. 
    /// </summary>
    public class ImagemapSize
    {
        /// <summary>
        /// Default rich menu size
        /// </summary>
        public static ImagemapSize RichMenuLong { get; } = new ImagemapSize(2500, 1686);
        
        /// <summary>
        /// Half rich menu size.
        /// </summary>
        public static ImagemapSize RichMenuShort { get; } = new ImagemapSize(2500, 843);

        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; }

        public ImagemapSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
    
