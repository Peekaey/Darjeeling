using Darjeeling.Models;
using Darjeeling.Models.LodestoneEntities;

namespace Darjeeling.Interfaces;

public interface ILodestoneApi
{
    Task<LodestoneWebResult> GetLodestoneCharacterFreeCompany(string firstName, string lastName, string world);
    Task<LodestoneWebResult> GetLodestoneCharacterId(string firstName, string lastName, string world);
    Task<LodestoneFCWebResult> GetLodestoneFreeCompanyMembers(string fcid);

}