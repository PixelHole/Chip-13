# Complicated Math
this time we're going to tackle something more complicated! let's calculate and store
this familiar formula!

`b ^ 2 + 4 * a * c`

in this tutorial we will make extensive use of the `mlt` instruction.

## Code
first let's move all the values we need into the registers

```asm
mov a, 1
mov b, 2
mov c, 1
```
now let's calculate `b^2`. which is easy with the `pow` or the `mlt` command.
here we will use the `mlt` command

```asm
mlt b, b
mov b, lw
```
the `mlt` command will multiply the given values and place the results in the 
**Low** and **High** registers. the result of the multiplication of two 32-bit
numbers will at most be 64 bits so Chip-13 breaks down this result into two 32
values and places the 32 least significant bits in the **Low** register and the
most significant bits in the **High** register, because our numbers are so small
they wont pass the integer limit so our result is stored in the **Low** register.
so in the second instruction we move the result from the **low** back in the **b** register

with the same logic let's calculate `4 * a * c`

```asm
mlt a, 4
mov a, lw
mlt a, c
mov a
```

now finally let's add out results and see what it is!

```asm
add a, a, b
```


Lastly, let's store the value we have calculated in the memory. this can _very_
easily be done with the `Memory write` instruction as follows

```asm
mmw a, 0
```
here we have written the contents of the register **A** to the memory at address
**zero**

for a fun bonus, you can read this value from the memory again by calling the
`memory read` command

```asm
mmr d, 0
```
let's see all the code at once
```asm
#loading the values
mov a, 1
mov b, 2
mov c, 1

# b = b ^ 2
mlt b, b
mov b, lw

# a = 4 * a * c
mlt a, 4
mov a, lw
mlt a, c
mov a

# a = a + b
add a, a, b

# write the result at zero
mmw a, 0

# read the result from zero and place it in d
mmr d, 0 
```

run the code and see the results!