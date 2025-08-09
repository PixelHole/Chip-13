# Print
- Template : out a
- Argument types :

	a : Register or Value

converts the given number into ascii characters and prints them to the screen.
for more information about the Input buffer and how it works read [this article](../Tutorials/Console and IO.md)

Examples:
`out 0x31323334`   =>    `1234`



# input
- Template : `inp a`
		   `in   a`

- Argument types :

	a : Register

gets ascii or numeric input from the Input buffer and stores it in the given register
for more information about the Input buffer and how it works read [this article](../Tutorials/Console and IO.md)

Examples:
`inp a`
`in a`



# Keyboard input
- Template : `gky a`

- Argument types :

  a : Register

receives a single key press from the keyboard and stores the pressed key in the given register.
the key is in represented as an integer, each key on the keyboard has its own unique integer ascociated with it.
you can find the full list in [this article](../LookupTables/Keys.md)

Examples:
`gky a`


# Clear console
- Template : `clr`

- Argument types : None

Clears the console of all printed glyphs

Examples:
`clr`



# Set Background
- Template : `bkg a`

- Argument types :

  a : Register or Value

sets the index of the color that will be used as the background color in the subsequent print instructions

Examples:
`bkg 2`
`bkg a`



# Set Foreground
- Template : `frg a`

- Argument types :
  a : Register or Value

sets the index of the color that will be used as the foreground color (Text color) in the subsequent print instructions

Examples:
`frg 2`
`frg a`



# Set Console background
- Template : `frg a`

- Argument types :
  a : Register or Value

sets the background of the console itself (not the printed glyphs) to the color
with the given index

Examples:
`cbg 2`
`cbg a`



# Set Cursor
- Template : `crs a , b`

- Argument types :

	a : Register or Value

	b : Register or Value

sets the position of the cursor, the cursors position is where the print instruction writes at
Examples:
`crs 2 , 2`
`crs a , b[2]`



# Set Cursor top
- Template : `crt a`

- Argument types :
- 
  a : Register or Value

sets the y position of the cursor, the cursors position is where the print instruction writes at
Examples:
`crt 2`
`crt a`



# Set Cursor left
- Template : `crl a`

- Argument types :

  a : Register or Value

sets the x position of the cursor, the cursors position is where the print instruction writes at
Examples:
`crl 2`
`crl a`


