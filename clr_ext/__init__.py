import platform
from clr_ext import patch

if platform.platform() == "Windows":
    AddReference = patch.AddReferenceUnManage
else:
    AddReference = patch.AddReferenceFrom

del platform, patch
