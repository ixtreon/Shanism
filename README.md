# Shanism

Shanism is a highly extendable top-down 2D RPG game platform. It is written entirely in C# and all of the scripting is done in C#, too, (ab)using the powers of the [Roslyn](https://github.com/dotnet/Roslyn) compiler. 

The game exposes a complete API which can be leveraged to create custom maps (scenarios) with no knowledge of the game engine internals, as long as one's aware of common RPG mechanics (buffs, auras, abilities). It started as a fun side-project to teach some friends "serious" programming (thus the C# instead of a more widely-used scripting language.. sorry LUA). 


## Building (Windows)

The game is most easily compiled using a recent version of Visual Studio but should be straightforward to compile from the command line (using MSBuild) as well. Most dependencies are in the form of NuGet packages, with the notable exception of shaders which use the [MonoGame Pipeline Tool]() for compilation. 

1. Grab the source code from Github
2. Download the MonoGame framework (along with the latest DX runtime)
3. Add the directory containing the `MGCB.exe` tool to your system path **OR** modify the pre-build event in `/Game/ClientContent/ClientContent.csproj` to point to the directory where the MGCB tool is installed.  (MGCB.exe can be found in the `%ProgramFiles%/MSBuild/MonoGame/v3.0/Tools` directory)
4. You should now be able to compile the game using Visual Studio or MSBuild from the command line.

If you have problems following these instructions, please file an issue with details of the errors you are getting and I will try to get 'em fixed ASAP.

## Screenshots

WIP


## Current Status

* Most™ in-game features ready - units, heroes, abilities, buffs, AI behaviours
* Earlier builds had a crappy no-diff networking. A rewrite with proper networking (a la Quake / [Doom 3](http://fabiensanglard.net/doom3_documentation/The-DOOM-III-Network-Architecture.pdf)) support is under way, currently being tested. 
* A WinForms editor with crude map editing support + texture & animation editor. 

## A WORD OF CAUTION

Since maps are essentially .NET libraries, DO NOT EVER CONNECT TO SERVERS UNLESS YOU PERSONALLY KNOW THE HOST. There's an experimental sandbox during compilation which only allows whitelisted assemblies and methods, but it's TOTALLY UNTESTED IN THE WILD. Furthermore, the (current) network protocol dictates that a scenario are sent PRECOMPILED which means that sandbox is (currently) useless. 


### DO NOT BE A COMPLETE FOOL BY DISREGARDING THE MESSAGE ABOVE. 


### Obligatory disclaimer:

In no event can the developers of Shanism can be blamed for any damages, perceived or real, stemming from the use of the software. Go blame someone else: natural selection would be my first bet.

## Further Reading

Please refer to the [Github wiki](https://github.com/ixtreon/ShanoRPG/wiki). 
