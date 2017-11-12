# Microsoft Bot Framework and LINE Messaging API Connector

[日本語の説明はこちら](./README_JP.md)

This folder contains Microsoft Bot Framework and LINE Messaging API Connector.

# Microsoft Bot Framework
Microsoft Bot Framework provides flexible, yet powerful framework to build bot, and it is super easy to connect to multiple chat platform such as Skype, Facebook, Slack, etc.

See more detail [here](https://dev.botframework.com/).

# LINE connector for Microsoft Bot Framework
Unfortunately, there is no native connector for LINE platform. This program works as a connector, which you just simply deply to somewhere public.

## Who should use this?
There are two types of users.

- Developers who already running the bot using Bot Framework, and who want to expand the channel to LINE.
- LINE Bot developers who want to utilize Bot Framework.


# How this works
Idea is simple. 
1. When user send message to bot, this connector receives it as LINE events.
1. Extrat the LINE events and craft messages for Bot Framework.
1. Then send the message to Bot Connector via DirectLine.
1. Once Bot Framework processes the request, the connector retrives the reply.
1. Parse the reply from Direct Line, and craft reply message for LINE platform.

# Setup
## [Prerequisites]
You need to have followings.

- [Azure Subscription](https://azure.microsoft.com)
- [Visual Studio with ASP.NET and Web development workload](https://www.visualstudio.com/vs/)
- [Microsoft Bot Framework account](https://dev.botframework.com/)
- [git](https://git-scm.com/downloads)
- [LINE Developer Account and Messaging API application](https://developers.line.me/en/)
- [ngrok (for local test)](https://ngrok.com/)

## [Get the source code]
You can download zip file or use git clone command to copy source. You can also create the project from template.

## [Config update]
Fill the parameters in Web.config

 - StorageConnectionString: Azure Storage connection string
 - DirectLineSecret: Bot Framework Direct Line Secret
 - ChannelSecret: LINE Messaging API Channel Secret
 - ChannelAccessToken: LINE Messaging API AccessToken

# Compile, Test and Publish
## [Compile]
Use Visual Studio to compile the app.
1. Open Visual Studio and open the solution.
2. From Build menu, run "Build Solution". Select "Debug" or "Release" depending on how you want to build.

## [Test locally]
If you want to test it locally, do the following.
1. In Visual Studio, make the project as start up project.
1. Hit F5 to start debugging. It opens a web browser, so confirm the port number (default: 1590)
1. Open command prompt or console and run ```>ngrok http --host-header=localhost:1590 1590``` Change the port number appropriately. Copy the output of ngrok address. <br/>
![ngrok.PNG](../ImagesForREADME/ngrok.PNG)
1. Go to [Line Developer Console](https://developers.line.me/console/) and open the Messaging API application.
1. Update "Webhook URL Requires SSL" setting for ngrok URL. Do not forget to add /api/LineBot at the end, then save. <br/>
![LINEWebhookUrl](../ImagesForREADME/LINEWebhookUrl.PNG)

Now when you send message to the bot via LINE application, the request is routing to the local instance, thus you can debug it.

Whenever you need to chagne the code, just stop the Visual Studio debugging only, and do not end ngrok session. As long as you keep running the ngrok session, you can keep using the same ngrok URL for debug.

## [Publish the Web App]
You can publish the Web App to Azure from Visual Studio.
1. Right-click the project and select "publish".
1. Follow the wizard to publish it to Azure Web App.
1. Once publish completed, confirm the URL.
1. Go to [Line Developer Console](https://developers.line.me/console/) and open the Messaging API application.
1. Update "Webhook URL Requires SSL" setting for Azure Web App URL, then save.
1. Send message to the bot from LINE application.

Enjoy!