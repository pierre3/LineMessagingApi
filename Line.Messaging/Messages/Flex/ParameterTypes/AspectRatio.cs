using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Line.Messaging
{
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class AspectRatio
    {
        public static readonly AspectRatio _1_1 = new AspectRatio(1, 1);
        public static readonly AspectRatio _151_1 = new AspectRatio(151, 100);
        public static readonly AspectRatio _191_1 = new AspectRatio(191, 100);
        public static readonly AspectRatio _4_3 = new AspectRatio(4, 3);
        public static readonly AspectRatio _16_9 = new AspectRatio(16, 9);
        public static readonly AspectRatio _20_13 = new AspectRatio(20, 13);
        public static readonly AspectRatio _2_1 = new AspectRatio(2, 1);
        public static readonly AspectRatio _3_1 = new AspectRatio(3, 1);
        public static readonly AspectRatio _3_4 = new AspectRatio(3, 4);
        public static readonly AspectRatio _9_16 = new AspectRatio(9, 16);
        public static readonly AspectRatio _1_2 = new AspectRatio(1, 2);
        public static readonly AspectRatio _1_3 = new AspectRatio(1, 3);

        private readonly int _width;
        private readonly int _height;

        public AspectRatio(int width, int height)
        {
            _width = width;
            _height = height;
        }
        public override string ToString()
        {
            return _width + ":" + _height;
        }
    }

    
}
