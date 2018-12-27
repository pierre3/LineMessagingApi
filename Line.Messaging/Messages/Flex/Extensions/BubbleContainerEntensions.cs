using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    public static class BubbleContainerExtensions
    {
        /// <summary>
        /// Sets a BoxComponent to the BubbleContainer header
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="header">BoxComponent</param>
        /// <returns>BubbleContainer</returns>
        public static BubbleContainer SetHeader(this BubbleContainer self, BoxComponent header)
        {
            self.Header = header;
            return self;
        }

        /// <summary>
        /// Sets a BoxComponent to the BubbleContainer header
        /// </summary>
        /// <param name="boxLayout">
        /// The placement style of components in this box. Specify one of the following values
        /// </param>
        /// <param name="self">BubbleContainer</param>
        /// <param name="flex">
        /// The ratio of the width or height of this box within the parent box. 
        /// The default value for the horizontal parent box is 1, and the default value for the vertical parent box is 0. 
        /// </param>
        /// <param name="spacing">
        /// Minimum space between components in this box. 
        /// You can specify one of the following values: none, xs, sm, md, lg, xl, or xxl. 
        /// none does not set a space while the other values set a space whose size increases in the order of listing. <para>
        /// The default value is none. 
        /// To override this setting for a specific component, set the margin property of that component.</para>
        /// </param>
        /// <param name="margin">
        /// Minimum space between this box and the previous component in the parent box. 
        /// You can specify one of the following values: none, xs, sm, md, lg, xl, or xxl. 
        /// none does not set a space while the other values set a space whose size increases in the order of listing. <para>
        /// The default value is the value of the spacing property of the parent box. 
        /// If this box is the first component in the parent box, the margin property will be ignored.</para>
        /// </param>
        /// <returns>BoxContainer</returns>
        public static BubbleContainer SetHeader(this BubbleContainer self, BoxLayout boxLayout,
            int? flex = null, Spacing? spacing = null, Spacing? margin = null)
        {
            self.Header = new BoxComponent(boxLayout)
            {
                Flex = flex,
                Spacing = spacing,
                Margin = margin
            };
            return self;
        }

        /// <summary>
        /// Add a flex component to BubbleContainer header
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="component">Flex Conmonent</param>
        /// <returns>BubbleContainer</returns>
        public static BubbleContainer AddHeaderContents(this BubbleContainer self, IFlexComponent component)
        {
            if (self.Header == null) { throw new InvalidOperationException("Header not exists."); }
            self.Header.Contents.Add(component);
            return self;
        }

        /// <summary>
        /// Sets a BoxComponent to the BubbleContainer body
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="body">Box Component</param>
        /// <returns>BubbleContainer</returns>
        public static BubbleContainer SetBody(this BubbleContainer self, BoxComponent body)
        {
            self.Body = body;
            return self;
        }

        /// <summary>
        /// Sets a BoxComponent to the BubbleContainer body
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="boxLayout">
        /// The placement style of components in this box. Specify one of the following values
        /// </param>
        /// <param name="flex">
        /// The ratio of the width or height of this box within the parent box. 
        /// The default value for the horizontal parent box is 1, and the default value for the vertical parent box is 0. 
        /// </param>
        /// <param name="spacing">
        /// Minimum space between components in this box. 
        /// You can specify one of the following values: none, xs, sm, md, lg, xl, or xxl. 
        /// none does not set a space while the other values set a space whose size increases in the order of listing. <para>
        /// The default value is none. 
        /// To override this setting for a specific component, set the margin property of that component.</para>
        /// </param>
        /// <param name="margin">
        /// Minimum space between this box and the previous component in the parent box. 
        /// You can specify one of the following values: none, xs, sm, md, lg, xl, or xxl. 
        /// none does not set a space while the other values set a space whose size increases in the order of listing. <para>
        /// The default value is the value of the spacing property of the parent box. 
        /// If this box is the first component in the parent box, the margin property will be ignored.</para>
        /// </param>
        /// <returns>BoxContainer</returns>
        public static BubbleContainer SetBody(this BubbleContainer self, BoxLayout boxLayout,
            int? flex = null, Spacing? spacing = null, Spacing? margin = null)
        {
            self.Body = new BoxComponent(boxLayout)
            {
                Flex = flex,
                Spacing = spacing,
                Margin = margin
            };
            return self;
        }

        /// <summary>
        /// Add a flex component to BubbleContainer body
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="component">Flex Component</param>
        /// <returns>BubbleContainer</returns>
        public static BubbleContainer AddBodyContents(this BubbleContainer self, IFlexComponent component)
        {
            if (self.Body == null) { throw new InvalidOperationException("Body not exists."); }
            self.Body.Contents.Add(component);
            return self;
        }

        /// <summary>
        /// Sets a BoxComponent to the BubbleContainer footer
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="footer">BoxComponent</param>
        /// <returns>BubbleContainer</returns>
        public static BubbleContainer SetFooter(this BubbleContainer self, BoxComponent footer)
        {
            self.Footer = footer;
            return self;
        }

        /// <summary>
        /// Sets a BoxComponent to the BubbleContainer footer
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="boxLayout">
        /// The placement style of components in this box. Specify one of the following values
        /// </param>
        /// <param name="flex">
        /// The ratio of the width or height of this box within the parent box. 
        /// The default value for the horizontal parent box is 1, and the default value for the vertical parent box is 0. 
        /// </param>
        /// <param name="spacing">
        /// Minimum space between components in this box. 
        /// You can specify one of the following values: none, xs, sm, md, lg, xl, or xxl. 
        /// none does not set a space while the other values set a space whose size increases in the order of listing. <para>
        /// The default value is none. 
        /// To override this setting for a specific component, set the margin property of that component.</para>
        /// </param>
        /// <param name="margin">
        /// Minimum space between this box and the previous component in the parent box. 
        /// You can specify one of the following values: none, xs, sm, md, lg, xl, or xxl. 
        /// none does not set a space while the other values set a space whose size increases in the order of listing. <para>
        /// The default value is the value of the spacing property of the parent box. 
        /// If this box is the first component in the parent box, the margin property will be ignored.</para>
        /// </param>
        /// <returns>BoxContainer</returns>
        public static BubbleContainer SetFooter(this BubbleContainer self, BoxLayout boxLayout,
           int? flex = null, Spacing? spacing = null, Spacing? margin = null)
        {
            self.Footer = new BoxComponent(boxLayout)
            {
                Flex = flex,
                Spacing = spacing,
                Margin = margin
            };
            return self;
        }

        /// <summary>
        /// Add a flexComponent to BubbleContainer footer
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="component">Flex Comopnent</param>
        /// <returns>BubbleContainer</returns>
        public static BubbleContainer AddFooterContents(this BubbleContainer self, IFlexComponent component)
        {
            if (self.Footer == null) { throw new InvalidOperationException("Footer not exists."); }
            self.Footer.Contents.Add(component);
            return self;
        }

        /// <summary>
        /// Sets a ImageComponent to the BobbleContainer hero 
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="hero">
        /// ImageComponent
        /// </param>
        /// <para>BubbleContainer</para>
        public static BubbleContainer SetHero(this BubbleContainer self, ImageComponent hero)
        {
            self.Hero = hero;
            return self;
        }

        /// <summary>
        /// Sets a ImageComponent to the BobbleContainer hero 
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="imageUrl">
        /// Image URL (Required)<para>
        /// / Protocol: HTTPS
        /// / Image format: JPEG or PNG
        /// / Maximum image size: 1024×1024 pixels
        /// / Maximum data size: 1 MB</para>
        /// </param>
        /// <param name="flex">
        /// The ratio of the width or height of this component within the parent box. <para>
        /// The default value for the horizontal parent box is 1, and the default value for the vertical parent box is 0. 
        /// For more information, see Width and height of components.</para>
        /// </param>
        /// <param name="margin">
        /// Minimum space between this component and the previous component in the parent box.<para> 
        /// You can specify one of the following values: none, xs, sm, md, lg, xl, or xxl. 
        /// none does not set a space while the other values set a space whose size increases in the order of listing. 
        /// The default value is the value of the spacing property of the parent box. 
        ///If this component is the first component in the parent box, the margin property will be ignored.</para>
        /// </param>
        /// <param name="align">
        /// Horizontal alignment style.<para> 
        /// Specify one of the following values:
        /// / start: Left-aligned
        /// / end: Right-aligned
        /// / center: Center-aligned
        /// , The default value is center.</para>
        /// </param>
        /// <param name="gravity">
        /// Vertical alignment style.<para>
        /// Specify one of the following values:
        /// / top: Top-aligned
        /// / bottom: Bottom-aligned
        /// / center: Center-aligned
        /// , The default value is top.</para><para>
        /// If the layout property of the parent box is baseline, the gravity property will be ignored.</para>
        /// </param>
        /// <param name="size">
        /// Maximum size of the image width.<para> 
        /// You can specify one of the following values: xxs, xs, sm, md, lg, xl, xxl, 3xl, 4xl, 5xl, or full. 
        /// The size increases in the order of listing. 
        /// The default value is md.</para>
        /// </param>
        /// <param name="aspectRatio">
        /// Aspect ratio of the image. 
        /// Specify in the {width}:{height} format. <para>
        /// Specify the value of the {width} property and the {height} property in the range from 1 to 100000. However, 
        /// you cannot set the {height} property to a value that is more than three times the value of the {width} property. </para>
        /// The default value is 1:1.
        /// </param>
        /// <param name="aspectMode">
        /// Style of the image.<para> 
        /// Specify one of the following values:
        /// / cover: The image fills the entire drawing area.Parts of the image that do not fit in the drawing area are not displayed.
        /// / fit: The entire image is displayed in the drawing area.The background is displayed in the unused areas to the left and right of vertical images and in the areas above and below horizontal images.
        /// The default value is fit.</para>
        /// </param>
        /// <param name="backgroundColor">
        /// Background color of the image. Use a hexadecimal color code.
        /// </param>
        /// <returns>BubbleContainer</returns>
        public static BubbleContainer SetHero(this BubbleContainer self, string imageUrl,
            int? flex = null, Spacing? margin = null, Align? align = null,
            Gravity? gravity = null, ComponentSize? size = null, AspectRatio aspectRatio = null,
            AspectMode? aspectMode = null, string backgroundColor = null)
        {
            self.Hero = new ImageComponent(imageUrl)
            {
                Flex = flex,
                Margin = margin,
                Align = align,
                Gravity = gravity,
                Size = size,
                AspectRatio = aspectRatio,
                AspectMode = aspectMode,
                BackgroundColor = backgroundColor
            };
            return self;
        }

        /// <summary>
        /// Sets a action to the BubbleContainer hero
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="action">ITemplateAction object</param>
        /// <returns>BubbleContainer</returns>
        public static BubbleContainer SetHeroAction(this BubbleContainer self, ITemplateAction action)
        {
            if (self.Hero == null) { throw new InvalidOperationException("Hero not exists."); }
            self.Hero.Action = action;
            return self;
        }

        /// <summary>
        /// Sets styles to the BubbleContainer
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="styles">Bubble container style</param>
        /// <returns>BubbleContainer</returns>
        public static BubbleContainer SetStyles(this BubbleContainer self, BubbleStyles styles)
        {
            self.Styles = styles;
            return self;
        }

        /// <summary>
        /// Sets style to the header of BubbleContainer
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="style">BlockStlye</param>
        /// <returns>BubbleContainer</returns>
        public static BubbleContainer SetHeaderStyle(this BubbleContainer self, BlockStyle style)
        {
            if (self.Styles == null)
            {
                self.Styles = new BubbleStyles();
            }
            self.Styles.Header = style;
            return self;
        }

        /// <summary>
        /// Sets style to the hero of BubbleContainer
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="style">BlockStlye</param>
        /// <returns>BubbleContainer</returns>
        public static BubbleContainer SetHeroStyle(this BubbleContainer self, BlockStyle style)
        {
            if (self.Styles == null)
            {
                self.Styles = new BubbleStyles();
            }
            self.Styles.Hero = style;
            return self;
        }

        /// <summary>
        /// Sets style to the body of BubbleContainer
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="style">BlockStlye</param>
        /// <returns>BubbleContainer</returns>
        public static BubbleContainer SetBodyStyle(this BubbleContainer self, BlockStyle style)
        {
            if (self.Styles == null)
            {
                self.Styles = new BubbleStyles();
            }
            self.Styles.Body = style;
            return self;
        }

        /// <summary>
        /// Sets style to the footer of BubbleContainer
        /// </summary>
        /// <param name="self">BubbleContainer</param>
        /// <param name="style">BlockStlye</param>
        /// <returns>BubbleContainer</returns>
        public static BubbleContainer SetFooterStyle(this BubbleContainer self, BlockStyle style)
        {
            if (self.Styles == null)
            {
                self.Styles = new BubbleStyles();
            }
            self.Styles.Footer = style;
            return self;
        }
    }
}
