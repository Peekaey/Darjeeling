using Darjeeling.Interfaces;
using Darjeeling.Models.DTOs;
using Darjeeling.Models.Entities;
using Darjeeling.Models.LodestoneEntities;

namespace Darjeeling.Helpers;

public class MappingHelper : IMappingHelper
{
    
    public FCGuildServer MapFCGuildServerDTOToFCGuildServer(RegisterFCGuildServerDTO registerFcGuildServerDto)
    {
        return new FCGuildServer
        {
            AdminRoleId = registerFcGuildServerDto.AdminRoleId,
            DiscordGuildUid = registerFcGuildServerDto.DiscordGuildUid,
            FCMembers = registerFcGuildServerDto.FcMembers,
            DiscordGuildName = registerFcGuildServerDto.DiscordGuildName,
            FreeCompanyName = registerFcGuildServerDto.FreeCompanyName
        };
    }

    public async Task<List<FCMember>> MapLodestoneFCMembersToFCMembers(ICollection<LodestoneFCMember>? lodestoneFcMembers)
    {
        var fcMembers = new List<FCMember>();

        if (lodestoneFcMembers != null && lodestoneFcMembers.Any())
        {

            foreach (var loddestoneFcMember in lodestoneFcMembers)
            {
                fcMembers.Add(new FCMember
                {

                    LodestoneId = loddestoneFcMember.CharacterId,
                    DateCreated = DateTime.Now,
                    NameHistories = (new List<NameHistory>
                    {
                        new NameHistory
                        {
                            
                            FirstName = loddestoneFcMember.FirstName,
                            LastName = loddestoneFcMember.LastName,
                            DateAdded = DateTime.Now
                        }
                    })
                });
            }
        }
        return fcMembers;

    }
}