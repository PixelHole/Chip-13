# Running scripts

this is a very short article on running scripts in Chip-13. if you are familiar
with how programs like Chip-13 work, you can skip the next part

## The basics
Programs like Chip-13 usually have two modes of operation :
### 1. Single-step Mode
the program will execute the instructions one at a time and wait for the user's
input to move to the next instruction and execute it.
### 2. Continuous Mode
the program will execute the instructions back to back, without waiting for user
input (unless console input is requested). 

you can think of **Single-step** as the Debugging mode and **Continuous** as
the final form of the program. basically how the program will be seen by the end
user


## How Chip-13 works
Pressing the **Run** button (located on the top right of the window) or alternatively
it's keyboard shortcut (_F5 or F10_) will compile
your script and load it into Chip-13 for execution. this will put the program in
**Run** mode,

![bandicam 2025-08-09 20-21-58-601.jpg](../../Images/toolbar%20right%20edit.jpg)

being in **Run** mode will disable the code edit area and will move you to the
console tab. more importantly, it will reveal some new buttons atop the screen

![bandicam 2025-08-09 20-22-07-923.jpg](../../Images/toolbar%20right%20run.jpg)

these buttons in order from left to right are as follows :
### 1. Run to end
- Shortcut : F9

pressing this button will execute the program in **Continuous** mode, meaning it
will execute the program to the end as fast as possible. pressing the **Next step**
button or hitting a breakpoint will take the program out of this mode and into 
**single step** mode
### 2. Next step
- Shortcut : F8

executes the current instruction (Highlighted in pink in the console tab) and 
proceeds to the next instruction. 
### 3. Restart
- Shortcut : Ctrl + R

stops the current program and runs it from the start.
### 4. Halt
- Shortcut : Ctrl + S

Stops the current program

When Chip-13 runs the program, it will start execution in the **single-step** mode.
you can press the **Run to end** button to take the program out of this mode and
run the program in **Continuous** mode.

## Breakpoints
breakpoints are useful tools in programming that will allow the user to specify
an instruction for the program and tell it to "whenever you reach this instruction,
go back into single step mode and let me step through the instructions".

it is a way of having control over the program's execution and very useful for debugging

you can create a breakpoint by placing a `>` at the start of an instruction
```asm
> add a, a, 2
```
after hitting a breakpoint, the program will switch to **single-step** mode
and wait for your input to proceed with the execution. here you can either press
the **Next Step** button or the **Run to end** button to re-enter **Continuous**
mode