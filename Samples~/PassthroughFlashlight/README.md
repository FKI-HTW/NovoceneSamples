# Passthrough Flashlight

Shows an example of layering passthrough with different brightness scalings to create a flashlight effect in the passthrough.

## Usage

To imitate the setup provided by the samples, several steps have to be taken:

1. Simply add 2+ Passthrough Building Blocks to the scene.
2. The background passthrough must have the following values:
    - ProjectionSurface: Reconstructed
    - Placement: Underlay
3. The additional passthrough layers must have their ProjectionSurface set to UserDefined and be given a reference to a MeshFilter. That mesh must have Read/Write enabled in its import settings. 
4. The pasthrough layers' CompositionDepths must be ordered by lowest value will be in front to highest value must be the background layer.
5. In the ColorControl the layers can be invidually adjusted as needed.

The additional passthrough layers can also be enabled/disabled as needed. This can be seen in the sample, where the flashlight layer is enabled once the Grabbable is selected.