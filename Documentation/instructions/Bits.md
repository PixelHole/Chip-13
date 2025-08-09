# And
- Template : `and d , a , b`

- Argument types :

    **d** : Register

    **a** : Register or Value

    **b** : Register or Value

logically does the and operation on every bit of the given values and places the result in the given register

Examples:
`and a, 1100b, 1010b`
`and b, c, 5`



# Or
- Template : `or d , a , b`

- Argument types :

  **d** : Register

  **a** : Register or Value

  **b** : Register or Value

logically does the Or operation on every bit of the given values and places the result in the given register

Examples:
`or a, 1100b, 1010b`
`or b, c, 5`



# Nand

- Template : `nand d , a , b`

- Argument types :

  **d** : Register

  **a** : Register or Value

  **b** : Register or Value

logically does the Nand operation on every bit of the given values and places the result in the given register

Examples:
`nand a, 1100b, 1010b`
`nand b, c, 5`



# Xor
- Template : `xor d , a , b`

- Argument types :

  **d** : Register

  **a** : Register or Value

  **b** : Register or Value

logically does the exclusive Or operation on every bit of the given values and places the result in the given register

Examples:
`xor a, 1100b, 1010b`
`xor b, c, 5`



# Nor
- Template : `nor d , a , b`

- Argument types :

  **d** : Register

  **a** : Register or Value

  **b** : Register or Value

logically does the Nor operation on every bit of the given values and places the result in the given register

Examples:
`nor a, 1100b, 1010b`
`nor b, c, 5`


# Xnor
- Template : `xnor d , a , b`

- Argument types :

  **d** : Register

  **a** : Register or Value

  **b** : Register or Value

logically does the exclusive nor operation on every bit of the given values and places the result in the given register

Examples:
`xnor a, 1100b, 1010b`
`xnor b, c, 5`



# Not
- Template : `not d , a`

- Argument types :

  **d** : Register

  **a** : Register or Value

logically does the Not operation on every bit of the given value A and places the result in the given register D

Examples:
`not a , 1100b`
`not a , b`