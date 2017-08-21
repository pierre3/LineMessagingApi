using System.Collections.Generic;

namespace Line.Messaging
{
    public class ConfirmTemplate : ITemplate
    {
        public TemplateType Type { get; } = TemplateType.Confirm;

        public string Text { get; }

        public IList<ITemplateAction> Actions { get; }

        public ConfirmTemplate(string text, IList<ITemplateAction> actions)
        {
            Text = text;
            Actions = actions;
        }
    }
}
