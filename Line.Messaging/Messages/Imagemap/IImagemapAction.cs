namespace Line.Messaging
{
    public interface IImagemapAction
    {
        ImagemapActionType Type { get; }
        ImagemapArea Area { get; }
    }    
}
