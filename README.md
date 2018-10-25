# Biological Evolution Simulator
Advanced Programming coursework


### OneNote Notebook 
We use a OneNote notebook to keep track of any notes concerning the project. That can be found [here](https://1drv.ms/f/s!AvFOONsV_sCtwxydsecMMiaq0Nvv).

### OneNote Notebook 
The project board keeps track of current progress and activities. 

Publish issues and label them to add them to the project. 

## Technology
C#

## Using Git
Once you are added as a developer of the project you can start editing the repo code. It is recommended that you use git desktop to manage your contributions. 

To contribute properly you need to:
- Make a new branch whenever you want to change something. 
- Commit on this branch often. It is better practice to commit frequently. 
- Once you have solved an issue, reference it in the commit or pull requests. Put a message in the commit and then reference an issue by using the keyword Fix followed by the repo address and the issue number. e.g. `Fixed roaming bug. Fix benjalon/evolution-sim#14`
- Referencing the issues in this way allows them to be automatically closed, saving time for later and showing the exact bit of code that fixed the problem. 

## Adding New Textures
This can't be done on University computers because you need admin rights to install the thing in step 1

1) Download and install [Monogame](http://teamcity.monogame.net/repository/download/MonoGame_PackagingWindows/latest.lastSuccessful/MonoGameSetup.exe?guest=1)
2) Navigate to "evolution-sim\EvolutionSim\Content" (You will know if you're in the right folder because you will see a GeonBit.UI folder, a Content.mgcb file and potentially other .png files if they've been added to git)
3) Copy your texture to this folder
4) Double click on the "Content.mgcb" application, if it doesn't run or have an orange icon then make sure you properly carried out step 1
5) Right click on "Content" in the directory tree in the screen's top left pane
6) Select "Add" -> "Existing Item..." And find your new texture in the window that opens
7) Right click on your new texture in the directory tree in the screen's top left pane from before
8) Select "Rebuild"
9) Navigate to "evolution-sim\EvolutionSim\bin\Debug\Content" and check your file is here, if so: success!

Q: Can't we simplify this?
A: This should actually work without any installation, but there's a bug in MonoGame where the content tool doesn't get distributed properly so you have to manually install it on each new machine.

Q: Can't we at least make it run automatically when you run the project?
A: Yes but if a computer doesn't have the content tool installed, then the project won't load on that machine. This means that until that bug I mentioned above is fixed, we would never be able to open our project on a University computer.
