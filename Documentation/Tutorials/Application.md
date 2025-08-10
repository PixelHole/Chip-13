# Navigation
Welcome to Chip-13's Navigation tutorial! Chip-13 is very minimalist in its UI
so this article will be very short.

![bandicam 2025-08-09 20-19-46-528.jpg](../../Images/Main%20window.jpg)

Opening the program will place you in this window. This is the main window of the
program and where you will be spending all of your time in. So better get to know
it well!

## Toolbar
![bandicam 2025-08-10 15-06-03-787.jpg](../../Images/toolbar.jpg)

on top of the window, you can see the window toolbar. The toolbar houses
many important buttons that you need to navigate and control Chip-13, these include:
saving and loading, switching tabs, running scripts and closing the program.

We can split the toolbar into two parts, the left side and the right side.
### Toolbar left
![toolbar left.jpg](../../Images/toolbar%20left.jpg)

the left side of the toolbar contains some control buttons and the **Tab select**
buttons 
#### Control buttons
the control buttons in order of left to right are :
##### 1. About ![logo.png](../../Chippie_Lite_Editor/Resources/Images/Icons/logo.png)
This button opens the about window where you can see Chip-13's info.
##### 2. New project ![File.png](../../Chippie_Lite_Editor/Resources/Images/Icons/File.png)
Pressing this button will create a new Chip-13 project
##### 3. Open project ![file2.png](../../Chippie_Lite_Editor/Resources/Images/Icons/file2.png)
Pressing this button will open a file prompt where you can locate and open
Chip-13 projects and load them into the app
##### 4. Save ![Floppy.png](../../Chippie_Lite_Editor/Resources/Images/Icons/Floppy.png)
This button will save your project. if the project is fresh and not yet saved
on the disk, this button will first present a file prompt where you can choose
where to save your project
##### 5. Save as ![SaveAs.png](../../Chippie_Lite_Editor/Resources/Images/Icons/SaveAs.png)
This button will save the current project as a new instance in the location of 
your choosing. if the project is not yet saved, any future saves will be written
to the file created by this action
##### 6. Help ![questionMark.png](../../Chippie_Lite_Editor/Resources/Images/Icons/questionMark.png)
Pressing with button will open the documentation window. The current documentation
window does not yet support markdown, so currently it only has basic info about
Chip-13. Such as the instruction set and the look-up tables.

#### Tab select buttons
The tab select buttons help you switch to the three different tabs of Chip-13.
These tabs each perform a specific function. I'll explain them all in detail soon. but for now
let's finish the toolbar

This concludes all the elements on the left side of the toolbar, now lets switch
to the right side

### Toolbar right
![toolbar right edit.jpg](../../Images/toolbar%20right%20edit.jpg)

the right side of the toolbar might seem simple at first, because it really is!
let's take it from the right this time:
#### 1. Close ![strike_invert.png](../../Chippie_Lite_Editor/Resources/Images/Icons/strike_invert.png)
this button will close program
#### 2. Fullscreen ![square.png](../../Chippie_Lite_Editor/Resources/Images/Icons/square.png)
this button will set the window to fullscreen and if it's already in fullscreen,
it will set it to its default size
#### 3. Minimize ![Underline.png](../../Chippie_Lite_Editor/Resources/Images/Icons/Underline.png)
this button will minimise the program
#### 4. Run ![play.png](../../Chippie_Lite_Editor/Resources/Images/Icons/play.png)
pressing this button will compile and run the current script. running scripts is 
explained in depth in [this article](Running%20scripts.md)

now that we know what the toolbar does, let's move on to the program tabs

## Tabs
### 1. Code Tab ![brackets.png](../../Chippie_Lite_Editor/Resources/Images/Icons/brackets.png)
![Main window.jpg](../../Images/Main%20window.jpg)

This button will take you to the code tab. this is the default tab of the program
and where the program starts at. here you can write code and check for errors

this tab contains a **syntax highlighted** textbox. you can see all the syntax highlighted formats [here](Syntaxes.md)

### 2. Memory Tab ![FourSquares.png](../../Chippie_Lite_Editor/Resources/Images/Icons/FourSquares.png)
![memory edit tab.jpg](../../Images/memory%20edit%20tab.jpg)

the memory edit tab allows you to edit the initial values of both the memory and
the registers. these initial values will be loaded into the memory and the registers
when the script starts running.

![Memory grid.jpg](../../Images/Memory%20grid.jpg)

Each cell in this grid represents a cell in the memory. As you can see, each cell
is 4 bytes in length. you can edit these cell's initial values by typing in the
Top textbox. the memory address of each cell is also written in grey at the bottom
if each cell. this value cannot be edited

![Registers list.jpg](../../Images/Registers%20list.jpg)

to the right of the screen you can see the registers list. here you can define
their initial values. all registers that are in Chip-13 are presented here.

### 3. Console Tab ![Bug.png](../../Chippie_Lite_Editor/Resources/Images/Icons/Bug.png)
![console tab.jpg](../../Images/console%20tab.jpg)

the console tab is the place where you will be running your scripts! pressing
Run and compiling the program successfully will automatically take you here.

in this tab you can monitor :
#### 1. The Loaded instructions
ordered from top to bottom in the `instructions` list. this list also shows you
the current instruction that is about to be executed by highlighting it in pink
#### 2. The Console
presented in the middle of the screen. you can read all about the console [here](Console%20and%20IO.md)

#### 3. The Live state of the registers
ordered from top to bottom in the `Registers` list. you can click on these elements
to change the format they are displayed in. The supported formats currently are :
##### i. Decimal
![Register display mode 1.jpg](../../Images/Register%20display%20mode%201.jpg)
##### ii. Binary
![Register display mode 2.jpg](../../Images/Register%20display%20mode%202.jpg)
##### iii. Hex
![Register display mode 3.jpg](../../Images/Register%20display%20mode%203.jpg)

#### 4. The Live state of the memory
presented in grid form, at the bottom of the screen
![Memory view.jpg](../../Images/Memory%20view.jpg)

