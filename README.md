# TopDownARPGProject
Unity 5.6 Project for TopDown Action-RPG game.

![](http://i.imgur.com/ZZLKHlG.gif)
![](http://i.imgur.com/SHmtZPJ.gif)

This is an old unfinished project from me. It contains a costum top down Rigidbody PlayerController,
a nice Entity Logic system to connect things in the editor (like press Button -> Platform starts moving) using the Unity UI-Callbacks
and serveral small useful scripts. Its missing some stuff (like some Animations) to be a fully functional demo, but you can start 
the Test-Scene, walk around etc.

Features:
* Rigidbody PlayerController
* Simple AI
* Simple Health-System
* Useable Map-Entities with a Custom Event-Trigger system:
  * Moveable platform
  * Logic entities (Branch, Case, Compare, Counter, Timer)
  * Useable and triggerable
  * Sliding door
  * Floor buttons and switches
* Simple demo scene


It contains some of the standard assets (like some animations, models, scripts etc.) which are made by Unity and are available 
at the AssetStore.

Feel free to download / fork it and play with it.

## PlayerController

The PlayerController is designed in a way so it can also be controlled by an AI without chaning the controller itself. 
The Test-Scene also contains one AI as Example. It can Auto-Jump if it comes to a ledge (like in Zelda- OOT), 
but can also be set to manual jump. It uses a Rigidbody so is fully affected by the physics system.

## EntitySystem


<img src="http://i.imgur.com/HnMtT8Z.png" width="405" height="275">  
  | <img src="http://i.imgur.com/ve3P5fz.png" width="217" height="280" >
You'll find the scripts under TopDownRPGController\Scripts\LevelObjects\EntitySystem and also some Example entities 
in the TestScene.

