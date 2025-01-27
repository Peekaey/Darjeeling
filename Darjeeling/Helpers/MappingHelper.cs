using Darjeeling.Interfaces;
using Darjeeling.Models;
using Darjeeling.Models.DTOs;
using Darjeeling.Models.Entities;
using Darjeeling.Models.LodestoneEntities;
using NetCord;

namespace Darjeeling.Helpers;

public class MappingHelper : IMappingHelper
{
    public async Task<List<FCGuildMember>> MapLodestoneFCMembersToFCMembers(List<LodestoneFCMember> lodestoneFcMembers, List<GuildUser> guildUsers)
    {
        var fcMembers = new List<FCGuildMember>();
        if (lodestoneFcMembers != null && lodestoneFcMembers.Any())
        {
            var lodestoneMemberDict = lodestoneFcMembers.ToDictionary(
                m => m.FirstName.ToLower() + " " + m.LastName.ToLower(),
                m => m
            );

            foreach (var member in guildUsers)
            {
                var memberName = (member.Nickname ?? member.Username).ToLower();
                foreach (var lodestoneMember in lodestoneMemberDict.Values)
                {
                    if (memberName.Contains(lodestoneMember.FirstName.ToLower()) ||
                        memberName.Contains(lodestoneMember.LastName.ToLower()))
                    {
                        fcMembers.Add(new FCGuildMember
                        {
                            DiscordUserUId = member.Id.ToString(),
                            LodestoneId = lodestoneMember.CharacterId,
                            DateCreated = DateTime.UtcNow,
                            LodestoneNameHistories = new List<LodestoneNameHistory>
                            {
                                new LodestoneNameHistory
                                {
                                    FirstName = lodestoneMember.FirstName,
                                    LastName = lodestoneMember.LastName,
                                    DateAdded = DateTime.UtcNow
                                }
                            },
                            DiscordNameHistories = new List<DiscordNameHistory>
                            {
                                new DiscordNameHistory
                                {
                                    DiscordUsername = member.GlobalName,
                                    DiscordNickName = member.Nickname ?? member.Username,
                                    DateAdded = DateTime.UtcNow
                                }
                            }
                        });
                        break;
                    }
                }
            }
        }

        return fcMembers;
    }
    
    
    public async Task<FCGuildRole> MapRegisterFCGuildServerDTOToFCGuildRoleAdmin(
        RegisterFCGuildServerDTO registerFcGuildServerDto, Role guildRole)
    {
        return new FCGuildRole
        {
            RoleId = registerFcGuildServerDto.AdminRoleId,
            RoleName = guildRole.Name,
            DiscordGuildUid = registerFcGuildServerDto.DiscordGuildUid,
            DateCreated = DateTime.UtcNow,
            RoleType = RoleType.FCAdmin,
        };
    }

    public async Task<FCGuildServer> MapRegisterFCGuildServerDTOToFCGuildServer(
        RegisterFCGuildServerDTO registerFcGuildServerDto, List<FCGuildMember> fcGuildMembers)
    {
        return new FCGuildServer
        {
            AdminRoleId = registerFcGuildServerDto.AdminRoleId,
            DiscordGuildUid = registerFcGuildServerDto.DiscordGuildUid,
            DateCreated = DateTime.UtcNow,
            FCMembers = fcGuildMembers,
            DiscordGuildName = registerFcGuildServerDto.DiscordGuildName,
            FreeCompanyName = registerFcGuildServerDto.FreeCompanyName,
            AdminChannelId = registerFcGuildServerDto.AdminChannelId
        };
    }
    
}