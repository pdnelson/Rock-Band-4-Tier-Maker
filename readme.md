## Introduction

Hello, and thank you for downloading the Rock Band 4 Tier Maker! This is a program that can be used to create Rock Band 4 tiers and export them to an image.

This project was done by request. It was written in Visual Basic using Microsoft Visual Studio 2013,
and development progressed during my occasional free time across a few weeks.

It is essential that, when you unzip the contents of the program, you keep everything in the "RB4 Tier Maker"
folder intact. The reason for this is because these files are accessed by the program to store settings, as
well as read from the font files. So, in simple terms, make sure the Sys folder is in the same directory
as the .exe!

## Antivirus warning:

You may have already guessed, but I am not a certified Microsoft publisher. For this reason, this program
is linked to no publishing certificate. This is a red flag to most antivirus software, and it will likely tell
you that the program could be, or is harmful. To get around this, you will have to add this program to your
antivirus software as an exception. All antivirus software is different, so I can't tell you how to do that
here, but a quick Google search should be able to be of assistance.

## Basic Usage/Features
### Create Tab
- Export to Image File
  - Save your completed tier to your favorite image format. (Mine is .png, so that's what it defaults to)
- Copy to Clipboard
  - Copy the image directly to your clipboard if you don't want it taking up space in your pictures folder.
-Clear All
  - Clears all the fields and sets the cursor back to the beginning, ready for a new tier!

### View Tab
- Show Preview
  - Shows the preview so that you know exactly what your tier is going to look like.
- Hide Preview
  - Hides the preview, and makes the window much smaller and not take up your entire screen.          
  - IMPORTANT: If your screen resolution's width is less than 1440px, the preview will not fit and it will be hidden by default, with the "Show Preview" option disabled. The reason for this is because Windows Forms cannot create a window size larger than your resolution, so there would be visual errors in the preview.                                                

### Help Tab
- About
  - Version information and my contact details!

## Version History

### v1.0: 3 April 2019
- Initial release.

### v1.1: 7 April 2019:
- Fixed a bug where ampersands (&) wouldn't show up correctly in the preview.

### v1.2: 9 April 2019:
- Made the "blank" dots darker, so it's easier to tell they're blank on a smaller screen
- Font size is now dynamic, updates automatically, and doesn't run off the edge of the screen
- Text location is now dynamic, updates automatically, and closes awkward gaps when the text size decreases
- Text boxes now have a character limit of 175

### v1.3: 3 August 2021:
- Warning appears if the "Sys" directory cannot be found
- Warning appears if the fonts cannot be found (but allows the user to continue using the program)
- Warning appears if the view settings cannot be found (but allows the user to continue using the program, just doesn't save)