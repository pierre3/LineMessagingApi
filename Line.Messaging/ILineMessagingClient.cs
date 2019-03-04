using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Line.Messaging
{
    /// <summary>
    /// LINE Messaging API client, which handles request/response to LINE server.
    /// </summary>
    public interface ILineMessagingClient
    {
        #region Message 

        /// <summary>
        /// Respond to events from users, groups, and rooms
        /// https://developers.line.me/en/docs/messaging-api/reference/#send-reply-message
        /// </summary>
        /// <param name="replyToken">ReplyToken</param>
        /// <param name="messages">Reply messages. Up to 5 messages.</param>
        Task ReplyMessageAsync(string replyToken, IList<ISendMessage> messages);

        /// <summary>
        /// Respond to events from users, groups, and rooms
        /// https://developers.line.me/en/docs/messaging-api/reference/#send-reply-message
        /// </summary>
        /// <param name="replyToken">ReplyToken</param>
        /// <param name="messages">Reply Text messages. Up to 5 messages.</param>
        Task ReplyMessageAsync(string replyToken, params string[] messages);

        /// <summary>
        /// Respond to events from users, groups, and rooms
        /// https://developers.line.me/en/docs/messaging-api/reference/#send-reply-message
        /// </summary>
        /// <param name="replyToken">ReplyToken</param>
        /// <param name="messages">Set reply messages with Json string.</param>
        Task ReplyMessageWithJsonAsync(string replyToken, params string[] messages);

        /// <summary>
        /// Send messages to a user, group, or room at any time.
        /// Note: Use of push messages are limited to certain plans.
        /// </summary>
        /// <param name="to">ID of the receiver</param>
        /// <param name="messages">Reply messages. Up to 5 messages.</param>
        Task PushMessageAsync(string to, IList<ISendMessage> messages);

        /// <summary>
        /// Send messages to a user, group, or room at any time.
        /// Note: Use of push messages are limited to certain plans.
        /// </summary>
        /// <param name="to">ID of the receiver</param>
        /// <param name="messages">Set reply messages with Json string.</param>
        Task PushMessageWithJsonAsync(string to, params string[] messages);


        /// <summary>
        /// Send text messages to a user, group, or room at any time.
        /// Note: Use of push messages are limited to certain plans.
        /// </summary>
        /// <param name="to">ID of the receiver</param>
        /// <param name="messages">Reply text messages. Up to 5 messages.</param>
        Task PushMessageAsync(string to, params string[] messages);

        /// <summary>
        /// Send push messages to multiple users at any time.
        /// Only available for plans which support push messages. Messages cannot be sent to groups or rooms
        /// https://developers.line.me/en/docs/messaging-api/reference/#send-multicast-messages
        /// </summary>
        /// <param name="to">IDs of the receivers. Max: 150 users</param>
        /// <param name="messages">Reply messages. Up to 5 messages.</param>
        Task MultiCastMessageAsync(IList<string> to, IList<ISendMessage> messages);

        /// <summary>
        /// Send push messages to multiple users at any time.
        /// Only available for plans which support push messages. Messages cannot be sent to groups or rooms
        /// https://developers.line.me/en/docs/messaging-api/reference/#send-multicast-messages
        /// </summary>
        /// <param name="to">IDs of the receivers. Max: 150 users</param>
        /// <param name="messages">Set reply messages with Json string.</param>
        Task MultiCastMessageWithJsonAsync(IList<string> to, params string[] messages);

        /// <summary>
        /// Send push text messages to multiple users at any time.
        /// Only available for plans which support push messages. Messages cannot be sent to groups or rooms
        /// https://developers.line.me/en/docs/messaging-api/reference/#send-multicast-messages
        /// </summary>
        /// <param name="to">IDs of the receivers. Max: 150 users</param>
        /// <param name="messages">Reply text messages. Up to 5 messages.</param>
        Task MultiCastMessageAsync(IList<string> to, params string[] messages);


        /// <summary>
        /// Retrieve image, video, and audio data sent by users as Stream
        /// https://developers.line.me/en/docs/messaging-api/reference/#get-content
        /// </summary>
        /// <param name="messageId">Message ID</param>
        /// <returns>Content as ContentStream</returns>
        Task<ContentStream> GetContentStreamAsync(string messageId);

        /// <summary>
        /// Retrieve image, video, and audio data sent by users as byte array
        /// https://developers.line.me/en/docs/messaging-api/reference/#get-content
        /// </summary>
        /// <param name="messageId">Message ID</param>
        /// <returns>Content as byte array</returns>
        Task<byte[]> GetContentBytesAsync(string messageId);

        #endregion

        #region Profile

        /// <summary>
        /// Get user profile information.
        /// https://developers.line.me/en/docs/messaging-api/reference/#get-profile
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns></returns>
        Task<UserProfile> GetUserProfileAsync(string userId);

        #endregion

        #region Group

        /// <summary>
        /// Gets the user profile of a member of a group that the bot is in. This includes user profiles of users who have not added the bot as a friend or have blocked the bot.
        /// Use the group ID and user ID returned in the source object of webhook event objects. Do not use the LINE ID used in the LINE app. 
        /// https://developers.line.me/en/docs/messaging-api/reference/#get-group-member-profile
        /// </summary>
        /// <param name="groupId">Identifier of the group</param>
        /// <param name="userId">Identifier of the user</param>
        /// <returns>User Profile</returns>
        Task<UserProfile> GetGroupMemberProfileAsync(string groupId, string userId);

        /// <summary>
        /// Gets the user IDs of the members of a group that the bot is in. This includes the user IDs of users who have not added the bot as a friend or has blocked the bot.
        /// This feature is only available for LINE@ Approved accounts or official accounts.
        /// Use the group Id returned in the source object of webhook event objects. 
        /// Users who have not agreed to the Official Accounts Terms of Use are not included in memberIds. There is no fixed number of memberIds. 
        /// https://developers.line.me/en/docs/messaging-api/reference/#get-group-member-user-ids
        /// </summary>
        /// <param name="groupId">Identifier of the group</param>
        /// <param name="continuationToken">ContinuationToken</param>
        /// <returns>GroupMemberIds</returns>
        Task<GroupMemberIds> GetGroupMemberIdsAsync(string groupId, string continuationToken);

        /// <summary>
        /// Gets the user profiles of the members of a group that the bot is in. This includes the user IDs of users who have not added the bot as a friend or has blocked the bot.
        /// Use the group Id returned in the source object of webhook event objects. 
        /// This feature is only available for LINE@ Approved accounts or official accounts
        /// </summary>
        /// <param name="groupId">Identifier of the group</param>
        /// <returns>List of UserProfile</returns>
        Task<IList<UserProfile>> GetGroupMemberProfilesAsync(string groupId);

        /// <summary>
        /// Leave a group.
        /// Use the ID that is returned via webhook from the source group. 
        /// https://developers.line.me/en/docs/messaging-api/reference/#leave-group
        /// </summary>
        /// <param name="groupId">Group ID</param>
        /// <returns></returns>
        Task LeaveFromGroupAsync(string groupId);

        #endregion

        #region Room

        /// <summary>
        /// Gets the user profile of a member of a room that the bot is in. This includes user profiles of users who have not added the bot as a friend or have blocked the bot.
        /// Use the room ID and user ID returned in the source object of webhook event objects. Do not use the LINE ID used in the LINE app
        /// </summary>
        /// <param name="roomId">Identifier of the room</param>
        /// <param name="userId">Identifier of the user</param>
        /// <returns></returns>
        Task<UserProfile> GetRoomMemberProfileAsync(string roomId, string userId);

        /// <summary>
        /// Gets the user IDs of the members of a room that the bot is in. This includes the user IDs of users who have not added the bot as a friend or has blocked the bot.
        /// Use the room ID returned in the source object of webhook event objects. 
        /// This feature is only available for LINE@ Approved accounts or official accounts.
        /// https://developers.line.me/en/docs/messaging-api/reference/#get-room-member-user-ids
        /// </summary>
        /// <param name="roomId">Identifier of the room</param>
        /// <param name="continuationToken">ContinuationToken</param>
        /// <returns>GroupMemberIds</returns>
        Task<GroupMemberIds> GetRoomMemberIdsAsync(string roomId, string continuationToken = null);

        /// <summary>
        /// Gets the user profiles of the members of a room that the bot is in. This includes the user IDs of users who have not added the bot as a friend or has blocked the bot.
        /// Use the room ID returned in the source object of webhook event objects. 
        /// This feature is only available for LINE@ Approved accounts or official accounts.
        /// </summary>
        /// <param name="roomId">Identifier of the room</param>
        /// <returns>List of UserProfiles</returns>
        Task<IList<UserProfile>> GetRoomMemberProfilesAsync(string roomId);

        /// <summary>
        /// Leave a room.
        /// Use the ID that is returned via webhook from the source room. 
        /// </summary>
        /// <param name="roomId">Room ID</param>
        Task LeaveFromRoomAsync(string roomId);

        #endregion

        #region Rich menu

        /// <summary>
        /// Gets a rich menu via a rich menu ID.
        /// https://developers.line.me/en/docs/messaging-api/reference/#get-rich-menu
        /// </summary>
        /// <param name="richMenuId">ID of an uploaded rich menu</param>
        /// <returns>RichMenu</returns>
        Task<RichMenu> GetRichMenuAsync(string richMenuId);

        /// <summary>
        /// Creates a rich menu. 
        /// Note: You must upload a rich menu image and link the rich menu to a user for the rich menu to be displayed.You can create up to 10 rich menus for one bot.
        /// The rich menu represented as a rich menu object.
        /// https://developers.line.me/en/docs/messaging-api/reference/#create-rich-menu
        /// </summary>
        /// <param name="richMenu">RichMenu</param>
        /// <returns>RichMenu Id</returns>
        Task<string> CreateRichMenuAsync(RichMenu richMenu);

        /// <summary>
        /// Deletes a rich menu.
        /// https://developers.line.me/en/docs/messaging-api/reference/#delete-rich-menu
        /// </summary>
        /// <param name="richMenuId">RichMenu Id</param>
        Task DeleteRichMenuAsync(string richMenuId);

        /// <summary>
        /// Gets the ID of the rich menu linked to a user.
        /// https://developers.line.me/en/docs/messaging-api/reference/#get-rich-menu-id-of-user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>RichMenu Id</returns>
        Task<string> GetRichMenuIdOfUserAsync(string userId);

        /// <summary>
        /// Sets a default ritch menu
        /// </summary>
        /// <param name="richMenuId">
        /// ID of an uploaded rich menu
        /// </param>
        Task SetDefaultRichMenuAsync(string richMenuId);

        /// <summary>
        /// Links a rich menu to a user.
        /// Note: Only one rich menu can be linked to a user at one time.
        /// https://developers.line.me/en/docs/messaging-api/reference/#link-rich-menu-to-user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="richMenuId">ID of an uploaded rich menu</param>
        /// <returns></returns>
        Task LinkRichMenuToUserAsync(string userId, string richMenuId);

        /// <summary>
        /// Unlinks a rich menu from a user.
        /// https://developers.line.me/en/docs/messaging-api/reference/#unlink-rich-menu-from-user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns></returns>
        Task UnLinkRichMenuFromUserAsync(string userId);

        /// <summary>
        /// Downloads an image associated with a rich menu.
        /// https://developers.line.me/en/docs/messaging-api/reference/#download-rich-menu-image
        /// </summary>
        /// <param name="richMenuId">RichMenu Id</param>
        /// <returns>Image as ContentStream</returns>
        Task<ContentStream> DownloadRichMenuImageAsync(string richMenuId);

        /// <summary>
        /// Uploads and attaches a jpeg image to a rich menu.
        /// Images must have one of the following resolutions: 2500x1686, 2500x843. 
        /// You cannot replace an image attached to a rich menu.To update your rich menu image, create a new rich menu object and upload another image.
        /// https://developers.line.me/en/docs/messaging-api/reference/#upload-rich-menu-image
        /// </summary>
        /// <param name="stream">Jpeg image for the rich menu</param>
        /// <param name="richMenuId">The ID of the rich menu to attach the image to.</param>
        Task UploadRichMenuJpegImageAsync(Stream stream, string richMenuId);

        /// <summary>
        /// Uploads and attaches a png image to a rich menu.
        /// Images must have one of the following resolutions: 2500x1686, 2500x843. 
        /// You cannot replace an image attached to a rich menu.To update your rich menu image, create a new rich menu object and upload another image.
        /// https://developers.line.me/en/docs/messaging-api/reference/#upload-rich-menu-image
        /// </summary>
        /// <param name="stream">Png image for the rich menu</param>
        /// <param name="richMenuId">The ID of the rich menu to attach the image to.</param>
        Task UploadRichMenuPngImageAsync(Stream stream, string richMenuId);

        /// <summary>
        /// Gets a list of all uploaded rich menus.
        /// https://developers.line.me/en/docs/messaging-api/reference/#get-rich-menu-list
        /// </summary>
        /// <returns>List of ResponseRichMenu</returns>
        Task<IList<ResponseRichMenu>> GetRichMenuListAsync();

        #endregion

        #region Account Link

        /// <summary>
        /// Issues a link token used for the account link feature.
        /// <para>https://developers.line.me/en/docs/messaging-api/linking-accounts</para>
        /// </summary>
        /// <param name="userId">
        /// User ID for the LINE account to be linked. Found in the source object of account link event objects. Do not use the LINE ID used in the LINE app.
        /// </param>
        /// <returns>
        /// Returns the status code 200 and a link token. Link tokens are valid for 10 minutes and can only be used once.
        /// Note: The validity period may change without notice.
        /// </returns>
        Task<string> IssueLinkTokenAsync(string userId);

        #endregion

        #region Number of sent messages

        /// <summary>
        /// Gets the number of messages sent with the /bot/message/reply endpoint.
        /// The number of messages retrieved by this operation does not include the number of messages sent from LINE@ Manager.
        /// </summary>
        /// <param name="date">
        /// - Date the messages were sent
        /// - Format: yyyyMMdd(Example: 20191231)
        /// - Timezone: UTC+9
        /// </param>
        /// <returns>
        /// <see cref="Line.Messaging.NumberOfSentMessages"/>
        /// </returns>
        Task<NumberOfSentMessages> GetNumberOfSentReplyMessagesAsync(DateTime date);

        /// <summary>
        /// Gets the number of messages sent with the /bot/message/push endpoint.
        /// The number of messages retrieved by this operation does not include the number of messages sent from LINE@ Manager.
        ///</summary>
        /// <param name="date">
        /// - Date the messages were sent
        /// - Format: yyyyMMdd(Example: 20191231)
        /// - Timezone: UTC+9
        /// </param>
        /// <returns>
        /// <see cref="Line.Messaging.NumberOfSentMessages"/>
        /// </returns>
        Task<NumberOfSentMessages> GetNumberOfSentPushMessagesAsync(DateTime date);

        /// <summary>
        /// Gets the number of messages sent with the /bot/message/push endpoint.
        /// The number of messages retrieved by this operation does not include the number of messages sent from LINE@ Manager.
        /// </summary>
        /// <param name="date">
        /// - Date the messages were sent
        /// - Format: yyyyMMdd(Example: 20191231)
        /// - Timezone: UTC+9
        /// </param>
        /// <returns>
        /// <see cref="Line.Messaging.NumberOfSentMessages"/>
        /// </returns>
        Task<NumberOfSentMessages> GetNumberOfSentMulticastMessagesAsync(DateTime date);

        #endregion
    }
}
