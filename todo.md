# TODO: 


- display received message properties (if not empty)
  - for dead letter queue message there are additional properties
- add posibility to set message properties when sending message (use expander ?)
- add when adding new sender receiver so that it copies data from already selected
- add SetFont popup for all textBoxes and save this in config
- create applicaiton icon
- make log window using expander ?

## Bugs:

## Needed fixes refactoring
 - Refactor how DataContext for Windowed and non-windowed configs are set. Currently they are two different data contexts but with the same properties.
    This makes changing some code in Code-behind very dirty because we need to use reflection to access properties with the same name but for differnt types.
 


