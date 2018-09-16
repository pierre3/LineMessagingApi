using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    /// <summary>
    /// This component draws a separator between components in the parent box.
    /// </summary>
    public class SeparatorComponent : IFlexComponent
    {
        public FlexComponentType Type => FlexComponentType.Separator;

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
        /// Color of the separator. Use a hexadecimal color code.
        /// <para>(Optional)</para>
        /// </summary>
        public string Color { get; set; }
    }
}
