# TopDownARPGController

This is an old unfinished project from me. It contains a costum top down Rigidbody PlayerController,
a nice Entity Logic system to connect things in the editor (like press Button -> Platform starts moving) using the Unity UI-Callbacks
and serveral small useful scripts. Its missing some stuff (like some Animations) to be a fully functional demo, but you can start 
the Test-Scene, walk around etc.

It contains some of the standard assets (like some animations, models, scripts etc.) which are made by Unity and are available 
at the AssetStore.

Feel free to download / fork it and play with it.

## PlayerController

The PlayerController is designed in a way so it can also be controlled by an AI without chaning the controller itself. 
The Test-Scene also contains one AI as Example. It can Auto-Jump if it comes to a ledge (like in Zelda- OOT), 
but can also be set to manual jump. It uses a Rigidbody so is fully affected by the physics system.

## EntitySystem

<img src="http://i.imgur.com/HnMtT8Z.png" alt="alt text" width="405" height="275">
You'll find the scripts under TopDownRPGController\Scripts\LevelObjects\EntitySystem and also some Example entities 
in the TestScene.

