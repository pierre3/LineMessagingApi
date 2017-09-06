using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Line.Messaging.Webhooks;
using System.Linq;

namespace Line.MessagingTest
{
    [TestClass]
    public class WebhookEventParserTest
    {
        [TestMethod]
        public void ParseTest()
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

            var json = $@"{{
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
         }}
    ]
}}";

            var events = WebhookEventParser.Parse(json).ToArray();
            var messageEvent = (MessageEvent)events[0];
            Assert.AreEqual(messageEvent.ReplyToken, message_replyToken);
            Assert.AreEqual(messageEvent.Type.ToString().ToLower(), message_type);
            Assert.AreEqual(messageEvent.Timestamp, message_timestamp);
            Assert.AreEqual(messageEvent.Source.Type.ToString().ToLower(), message_source_type);
            Assert.AreEqual(messageEvent.Source.EntryId, message_source_userId);
            Assert.AreEqual(messageEvent.Message.Id, message_message_id);
            Assert.AreEqual(messageEvent.Message.Type.ToString().ToLower(), message_message_type);
            Assert.AreEqual(((TextEventMessage)(messageEvent.Message)).Text, message_message_text);

            var followEvent = (FollowEvent)events[1];
            Assert.AreEqual(followEvent.ReplyToken, follow_replyToken);
            Assert.AreEqual(followEvent.Type.ToString().ToLower(), follow_type);
            Assert.AreEqual(followEvent.Timestamp, follow_timestamp);
            Assert.AreEqual(followEvent.Source.Type.ToString().ToLower(), follow_source_type);
            Assert.AreEqual(followEvent.Source.EntryId, follow_source_userId);

            var unfollowEvent = (UnfollowEvent)events[2];
            Assert.AreEqual(unfollowEvent.Type.ToString().ToLower(), unfollow_type);
            Assert.AreEqual(unfollowEvent.Timestamp, unfollow_timestamp);
            Assert.AreEqual(unfollowEvent.Source.Type.ToString().ToLower(), unfollow_source_type);
            Assert.AreEqual(unfollowEvent.Source.EntryId, unfollow_source_userId);

            var joinEvent = (JoinEvent)events[3];
            Assert.AreEqual(joinEvent.Type.ToString().ToLower(), join_type);
            Assert.AreEqual(joinEvent.Timestamp, join_timestamp);
            Assert.AreEqual(joinEvent.Source.Type.ToString().ToLower(), join_source_type);
            Assert.AreEqual((joinEvent.Source).EntryId, join_source_groupId);

            var leaveEvent = (LeaveEvent)events[4];
            Assert.AreEqual(leaveEvent.Type.ToString().ToLower(), leave_type);
            Assert.AreEqual(leaveEvent.Timestamp, leave_timestamp);
            Assert.AreEqual(leaveEvent.Source.Type.ToString().ToLower(), leave_source_type);
            Assert.AreEqual((leaveEvent.Source).EntryId, leave_source_groupId);

            var bostbackEvent = (PostbackEvent)events[5];
            Assert.AreEqual(bostbackEvent.Type.ToString().ToLower(), postback_type);
            Assert.AreEqual(bostbackEvent.Timestamp, postback_timestamp);
            Assert.AreEqual(bostbackEvent.Source.Type.ToString().ToLower(), postback_source_type);
            Assert.AreEqual(bostbackEvent.Source.EntryId, postback_source_userId);
            Assert.AreEqual(bostbackEvent.Postback.Data, postback_postback_data);

            var beaconEvent = (BeaconEvent)events[6];
            Assert.AreEqual(beaconEvent.Type.ToString().ToLower(), beacon_type);
            Assert.AreEqual(beaconEvent.Timestamp, beacon_timestamp);
            Assert.AreEqual(beaconEvent.Source.Type.ToString().ToLower(), beacon_source_type);
            Assert.AreEqual(beaconEvent.Source.EntryId, beacon_source_userId);
            Assert.AreEqual(beaconEvent.Beacon.Hwid, beacon_beacon_hwid);
            Assert.AreEqual(beaconEvent.Beacon.Type.ToString().ToLower(), beacon_beacon_type);
            Assert.AreEqual(beaconEvent.Beacon.Dm, beacon_beacon_dm);

        }
    }
}
