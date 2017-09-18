using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Line.Messaging
{
    public class ImagemapMessage : ISendMessage
    {
        public MessageType Type { get; } = MessageType.Imagemap;

        public string BaseUrl { get; }

        public string AltText { get; }

        public ImagemapSize BaseSize { get; }

        public IList<IImagemapAction> Actions { get; }

        public ImagemapMessage(string baseUrl, string altText, ImagemapSize baseSize, IList<IImagemapAction> actions)
        {
            BaseUrl = baseUrl;
            AltText = altText;
            BaseSize = baseSize;
            Actions = actions;
        }
    }

}
