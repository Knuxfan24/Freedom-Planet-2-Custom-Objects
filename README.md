# Freedom Planet 2 Custom Objects

A repository of code for any custom objects I write for my own Freedom Planet 2 stage mods, organised by the stages they were originally intended for.

## Building

First off, ensure that your system has [Visual Studio 2022](https://visualstudio.microsoft.com/) installed alongside the `.NET Framework 3.5 development tools`, as well as [Unity 5.6.3](https://unity.com/releases/editor/whats-new/5.6.3#installs).

Open the solution file in VS2022 then go to `Tools > Options` and select `Package Sources` under the `NuGet Package Manager` category. Then add a package source called `BepInEx` with the source url set to `https://nuget.bepinex.dev/v3/index.json`.

Next, go to the `Assemblies` category in the `Dependencies` for the project, then delete the `Assembly-CSharp` reference. Right click on the Assemblies category and click `Add Assembly Reference...`, then click `Browse...` and navigate to Freedom Planet 2's install directory. Open the `FP2_Data` directory, then the `Managed` directory and select the `Assembly-CSharp.dll` file. Click Add then click OK.

You should now be able to right click the solution and choose `Rebuild` to build the mod.

## Installing

Navigate to `BepInEx/plugins` and copy the `FP2_Knux_Objects.dll` file from the build (`bin/Debug/net35` or `bin/Release/net35`) into it.

## Included Objects

### Common

#### Item Monitor

A Sonic styled item monitor, currently supports giving any of the five shield types, 10 crystal shards, 7 life petals, 7 blood crystals, invincibility, an Extra Life or a damaging Merga item. The monitor's item type will respect usage of the No Petals, Crystals to Petals and Items to Bombs Brave Stones, as well as the Strong Shields Potion.

This object expects to be attached to the root of an object for the monitor itself, with the display being a child object. Both need an animator, with the main monitor body's animator having an animation called "Broken" (for when the monitor is opened and its remains are left behind) and the child animator's animator having animations for all the different items (see the itemType switch statement in the idle state for the animation names and which item they correspond to).

For this reason, an Asset Bundle should be used by any stage mods utilising this object. Said Asset Bundle needs to be set in the monitor script upon creation, or the monitor object will delete itself with an error in the BepInEx console.

The sound names used by the monitor object (by default) match things from Sonic Origins and Sonic Mania, but the names can be overridden when creating the monitor script and attaching it to whatever object requires it.

#### Scripted Tube

A tube that forces the player character to roll along a preset path, ignoring any stage collision not placed on the Foreground C plane (which seems to be unused in the base game anyway) along the way.

This object requires an array of points to be assigned to it when creating the script, these points will be travelled to in sequence, with the final one ejecting the player out to the right in Carol's rolling state while setting them back to the Foreground A plane.

#### Zoom Tube Point

A tube that puts the player into the Ball Physics state, causing them to roll without player input.

This object needs to have an exit direction set when creating it. This direction controls where a second hitbox to remove the player from their rolling state is placed. In addition, an array of animation names can be set as BlacklistedAnimations, if the player passes through the Zoom Tube Point hitbox while in an animation in this array, they will NOT be set to the Ball Physics state and will instead just pass straight through the point.

### Hidden Palace Zone

#### HPZ Pipe Valve

A valve that shoots the player up in a water geyser when they cross it. This object expects multiple child objects for the various pieces of the geyser, as well as an animator containing an animation called spin, which should loop back into its idle animation upon completion.

This object requires an Asset Bundle to be set when creating it, or it will delete itself with an error in the BepInEx console. In addition, the sound name used by this object defaults to `classic_watergeyser`, but can be changed when creating and attaching the script to an object.

#### HPZ Sinking Platform

A platform which begins to move down when the player steps onto it. Upon stepping off, the platform rises back to its original height. This is intended to simulate a platform floating on the surface of water and rising back to said surface once the weight is removed from it, but water is not strictly required (and does not impact this object's behaviour at all).

#### HPZ Waterfall

A script used to build the waterfalls found in Hidden Palace Zone. This is intended to be attached to a dummy object that is located where the top piece of the waterfall is created, with a segment count specified. The object will then find three template objects (called `WaterfallTop`, `WaterfallMiddle` and `WaterfallBottom`) and use them to construct the fall. The amount of segments determines how many middle segments are created, each being 32 units lower than the previous one, with the bottom segment placed at the lowest point, but 16 units higher to account for its differing height.