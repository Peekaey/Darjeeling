using Darjeeling.Interfaces;
using Darjeeling.Models;
using Darjeeling.Models.DTOs;
using Darjeeling.Models.Entities;
using Darjeeling.Models.LodestoneEntities;
using NetCord;

namespace Darjeeling.Helpers;

public class MappingHelper : IMappingHelper
{
    public async Task<List<FCGuildMember>> MapLodestoneFCMembersToFCMembers(List<LodestoneFCMember> lodestoneFcMembers)
    {
        var fcMembers = new List<FCGuildMember>();

        if (lodestoneFcMembers != null && lodestoneFcMembers.Any())
        {

            foreach (var loddestoneFcMember in lodestoneFcMembers)
            {
                fcMembers.Add(new FCGuildMember
                {

                    LodestoneId = loddestoneFcMember.CharacterId,
                    DateCreated = DateTime.UtcNow,
                    NameHistories = (new List<NameHistory>
                    {
                        new NameHistory
                        {
                            
                            FirstName = loddestoneFcMember.FirstName,
                            LastName = loddestoneFcMember.LastName,
                            DateAdded = DateTime.UtcNow
                        }
                    })
                });
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