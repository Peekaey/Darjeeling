# Darjeeling

Discord bot used to keep track of members 

This works on the assumption of FC members are required to use their in-game name as their nickname within the Free Company discord Server.

Therefore depending on how your specific FC server is setup. This bot may not be suitable



### Current Commands

| Command           | Parameters                        | Description                                                                                                                                  |
|-------------------|-----------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------|
| getcharacterid    | firstname, lastname, world        | Returns the Lodestone Character ID of a character                                                                                            |
| getfreecompany    | firstname, lastname, world        | Returns the FreeCompany of a user                                                                                                            |
| getfreecompanylist | fcid                              | Returns a CSV list of all members within the FC from the lodestone                                                                           |
| registerfcguild   | fcid, adminroleid, adminchannelid | Stores FC Lodestone,Guild, Admin Role, Admin Channel Id as well as storing data of guild member/lodestone character data if a match is found |
| getmatchedmemberlist |                                   | Returns a CSV list of all members within the FC that have been matched to a lodestone character                                              |
| updatememberdata|    | Update Discord/Name History of existing matched members as well storing any new members and removing unmatched members                       |
| greet |                                               | Ping equivalent command                                                                                                                      

