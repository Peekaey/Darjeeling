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
                                    DiscordUsername = member.Username,
                                    DiscordGuildNickname = member.Nickname ?? "",
                                    DiscordNickName = member.GlobalName ?? "",
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
    List<FCGuildMember> lodestoneFcGuildMembers, List<FCGuildMember> existingFcGuildMembers, int guildId)
{
    List<FCGuildMember> updatedFcGuildMembers = new List<FCGuildMember>();
    List<FCGuildMember> newFcGuildMembers = new List<FCGuildMember>();

    foreach (var lodestoneFcMember in lodestoneFcGuildMembers)
    {
        var existingFcMember = existingFcGuildMembers.FirstOrDefault(m => m.DiscordUserUId == lodestoneFcMember.DiscordUserUId);
        if (existingFcMember != null)
        {
            bool isLodestoneNameChanged = existingFcMember.LodestoneNameHistories.OrderByDescending(h => h.DateAdded).First().FirstName != lodestoneFcMember.LodestoneNameHistories.First().FirstName ||
                                          existingFcMember.LodestoneNameHistories.OrderByDescending(h => h.DateAdded).First().LastName != lodestoneFcMember.LodestoneNameHistories.First().LastName;

            bool isDiscordNameChanged = existingFcMember.DiscordNameHistories.OrderByDescending(h => h.DateAdded).First().DiscordUsername != lodestoneFcMember.DiscordNameHistories.First().DiscordUsername ||
                                        existingFcMember.DiscordNameHistories.OrderByDescending(h => h.DateAdded).First().DiscordNickName != lodestoneFcMember.DiscordNameHistories.First().DiscordNickName ||
                                        existingFcMember.DiscordNameHistories.OrderByDescending(h => h.DateAdded).First().DiscordGuildNickname != lodestoneFcMember.DiscordNameHistories.First().DiscordGuildNickname;

            if (isLodestoneNameChanged)
            {
                existingFcMember.LodestoneNameHistories.Add(new LodestoneNameHistory
                {
                    FirstName = lodestoneFcMember.LodestoneNameHistories.First().FirstName,
                    LastName = lodestoneFcMember.LodestoneNameHistories.First().LastName,
                    DateAdded = DateTime.UtcNow
                });
            }

            if (isDiscordNameChanged)
            {
                existingFcMember.DiscordNameHistories.Add(new DiscordNameHistory
                {
                    DiscordUsername = lodestoneFcMember.DiscordNameHistories.First().DiscordUsername,
                    DiscordNickName = lodestoneFcMember.DiscordNameHistories.First().DiscordNickName,
                    DiscordGuildNickname = lodestoneFcMember.DiscordNameHistories.First().DiscordGuildNickname,
                    DateAdded = DateTime.UtcNow
                });
            }

            updatedFcGuildMembers.Add(existingFcMember);
        }
        else
        {
            newFcGuildMembers.Add(new FCGuildMember
            {
                DiscordUserUId = lodestoneFcMember.DiscordUserUId,
                LodestoneId = lodestoneFcMember.LodestoneId,
                LodestoneNameHistories = lodestoneFcMember.LodestoneNameHistories,
                DiscordNameHistories = lodestoneFcMember.DiscordNameHistories,
                FCGuildServerId = guildId,
                DateCreated = DateTime.UtcNow
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
                DiscordGuildNickName = fcGuildMember.DiscordNameHistories.OrderByDescending(dnh => dnh.DateAdded).FirstOrDefault().DiscordGuildNickname,
                LodestoneCharacterId = fcGuildMember.LodestoneId,
                LodestoneFirstName = fcGuildMember.LodestoneNameHistories.OrderByDescending(lnh => lnh.DateAdded).FirstOrDefault().FirstName,
                LodestoneLastName = fcGuildMember.LodestoneNameHistories.OrderByDescending(lnh => lnh.DateAdded).FirstOrDefault().LastName
            });
        }

        return fcGuildMemberDtos;
    }
    
    public async Task<List<GuildMemberDTO>> MapGuildUserToGuildMemberDTO(List<GuildUser> guildUsers)
    {
        List<GuildMemberDTO> guildMemberDtos = new List<GuildMemberDTO>();
        foreach (var guildUser in guildUsers)
        {
            guildMemberDtos.Add(new GuildMemberDTO
            {
                DiscordUsername = guildUser.Username,
                DiscordGuildNickname = guildUser.Nickname ?? "",
                DiscordNickName = guildUser.GlobalName ?? "",
                DiscordUserId = guildUser.Id.ToString()
            });
        }
        return guildMemberDtos;
    }

    public async Task<GuildMemberNameHistoryDTO> MapGuildUserToGuildMemberNameHistoryDTO(FCGuildMember guildUser)
    {
        return new GuildMemberNameHistoryDTO
        {
            DiscordNameHistories = guildUser.DiscordNameHistories
                .OrderByDescending(dnh => dnh.DateAdded)
                .Select(dnh => new DiscordNameHistoryDTO
                {
                    DiscordUsername = dnh.DiscordUsername,
                    DiscordNickName = dnh.DiscordNickName,
                    DiscordGuildNickname = dnh.DiscordGuildNickname,
                    DateAdded = dnh.DateAdded
                }).ToList(),
            LodestoneNameHistories = guildUser.LodestoneNameHistories
                .OrderByDescending(lnh => lnh.DateAdded)
                .Select(lnh => new LodestoneNameHistoryDTO
                {
                    FirstName = lnh.FirstName,
                    LastName = lnh.LastName,
                    DateAdded = lnh.DateAdded
                }).ToList()
        };
    }
}