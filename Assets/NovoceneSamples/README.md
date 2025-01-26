# NovoceneSamples

This repository provides a Unity package, which contains samples from the [Novocene](https://aurora.htw-berlin.de/novocene/) project developed by [AURORA XR School for Artists](https://aurora.htw-berlin.de/) and [Jens Isensee](https://jensisensee.de/). This project is an interactive Mixed Reality experience for the Meta Quest HMDs. The provided samples are open-source and can be imported and used in any other Unity project, which uses a compatible version of the Meta XR SDK and Universal Rendering Pipeline.

## Samples

The following samples are contained within the package and can be imported individually into any project:

- [Ceiling Cutout](https://github.com/FKI-HTW/NovoceneSamples/tree/develop/Assets/NovoceneSamples/Samples~/CeilingCutout):
    - Enables Meta Quest passthrough and spawns a cutout in the physical room's ceiling to show the skybox above.
- [Passthrough Flashlight](https://github.com/FKI-HTW/NovoceneSamples/tree/develop/Assets/NovoceneSamples/Samples~/PassthroughFlashlight):
    - Shows an example of layering passthrough with different brightness scalings to create a flashlight effect in the passthrough.
- [Scene Anchoring](https://github.com/FKI-HTW/NovoceneSamples/tree/develop/Assets/NovoceneSamples/Samples~/SceneAnchoring):
    - Provides scripts for anchoring objects in user-defined position, rotation, and scaling, saving the anchors between sessions and scenes.
- [Underwater Scene](https://github.com/FKI-HTW/NovoceneSamples/tree/develop/Assets/NovoceneSamples/Samples~/UnderwaterScene):
    - Contains a rising and lowering water shader in the physical passthrough room with object's buoyancy being physically simulated.

While these samples can be used as they are, they are primarily meant to be used as bases for you to implement your own solutions on top. 
Each individual sample also provides an additional README file for information on its functionality and usage.

## Installation

Download the package through the <strong>Window > Package Manager</strong> in the Unity Editor with <strong>Add package from git URL...</strong>.
Using the URL <strong>https://github.com/FKI-HTW/NovoceneSamples.git#upm</strong> for the newest version, or <strong>https://github.com/FKI-HTW/NovoceneSamples.git#VERSION_TAG</strong> for a specific one. The available version tags can be found in [Tags](https://github.com/FKI-HTW/NovoceneSamples/tags).

Once downloaded, the individual samples can be imported in the package manager, in the NovoceneSamples package > Samples. The samples will then be imported into the Assets/Samples folder and can either be used directly or used as a base to add your own implementations on top.