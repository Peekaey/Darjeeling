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

    public async Task<UpdatedFCMembersResult> MapMatchedLodestoneMemberToExistingFCGuildMember(
        List<FCGuildMember> lodestoneFcGuildMembers, List<FCGuildMember> existingFcGuildMembers)
    {
        List<FCGuildMember> updatedFcGuildMembers = new List<FCGuildMember>();
        List<FCGuildMember> newFcGuildMembers = new List<FCGuildMember>();
        foreach (var lodestoneFcMember in lodestoneFcGuildMembers)
        {
            foreach (var existingFcMember in existingFcGuildMembers)
            {
                if (existingFcMember.LodestoneId == lodestoneFcMember.LodestoneId)
                {
                    if (existingFcMember.LodestoneNameHistories.First().FirstName !=
                        lodestoneFcMember.LodestoneNameHistories.First().FirstName ||
                        existingFcMember.LodestoneNameHistories.First().LastName !=
                        lodestoneFcMember.LodestoneNameHistories.First().LastName)
                    {
                        existingFcMember.LodestoneNameHistories.Add(new LodestoneNameHistory
                        {
                            FirstName = lodestoneFcMember.LodestoneNameHistories.First().FirstName,
                            LastName = lodestoneFcMember.LodestoneNameHistories.First().LastName,
                            DateAdded = DateTime.UtcNow
                        });
                    }

                    if (existingFcMember.DiscordNameHistories.First().DiscordUsername !=
                        lodestoneFcMember.DiscordNameHistories.First().DiscordUsername ||
                        existingFcMember.DiscordNameHistories.First().DiscordNickName !=
                        lodestoneFcMember.DiscordNameHistories.First().DiscordNickName)
                    {
                        existingFcMember.DiscordNameHistories.Add(new DiscordNameHistory
                        {
                            DiscordUsername = lodestoneFcMember.DiscordNameHistories.First().DiscordUsername,
                            DiscordNickName = lodestoneFcMember.DiscordNameHistories.First().DiscordNickName,
                            DateAdded = DateTime.UtcNow
                        });
                    }
                    updatedFcGuildMembers.Add(existingFcMember);
                    break;
                }

                newFcGuildMembers.Add(new FCGuildMember
                {
                    DiscordUserUId = lodestoneFcMember.DiscordUserUId,
                    LodestoneId = lodestoneFcMember.LodestoneId,
                    LodestoneNameHistories = lodestoneFcMember.LodestoneNameHistories,
                    DiscordNameHistories = lodestoneFcMember.DiscordNameHistories
                });
            }
        }
        return new UpdatedFCMembersResult
        {
            UpdatedFCGuildMembers = updatedFcGuildMembers,
            NewFCGuildMembers = newFcGuildMembers
        };
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
        RegisterFCGuildServerDTO registerFcGuildServerDto, List<FCGuildMember> fcGuildMembers, FCGuildRole fcGuildRole)
    {
        return new FCGuildServer
        {
            AdminRoleId = registerFcGuildServerDto.AdminRoleId,
            DiscordGuildUid = registerFcGuildServerDto.DiscordGuildUid,
            DateCreated = DateTime.UtcNow,
            FCMembers = fcGuildMembers,
            DiscordGuildName = registerFcGuildServerDto.DiscordGuildName,
            FreeCompanyName = registerFcGuildServerDto.FreeCompanyName,
            AdminChannelId = registerFcGuildServerDto.AdminChannelId,
            FreeCompanyId = registerFcGuildServerDto.FreeCompanyId,
            FCAdminRole = fcGuildRole
        };
    }

    public async Task<List<FCGuildMemberDTO>> MapFCGuildMemberToFCGUildMemberDTO(List<FCGuildMember> fcGuildMembers)
    {
        List<FCGuildMemberDTO> fcGuildMemberDtos = new List<FCGuildMemberDTO>();

        foreach (var fcGuildMember in fcGuildMembers)
        {
            fcGuildMemberDtos.Add(new FCGuildMemberDTO
            {
                // Deference of null however fcguildMember can't be created without an initial discord/lodestone name history
                // TODO Fix Nullable Reference Types (only cosmetic for now)
                DiscordUsername = fcGuildMember.DiscordNameHistories.OrderByDescending(dnh => dnh.DateAdded).FirstOrDefault().DiscordUsername,
                DiscordNickname = fcGuildMember.DiscordNameHistories.OrderByDescending(dnh => dnh.DateAdded).FirstOrDefault().DiscordNickName,
                LodestoneCharacterId = fcGuildMember.LodestoneId,
                LodestoneFirstName = fcGuildMember.LodestoneNameHistories.OrderByDescending(lnh => lnh.DateAdded).FirstOrDefault().FirstName,
                LodestoneLastName = fcGuildMember.LodestoneNameHistories.OrderByDescending(lnh => lnh.DateAdded).FirstOrDefault().LastName
            });
        }

        return fcGuildMemberDtos;
    }
    
}