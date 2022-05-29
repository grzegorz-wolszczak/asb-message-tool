# TODO:


  - add hints over message properties button to display what are the current sb messages properties without the need to
    push the button and open the window
- add missing TimeToLive field to message properties
- add missing ScheduledEnqueueTimeUtc field to message properties

- add runtime detection that topic on which receiver is listening, was deleted from service bus

- add new config type - ServiceBus config - ability to fetch all information about topics/queue/subscriptions and display them
  - add feature - connect ServiceBusConfig with other config types so they use servicebusconfig's 'connectionString' and user would not have to duplicate this connectionstring in every sender/receiver config
  - add feature - sender/receiver configs will only be able to choose topics/queues/subscriptions (comboboxes?) only if they exist in serviceBusConfig
  - add feature - ability to acutally manage topics/queues/subscriptions (add/modify/delete) from the servicebusconfig tab

## Bugs:



+ add ability to stop sending messages (if there is no connectivity to service bus sender will 'hang' on sending phase)



Done
+ add: dialog box when with confirmation whether user wants to delete configuration
+ fix: detached windows should be bigger by default (taller)
+ fix: all buttons 'Detach' pane have the same style
+ add: ability to remember whether detached windows had collapsed config expander
+ fix: validate configuration button widths so they are the same
+ add: application icon
+ add ability to stop peeking messages