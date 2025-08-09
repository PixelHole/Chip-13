this is a general guide on passing and understanding instruction arguments to instructions.

### Argument types
there are three types of arguments supported by the current version of chip-13 :
1. Register : the instruction expects either the name of a register or the index of a register
2. Value : the intruction expects a numeric value, this can be in binary, hex or decimal
3. Register or value : the instruction expects either a register or a value.


each argument type is used in a specific context

#### 1. Register:
this is for the instructions that either need to store a value inside a register or need the contents of a register for their operation.
examples include :
- Memory read instruction : the instruction expects a register as its first arguments. this is because it needs a place to save the contents of the memory
- input and read key instructions : the instruction requires a place to save the user input

Note : when using an index to refer to a register, the value of the index will automatically be modulated by the register count. this means if you pass for example the number 18 as the index and we only have 7 registers, the remainder of 18 by 7 will be used as the index, here being 4.


#### 2. Value :
this argument type is never used.


#### 3. Register or value
this is for instructions that primarily require a value to perform their operation. so you can either pass a register (for its contents to be read and used as value) or a direct value (to be used in the operation). 
examples include:
add (or any mathematical) instruction : the second and third arguments could either be a register or a value
jump instructions : the jump argument could either be a register or a value


### Methods of passing arguments

there are multiple ways to address or write instructions arguments, below is a list of all
the ways you can pass arguments to chip-13 instructions

#### 1. Registers:
- **name** : 
	`add a, b, c`

- **dollar sign with name**
	`add $a, $b, $c`

- **index**
	`mmr 1, sp`

- **dollar sign with index**
	`mmr $1, sp`

#### 2. Value
- **Binary**
	`out 1010b`

- **Hexadecimal**
	`out 0xA`

- **Decimal**
	`out 10`


### Argument Offsets
in all places where a register is expected, you can also pass in an offset value.
the offset value will be added to the register contents (only for this operation)
and used as the final value for the operation

`e.g : mmw sp[1] , 10`

in this memory write example, the instruction expects an address to write the 
value **(10)** to. for that we have passed the  stack pointer register **(sp)** and an
offset of **1**. when trying to write to the memory, chip-13 will grab the contents
of the stack pointer register, add the offset **(1)** to it, and then use the 
resulting value as the address for writing to memory.






