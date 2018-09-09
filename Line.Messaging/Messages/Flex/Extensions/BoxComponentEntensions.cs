using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    public static class BoxComponentExtensions
    {
        public static BoxComponent AddContents(this BoxComponent self, IFlexComponent component)
        {
            self.Contents.Add(component);
            return self;
        }


    }
}
