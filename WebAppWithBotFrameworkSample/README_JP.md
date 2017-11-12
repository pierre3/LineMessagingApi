# LINE Messaging API 用 Microsoft Bot Framework コネクター


# Microsoft Bot Framework
Microsoft Bot Framework はボット開発者に、柔軟で強力なフレームワークを提供します。また開発したボットは、Skype や Facebook、Slack などの多くのチャネルに公開が可能です。

詳細は [こちら](https://dev.botframework.com/).

# LINE 用 Microsoft Bot Framework コネクター 
残念ながら現時点で LINE 用のネイティブコネクターが Bot Framework で提供されていません。このプロジェクトはその問題を解決するもので、外部からアクセスできる場所に公開するだけでコネクターとして機能します。

## 対象者
主に以下の方を対象としています。

- Bot Framework でボットを既に開発していて、LINE にも公開したい方。
- LINE ボットの開発者で Bot Framework を活用したい方。

# どのように機能するか
考え方はシンプルです。 
1. ユーザーが LINE でメッセージをボットに送信すると、コネクターがそのメッセージを受信。
1. 受信したメッセージをパースし、Bot Framework 用のメッセージに変換。
1. 変換したメッセージをダイレクトライン経由で Bot Connector に送信。
1. 処理された結果をコネクターで受信。
1. ダイレクトラインからの返信をパースして、LINE 用のメッセージに変更。
1. LINE に返信。

# セットアップ
## [前提条件]
以下のモジュールが必要です。

- [Azure サブスクリプション](https://azure.microsoft.com)
- [Visual Studio および ASP.NET と Web 開発ワークロード](https://www.visualstudio.com/vs/)
- [Microsoft Bot Framework account](https://dev.botframework.com/)
- [git](https://git-scm.com/downloads)
- [LINE 開発者アカウントおよび Messaging API アプリケーション](https://developers.line.me/en/)
- [ngrok (ローカルテスト用)](https://ngrok.com/)

## [ソースコードの取得]
zip ファイルをダウンロードして解凍するか、git clone コマンドでレポジトリをコピーします。テンプレートからも同様のプロジェクトを作成できます。

## [構成の更新]
Web.config の設定を更新します。

- StorageConnectionString: Azure ストレージの接続文字列
- DirectLineSecret: Bot Framework ダイレクトラインシークレット
- ChannelSecret: LINE Messaging API チャネルシークレット
- ChannelAccessToken: LINE Messaging API アクセストークン

# コンパイル、テストおよび公開
## [コンパイル]
Visual Studio でコンパイルします。
1. Visual Studio でソリューションを開く。
1. ビルドメニューより ”ソリューションのビルド” を実行します。必要に応じてデバッグやリリース構成を切り替え。

## [ローカルでのテスト]
ローカルでデバッグテストをしたい場合は、以下の手順を行います。
1. Visual Studio で Web App を右クリックし ”スタートアップ プロジェクトに設定” をクリック。
![SetWebAppStartUpProject_JA.PNG](../ImagesForREADME/SetWebAppStartUpProject_JA.PNG)
1. F5 を押下してデバッグを開始。ブラウザが開くのでポートを確認。(既定: 1590)
1. マンドプロンプトやターミナルを開いて、>ngrok http --host-header=localhost:1590 1590 を実行。ポートは環境に合わせて変更。出力された ngrok のアドレスをコピー。<br/>
![ngrok.PNG](../ImagesForREADME/ngrok.PNG)
1. [Line 開発者コンソール](https://developers.line.me/console/) を開き、Messaging API アプリケーションを開く。
1. "Webhook URL Requires SSL" 設定を ngrok URL に変更。/api/LineBot をつけ忘れないように注意。 <br/>
![LINEWebhookUrl](../ImagesForREADME/LINEWebhookUrl.PNG)

この状態で LINE アプリケーションよりボットにメッセージを送ると、ローカルのデバッグ環境に送信されます。コードを変更したい場合は Visual Studio のデバッグ実行を停止するだけで、ngrok のセッションは止めないでください。セッションが有効な限り、LINE 側の設定変更は不要です。

## [Web App の公開]
Visual Studio から直接 Azure に公開できます。
1. プロジェクトを右クリックして、”発行” を選択。
1. ウィザードに従ってアプリケーションを公開。
1. ブラウザが開くので、URL を確認。
1. [Line 開発者コンソール](https://developers.line.me/console/) Messaging API アプリケーションを開く。
1. "Webhook URL Requires SSL" 設定を行更新。
1. LINE アプリケーションよりボットにメッセージを送信して動作確認。