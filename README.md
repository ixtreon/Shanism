# Shanism

Shanism is a highly extendable top-down 2D RPG game platform. It is written entirely in C# and all of the scripting is done in C#, too, (ab)using the powers of the [Roslyn](https://github.com/dotnet/Roslyn) compiler. 

The game exposes a complete API which can be leveraged to create custom maps (scenarios) with no knowledge of the game engine internals, as long as one's aware of common RPG mechanics (buffs, auras, abilities). It started as a fun side-project to teach some friends "serious" programming (thus the C# instead of a more widely-used scripting language.. sorry LUA). 


## Building

### Windows

The game is most easily compiled using a recent version of Visual Studio but should be straightforward to compile from the command line (using MSBuild) as well. Most dependencies are in the form of NuGet packages, with the notable exception of shaders which use the [MonoGame Pipeline Tool]() for compilation. 

1. Grab the source code from Github
2. Download the MonoGame framework (along with the latest DX runtime, as indicated on the "Development Builds" section of the [download page](http://www.monogame.net/downloads/))
3. Open the `Shanism.sln` file in Visual Studio, select build config (debug or release), build the solution!
4. If compiling in Release mode, binaries can be found in the `bin/` folder. 

If you have problems following these instructions, please file an issue with details of the errors you are getting and I will try to get 'em fixed ASAP.

### Other Platforms

While, theoretically, MonoGame can run on other platforms (notably Desktop Linux & Mac OS) there are some issues supporting them out of the box. The game currently uses the Windows DX MonoGame library, as opposed to the Desktop GL one, which supports 

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
