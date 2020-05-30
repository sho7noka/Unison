アセットとクライアント、ランタイムの双方向間IDL定義を行い、
IDL定義(Asset Client Runtime) クライアント・サーバ間でRESTやRPC間で双方向アセットとクライアント間の簡易的なIDL定義を提供します。
scripting, shell or debug command.



クライアントとランタイム(実機)
エディタ拡張側で実行できる形式にする
unity incremental compiler
playerloop updateをフックしてawaitに戻す?
ScriptableObjectにマスタデータを持たせるメリットについて

# デバッグ
https://github.com/proletariatgames/CUDLR
https://techblog.kayac.com/transfer-assets-from-unity-editor-to-device
https://docs.unity3d.com/Manual/CustomPackages.html
https://docs.godotengine.org/ja/latest/tutorials/assetlib/uploading_to_assetlib.html
http://sprfield.hatenablog.jp/entry/2017/09/27/115754
https://github.com/yasirkula/UnityIngameDebugConsole
https://www.stompyrobot.uk/tools/srdebugger/documentation/#console-1
http://sprfield.hatenablog.jp/entry/2017/09/27/115754

### コード生成
一般におけるコード生成
- リフレクション、ジェネリックのコスト削減, AOT
- データフォーマットのシリアライズ
- ネイティブコードのデータバインド

- テンプレートを使ったコード生成。ASP.NET、XML Web サービス クライアント プロキシ、コード ウィザード、デザイナー
- 動的コンパイル。1つ以上の言語でのコードのコンパイルをサポートします。

ゲーム制作におけるコード生成
- アセットのランタイムパラメーター
- デバッグコマンド、スクリプトエンジン

#### コード生成
従来のStringBuilderを含め、動的コード生成には以下のようなものがありますが、

AOTや.NET互換性を考慮して下記２つを採用します。

- [x] CodeDom (System.CodeDom)
- [x] AssemblyBuilder (System.Reflection.Emit)
- [ ] DynamicMethod (System.Reflection.Emit)
- [ ] ExpressionTree (System.Linq.Expressions) 
- [ ] Roslyn (Unity requires .Net 4.x API compatibility level)
- [ ] [T4](http://neue.cc/2019/12/06_585.html)
- [ ] [Mono.Cecil](https://qiita.com/pCYSl5EDgo/items/4146989d08e169dde81d)
- [ ] [Fody](https://github.com/Fody/Fody)
- [ ] [.NET5](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/)

##### 参考URL
- [公式](https://docs.microsoft.com/ja-jp/dotnet/framework/reflection-and-codedom/)
- [metapro](http://blog.shos.info/archives/2013/11/csharp_metaprogramming.html)
- [dynamic](https://ufcpp.net/study/csharp/misc_dynamic.html)
- [template](http://neue.cc/2017/12/04_560.html)
- [UnityNative](https://jacksondunstan.com/articles/3938)
