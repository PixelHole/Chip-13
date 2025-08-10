# Hello world!
let's write and run chip-13's version of the hello world! program (do not worry
we will print hello world later!).
for this article we will just perform some basic mathematics and leave you to discover
the rest!

## Concept
let's write a program that given a number, it will multiply it by 2 and add 2 to it

## Code
in chip-13 and low level programming in general, instead of variables, we work with
registers, registers are physical storage spaces that hold small amounts of data
for us to use. in Chip-13, all registers are **32 bits** or **4 byts**.

in this concept we will primarily be working with register **A**. so let's first 
put the number we want to work with in this register!

there are many ways to do this, but the most straight forward is the `Move` instruction!
this instruction _moves_ a specified value into the stated register.

```asm
mov a, 5
```

you can also do the same thing with the `subtract` and `add` instruction aswell!
try figuring that one out yourself :p

alright so now that we have our number in the **A** register, let's multiply it by 2.
Chip-13 multiplication but the easiest way to multiply by two is to left shift 
a value by one.

```asm
sll a, a, 1
```

here we shift the value stored inside the **A** register by one bit to the left,
functionally multiplying it by to and then storing it into the **A** register again

now finally let's add 2 to our value

``` asm
add a, a, 2
```

here again we have added 2 to the value stored in the register **A** and then
stored the value in the **A** register

here's the final code:

``` asm
mov a, 5
sll a, a, 1
add a, a, 2
```

Before you hit Run, if you are unfamiliar with Chip-13, please read [this article](../Tutorials/Running%20scripts.md)
to get familiar with how running programs works in Chip-13. you might get a little
confused otherwise :p