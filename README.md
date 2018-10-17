# Shanism

Shanism is a C#-scriptable top-down 2D RPG game platform. 

The idea is to (be able to) create custom maps/scenarios with no knowledge of the game engine internals, 
as long as one's aware of common RPG mechanics such as buffs, auras, mana or abilities. 

## Building

### Windows

Requires .NET Core 2.1

1. Grab the source code from Github.
2. Initialize all submodules.
3. Open the `Shanism.sln` file in Visual Studio, hit Build/Debug. Alternatively use `dotnet run` or `dotnet publish` from the command-line.

When compiling in Release mode, binaries can be found in the `/bin` folder. 

### Other Platforms

Let me know if you get the build running on other platforms, such as MacOS or Linux. 

## Screenshots

WIP


## Current Status

* Most™ in-game features ready - units, heroes, abilities, buffs, AI behaviours. 
* Earlier builds had a crappy no-diff networking. A rewrite with proper networking (a la Quake / [Doom 3](http://fabiensanglard.net/doom3_documentation/The-DOOM-III-Network-Architecture.pdf)) support is under way, currently being tested. 
* A limited-functionality map editor with crude map editing support + texture & animation editor. 

## A WORD OF CAUTION

Since maps are essentially .NET libraries, DO NOT EVER CONNECT TO SERVERS UNLESS YOU PERSONALLY KNOW THE HOST. 
There's an experimental sandbox during compilation which only allows methods from "safe" assemblies (lol), 
but it's TOTALLY UNTESTED IN THE WILD. 

### DO NOT BE A COMPLETE FOOL BY DISREGARDING THE MESSAGE ABOVE. 

## Further Reading

Please refer to the [Github wiki](https://github.com/ixtreon/ShanoRPG/wiki). 

I should™ get an autodoc-style documentation up sometime soon™.

## License

This project is licensed under the GNU GPLv3. 
