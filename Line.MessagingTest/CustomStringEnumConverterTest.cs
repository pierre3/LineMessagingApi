using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Line.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace Line.MessagingTest
{
    [TestClass]
    public class CustomStringEnumConverterTest
    {
        static CustomStringEnumConverterTest()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                };

                settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                return settings;
            };
        }

        [TestMethod]
        public void ComponentSizeDeserializeTest()
        {
            var jsonString = "{{ \"type\": \"icon\", \"url\": \"{0}\", \"size\" : \"{1}\" }}";
            var iconSizes = new[] { "xxs", "xs", "sm", "md", "lg", "xl", "xxl", "3xl", "4xl", "5xl" };
            string testUrl = "https://example.com/icon/png/caution.png";

            var expectedSizes = new[] { ComponentSize.Xxs, ComponentSize.Xs, ComponentSize.Sm, ComponentSize.Md, ComponentSize.Lg, ComponentSize.Xl, ComponentSize.Xxl, ComponentSize._3xl, ComponentSize._4xl, ComponentSize._5xl };
            foreach (var size in iconSizes.Zip(expectedSizes, (test, exp) => new { Test = test, Expected = exp }))
            {
                var icon = JsonConvert.DeserializeObject<IconComponent>(string.Format(jsonString, testUrl, size.Test));

                Assert.AreEqual(FlexComponentType.Icon, icon.Type);
                Assert.AreEqual(testUrl, icon.Url);
                Assert.AreEqual(size.Expected, icon.Size);
            }
        }

        [TestMethod]
        public void ComponentSizeSerializeTest()
        {
            var iconSize = new[] { ComponentSize.Xxs, ComponentSize.Xs, ComponentSize.Sm, ComponentSize.Md, ComponentSize.Lg, ComponentSize.Xl, ComponentSize.Xxl, ComponentSize._3xl, ComponentSize._4xl, ComponentSize._5xl };
            string testUrl = "https://example.com/icon/png/caution.png";

            var expectedSizes = new[] { "xxs", "xs", "sm", "md", "lg", "xl", "xxl", "3xl", "4xl", "5xl" };
            var expectedJson = "{{\"type\":\"icon\",\"url\":\"{0}\",\"size\":\"{1}\"}}";

            foreach (var size in iconSize.Zip(expectedSizes, (test, exp) => new { Test = test, Expected = exp }))
            {
                var test = new IconComponent(testUrl) { Size = size.Test, Margin = null };
                var json = JsonConvert.SerializeObject(test);

                Assert.AreEqual(string.Format(expectedJson, testUrl, size.Expected), json);
            }
        }
    }
}
