ClientContent
===============

Contains the default content used with all ShanoRPG games. Some of the content is transformed using the MonoGame Content Builder tool (MGCB) or its GUI counterpart, the MonoGame Pipeline tool. 

To build the content using MGCB simply pass the `ClientContent.mgcb` file as an argument. The script places the output files in the same directory as the input. 

To build the content using the MonoGame Pipeline tool simply open the `ClientContent.mgcb` file with it. 

This allows us to use MSBuild to compile the content via a pre-build event, and then automatically copy that content to secondary projects that reference the content project. The last step is done via a hack in the secondary project where a single file called `touch` is marked as no-compile but always-copy. This makes the build process copy all necessary (and updated) files from all references (?!). At least _seems_ to work in VS2015 Update 2. 



The only project that currently uses this content project is the Launcher. 