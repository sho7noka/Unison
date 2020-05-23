# Unisonus
Unisonus is a python game development solution for C# (Unity, Godot and .NET)
which extend on [pythonnet](http://pythonnet.github.io/).

## About
In a C# scripted game engine, the amount of friction between programmers and artists is significant.

By brokering Python, we provide artists and game designers with the ability to solve complex game development problems.



For more general uses of tables extracted from databases and spreadsheets, such as serialization, ORM, and dynamic proxies, 

you can use Consider [T4](https://docs.microsoft.com/ja-jp/visualstudio/modeling/code-generation-and-t4-text-templates?view=vs-2019) or [LLBLGen](https://www.llblgen.com/).

## Installation
1. [](https://docs.unity3d.com/Packages/com.unity.scripting.python@2.0/manual/)
2. [forum](https://forum.unity.com/threads/introducing-python-for-unity-editor.812748/)
3. []

Packages/manifest.json for Unity(optional).
```json
{
  "dependencies": {
    "com.unity.scripting.python": "2.0.1-preview.2",
  }
}
```

### embeded Client on Lightweight Language
プログラマーがゲームクライアント上でPythonのような軽量スクリプト言語を組み込むと便利な機能に

以下のようなものがあります。特に保守期間の長いサービスを提供する上で工数削減を期待できます。

- シーン遷移を含むエディターでのオートゲームプレイとテスト
- アセット作成ツール(Maya, Photoshop等)とネイティブ相互作用
- 持続可能なサービスのためのエンジン内アセットの継続的なメンテナンス


インターフェース定義
```c#
using UnityEngine;
using Unison.Extensions;

public class TestScript {

    [PyRPC("get")]
    public static void Get(){
    
    }
}
```

RPCサーバの開始
```c#
using UnityEditor;
using Unison;

public class EditorPresenter
{
    [InitializeOnLoadMethod]
    void Start() {
        InterPreter.rpcServer("121.0.0.1", 8888)
    }
}
```

RPCクライアントからアクセス
```python
import clr_ext as clr

unity = clr.rpcClient("121.0.0.1", 8888)
unity.Play()
unity.Stop()
```

### Sourcecode as a Asset
アーティストが xml,csv,json あるいは独自ファイルフォーマットなどを仲介してComponent Entityを

生成する用途に以下のようなものがあります。プログラマーの工数を割く必要なしにアセットの細かい挙動を制御する際に便利です。

- シェーダ : マテリアル/パーティクル
- イベント : 物理エンジン/オーディオ/コリジョン
- レベル : 地形、天候、草木
- その他 : サンドボックス要素など

```python
import clr_ext.generate as gen

gen.gen_cs()
```


一般におけるコード生成
- リフレクション、ジェネリックのコスト削減, AOT
- データフォーマットのシリアライズ
- ネイティブコードのデータバインド

ゲーム制作におけるコード生成
- アセットのランタイムパラメーター
- デバッグコマンド、スクリプトエンジン
- 

イテレーション

# デバッグ
https://github.com/yasirkula/UnityIngameDebugConsole
https://www.stompyrobot.uk/tools/srdebugger/documentation/#console-1
http://sprfield.hatenablog.jp/entry/2017/09/27/115754

### ライセンス
[MIT](./License.md)
