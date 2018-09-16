using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    public class SpacerComponent : IFlexComponent
    {
        public FlexComponentType Type => FlexComponentType.Spacer;

        /// <summary>
        /// Size of the space.(Required) <para> 
        /// You can specify one of the following values: xs, sm, md, lg, xl, or xxl. 
        /// The size increases in the order of listing. The default value is md.</para>
        /// <para>(Required)</para>
        /// </summary>
        public ComponentSize Size { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="size">
        /// /// Size of the space.(Required) <para> 
        /// You can specify one of the following values: xs, sm, md, lg, xl, or xxl. 
        /// The size increases in the order of listing. The default value is md.</para>
        /// </param>
        public SpacerComponent(ComponentSize size)
        {
            Size = size;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SpacerComponent()
        {

        }


    }
}
