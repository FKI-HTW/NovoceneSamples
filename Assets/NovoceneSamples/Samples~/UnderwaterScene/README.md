# UnderwaterScene

Provides scripts for anchoring objects in user-defined position, rotation, and scaling, saving the anchors between sessions and scenes.

## Usage

To imitate the setup provided by the samples, several steps have to be taken:

1. Add a Passthrough, MRUK, and EffectMesh Scripts/Building Blocks to the scene.
2. The EffectMesh must apply an Occlusion material derived from Oculus/SelectivePassthrough shader and set the Render Queue to be 2451. Such a material can also be copied from the sample.
3. The EffectMesh SpawnOnStart must be set to None. The MRUK Room Created Event should call the EffectMesh.CreateMesh method.
4. The passthrough layer must be set to Reconstructed and Underlay.
5. Add the WaterSurface material and WaterVolume script to a plane.
6. Optionally, use the PlaneGenerator to generate a higher resolution plane than the default Unity one. To do this simply add the script to the plane, add the reference to the MeshFilter and define the path where the mesh should be saved. Then click the button outside the playmode.
7. Use the APIs and settings in the WaterVolume method to adjust the water height. Optionally adjust the WaterVolume script and remove the RaiseWater call in the start method to call it yourself.
8. Add the BuoyancySimulation script to any object that should be simulated inside the water plane.