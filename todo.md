# TODO: 


- add when adding new sender receiver so that it copies data from already selected
- add SetFont popup for all textBoxes and save this in config
- set application icon
- replace AvalonEdit with Scintilla.Net for better syntax highligting.
- add hints over message properties button to display what are the current sb messages properties without the need to push the button and open the window
- add posibility to sent receive mode on reciever (PeekLock vs ReceiveAndDelete) , for PeekLock set OnReceiveAction e.g. Complete, deadletter, abandon
  - add check that receiver with PeekLock with OnReceiveAction set to 'send to DeadLetterChannel' cannot listend on DeadLetter Channel already

## Bugs:

## Needed refactoring
 - Refactor how DataContext for Windowed and non-windowed configs are set. Currently they are two different data contexts but with the same properties.
    This makes changing some code in Code-behind very dirty because we need to use reflection to access properties with the same name but for different types.
 - Use async everywhre possible
 


