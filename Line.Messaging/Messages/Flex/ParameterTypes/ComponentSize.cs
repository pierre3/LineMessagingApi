using Newtonsoft.Json;
using System.Collections.Generic;

namespace Line.Messaging
{
    [JsonConverter(typeof(ComponentSizeEnumConverter))]
    public enum ComponentSize
    {
        Xxs,
        Xs,
        Sm,
        Md,
        Lg,
        Xl,
        Xxl,
        _3xl,
        _4xl,
        _5xl,
        Full
    }

    internal class ComponentSizeEnumConverter : CustomStringEnumConverter<ComponentSize>
    {
        public ComponentSizeEnumConverter() : base(new Dictionary<ComponentSize, string>
        {
            [ComponentSize._3xl] = "3xl",
            [ComponentSize._4xl] = "4xl",
            [ComponentSize._5xl] = "5xl",
        })
        {
            CamelCaseText = true;
        }
    }

}