# Unison
Unison is a python game development solution for C# (Unity, Godot and .NET)
which extend on [pythonnet](http://pythonnet.github.io/).

## About
In a C# scripted game engine, the amount of friction between programmers and artists is significant.

By brokering Python, we provide artists and game designers with the ability to solve complex game development problems.

For more general uses of tables extracted from databases and spreadsheets, such as serialization, ORM, and dynamic proxies, 

you can use Consider [T4](https://docs.microsoft.com/ja-jp/visualstudio/modeling/code-generation-and-t4-text-templates?view=vs-2019) or [LLBLGen](https://www.llblgen.com/).

## Installation
* pypi: `pip install clr_ext`
* nuget: `Install-Package python -Version 3.9.0-b1`
* Package Manager : add the following line `https://github.com/sho7noka/Unison.git`

Packages/manifest.json for Unity(optional).[document](https://docs.unity3d.com/Packages/com.unity.scripting.python@2.0/manual/)
```json
{
  "dependencies": {
    "com.unity.scripting.python": "2.0.1-preview.2"
  }
}
```

### embeded Client on Lightweight Language
ゲームクライアント上でPythonのような軽量スクリプト言語を組み込むと便利な機能に

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
        InterPreter.rpcServer("121.0.0.1", 8888);
    }
}
```

RPCクライアントからアクセス
```python
import clr_ext as clr

unity = clr.rpcClient("121.0.0.1", 8888)
unity.Play()
unity.update_scene()
unity.Stop()
```

### Sourcecode as a Asset
xml,csv,jsonあるいは独自ファイルフォーマットを仲介して、アセット周辺のマスターデータを生成するニーズには

以下のようなものがあります。プログラマーの工数が見込めない状況でアセットの細かい挙動を制御する際に便利です。

- シェーダ : マテリアル/パーティクル
- イベント : 物理エンジン/オーディオ/コリジョン
- レベル : 地形、天候、草木
- その他 : サンドボックス、ランタイムコマンドなど

パラメータの生成
```python
import clr_ext.generate as gen

gen.gen_cs()
```


```python

```

#### ライセンス
[MIT](./License.md)
