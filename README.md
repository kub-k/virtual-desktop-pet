# ฅᨐฅ - virtual desktop pet - ฅᨐฅ

![Version](https://img.shields.io/badge/version-1.0.0-pink) ![Platform](https://img.shields.io/badge/platform-Windows-lightpink)

**meet your new virtual workspace buddy!** a cute, animated pet that wanders, sits, and naps right on top of your screen as you work. plus, you can grab and drag it anywhere you want!
<p align="center">
<img width="120" height="90" alt="cat_22" src="https://github.com/user-attachments/assets/945b174a-8f4f-423e-b625-48709815411a" />  
</p>

## table of contents

- [features](#features)
- [requirements](#requirements)
- [installation](#installation)
- [how to use](#how-to-use)
  - [basic controls](#basic-controls)
  - [settings panel](#settings-panel)
- [behavior states](#behavior-states)
- [future enhancements](#future-enhancements)

  
## features

-  **animated pet** - smooth sprite animations with walking, sitting, and sleeping states
-  **multi-directional movement** - pet walks in all directions (up, down, left, right)
-  **customizable behavior** - adjust movement speed and behavior probabilities
-  **drag & drop** - move your pet around with a simple left-click drag
-  **smart boundaries** - pet automatically stays within screen boundaries
-  **context menu** - right-click for quick access to settings and exit

## requirements

- **OS:** Windows 10/11 recommended
- **.NET Framework:** 4.8 or higher

## installation

1. download the latest release from [releases page](https://github.com/kub-k/virtual-desktop-pet/releases)
2. extract the zip file to your desired location
3. run `VirtualDesktopPet.exe`
4. your pet will appear on your desktop!

## how to use

### basic controls
- **move pet:** click and drag with the left mouse button
- **open menu:** right-click on the pet
- **settings:** right-click > settings
- **exit:** right-click > exit

### settings panel

access settings by right-clicking on your pet:

| setting | range | description |
|---------|-------|-------------|
| movement speed | 1-10 | how fast your pet moves around |
| walk chance | 0-100 | probability of walking state (%) |
| sit chance | 0-100 | probability of sitting state (%) |
| sleep chance | 0-100 | probability of sleeping state (%) |

### tips:
- walk chances should total 100% 
- higher movement speed = faster walking pet

## behavior states

your pet cycles through three main states:

| state | animation | description |
|-------|-----------|-------------|
| **walking**  | 4-directional walk cycles | pet moves around the screen |
| **sitting**  | sit animation | pet stays in one place and sits |
| **sleeping**  | sleep animation | pet stays in one place and sleeps |

state transitions are randomized based on your probability settings.

## future enhancements

planned features for future releases:
- [ ] multi-monitor support
- [ ] additional pet types
- [ ] pet interactions (clicking, petting)

## **enjoy your virtual desktop companion!** 🐱✨
