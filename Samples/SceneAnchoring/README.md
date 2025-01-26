# Scene Anchoring

Provides scripts for anchoring objects in user-defined position, rotation, and scaling, saving the anchors between sessions and scenes.

## Usage

To imitate the setup provided by the samples, several steps have to be taken:

1. Add a Passthrough, MRUK, and EffectMesh Scripts/Building Blocks to the scene.
2. Add SpatialAnchorManager and AnchorObjectPlacer scripts to the scene. Optionally add a RequestRoomSetup script to the scene. 
4. The EffectMesh SpawnOnStart must be set to None. The MRUK Room Created Event should call the EffectMesh.CreateMesh and AnchorObjectPlacer.AddLoadedRoom method. The MRUK Room Remove Event should call the AnchorObjectPlacer.RemoveLoadedRoom.
5. Add AnchorID scripts to any object whose anchor should be set and define their name and positional label.
6. The AnchorObjectPlacer should have a reference to a controller or other transform for aiming and all AnchorIDs.
7. The anchors can now be set with the controls defined in the AnchorObjectPlacer script. The controls include selecting AnchorIDs, moving, rotation, and saving them etc.