# Darjeeling

Discord bot as a .NET Generic Host using Netcord & Docker to match FF14 Lodestone FC character names with respective discord guild members for Free Company Discord users.

This works on the assumption of FC members are required to use their in-game name as their nickname within the Free Company discord Server.

Therefore depending on how your specific FC server is setup. This bot may not be suitable.



### Current Commands

| Command           | Parameters                        | Description                                                                                                                                  |
|-------------------|-----------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------|
| getcharacterid    | firstname, lastname, world        | Returns the Lodestone Character ID of a character                                                                                            |
| getfreecompany    | firstname, lastname, world        | Returns the FreeCompany of a user                                                                                                            |
| getfreecompanylist | fcid                              | Returns a CSV list of all members within the FC from the lodestone                                                                           |
| registerfcguild   | fcid, adminroleid, adminchannelid | Stores FC Lodestone,Guild, Admin Role, Admin Channel Id as well as storing data of guild member/lodestone character data if a match is found |
| getmatchedmemberlist |                                   | Returns a CSV list of all members within the FC that have been matched to a lodestone character                                              |
| updatememberdata|    | Update Discord/Name History of existing matched members as well storing any new members and removing unmatched members                       |
| getguildmemberlist| | Returns a CSV list of all members within the guild irrespective of if they are matched to a lodestone character or not                       |
| getmatchedmembernamehistory | User | Returns the lodestone and discord name history of a matched member                                                                           |
| greet |                                               | Ping equivalent command                                                                                                                      

# Setup

## Building & Running through Docker
1. Ensure you are in the root of the folder
2. ``docker build -t image_name .`` to build the image
3. ``docker run -d --name container_name -e DISCORD_BOT_TOKEN=you_token -e POSTGRES_CONNECTION_STRING=postgres_connection_string`` to run the container

## Startup
By default, none of the commands will work till the bot is registered to a guild/fc via the registerfcguild command. After each command execution the bot
will check if the guild matches with one stored in the database and if any of the users role ID matches with the designated admin role ID as well as if the user is speaking
in the designated admin channel. If all conditions are met the command will be executed.

### Stored Data
The bot stores the following data in a postgres database:
FCGuildMember:
- DiscordUserUid
- LodestoneCharacterId

DiscordNameHistory:
- DiscordUsername
- DiscordNickName
- DiscordGuildNickName

LodestoneNameHistory:
- CharacterFirstName
- CharacterLastName

FCGuildServer:
- DiscordGuildId
- LodestoneFreeCompanyId
- DesignatedAdminRoleId
- DesignatedAdminChannelId

FCGuildRole:
- DiscordRoleId
- DiscordRoleName
- 


