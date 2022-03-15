# TODO:


- add SetFont popup for all textBoxes and save this in config
- set application icon
- replace AvalonEdit with Scintilla.Net for better syntax highligting.
- add hints over message properties button to display what are the current sb messages properties without the need to
  push the button and open the window

- add validation rules to WPF controls where possible
- warn if settings file is corrupted and cannot be read
- add missing TimeToLive field to message properties
- add missing ScheduledEnqueueTimeUtc field to message properties
- rename all windows titles
- add sending to topic or queue (just change the label, it is already sending to topic or queue)
- add receiving from topic/subscription or queue
## Bugs:


## Needed refactoring

- Refactor how DataContext for Windowed and non-windowed configs are set. Currently they are two different data contexts
  but with the same properties. This makes changing some code in Code-behind very dirty because we need to use
  reflection to access properties with the same name but for different types.


## Suppressed warnings - reevaluate and fix code when in 'free' time

- CA1058 
- CA1012 
- CA1002

