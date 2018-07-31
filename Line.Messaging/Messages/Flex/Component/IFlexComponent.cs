using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    public interface IFlexComponent
    {
        FlexComponentType Type { get; }
    }
}
