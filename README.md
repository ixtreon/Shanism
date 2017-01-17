# Shanism

Shanism is a highly extendable top-down 2D RPG game platform. It is written entirely in C# and all of the scripting is done in C#, too, (ab)using the powers of the [Roslyn](https://github.com/dotnet/Roslyn) compiler. 

The game exposes a complete API which can be leveraged to create custom maps (scenarios) with no knowledge of the game engine internals, as long as one's aware of common RPG mechanics (buffs, auras, abilities). It started as a fun side-project to teach some friends "serious" programming (thus the C# instead of a more widely-used scripting language.. sorry LUA). 


## Building

### Windows

You'll need a recent version of Visual Studio, although using a recent version of MSBuild should also do the trick (untested).

1. Grab the source code from Github
2. Download the MonoGame framework, along with the latest DX runtime, as indicated on the "Development Builds" section of the [download page](http://www.monogame.net/downloads/).
3. Open the `Shanism.sln` file in Visual Studio, select build config (any cpu, debug or release), hit build

When compiling in Release mode, binaries can be found in the `bin/` folder. 

### Other Platforms

While MonoGame supports other platforms (e.g. Linux & OSX) there are issues supporting them out of the box. MonoGame does not support WinForms-embedded drawing under GL as required by the Editor. Changing the MonoGame runtime (from DX to GL) should, in theory, allow compilation under different platforms, but doing that means the editor will become unusable. 

Furthermore, the MonoGame libraries share the same name, no matter the platform. Thus if the editor uses DX (i.e. available only for Windows) with the rest of the game being compiled for OpenGL, one of them will fail during runtime since it won't find the backend it requires. This can be avoided if one builds only the game, or if the game and editor are placed in different directories. 

## Screenshots

WIP


## Current Status

* Most™ in-game features ready - units, heroes, abilities, buffs, AI behaviours. 
* Earlier builds had a crappy no-diff networking. A rewrite with proper networking (a la Quake / [Doom 3](http://fabiensanglard.net/doom3_documentation/The-DOOM-III-Network-Architecture.pdf)) support is under way, currently being tested. 
* A WinForms editor with crude map editing support + texture & animation editor. 

## A WORD OF CAUTION

Since maps are essentially .NET libraries, DO NOT EVER CONNECT TO SERVERS UNLESS YOU PERSONALLY KNOW THE HOST. There's an experimental sandbox during compilation which only allows whitelisted assemblies and methods, but it's TOTALLY UNTESTED IN THE WILD. Furthermore, the (current) network protocol dictates that a scenario are sent PRECOMPILED which means that sandbox is (currently) useless. 


### DO NOT BE A COMPLETE FOOL BY DISREGARDING THE MESSAGE ABOVE. 


### Obligatory disclaimer:

In no event can the developers of Shanism can be blamed for any damages, perceived or real, stemming from the use of the software. Go blame someone else: natural selection would be my first bet.

## Further Reading

Please refer to the [Github wiki](https://github.com/ixtreon/ShanoRPG/wiki). 
