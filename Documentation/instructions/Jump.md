# Jump
- Template : `jmp a`

- Argument types :
  a : Register or Value

jumps to the given line of code

Examples :
`jmp 3`
`jmp a`



# Jump relative
- Template : `jmpr a`

- Argument types :
  a : Register or Value

jumps A lines of code from the current line

Examples :
`jmpr 3`
`jmpr a`



# Jump if equal
- Template : `jie a , b , j`

- Argument types :
  a : Register or Value
  b : Register or Value
  j : Register or Value

jumps to the given line of code if the valuse of A and B are equal

Examples :
`jie a , 2 , 3`
`jie a , b , 3`
`jie a , b , ip[2]`



# Jump if equal relative
- Template : `jier a , b , j`

- Argument types :
  a : Register or Value
  b : Register or Value
  j : Register or Value

jumps J lines of code from the current line if the valuse of A and B are equal

Examples :
`jier a , 2 , 3`
`jier a , b , 3`



# Jump if less
- Template : `jil a , b , j`

- Argument types :
  a : Register or Value
  b : Register or Value
  j : Register or Value

jumps to the given line of code if A is less than B

Examples :
`jil a , 2 , 3`
`jil a , b , 3`
`jil a , b , ip[2]`



# Jump if less relative
- Template : `jier a , b , j`

- Argument types :
  a : Register or Value
  b : Register or Value
  j : Register or Value

jumps J lines of code from the current line if A is less than B

Examples :
`jilr a , 2 , 3`
`jilr a , b , 3`



# Jump if greater
- Template : `jil a , b , j`

- Argument types :
  a : Register or Value
  b : Register or Value
  j : Register or Value

jumps to the given line of code if A is greater than B

Examples :
`jig a , 2 , 3`
`jig a , b , 3`
`jig a , b , ip[2]`



# Jump if greater relative
- Template : `jier a , b , j`

- Argument types :
    a : Register or Value
    b : Register or Value
    j : Register or Value

jumps J lines of code from the current line if A is greater than B

Examples :
`jigr a , 2 , 3`
`jigr a , b , 3`






