
# KeyOverlay
 A simple key overlay for osu! streaming
 
 If you're interested in per key coloring and custom width for your keys please check out [Friedchiken-42's version](https://github.com/Friedchicken-42/KeyOverlay) better suited for mania.
 
To change the keys used please use config.txt
# [Download Link](https://github.com/Blondazz/KeyOverlay/releases/latest)
IF YOU ARE HAVING PROBLEMS WITH THE PROGRAM WHEN OSU! IS ON FULLSCREEN, TRY USING GAME CAPTURE INSTEAD OF WINDOW CAPTURE IN OBS!

# MacOS

## Setup

1. Unzip `KeyOverlay-macos-latest.zip` into a folder.
2. Using the terminal cd inside the unzipped folder.
3. run `KeyOverlay` in terminal (`./KeyOverlay`).
4. Allow the terminal `Input Monitoring` permission.

# Linux

## Setup

1. Unzip `KeyOverlay-ubuntu-latest.zip` into a folder.
2. cd into the unzipped folder
3. make `KeyOverlay` executable using `chmod`
4. run `KeyOverlay` in terminal (`KeyOverlay`)

## Note

If you get an error similar to `System.DllNotFoundException: Unable to load shared library 'csfml-system'`, install the packages that you are missing using your distribution's package manager. Any issues created related to this issue will be automatically closed.

# config.txt properties
keyAmount - The amount of keys in the program (see the readme.txt for recommended widths for certain keyAmounts).

key1, key2 ... - Keys the program should use (UPPERCASE), for numbers and symbols [please refer to this table](https://www.sfml-dev.org/documentation/2.5.1/classsf_1_1Keyboard.php#acb4cacd7cc5802dec45724cf3314a142), for mouse buttons add m before the [mouse button options](https://www.sfml-dev.org/documentation/2.5.1/classsf_1_1Mouse.php#a4fb128be433f9aafe66bc0c605daaa90) ex. mLeft mRight. If you want more keys just add more fields.

displayKey1, displayKey2 - If the name of the key you are using is too large, or you would like to use a different symbol, you can use this property to override the original key name that is going to be displayed.

keyCounter - yes/no - Adds a keycounter beneath each key that counts total clicks in a session.

windowHeight, windowWidth - Used to change the resolution of the program.

keySize - Changes the size of the key (excluding border).

barSpeed - Adjusts the speed at which the bars are travelling upwards.

margin - Adjusts the margin of the keys from the sides.

outlineThickness - Changes the thickness of a square border

fading - yes/no - Adds/removes the fading effect on top 

backgroundColor, keyColor, PressFontColor, borderColor, barColor, fontColor - Changes the color of background (might be tricky, but possible to chroma key out in obs), key when not pressed, key when pressed, key border, bars and clicked key color, the font color using RGBA values.

backgroundImage - Lets you set a background. Put the image into Resources directory and then put the filename into this property ex. "bg.png" (without the quote symbols). Makes sure the background is the same resolution as your window and if you want transparency on your background you have to put the transparency on the image itself.

maxFPS - Sets the target FPS for the program to run

# GIF Preview

![](https://puu.sh/I6Kg1/4ff86be176.gif)

based on a similar app by an unknown author (if you are the author dm me so I can credit you)


