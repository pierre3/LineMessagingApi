using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging.Liff
{
    public class LiffApp
    {
        public string LiffId { get; }
        public View View { get; }

        public LiffApp(string liffId, View view)
        {
            LiffId = liffId;
            View = view;
        }
    }
}
