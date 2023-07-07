# Caffeine For Citrix Workspace

Originally developed by [Andrew Morgan way back in 2012](https://andrewmorgan.ie/2012/07/caffeine-for-citrix-receiver/),
since been [resurrected for the COVID-19 pandemic](https://github.com/andyjmorgan/CaffeineForCitrixWorkspace), when everyone
started working from home, and now extend to serve the continued WFH situation.

## Installation

- Install .NET 6
- Create the following two, DWORD values in the following key:  
  `HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Citrix\ICA Client\CCM`  
  `AllowLiveMonitoring`: DWORD: 1  
  `AllowSimulationAPI`: DWORD: 1
- Download the latest release: https://github.com/eXpl0it3r/CaffeineForCitrixWorkspace/releases/latest
- Run it!

Then go back to doing nothing safe in the knowledge that you shouldnt get a screensaver prompt again!

## Configurations

- If you want to tune the keystroke it sends, see the `Caffeine.dll.config` file
- If you want to tune the frequency, see the `Caffeine.dll.config` file.

## How It All Works

The tool loads the Citrix CCM DLL and simulates that the F15 key is being pressed at a fixed interval.
This in turn prevents the Citrix session from being locked, as an activity is still being detected.

In order for this to work, the two mentioned Registry keys need to be defined, allowing API simulations.

## Troubleshooting

- If you're running Citrix Workspace as 64-bits application, the registry key will be `HKEY_LOCAL_MACHINE\SOFTWARE\Citrix\ICA Client\CCM`
- Some versions of Citrix Workspace have caused issues when loading the DLL
  - You can notice this, when the About form doesn't open
  - Launch a **PowerShell (x86)** instance
  - Run the provided [PowerShell script](Caffeine.ps1)

## Acknowledgement

This is a fork of the [original work by Andrew Morgan](https://github.com/andyjmorgan/CaffeineForCitrixWorkspace).
Thank you for creating the original version!
