using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Line.Messaging
{
    [JsonConverter(typeof(AspectRatioStringEnumConverter))]
    public enum AspectRatio
    {
        _1_1,
        _151_1,
        _191_1,
        _4_3,
        _16_9,
        _20_13,
        _2_1,
        _3_1,
        _3_4,
        _9_16,
        _1_2,
        _1_3
    }

    internal class AspectRatioStringEnumConverter : CustomStringEnumConverter<AspectRatio>
    {
        public AspectRatioStringEnumConverter()
            : base(new Dictionary<AspectRatio, string>()
            {
                [AspectRatio._1_1] = "1:1",
                [AspectRatio._151_1] = "1.51:1",
                [AspectRatio._191_1] = "1.91:1",
                [AspectRatio._4_3] = "4:3",
                [AspectRatio._16_9] = "16:9",
                [AspectRatio._20_13] = "20:13",
                [AspectRatio._2_1] = "2:1",
                [AspectRatio._3_1] = "3:1",
                [AspectRatio._3_4] = "3:4",
                [AspectRatio._9_16] = "9:16",
                [AspectRatio._1_2] = "1:2",
                [AspectRatio._1_3] = "1:3",
            })
        {
        }
    }
}
