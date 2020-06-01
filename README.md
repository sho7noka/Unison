# Unison
Unison is a python game development solution for C# Script (Unity, Godot and .NET)
which extend on [pythonnet](http://pythonnet.github.io/).


## About
In a C# scripted game engine, the amount of friction between programmers and artists is significant.

By brokering Python, we provide artists and game designers with the ability to solve complex game development problems.

It provides a simple bi-directional IDL between asset and client and runtime, just like building REST or RPC between client and server.

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
Some of the features that can be useful in real-world development when incorporating a lightweight scripting language such as Python on a game client are

They include the following. It can be expected to reduce development man-hours, especially in providing services with long maintenance periods.

- Auto gameplay and testing in the editor, including scene transitions
- Native interaction with asset creation tools (Maya, Photoshop, etc.)
- Ongoing maintenance of in-engine assets for sustainable service

interface definition
```c#
using UnityEngine;
using Unison.Extensions;

public class TestScript {

    [PyRPC("get")]
    public static void Get(){
    
    }
}
```

start to RPC server
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

get RPC client
```python
import clr_ext as clr

unity = clr.rpcClient("121.0.0.1", 8888)
unity.Play()
unity.update_scene()
unity.Stop()
```


### Sourcecode as a Asset
For the need to generate master data around assets by intermediating XML, CSV, JSON, proprietary file formats, etc.

They include the following. This is especially useful for artists controlling the detailed behavior of assets in situations where the programmer's time is not available.

- Shader : Material, Particle
- Event : Physics, Audio, Collision
- Level : Landscape、Weather、Grass or Tree
- Other : SandBox、RuntimeCommand

make parameters
```python
import clr_ext.generate as gen

gen.gen_cs()
```

gen master data
```python
import clr_ext.generate as gen


```


#### License
[MIT](./License.md)
