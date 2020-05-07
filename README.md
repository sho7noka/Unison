# Python.Passing
Python.Passing is a python game programming solution for C# (Unity, Godot and .NET)
extends for [pythonnet](http://pythonnet.github.io/).


## 概要
C# をゲームスクリプトに据えたシステムにおいて、プログラマーとアーティスト双方向の摩擦労力は大きいです。

Pythonを仲介することで、テクニカルアーティストやゲームデザイナーに問題解決能力を提供します。

より一般的なシリアル化、ORM、動的プロキシ等、データベースやスプレッドシートから抽出したテーブルを扱う際には、

[T4](https://docs.microsoft.com/ja-jp/visualstudio/modeling/code-generation-and-t4-text-templates?view=vs-2019) や [LLBLGen](https://www.llblgen.com/) を検討してください。


### embeded Client on Lightweight Language
プログラマーがゲームクライアントを開発する上でPythonのような軽量スクリプト言語を組み込むと便利な機能に

以下のようなものがあります。特に開発/保守期間の長いサービスを提供する上で工数削減を期待できます。

- スコープを指定したオートゲームプレイとテスト(TCP/IP必要かな)
- アセット作成ツール(Maya, Photoshop等)とネイティブ相互作用
- 持続可能なサービスのための膨大なエンジン内アセットの継続的なメンテナンス


### Sourcecode as the Asset
アーティストがxml,csv,jsonあるいは独自ファイルフォーマットなどを仲介してComponent Entityを

生成する用途に以下のようなものがあります。プログラマーの工数を割く必要なしにアセットの細かく挙動を制御する際に便利です。

- シェーダ : マテリアル/パーティクル
- イベント : 物理エンジン/オーディオ/コリジョン
- レベル : 地形、天候、草木
- その他 : サンドボックス要素など

### ライセンス
[MIT](./License.md)

---

#### コード生成
従来のStringBuilderを含め、動的コード生成には以下のようなものがありますが、

JITではなくAOTや.NETの互換性を考慮して下記２つを採用します。

- [x] CodeDom (System.CodeDom)
- [x] AssemblyBuilder (System.Reflection.Emit)
- [ ] DynamicMethod (System.Reflection.Emit)
- [ ] ExpressionTree (System.Linq.Expressions) 
- [ ] Roslyn (Unity requires .Net 4.x API compatibility level)
- [ ] [T4](http://neue.cc/2019/12/06_585.html)
- [ ] [Mono.Cecil](https://qiita.com/pCYSl5EDgo/items/4146989d08e169dde81d)
- [ ] [Fody](https://github.com/Fody/Fody)_
- [ ] [.NET5](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/)

##### 参考URL
- [](http://blog.shos.info/archives/2013/11/csharp_metaprogramming.html)
- [](https://ufcpp.net/study/csharp/misc_dynamic.html)
- [](http://neue.cc/2017/12/04_560.html)
