using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    /// <summary>
    /// This is a component that defines the layout of child components. You can also include a box in a box.
    /// </summary>
    public class BoxComponent : IFlexComponent
    {
        /// <summary>
        /// Box
        /// </summary>
        public FlexComponentType Type => FlexComponentType.Box;

        /// <summary>
        /// The placement style of components in this box. Specify one of the following values
        /// </summary>
        /// <param name="layout">
        /// The placement style of components in this box. Specify one of the following values
        /// </param>
        public BoxComponent(BoxLayout layout)
        {
            Layout = layout;
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public BoxComponent() { }

        /// <summary>
        /// The placement style of components in this box. Specify one of the following values
        /// <para>(Required)</para>
        /// </summary>
        public BoxLayout Layout { get; set; }

        /// <summary>
        /// Components in this box. Here are the types of components available:<para>
        /// - When the layout property is horizontal or vertical: Box, button, filler, image, separator, and text components</para><para>
        /// - When the layout property is baseline: filler, icon, and text components</para>
        /// <para>(Required)</para>
        /// </summary>
        public IList<IFlexComponent> Contents { get; set; } = new List<IFlexComponent>();

        /// <summary>
        /// The ratio of the width or height of this box within the parent box. 
        /// The default value for the horizontal parent box is 1, and the default value for the vertical parent box is 0. 
        /// <para>(Optional)</para>
        /// </summary>
        public int? Flex { get; set; }

        /// <summary>
        /// Minimum space between components in this box. 
        /// You can specify one of the following values: none, xs, sm, md, lg, xl, or xxl. 
        /// none does not set a space while the other values set a space whose size increases in the order of listing. <para>
        /// The default value is none. 
        /// To override this setting for a specific component, set the margin property of that component.</para>
        /// <para>(Optional)</para>
        /// </summary>
        public Spacing? Spacing { get; set; }

        /// <summary>
        /// Minimum space between this box and the previous component in the parent box. 
        /// You can specify one of the following values: none, xs, sm, md, lg, xl, or xxl. 
        /// none does not set a space while the other values set a space whose size increases in the order of listing. <para>
        /// The default value is the value of the spacing property of the parent box. 
        /// If this box is the first component in the parent box, the margin property will be ignored.</para>
        /// <para>(Optional)</para>
        /// </summary>
        public Spacing? Margin { get; set; }

        /// <summary>
        /// Action performed when this box is tapped.
        /// Specify an action object. This property is supported on the following versions of LINE.<para>
        /// / LINE for iOS and Android: 8.11.0 and later
        /// / LINE for Windows and macOS: 5.9.0 and later
        /// </para>
        /// </summary>
        public ITemplateAction Action { get; set; }
    }
}
