# Input and Output

in this article we will learn about Chip-13's input and output instructions and
how you can use them. alongside that, you will learn about Chip-13's console 
utility instructions and most importantly of all, **Jump** instructions!

## Concept
let's create a confirmation app. after printing "Are you sure?" the app will
receive input from the user. if the input is "yes" it will print "Ok!" and 
terminate and if the input is "no" the app will print a frowny face

## code

let's start by printing the message. the below piece of code will print "Are you sure?"

```asm
out 0x41726520 
out 0x796F7520
out 0x73757265
out 0x3F 
```
after this we want to receive input from the user, it is a good idea to first flush
the IO buffers to not receive any junk data remaining from the previous answers.
to read more about chip-13's IO buffer, read [this article](../Tutorials/Console%20and%20IO.md)

```asm
fio
inp a
```
this piece of code is pretty easy. first we flush the io buffers of its contents,
and then we request input from the user. here the console is unlocked and you 
can type into the console! press enter when you're done to submit your input

after this we need to compare the user's input and see if it matches either "yes"
or "no" as an added bonus, if it matches none of the two words, we will clear the
console and return to the first line (to essentially run the program again)

```asm
# if a = "yes" jump to line 11
jie a, 0x796573, 11
# if a = "no" jump to line 13
jie a, 0x6E6F , 13
clr
jmp 1
```
here you can see `jump if equal` instructions! they're pretty cool. if the value
stored inside **A** is equal to `0x796573` or `yes` (case-sensitive) it will jump
to line 11 in the program.
the `jump if equal` does the same thing but with the values of `0x6E6F` for `no`
and line 13

so if we get past both of these instructions, that means the input is neither of 
these words! in which case, we clear the console with `clr` and jump back to line one
(start of the program) with `jmp 1`

and now here at the end we can place our print instructions to print `Ok!` or `:(`

```asm
# print "ok!" and terminate
out 0x6F6B21 
end
# print :(
out 0x3A28
```

**Notice**: that `end` instruction is very important! because if it wasn't there,
if the input was `yes`, the program would print `Ok!` and **also** `:(`

here is all the code together

```asm
# print are you sure?
out 0x41726520 
out 0x796F7520
out 0x73757265
out 0x3F 

# flush io buffer and get input
fio
inp a

# if a = "yes" jump to line 8
jie a, 0x796573, 11
# if a = "no" jump to line 10
jie a, 0x6E6F , 13
clr
jmp 1

# print "ok!" and terminate
out 0x6F6B21 
end
# print :(
out 0x3A28
```

run the program and try it out!