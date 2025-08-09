# Add
- Template : `add d , a , b`

- Argument types :

  d : Register

  a : Register or Value

  b : Register or Value

adds the given values and stores them in the destination register

Examples:
`add a, b[2], 5`
`add b, c, 5`



# Subtract
- Template : `sub d , a , b`

- Argument types :

  d : Register

  a : Register or Value

  b : Register or Value

subtracts the given values and stores them in the destination register

Examples:
`sub a, b[2], 5`
`sub b, c, 5`



# Multiply
- Template : `mlt a , b`

- Argument types :

  a : Register or Value

  b : Register or Value

multiplies the given values, doing this might cause an integer overflow so the result is split into to integers, the lower 32 bits are stored in the LOW register and the higher 32 bits are stored in the HIGH register

Examples:
`mlt a, b`
`mlt a, 5`
`mlt 5, b`



# Divide
- Template : `div d , a , b`

- Argument types :

  d : Register

  a : Register or Value

  b : Register or Value

divides the given values and stores them in the destination register

Examples:
`div a, b[2], 5`
`div b, c, 5`



# Move
- Template : `mov d , a`

- Argument types :

  d : Register

  a : Register or Value

sets the contents of the register to the stated value

Examples:
`mov a, 6`
`mov b, a`



# Modulus or Remainder
- Template : `rem d , a , b` `mod d , a , b`

- Argument types :

  d : Register

  a : Register or Value

  b : Register or Value

gets the remainder of the variable a from b and stores it in register d

Examples:
`mod a , 10 , 2`
`rem b , 5 , 3`