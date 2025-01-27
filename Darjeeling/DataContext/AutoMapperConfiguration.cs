// using AutoMapper;
// using Darjeeling.CommandModules.FCUtilities;
// using Darjeeling.Models.DTOs;
// using Darjeeling.Models.Entities;
// using Darjeeling.Models.LodestoneEntities;
//
// namespace Darjeeling.Repositories;
//
// public class AutoMapperConfiguration : Profile
// {
//     public AutoMapperConfiguration()
//     {
//         CreateMap<FCGuildRoleDTO, FCGuildRole>()
//             .ForMember(dest => dest.Id, opt => opt.Ignore())
//             .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
//             .ForMember(dest => dest.RoleType, opt => opt.Ignore())
//             
//             .AfterMap((src, dest ) => dest.DateCreated = DateTime.Now);
//         
//         CreateMap<FCGuildServerDTO, FCGuildServer>()
//             .AfterMap((src, dest) => dest.DateCreated = DateTime.Now);
//         
//         CreateMap<LodestoneFCMember, NameHistory>()
//             .AfterMap((src, dest) => dest.DateAdded = DateTime.Now);
//     }
// }