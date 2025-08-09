# Write
- Template : `mmw a , d`

- Argument types :

    a : Register or Value

    d : Register or Value

writes the value of A in the address D in the memory

Examples:
`mmw 2 , 1`
`mmw a , sp`
`mmw 3 , b`



# Read
- Template : `mmr a , d`

- Argument types :

    a : Register

    d : Register or Value

reads the value the memory in address D and writes it in the register A

Examples:
`mmr 2 , 1`
`mmr a , sp`
`mmr b , 3`








