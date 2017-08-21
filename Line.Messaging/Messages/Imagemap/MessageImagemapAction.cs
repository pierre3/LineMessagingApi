namespace Line.Messaging
{
    public class MessageImagemapAction : IImagemapAction
    {
        public ImagemapActionType Type { get; } = ImagemapActionType.Uri;

        public ImagemapArea Area { get; }

        public string Text { get; }

        public MessageImagemapAction(ImagemapArea area, string text)
        {
            Area = area;
            Text = text;
        }
    }
}
