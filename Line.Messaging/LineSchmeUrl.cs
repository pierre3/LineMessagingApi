using System;

namespace Line.Messaging
{
    public static class LineSchmeUrl
    {
        private static readonly string camera = "line://nv/camera/";
        private static readonly string cameraRollSingle = "line://nv/cameraRoll/single";
        private static readonly string cameraRollMulti = "line://nv/cameraRoll/multi";
        private static readonly string location = "line://nv/location";
        private static readonly string addFriend = "line://ti/p/{0}";
        private static readonly string recommendOA = "line://nv/recommendOA/{0}";
        private static readonly string main = "line://home/public/main?id={0}";
        private static readonly string profile = "line://home/public/profile?id={0}";
        private static readonly string post = "line://home/public/post?id={0}&postId={1}";
        private static readonly string msgText = "line://msg/text/{0}";
        private static readonly string oaMessage = "line://oaMessage/{0}/?{1}";

        /// <summary>
        /// Opens the camera screen.
        /// </summary>
        /// <returns>String of LINE Scheme URL</returns>
        public static string GetCameraUrl => camera;

        /// <summary>
        /// Opens the camera screen.
        /// </summary>
        /// <returns>URI template action</returns>
        public static UriTemplateAction GetCameraUriTemplateAction(string label) => new UriTemplateAction(label, GetCameraUrl);

        /// <summary>
        /// Opens the "Camera Roll" screen where users can select one image to share in the chat.
        /// </summary>
        /// <returns>String of LINE Scheme URL</returns>
        public static string GetCameraRollSingleUrl => cameraRollSingle;

        /// <summary>
        /// Opens the "Camera Roll" screen where users can select multiple images to share in the chat.
        /// </summary>
        /// <returns>String of LINE Scheme URL</returns>
        public static string GetCameraRollMultiUrl => cameraRollMulti;

        /// <summary>
        /// Opens the "Location" screen. Users can share the current location or drop a pin on the map to select the location they want to share.
        /// </summary>
        /// <remarks>
        /// Note: This scheme is only supported in one-on-one chats between a user and a bot(LINE@ account). 
        /// Not supported on external apps or other types of LINE chats.
        ///</remarks>
        /// <returns>String of LINE Scheme URL</returns>
        public static string GetLocationUrl => location;

        /// <summary>
        /// Opens one of the following screens depending on the user's friendship status with the bot.<para>
        /// Friend of bot: Opens the chat with the bot.</para><para>
        /// Not a friend or blocked by user: Opens the "Add friend" screen for your bot.</para>
        /// </summary>
        /// <param name="lineId">
        /// Find the LINE ID of your bot on the LINE@ Manager. Make sure you include the "@" symbol in the LINE ID.
        /// </param>
        /// <returns>String of LINE Scheme URL</returns>
        public static string GetAddFriendUrl(string lineId) => string.Format(addFriend, lineId);


        /// <summary>
        /// Opens the "Share with" screen where users can select friends, groups, or chats to share a link to your bot.
        /// </summary>
        /// <param name="lineId">
        /// Find the LINE ID of your bot on the LINE@ Manager. Make sure you include the "@" symbol in the LINE ID.
        /// </param>
        /// <returns>String of LINE Scheme URL</returns>
        public static string GetRemommendUrl(string lineId) => string.Format(recommendOA, lineId);


        /// <summary>
        /// Opens the Timeline screen for your bot.
        /// </summary>
        /// <param name="lineIdWithoutAt">
        /// Do not include the "@" symbol in the LINE ID.Find the LINE ID of your bot on the LINE@ Manager.
        /// </param>
        /// <returns>String of LINE Scheme URL</returns>
        public static string GetMainUrl(string lineIdWithoutAt) => string.Format(main, lineIdWithoutAt);

        /// <summary>
        /// Opens the account page for your bot.
        /// </summary>
        /// <param name="lineIdWithoutAt">
        /// Do not include the "@" symbol in the LINE ID.Find the LINE ID of your bot on the LINE@ Manager.
        /// </param>
        /// <returns>String of LINE Scheme URL</returns>
        public static string GetProfileUrl(string lineIdWithoutAt) => string.Format(profile, lineIdWithoutAt);

        /// <summary>
        /// Opens a specific Timeline post for your bot. 
        /// You can find the post ID of individual posts in the "Timeline (Home)" section of the LINE@ Manager.
        /// </summary>
        /// <param name="lineIdWithoutAt">
        /// Do not include the "@" symbol in the LINE ID.Find the LINE ID of your bot on the LINE@ Manager.
        /// </param>
        /// <param name="postId">post ID</param>
        /// <returns>String of LINE Scheme URL</returns>
        public static string GetPostUrl(string lineIdWithoutAt, string postId) => string.Format(post, lineIdWithoutAt, postId);

        /// <summary>
        /// Opens the "Share with" screen where users can select friends, groups, or chats to send a preset text message.<para> 
        /// Users can also post the message as a note in a chat or post the message to Timeline.</para>
        /// </summary>
        /// <param name="textMessage"></param>
        /// <returns>String of LINE Scheme URL</returns>
        public static string GetMsgTextUrl(string textMessage) => string.Format(msgText, Uri.EscapeUriString(textMessage));


    }
}
