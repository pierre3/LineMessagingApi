using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Line.Messaging.Webhooks;
using System.Linq;
using System.Threading.Tasks;

namespace Line.MessagingTest
{
    [TestClass]
    public class WebhookEventParserTest
    {
        [TestMethod]
        public async Task ParseTest()
        {
            var message_replyToken = "nHuyWiB7yP5Zw52FIkcQobQuGDXCTA";
            var message_type = "message";
            var message_timestamp = 1462629479859L;
            var message_source_type = "user";
            var message_source_userId = "U206d25c2ea6bd87c17655609a1c37cb8";
            var message_message_id = "325708";
            var message_message_type = "text";
            var message_message_text = "Hello, world";

            var follow_replyToken = "aKeIk4345Hilan1FIkcQobQuGDX4uU";
            var follow_type = "follow";
            var follow_timestamp = 3357629163789L;
            var follow_source_type = "user";
            var follow_source_userId = "481290";

            var unfollow_type = "unfollow";
            var unfollow_timestamp = 1234629163789L;
            var unfollow_source_type = "user";
            var unfollow_source_userId = "435876";

            var join_replyToken = "R2i0Be345Hilan1FIkcQobQuGDX4uU";
            var join_type = "join";
            var join_timestamp = 1638442438963L;
            var join_source_type = "group";
            var join_source_groupId = "cxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

            var leave_type = "leave";
            var leave_timestamp = 1991242938417L;
            var leave_source_type = "group";
            var leave_source_groupId = "cxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx1";

            var postback_replyToken = "aKeIk4345Hilan1FIkcQobQuGDX4uU";
            var postback_type = "postback";
            var postback_timestamp = 3357629163789L;
            var postback_source_type = "user";
            var postback_source_userId = "123456";
            var postback_postback_data = "action=buyItem&itemId=123123&color=red";

            var beacon_replyToken = "q12Ik4345Hilan1FIkcQobQp3g94uU";
            var beacon_type = "beacon";
            var beacon_timestamp = 3357629163789L;
            var beacon_source_type = "user";
            var beacon_source_userId = "123456";
            var beacon_beacon_hwid = "d41d8cd98f";
            var beacon_beacon_type = "enter";
            var beacon_beacon_dm = "beacon_dm";

            var videoMessage_replyToken = "nHuyWiB7yP5Zw52FIkcQobQuGDXCTA";
            var videoMessage_type = "message";
            var videoMessage_timestamp = 1462629479859L;
            var videoMessage_source_type = "user";
            var videoMessage_source_userId = "U206d25c2ea6bd87c17655609a1c37cb8";
            var videoMessage_message_id = "325708";
            var videoMessage_message_type = "video";
            var videoMessage_duration = 1000;
            var videoMessage_provider_type = "external";
            var videoMessage_provider_url = "https://sample.com/video/a001.mp4";
            var videoMessage_provider_preUrl = "https://sample.com/video/a001pre.jpeg";

            var imageMessage_replyToken = "nHuyWiB7yP5Zw52FIkcQobQuGDXCTA";
            var imageMessage_type = "message";
            var imageMessage_timestamp = 1462629479859L;
            var imageMessage_source_type = "user";
            var imageMessage_source_userId = "U206d25c2ea6bd87c17655609a1c37cb8";
            var imageMessage_message_id = "325708";
            var imageMessage_message_type = "image";
            var imageMessage_provider_type = "line";

            var memberJoin_replyToken = "0f3779fba3b349968c5d07db31eabf65";
            var memberJoin_type = "memberJoined";
            var memberJoin_timestamp = 1462629479859L;
            var memberJoin_source_type = "group";
            var memberJoin_source_id = "C4af4980629...";
            var memberJoin_joined_0_type = "user";
            var memberJoin_joined_0_userId = "U4af4980629...";
            var memberJoin_joined_1_type = "user";
            var memberJoin_joined_1_userId = "U91eeaf62d9...";

            var memberLeave_type = "memberLeft";
            var memberLeave_timestamp = 1462629479960L;
            var memberLeave_source_type = "group";
            var memberLeave_source_id = "C4af4980629...";
            var memberLeave_left_0_type = "user";
            var memberLeave_left_0_userId = "U4af4980629...";
            var memberLeave_left_1_type = "user";
            var memberLeave_left_1_userId = "U91eeaf62d9...";

            var deviceLink_type = "things";
            var deviceLink_timestamp = 1462629479859;
            var deviceLink_source_type = "user";
            var deviceLink_source_id = "U91eeaf62d...";
            var deviceLink_things_deviceId = "t2c449c9d1...";
            var deviceLink_things_type = "link";

            var deviceUnlink_type = "things";
            var deviceUnlink_timestamp = 1462629479859;
            var deviceUnlink_source_type = "user";
            var deviceUnlink_source_id = "U91eeaf62d...";
            var deviceUnlink_things_deviceId = "t2c449c9d1...";
            var deviceUnlink_things_type = "unlink";


            var json =
$@"{{
    ""events"": [
        {{
            ""replyToken"": ""{message_replyToken}"",
            ""type"": ""{message_type}"",
            ""timestamp"": {message_timestamp},
            ""source"": {{
                 ""type"": ""{message_source_type}"",
                ""userId"": ""{message_source_userId}""
             }},
             ""message"": {{
                 ""id"": ""{message_message_id}"",
                 ""type"": ""{message_message_type}"",
                 ""text"": ""{message_message_text}""
            }}
        }},
        {{
            ""replyToken"": ""{follow_replyToken}"",
            ""type"": ""{follow_type}"",
            ""timestamp"": {follow_timestamp},
            ""source"": {{
                ""type"": ""{follow_source_type}"",
                ""userId"": ""{follow_source_userId}""
            }}
        }},
        {{
            ""type"": ""{unfollow_type}"",
            ""timestamp"": {unfollow_timestamp},
            ""source"": {{
                ""type"": ""{unfollow_source_type}"",
                ""userId"": ""{unfollow_source_userId}""
            }}
        }},
        {{
            ""replyToken"": ""{join_replyToken}"",
            ""type"": ""{join_type}"",
            ""timestamp"": {join_timestamp},
            ""source"": {{
                ""type"": ""{join_source_type}"",
                ""groupId"": ""{join_source_groupId}""
            }}
        }},
        {{
            ""type"": ""{leave_type}"",
            ""timestamp"": {leave_timestamp},
            ""source"": {{
                ""type"": ""{leave_source_type}"",
                ""groupId"": ""{leave_source_groupId}""
            }}
        }},
        {{
            ""replyToken"": ""{postback_replyToken}"",
            ""type"": ""{postback_type}"",
            ""timestamp"": {postback_timestamp},
            ""source"": {{
                ""type"": ""{postback_source_type}"",
                ""userId"": ""{postback_source_userId}""
            }},
            ""postback"": {{
                ""data"": ""{postback_postback_data}""
            }}
        }},
        {{
            ""replyToken"": ""{beacon_replyToken}"",
            ""type"": ""{beacon_type}"",
            ""timestamp"": {beacon_timestamp},
            ""source"": {{
                ""type"": ""{beacon_source_type}"",
                ""userId"": ""{beacon_source_userId}""
            }},
            ""beacon"": {{
                ""hwid"": ""{beacon_beacon_hwid}"",
                ""type"": ""{beacon_beacon_type}"",
                ""dm"" : ""{beacon_beacon_dm}""
            }}
         }},
        {{
            ""replyToken"": ""{videoMessage_replyToken}"",
            ""type"": ""{videoMessage_type}"",
            ""timestamp"": {videoMessage_timestamp},
            ""source"": {{
                 ""type"": ""{videoMessage_source_type}"",
                ""userId"": ""{videoMessage_source_userId}""
             }},
             ""message"": {{
                 ""id"": ""{videoMessage_message_id}"",
                 ""type"": ""{videoMessage_message_type}"",
                 ""duration"": ""{videoMessage_duration}"",
                 ""contentProvider"": {{ 
                    ""type"": ""{videoMessage_provider_type}"",
                    ""originalContentUrl"": ""{videoMessage_provider_url}"",
                    ""previewContentUrl"": ""{videoMessage_provider_preUrl}"",
                 }}
            }}
        }},
        {{
            ""replyToken"": ""{imageMessage_replyToken}"",
            ""type"": ""{imageMessage_type}"",
            ""timestamp"": {imageMessage_timestamp},
            ""source"": {{
                 ""type"": ""{imageMessage_source_type}"",
                ""userId"": ""{imageMessage_source_userId}""
             }},
             ""message"": {{
                 ""id"": ""{imageMessage_message_id}"",
                 ""type"": ""{imageMessage_message_type}"",
                 ""contentProvider"": {{ 
                    ""type"": ""{imageMessage_provider_type}""
                 }}
            }}
        }},
        {{
            ""replyToken"": ""{memberJoin_replyToken}"",
            ""type"": ""{memberJoin_type}"",
            ""timestamp"": {memberJoin_timestamp},
            ""source"": {{
                ""type"": ""{memberJoin_source_type}"",
                ""groupId"": ""{memberJoin_source_id}""
            }},
            ""joined"": {{
                ""members"": [
                    {{
                        ""type"": ""{memberJoin_joined_0_type}"",
                        ""userId"": ""{memberJoin_joined_0_userId}""
                    }},
                    {{
                        ""type"": ""{memberJoin_joined_1_type}"",
                        ""userId"": ""{memberJoin_joined_1_userId}""
                    }}
                ]
            }}
         }},
        {{
            ""type"": ""{memberLeave_type}"",
            ""timestamp"": {memberLeave_timestamp},
            ""source"": {{
                ""type"": ""{memberLeave_source_type}"",
                ""groupId"": ""{memberLeave_source_id}""
            }},
            ""left"": {{
                ""members"": [
                    {{
                        ""type"": ""{memberLeave_left_0_type}"",
                        ""userId"": ""{memberLeave_left_0_userId}""
                    }},
                    {{  
                        ""type"": ""{memberLeave_left_1_type}"",
                        ""userId"": ""{memberLeave_left_1_userId}""
                    }}
                ]
            }}
        }},
        {{
            ""type"": ""{deviceLink_type}"",
            ""timestamp"": {deviceLink_timestamp},
            ""source"": {{
                ""type"": ""{deviceLink_source_type}"",
                ""userId"": ""{deviceLink_source_id}""
            }},
            ""things"": {{
                ""deviceId"": ""{deviceLink_things_deviceId}"",
                ""type"": ""{deviceLink_things_type}""
            }}
        }},
        {{
            ""type"": ""{deviceUnlink_type}"",
            ""timestamp"": {deviceUnlink_timestamp},
            ""source"": {{
                ""type"": ""{deviceUnlink_source_type}"",
                ""userId"": ""{deviceUnlink_source_id}""
            }},
            ""things"": {{
                ""deviceId"": ""{deviceUnlink_things_deviceId}"",
                ""type"": ""{deviceUnlink_things_type}""
            }}
        }}
    ]
}}";

            var events = WebhookEventParser.Parse(json).ToArray();
            var messageEvent = (MessageEvent)events[0];
            Assert.AreEqual(messageEvent.ReplyToken, message_replyToken);
            Assert.AreEqual(messageEvent.Type.ToString().ToLower(), message_type);
            Assert.AreEqual(messageEvent.Timestamp, message_timestamp);
            Assert.AreEqual(messageEvent.Source.Type.ToString().ToLower(), message_source_type);
            Assert.AreEqual(messageEvent.Source.Id, message_source_userId);
            Assert.AreEqual(messageEvent.Message.Id, message_message_id);
            Assert.AreEqual(messageEvent.Message.Type.ToString().ToLower(), message_message_type);
            Assert.AreEqual(((TextEventMessage)(messageEvent.Message)).Text, message_message_text);

            var followEvent = (FollowEvent)events[1];
            Assert.AreEqual(followEvent.ReplyToken, follow_replyToken);
            Assert.AreEqual(followEvent.Type.ToString().ToLower(), follow_type);
            Assert.AreEqual(followEvent.Timestamp, follow_timestamp);
            Assert.AreEqual(followEvent.Source.Type.ToString().ToLower(), follow_source_type);
            Assert.AreEqual(followEvent.Source.Id, follow_source_userId);

            var unfollowEvent = (UnfollowEvent)events[2];
            Assert.AreEqual(unfollowEvent.Type.ToString().ToLower(), unfollow_type);
            Assert.AreEqual(unfollowEvent.Timestamp, unfollow_timestamp);
            Assert.AreEqual(unfollowEvent.Source.Type.ToString().ToLower(), unfollow_source_type);
            Assert.AreEqual(unfollowEvent.Source.Id, unfollow_source_userId);

            var joinEvent = (JoinEvent)events[3];
            Assert.AreEqual(joinEvent.Type.ToString().ToLower(), join_type);
            Assert.AreEqual(joinEvent.Timestamp, join_timestamp);
            Assert.AreEqual(joinEvent.Source.Type.ToString().ToLower(), join_source_type);
            Assert.AreEqual((joinEvent.Source).Id, join_source_groupId);

            var leaveEvent = (LeaveEvent)events[4];
            Assert.AreEqual(leaveEvent.Type.ToString().ToLower(), leave_type);
            Assert.AreEqual(leaveEvent.Timestamp, leave_timestamp);
            Assert.AreEqual(leaveEvent.Source.Type.ToString().ToLower(), leave_source_type);
            Assert.AreEqual((leaveEvent.Source).Id, leave_source_groupId);

            var bostbackEvent = (PostbackEvent)events[5];
            Assert.AreEqual(bostbackEvent.Type.ToString().ToLower(), postback_type);
            Assert.AreEqual(bostbackEvent.Timestamp, postback_timestamp);
            Assert.AreEqual(bostbackEvent.Source.Type.ToString().ToLower(), postback_source_type);
            Assert.AreEqual(bostbackEvent.Source.Id, postback_source_userId);
            Assert.AreEqual(bostbackEvent.Postback.Data, postback_postback_data);

            var beaconEvent = (BeaconEvent)events[6];
            Assert.AreEqual(beaconEvent.Type.ToString().ToLower(), beacon_type);
            Assert.AreEqual(beaconEvent.Timestamp, beacon_timestamp);
            Assert.AreEqual(beaconEvent.Source.Type.ToString().ToLower(), beacon_source_type);
            Assert.AreEqual(beaconEvent.Source.Id, beacon_source_userId);
            Assert.AreEqual(beaconEvent.Beacon.Hwid, beacon_beacon_hwid);
            Assert.AreEqual(beaconEvent.Beacon.Type.ToString().ToLower(), beacon_beacon_type);
            Assert.AreEqual(beaconEvent.Beacon.Dm, beacon_beacon_dm);

            var videoMessageEvent = (MessageEvent)events[7];
            Assert.AreEqual(videoMessageEvent.ReplyToken, videoMessage_replyToken);
            Assert.AreEqual(videoMessageEvent.Type.ToString().ToLower(), videoMessage_type);
            Assert.AreEqual(videoMessageEvent.Timestamp, videoMessage_timestamp);
            Assert.AreEqual(videoMessageEvent.Source.Type.ToString().ToLower(), videoMessage_source_type);
            Assert.AreEqual(videoMessageEvent.Source.Id, videoMessage_source_userId);
            Assert.AreEqual(videoMessageEvent.Message.Id, videoMessage_message_id);
            Assert.AreEqual(videoMessageEvent.Message.Type.ToString().ToLower(), videoMessage_message_type);
            var media = (MediaEventMessage)(videoMessageEvent.Message);
            Assert.AreEqual(media.Duration, videoMessage_duration);
            Assert.AreEqual(media.ContentProvider.Type.ToString().ToLower(), videoMessage_provider_type);
            Assert.AreEqual(media.ContentProvider.OriginalContentUrl, videoMessage_provider_url);
            Assert.AreEqual(media.ContentProvider.PreviewImageUrl, videoMessage_provider_preUrl);

            var imageMessageEvent = (MessageEvent)events[8];
            Assert.AreEqual(imageMessageEvent.ReplyToken, imageMessage_replyToken);
            Assert.AreEqual(imageMessageEvent.Type.ToString().ToLower(), imageMessage_type);
            Assert.AreEqual(imageMessageEvent.Timestamp, imageMessage_timestamp);
            Assert.AreEqual(imageMessageEvent.Source.Type.ToString().ToLower(), imageMessage_source_type);
            Assert.AreEqual(imageMessageEvent.Source.Id, imageMessage_source_userId);
            Assert.AreEqual(imageMessageEvent.Message.Id, imageMessage_message_id);
            Assert.AreEqual(imageMessageEvent.Message.Type.ToString().ToLower(), imageMessage_message_type);
            var mediaImage = (MediaEventMessage)(imageMessageEvent.Message);
            Assert.AreEqual(mediaImage.ContentProvider.Type.ToString().ToLower(), imageMessage_provider_type);
            Assert.AreEqual(mediaImage.ContentProvider.OriginalContentUrl, null);
            Assert.AreEqual(mediaImage.ContentProvider.PreviewImageUrl, null);

            var memberJoinEvent = (MemberJoinEvent)events[9];
            Assert.AreEqual(memberJoinEvent.ReplyToken, memberJoin_replyToken);
            Assert.AreEqual(memberJoinEvent.Type.ToString().ToLower(), memberJoin_type.ToLower());
            Assert.AreEqual(memberJoinEvent.Timestamp, memberJoin_timestamp);
            Assert.AreEqual(memberJoinEvent.Source.Type.ToString().ToLower(), memberJoin_source_type);
            Assert.AreEqual(memberJoinEvent.Source.Id, memberJoin_source_id);
            Assert.AreEqual(memberJoinEvent.Joined.Members[0].Type.ToString().ToLower(), memberJoin_joined_0_type);
            Assert.AreEqual(memberJoinEvent.Joined.Members[0].UserId, memberJoin_joined_0_userId);
            Assert.AreEqual(memberJoinEvent.Joined.Members[1].Type.ToString().ToLower(), memberJoin_joined_1_type);
            Assert.AreEqual(memberJoinEvent.Joined.Members[1].UserId, memberJoin_joined_1_userId);

            var memberLeaveEvent = (MemberLeaveEvent)events[10];
            Assert.AreEqual(memberLeaveEvent.Type.ToString().ToLower(), memberLeave_type.ToLower());
            Assert.AreEqual(memberLeaveEvent.Timestamp, memberLeave_timestamp);
            Assert.AreEqual(memberLeaveEvent.Source.Type.ToString().ToLower(), memberLeave_source_type);
            Assert.AreEqual(memberLeaveEvent.Source.Id, memberLeave_source_id);
            Assert.AreEqual(memberLeaveEvent.Left.Members[0].Type.ToString().ToLower(), memberLeave_left_0_type);
            Assert.AreEqual(memberLeaveEvent.Left.Members[0].UserId, memberLeave_left_0_userId);
            Assert.AreEqual(memberLeaveEvent.Left.Members[1].Type.ToString().ToLower(), memberLeave_left_1_type);
            Assert.AreEqual(memberLeaveEvent.Left.Members[1].UserId, memberLeave_left_1_userId);

            var deviceLink = (DeviceLinkEvent)events[11];
            Assert.AreEqual(deviceLink.Type.ToString().ToLower(), deviceLink_type);
            Assert.AreEqual(deviceLink.Timestamp, deviceLink_timestamp);
            Assert.AreEqual(deviceLink.Source.Type.ToString().ToLower(), deviceLink_source_type);
            Assert.AreEqual(deviceLink.Source.Id, deviceLink_source_id);
            Assert.AreEqual(deviceLink.Things.DeviceId, deviceLink_things_deviceId);
            Assert.AreEqual(deviceLink.Things.Type.ToString().ToLower(), deviceLink_things_type);

            var deviceUnlink = (DeviceUnlinkEvent)events[12];
            Assert.AreEqual(deviceUnlink.Type.ToString().ToLower(), deviceUnlink_type);
            Assert.AreEqual(deviceUnlink.Timestamp, deviceUnlink_timestamp);
            Assert.AreEqual(deviceUnlink.Source.Type.ToString().ToLower(), deviceUnlink_source_type);
            Assert.AreEqual(deviceUnlink.Source.Id, deviceUnlink_source_id);
            Assert.AreEqual(deviceUnlink.Things.DeviceId, deviceUnlink_things_deviceId);
            Assert.AreEqual(deviceUnlink.Things.Type.ToString().ToLower(), deviceUnlink_things_type);

            var testApp = new TestApp();
            await testApp.RunAsync(events);
        }
    }

    class TestApp : WebhookApplication
    {
        protected override Task OnDeviceLinkAsync(DeviceLinkEvent ev)
        {
            Console.WriteLine($"OnDeviceLinkAsync( DeviceId: {ev.Things.DeviceId} )");
            return base.OnDeviceLinkAsync(ev);
        }

        protected override Task OnDeviceUnlinkAsync(DeviceUnlinkEvent ev)
        {
            Console.WriteLine($"OnDeviceUnlinkAsync( DeviceId: {ev.Things.DeviceId} )");
            return base.OnDeviceUnlinkAsync(ev);
        }

        protected override Task OnMemberJoinAsync(MemberJoinEvent ev)
        {
            Console.WriteLine($"OnMemberJoinAsync( Members: [{string.Join(",",ev.Joined.Members.Select(m=>m.UserId))}]");
            return base.OnMemberJoinAsync(ev);
        }

        protected override Task OnMemberLeaveAsync(MemberLeaveEvent ev)
        {
            Console.WriteLine($"OnMemberJoinAsync( Members: [{string.Join(",", ev.Left.Members.Select(m => m.UserId))}]");
            return base.OnMemberLeaveAsync(ev);
        }
    }
}
