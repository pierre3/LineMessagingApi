using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    public class TextComponent : IFlexComponent
    {
        public FlexComponentType Type => FlexComponentType.Text;

        /// <summary>
        /// Text
        /// <para>(Required)</para>
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The ratio of the width or height of this component within the parent box.<para>
        /// The default value for the horizontal parent box is 1, and the default value for the vertical parent box is 0. 
        /// For more information, see Width and height of components.</para>
        /// <para>(Optional)</para>
        /// </summary>
        public int? Flex { get; set; }

        /// <summary>
        /// Minimum space between this component and the previous component in the parent box.<para> 
        /// You can specify one of the following values: none, xs, sm, md, lg, xl, or xxl. 
        /// none does not set a space while the other values set a space whose size increases in the order of listing. 
        /// The default value is the value of the spacing property of the parent box. 
        /// If this component is the first component in the parent box, the margin property will be ignored.</para>
        /// <para>(Optional)</para>
        /// </summary>
        public Spacing? Margin { get; set; }

        /// <summary>
        /// Font size.<para> 
        /// You can specify one of the following values: xxs, xs, sm, md, lg, xl, xxl, 3xl, 4xl, or 5xl. 
        /// The size increases in the order of listing. 
        /// The default value is md.</para>
        /// <para>(Optional)</para>
        /// </summary>
        public ComponentSize? Size { get; set; }

        /// <summary>
        /// Horizontal alignment style.<para>
        /// Specify one of the following values:
        /// / start: Left-aligned
        /// / end: Right-aligned
        /// / center: Center-aligned
        /// , The default value is start.</para>
        /// <para>(Optional)</para>
        /// </summary>
        public Align? Align { get; set; }

        /// <summary>
        /// Vertical alignment style.<para> 
        /// Specify one of the following values:
        /// / top: Top-aligned
        /// / bottom: Bottom-aligned
        /// / center: Center-aligned
        /// The default value is top.</para>
        /// If the layout property of the parent box is baseline, the gravity property will be ignored.
        /// <para>(Optional)</para>
        /// </summary>
        public Gravity? Gravity { get; set; }

        /// <summary>
        /// true to wrap text. The default value is false. If set to true, you can use a new line character (\n) to begin on a new line.
        /// <para>(Optional)</para>
        /// </summary>
        public bool? Wrap { get; set; }

        /// <summary>
        /// Max number of lines. <para>
        /// If the text does not fit in the specified number of lines, an ellipsis (…) is displayed at the end of the last line. 
        /// If set to 0, all the text is displayed. 
        /// , The default value is 0.</para> 
        /// This property is supported on the following versions of LINE.
        /// LINE for iOS and Android: 8.11.0 and later
        /// LINE for Windows and macOS: 5.9.0 and later
        /// <para>(Optional)</para>
        /// </summary>
        public int? MaxLines { get; set; }

        /// <summary>
        /// Font weight. You can specify one of the following values: regular, or bold. Specifying bold makes the font bold. The default value is regular.
        /// <para>(Optional)</para>
        /// </summary>
        public Weight? Weight { get; set; }

        /// <summary>
        /// Font weight. You can specify one of the following values: regular, or bold. Specifying bold makes the font bold. The default value is regular.
        /// <para>(Optional)</para>
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Action performed when this text is tapped. Specify an action object.
        /// <para>(Optional)</para>
        /// </summary>
        public ITemplateAction Action { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">Text</param>
        public TextComponent(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TextComponent()
        {

        }
        
    }
    
}
