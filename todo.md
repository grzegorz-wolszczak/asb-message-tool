# TODO:


- add SetFont popup for all textBoxes and save this in config
- set application icon
  - add hints over message properties button to display what are the current sb messages properties without the need to
    push the button and open the window
- add missing TimeToLive field to message properties
- add missing ScheduledEnqueueTimeUtc field to message properties

- add dialog box when with confirmation whether user wants to delete configuration
- add runtime detection that topic on which receiver is listening, was deleted from service bus
- add new config type - PeekerConfig - ability to peek message in queue without receiving/consuming them from service bus
- add capability to ReceiverConfig to be able to receive distinct number of messages (e.g. 1) and immediately stop receiving
- add new config type - ServiceBus config - ability to fetch all information about topics/queue/subscriptions and display them
  - add feature - connect ServiceBusConfig with other config types so they use servicebusconfig's 'connectionString' and user would not have to duplicate this connectionstring in every sender/receiver config
  - add feature - sender/receiver configs will only be able to choose topics/queues/subscriptions (comboboxes?) only if they exist in serviceBusConfig
  - add feature - ability to acutally manage topics/queues/subscriptions (add/modify/delete) from the servicebusconfig tab

## Bugs:

