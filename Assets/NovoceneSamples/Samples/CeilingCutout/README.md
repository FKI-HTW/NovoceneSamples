# Ceiling Cutout

Enables Meta Quest passthrough and spawns a cutout in the physical room's ceiling to show the skybox above.

In normal passthrough applications, the skybox is disabled in favor of a transparent background. This sample provides an example scene where a skybox can be used alongside the passthrough projected onto the scene mesh. The script CutoutSpawner shows a simple method to spawning any mesh. This mesh contains a custom stencil shader, provided alongside the sample. This stencil shader cuts a visual hole into the effect mesh, showing the skybox behind it. This allows developers to create a passthrough application where custom-shaped windows can be cut into the passthrough and show a virtual scene underneath.

## Usage

To imitate the setup provided by the samples, several steps have to be taken:

1. Add a Meta Camera Rig to the scene with the CenterEyeAnchor BackgroundType set to Skybox.
2. Add a Passthrough, MRUK, EffectMesh, and LayerApplier Scripts/Building Blocks to the scene.
3. The EffectMesh must apply an Occlusion material derived from Oculus/SelectivePassthrough shader and set the Render Queue to be below 3000 (ex. AlphaTest = 2450). Such a material can also be copied from the sample.
4. Add new Wall and Furniture Layers to the Unity project.
5. The EffectMesh SpawnOnStart must be set to None. The MRUK Room Created Event should call the EffectMesh.CreateMesh and LayerApplier.ApplyMRUKLayers in that order.
6. Create a new material derived from the provided Stencil shader and set its StencilID to a desired value and RenderQueue to 3000+.
7. Add a Renderer Feature to your URP Data Assets (ex. Medium_PipelineAsset_ForwardRenderer) for the Wall that should be cutout. The Renderer Feature must have the following values:
    - Event: AfterRendering
    - Queue: Opaque
    - LayerMask: Wall (or Furniture if the cutout should be in furniture instead)
    - Stencil: True
    - Value: the previously set StencilID
    - CompareFunction: Not Equal
    - Pass/Fail/ZFail: Keep
7. Add the stencil material to a mesh and place/instantiate the mesh inside a room wall. For this, the CutoutSpawner can be called in the MRUK Room Created Event after the LayerApplier.