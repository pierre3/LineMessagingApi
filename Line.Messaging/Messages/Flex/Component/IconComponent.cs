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
        /// Aspect ratio of the icon. You can specify one of the following values: 1:1, 2:1, or 3:1. The default value is 1:1.
        /// <para>(Optional)</para>
        /// </summary>
        public AspectRatio? AspectRatio { get; set; }

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
