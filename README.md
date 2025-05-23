# Freedom Planet 2 Custom Objects

A repository of code for any custom objects I write for my own Freedom Planet 2 stage mods, organised by the stages they were originally intended for. For documentation regarding setting up and utilising these objects within your own code, check this repositories' wiki.

## Building

First off, ensure that your system has [Visual Studio 2022](https://visualstudio.microsoft.com/) installed alongside the `.NET Framework 3.5 development tools`, as well as [Unity 5.6.3](https://unity.com/releases/editor/whats-new/5.6.3#installs).

Open the solution file in VS2022 then go to `Tools > Options` and select `Package Sources` under the `NuGet Package Manager` category. Then add a package source called `BepInEx` with the source url set to `https://nuget.bepinex.dev/v3/index.json`.

Next, go to the `Assemblies` category in the `Dependencies` for the project, then delete the `Assembly-CSharp` reference. Right click on the Assemblies category and click `Add Assembly Reference...`, then click `Browse...` and navigate to Freedom Planet 2's install directory. Open the `FP2_Data` directory, then the `Managed` directory and select the `Assembly-CSharp.dll` file. Click Add then click OK.

You should now be able to right click the solution and choose `Rebuild` to build the mod.

## Installing

> **Note**
> The intended installation setup for this mod's DLL has yet to be entirely decided upon, so this instruction may change on a whim!

Navigate to `BepInEx/plugins` and copy the `FP2_Knux_Objects.dll` file from the build (`bin/Debug/net35` or `bin/Release/net35`) into it.