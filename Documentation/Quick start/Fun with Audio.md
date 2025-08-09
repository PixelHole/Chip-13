# Fun with Audio

Chip-13 also supports audio wave generation and playback! in this article we
take a look at playing audio waves and make a little jingle!

## Concept
let's make a little jingle!

## Code
this article's code is very easy! we first tell Chip-13 to play a certain audio 
waveform at a specific frequency for a specific amount of time, and then wait for
that same amount of time to pass to play the next audio form!

```asm
snw 550, 250
wait 250
```

here we play a sine wave at the frequency of 550 hertz for 250 milliseconds. and then
we wait for 250 milliseconds to pass to execute the next instruction. 

heres a little pre-made jingle! run the script and _listen_ to the results!

```asm
trw 261, 250
wait 250
trw 261, 250
wait 250
trw 523, 500
wait 500
trw 392, 500
```