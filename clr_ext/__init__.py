import platform
import clr_ext.patch as __patch

if platform.platform() == "Windows":
    AddReference = __patch.AddReferenceUnManage
else:
    AddReference = __patch.AddReferenceFrom

del platform, __patch
