# Labels
labels are a concept in assembly languages that is very close in concept to variables. 
with the only difference being that they are used to store constants.

a constant is a variable that cannot change, for example, pi is a constant. it is 
always equal to `3.141592653589793238462643383279502884197169` if rounded to 42
digits.

labels in assembly are used to store two things:

## 1. Line indices
one of the most primary uses of labels is to store line indices. these indices
are later used in `jump` instructions. labels are useful here because with the 
nature of assembly, lines of code are always shifting around and moving from 
one line index to another.

for example, you might be working on a script and realize that you need to insert
an instruction in the middle of the script. with that, all the jump instructions
after that inserted instruction will have their jump value messed up!

in this scenario, it is best associate the index of a line to a label, and let the
assembler replace the value of that label with its associated line index when the
program is being compiled and translated! this way you don't have to worry about 
messing up your jump instructions!

if not given a value, labels will automatically store the index of the line after
them as their value

```asm
jmp @label2

@label1
out 0x3132
end

@label1
out 0x3334
```

## 2. Storing constant values
the second most obvious use, is storing useful constants that are used alot in
the program's runtime! this can be the price of an item, the area of a house,
your credit card information, basically anything you want!

you can assign labels constant values like so

```asm
@ok = 0x6F6B21

out @ok
```