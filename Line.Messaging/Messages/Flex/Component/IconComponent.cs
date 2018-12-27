using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    public class IconComponent : IFlexComponent
    {
        public FlexComponentType Type => FlexComponentType.Icon;

        /// <summary>
        /// Image URL<para>
        /// Protocol: HTTPS
        /// / Image format: JPEG or PNG
        /// / Maximum image size: 240×240 pixels
        /// / Maximum data size: 1 MB
        /// </para>
        /// <para>(Required)</para>
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Minimum space between this component and the previous component in the parent box.<para> 
        /// You can specify one of the following values: none, xs, sm, md, lg, xl, or xxl. 
        /// none does not set a space while the other values set a space whose size increases in the order of listing.</para><para> 
        /// The default value is the value of the spacing property of the parent box.
        /// If this component is the first component in the parent box, the margin property will be ignored.</para>
        /// <para>(Optional)</para>
        /// </summary>
        public Spacing? Margin { get; set; }

        /// <summary>
        /// Maximum size of the icon width. <para>
        /// You can specify one of the following values: xxs, xs, sm, md, lg, xl, xxl, 3xl, 4xl, or 5xl. 
        /// The size increases in the order of listing. The default value is md.</para>
        /// <para>(Optional)</para>
        /// </summary>
        public ComponentSize? Size { get; set; }

        /// <summary>
        /// Aspect ratio of the image. 
        /// Specify in the {width}:{height} format. <para>
        /// Specify the value of the {width} property and the {height} property in the range from 1 to 100000. However, 
        /// you cannot set the {height} property to a value that is more than three times the value of the {width} property. </para>
        /// The default value is 1:1.
        /// <para>(Optional)</para>
        /// </summary>
        public AspectRatio AspectRatio { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url">
        /// Image URL<para>
        /// Protocol: HTTPS
        /// / Image format: JPEG or PNG
        /// / Maximum image size: 240×240 pixels
        /// / Maximum data size: 1 MB
        /// </para>
        /// </param>
        public IconComponent(string url)
        {
            Url = url;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public IconComponent()
        {

        }
    }
}
