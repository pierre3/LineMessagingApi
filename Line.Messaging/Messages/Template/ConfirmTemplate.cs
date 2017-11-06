using System.Collections.Generic;

namespace Line.Messaging
{
    /// <summary>
    /// Template message with two action buttons.
    /// </summary>
    public class ConfirmTemplate : ITemplate
    {
        public TemplateType Type { get; } = TemplateType.Confirm;

        /// <summary>
        /// Message text
        /// Max: 240 characters
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Action when tapped
        /// Set 2 actions for the 2 buttons
        /// </summary>
        public IList<ITemplateAction> Actions { get; }

        public ConfirmTemplate(string text, IList<ITemplateAction> actions)
        {
            Text = text;
            Actions = actions;
        }
    }
}
