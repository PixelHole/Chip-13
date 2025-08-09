this is an in-depth guide on chip-13's Console and its IO system.

## Console
the console is very similar to any regular operating system command terminal.
the user can enter text into the console via keyboard and then press enter to 
submit that input. the input is then passed to chip-13's Input Buffer, 
which the programmer can use to process the user's inputs. 
the user is only allowed to type in the console when input is requested by 
the script and the input buffer is empty, otherwise the user cannot type 
or delete text.

## Input Buffer
the input buffer is a Queue that will pre-process and store the user's input
in order of their submission for usage by the script. due to the pre-processing,
one input from the user may be split into many entries in the input buffer.
each subsequent input instruction will then fetch the pre-buffered input
rather than wait for a new input from the console. 
so when the input instruction is called and the input buffer isn't empty, 
the script will not wait for the user to submit input, but if it is, it will.

Chip-13 will at all times keep the number of values buffered in 
the input buffer in the **Input Buffer Count** register.


### Pre process
if the input string is a number, no processing will be performed and 
the number will be directly stored in the queue.
if input is a piece of text (_i.e a string_), it is first broken 
into substrings with the maximum length of **4**

`Hello!`  ---->   `Hell`,`o!`

`this is a text`   ---->    `this`,` is `,`a te`,`ext`

(**notice** : the spaces are a part of the string)

each substring will then be converted into its **ascii numeric** representation.
each letter is converted to its ascii code which is one byte in length, 
this code is then placed in its correct place inside a **4 byte** number (**_an integer_**)
after which this integer is pushed in the input buffer

here are some examples of this pre-processing step :

`abcd`  ---->  `0x61626364`

`abc`    ---->  `0x00616263`  -->  `0x616263`

`ab`      ---->  `0x00006162`  -->  `0x6162`

as you can see, the number is filled from right to left. 
this is to ease the process of comparing these numbers in the script.
the general formula for this preprocess is as follows:

1. for each four characters (or less depending on how many characters remain in the string)
2. create an empty integer (4 bytes of zero)
3. repeat for all characters from left to right

   i. shift the integer by 1 byte to left

   ii. add the character's ascii code to the intege.

this will look like this in execution :

`abc`

`0x00000000`  -->  `0x00000061`  -->  `0x00006162`  -->  `0x00616263`

after the pre-processing, the resulting integer is enqueued in the input buffer.
the first element will be popped (_removed_) and returned with the input instruction
that called it, each subsequent input instruction call then will fetch values from
the buffer and won't wait for user submission.

`aaaabbbbcccc`

in this example the first input instruction (the one that waited for user input)
will return with `aaaa`. the input instruction after that will return 
with `bbbb` and the one after that with `cccc`




