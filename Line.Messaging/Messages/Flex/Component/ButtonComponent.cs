using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    /// <summary>
    /// This component draws a button. When the user taps a button, a specified action is performed.
    /// </summary>
    public class ButtonComponent : IFlexComponent
    {
        public FlexComponentType Type => FlexComponentType.Button;

        /// <summary>
        /// Action performed when this button is tapped. Specify an action object.
        /// <para>(Required)</para>
        /// </summary>
        public ITemplateAction Action { get; set; }

        /// <summary>
        /// The ratio of the width or height of this component within the parent box. 
        /// The default value for the horizontal parent box is 1, and the default value for the vertical parent box is 0. For more information, see Width and height of components.
        /// <para>(Optional)</para>
        /// </summary>
        public int? Flex { get; set; }

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
        /// Height of the button. You can specify sm or md. The default value is md.
        /// <para>(Optional)</para>
        /// </summary>
        public ButtonHeight? Height { get; set; }

        /// <summary>
        /// Style of the button.<para> 
        /// Specify one of the following values:
        /// - link: HTML link style
        /// - primary: Style for dark color buttons
        /// - secondary: Style for light color buttons</para>
        /// The default value is link.
        /// <para>(Optional)</para>
        /// </summary>
        public ButtonStyle? Style { get; set; }

        /// <summary>
        /// Character color when the style property is link. 
        /// Background color when the style property is primary or secondary. Use a hexadecimal color code.
        /// <para>(Optional)</para>
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Vertical alignment style.<para> 
        /// Specify one of the following values:
        /// - top: Top-aligned
        /// - bottom: Bottom-aligned
        /// - center: Center-aligned</para>
        /// The default value is top.
        /// If the layout property of the parent box is baseline, the gravity property will be ignored.
        /// <para>(Optional)</para>
        /// </summary>
        public Gravity? Gravity { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="action">
        /// Action performed when this button is tapped. Specify an action object.
        /// </param>
        public ButtonComponent(ITemplateAction action)
        {
            Action = action;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ButtonComponent()
        {

        }
    }
}
