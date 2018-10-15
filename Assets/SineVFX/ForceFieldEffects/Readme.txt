ForceField Effects
Version 1.0 (16.07.2018)


IMPORTANT NOTES:

- Use A and D or Arrows to change the effects
- Turn on "HDR" on your Camera, Shaders requires it
- This VFX Asset looks much better in "Linear Rendering", but there is also optimized Prefabs for "Gamma Rendering" Mode
- Image Effects are necessary in order to make a great looking game, as well as our asset. Be sure you use "Tone Mapping" and "Bloom"
- We also recommend using Deferred Rendering for better performance

PERFORMANCE TIPS:

- If you planning to use multiple ForceField effects in a limited space, we highly recommend you to use "Multiple" type of prefabs
- You can turn off "Distortion Noise" and "Noise02" in material properties for better performance
- Reduce the size of the textures that are used in the material
- Turn On texture image compression
- Turn Off an entire distortion layer, like in prefab 04 and 13 prefabs
- In a "ForceFieldController" scripts do not enable "ProcedrualGradientUpdate" boot if you don't need to update your gradient constantly

HOW TO USE:

First of all, check for Demo Scene in Scenes folder, all effects are located in a "CompleteEffects" empty GameObject. Also, there is a Prefabs folder with complete effects.
Just Drag and Drop prefabs from "Prefabs" folder into your scene.
We made all Shaders very tweakable, so you can create your own unique effects.

FORCEFIELD BASIC:

It's a simple ForceField prefab, which contains only one spherical field. You can drop them in any Scene and use them without any additional tweaks.

FORCEFIELD BASIC MULTIPLE:

Same as prefabs above, but using multiple sphere meshes to generate ForceField array. All the sub-meshes are located in a "ForceFieldMeshes"
gameObject. You can move and scale them as you like, you can also add unlimited amount of sphere meshes to them. Also, make sure that all the spheres have colliders and
"ControlParticleSpawner" scripts on them.

FORCEFIELD BASIC MESH:

ForceField effects on regular meshes, vehicles for example. If you want to add ForceField effect on your custom mesh you need to do some steps.
Add a "ForceFieldController" script to your mesh or any child gameObject. Add a control particle system to your mesh, you can copy it from one of the prefabs.
Configure the first script, you may just copy the values from one of the prefabs. If your mesh consists of a small number of child meshes, you may add each of them in a
"GetRenderersCustom" variable. Otherwise, use the "GetRenderersInChildren" variable. You also need to add colliders to your mesh and "ControlParticleSpawner" for each
of them.

SHADERS CONTROL:

Affector Count - Used in Array/Multiple shaders, defines how many affectors/sources your Array ForceField effect will have
Final Power - Final brightness of the image, you need to lower this value if you using "Gamma Rendering" Mode
Final Power Adjust - Subtract the value from the mask, use it to tweak the initial look of the effect
Opacity Power - Power of the opacity mask, does not affect the brightness
Local Noise Position - Lock noise position to the object
Normal Vertex Offset - Simple vertex offset, use it for distortion layers to make them slightly bigger
Cubemap Reflection - Adds a reflection from the Reflection probe to the shader

Ramp - Ramp gradient texture, used for better coloring
Ramp Color Ting - multiply Ramp texture but this color
Ramp Multiply Tiling - Modifies Ramp texture and moving its colors
Ramp Flip - Flip the Ramp texture

Mask Fresnel Exp - Exp value of the Rim effect mask
Mask Depth Fade - Depth Fade effect, used mostly of spherical ForceFields, don't use it in custom meshes
Mask Soft Borders - For Distortion shaders only, Control how soft the borders of a distortion

Noise Mask Power - Power of the noise mask
Noise Mask Add - Power of the rim effect mask
Noise 01 - The Main noise of the effects
Noise 02 - Aditional noise
Noise Distortion - Settings for noise distortion control

Mask Appear Local Y Remap - Value to control the length of the mask edges
Mask Appear Local Y Add - Offset parameter for the mask
Mask Appear Invert - Invert the opening effect
Mask Appear Progress - Open/Close ForceField value
Mask Appear Noise - Main noise for the opening effect
Mask Appear Noise Triplanar - Use triplanar noise distribution instead of mesh UV
Mask Appear Use World Position - Use World Pixel Position instead of Local Vertex Position, use this for custom mesh effects

Hit Wave Noise Negate - Control how much noise affect the shape of the hit wave
Hit Wave Length - Length of the wave
Hit Wave Fade Distance - Fade distance of the hit wave
Hit Wave Fade Distance Power - Multiply Fate Distance mask by this value
Hit Wave Ramp Mask - Uses Ramp texture for better hit wave effect
Hit Wave Distortion Power - Control how much Distortion noise affect the shape of the hit wave

Threshold For Interception - Threshold value to control the interception edges of the spheres, adjust carefully (Default value is 1.001)
Threshold For Spheres - Threshold value to control the overlay mask of the spheres, adjust carefully (Default value is 0.99)




Support email: sinevfx@gmail.com