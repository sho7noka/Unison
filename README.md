# {Key}
{Key} is a python game programming solution for C# (Unity, Godot and .NET)
which extend on [pythonnet](http://pythonnet.github.io/).

## About
In a C# scripted game engine, the amount of friction between programmers and artists is significant.

By brokering Python, we provide technical artists and game designers with the ability to solve complex game development problems.

IDL定義(Asset Client Runtime) クライアント・サーバ間でRESTやRPC間で双方向アセットとクライアント間の簡易的なIDL定義を提供します。

For more general uses of tables extracted from databases and spreadsheets, such as serialization, ORM, and dynamic proxies, 

you can use Consider [T4](https://docs.microsoft.com/ja-jp/visualstudio/modeling/code-generation-and-t4-text-templates?view=vs-2019) or [LLBLGen](https://www.llblgen.com/).

## Installation
[](https://docs.unity3d.com/Packages/com.unity.scripting.python@2.0/manual/)
[forum](https://forum.unity.com/threads/introducing-python-for-unity-editor.812748/)

Packages/manifest.json
```json
{
  "dependencies": {
    "com.unity.scripting.python": "2.0.1-preview.2",
  }
}
```

### embeded Client on Lightweight Language
プログラマーがゲームクライアントを開発する上でPythonのような軽量スクリプト言語を組み込むと便利な機能に

以下のようなものがあります。特に保守期間の長いサービスを提供する上で工数削減を期待できます。

- シーン遷移を含むエディターでのオートゲームプレイとテスト
- アセット作成ツール(Maya, Photoshop等)とネイティブ相互作用
- 持続可能なサービスのためのエンジン内アセットの継続的なメンテナンス


```c#
using UnityEngine;
using Python.Passing;

public class TestScript {

    [PyRPC("get")]
    public static void Get(){
    
    }

    public static void register(){
        PyRPC.Add(Get);
    }
}
```

```c#
using UnityEditor;
using Python.Passing;

public class EditorPresenter
{
    [InitializeOnLoad]
    void Start() {
        InterPreter.rpcClient()
    }
}
```

```python
import clr_ext as clr

unity = clr.rpcClient()
unity.Play()
unity.Get()
```

### Sourcecode as a Asset
アーティストが xml,csv,json あるいは独自ファイルフォーマットなどを仲介してComponent Entityを

生成する用途に以下のようなものがあります。プログラマーの工数を割く必要なしにアセットの細かい挙動を制御する際に便利です。

- シェーダ : マテリアル/パーティクル
- イベント : 物理エンジン/オーディオ/コリジョン
- レベル : 地形、天候、草木
- その他 : サンドボックス要素など

```python
import clr_ext.generate
import clr_ext as clr
clr_ext.generate.gen_cs()
```




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

https://docs.unity3d.com/Manual/CustomPackages.html
https://docs.godotengine.org/ja/latest/tutorials/assetlib/uploading_to_assetlib.html