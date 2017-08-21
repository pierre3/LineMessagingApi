namespace Line.Messaging
{
    public class UriImagemapAction : IImagemapAction
    {
        public ImagemapActionType Type { get; } = ImagemapActionType.Uri;

        public ImagemapArea Area { get; }

        public string LinkUri { get; }

        public UriImagemapAction(ImagemapArea area, string linkUri)
        {
            Area = area;
            LinkUri = linkUri;
        }
    }
    
}
