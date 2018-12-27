using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Line.Messaging
{
    /// <summary>
    /// Aspect ratio of the image. 
    /// Specify in the {width}:{height} format. <para>
    /// Specify the value of the {width} property and the {height} property in the range from 1 to 100000. However, 
    /// you cannot set the {height} property to a value that is more than three times the value of the {width} property. </para>
    /// The default value is 1:1.
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class AspectRatio
    {
        /// <summary>1:1</summary>
        public static readonly AspectRatio _1_1 = new AspectRatio(1, 1);
        /// <summary>1.51:1</summary>
        public static readonly AspectRatio _151_1 = new AspectRatio(151, 100);
        /// <summary>1.91:1</summary>
        public static readonly AspectRatio _191_1 = new AspectRatio(191, 100);
        /// <summary>4:3</summary>
        public static readonly AspectRatio _4_3 = new AspectRatio(4, 3);
        /// <summary>16:9</summary>
        public static readonly AspectRatio _16_9 = new AspectRatio(16, 9);
        /// <summary>20:13</summary>
        public static readonly AspectRatio _20_13 = new AspectRatio(20, 13);
        /// <summary>2:1</summary>
        public static readonly AspectRatio _2_1 = new AspectRatio(2, 1);
        /// <summary>3:1</summary>
        public static readonly AspectRatio _3_1 = new AspectRatio(3, 1);
        /// <summary>3:4</summary>
        public static readonly AspectRatio _3_4 = new AspectRatio(3, 4);
        /// <summary>9:16</summary>
        public static readonly AspectRatio _9_16 = new AspectRatio(9, 16);
        /// <summary>1:2</summary>
        public static readonly AspectRatio _1_2 = new AspectRatio(1, 2);
        /// <summary>1:3</summary>
        public static readonly AspectRatio _1_3 = new AspectRatio(1, 3);

        private readonly int _width;
        private readonly int _height;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">Width of aspect ratio</param>
        /// <param name="height">Height of aspect ratio</param>
        public AspectRatio(int width, int height)
        {
            if (width < 1 || width > 100000) { throw new ArgumentException($"The {nameof(width)} property must be in range from 1 to 100000.", nameof(width)); }
            if (height < 1 || height > 100000) { throw new ArgumentException($"The {nameof(height)} property must be in range from 1 to 100000.", nameof(height)); }
            if(height > width * 3) { throw new ArgumentException($"Cannot set the {nameof(height)} property to a value that is more than three times the value of the {nameof(width)} property."); }

            _width = width;
            _height = height;
        }
        public override string ToString()
        {
            return _width + ":" + _height;
        }
    }

    
}
