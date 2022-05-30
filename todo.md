# TODO:


  - add hints over message properties button to display what are the current sb messages properties without the need to
    push the button and open the window

- add runtime detection that topic on which receiver is listening, was deleted from service bus

- add new config type - ServiceBus config - ability to fetch all information about topics/queue/subscriptions and display them
  - add feature - connect ServiceBusConfig with other config types so they use servicebusconfig's 'connectionString' and user would not have to duplicate this connectionstring in every sender/receiver config
  - add feature - sender/receiver configs will only be able to choose topics/queues/subscriptions (comboboxes?) only if they exist in serviceBusConfig
  - add feature - ability to acutally manage topics/queues/subscriptions (add/modify/delete) from the servicebusconfig tab

## Bugs:



