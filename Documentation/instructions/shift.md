**Note** : the difference between _arithmatic_ and _logical_ shift is 
the value of the inserted bits at the right or left of the number
in arithmatic shifts, one is inserted in the place of the bits and 
in logical shifts, zero is inserted

for example
arithmatic shift :

`0000-0110` ----> `0110-1111`

logical shift :

`0000-0110` ----> `0110-0000`

both values were shifted to the **left** by 4 bits. in the arithmatic shift, 
**one** was inserted in the place of new bits and in the logical shift, 
**zero** was inserted




# Shift left logic
- Template : `sll d , a , b`

- Argument types :

  d : Register

  a : Register or Value

  b : Register or Value

logically shifts a by b bits to the left and stores the result in d

Examples:
`sll a , 1 , 2`
`sll a , b , c`



# Shift Right logic
- Template : `srl d , a , b`

- Argument types :

  d : Register

  a : Register or Value

  b : Register or Value

logically shifts a by b bits to the right and stores the result in d

Examples:
`srl a , 1 , 2`
`srl a , b , c`



# Shift left Arithmatic
- Template : `sla d , a , b`

- Argument types :

  d : Register

  a : Register or Value

  b : Register or Value

arithmatically shifts a by b bits to the left and stores the result in d

Examples:
`sla a , 1 , 2`
`sla a , b , c`



# Shift Right Arithmatic
- Template : `sra d , a , b`

- Argument types :

    d : Register

    a : Register or Value

    b : Register or Value

arithmatically shifts a by b bits to the right and stores the result in d

Examples:
`sra a , 1 , 2`
`sra a , b , c`










