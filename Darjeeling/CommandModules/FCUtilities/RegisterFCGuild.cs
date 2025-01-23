using Darjeeling.Helpers;
using Darjeeling.Interfaces;
using Darjeeling.Models.DTOs;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Darjeeling.CommandModules.FCUtilities;

public class RegisterFCGuild : ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<RegisterFCGuild> _logger;
    private readonly ILodestoneApi _lodestoneApi;
    private readonly IDomainService _domainService;
    private readonly IMappingHelper _mappingHelper;
    
    public RegisterFCGuild(ILogger<RegisterFCGuild> logger, ILodestoneApi lodestoneApi, IDomainService domainService,
            IMappingHelper mappingHelper)
        
    {
        _logger = logger;
        _lodestoneApi = lodestoneApi;
        _domainService = domainService;
        _mappingHelper = mappingHelper;
    }
    
    [SlashCommand("registerfcguild", "Registers a Free Company to the bot")]
    public async Task ReturnRegisterFCGuild(
        [SlashCommandParameter(Name = "fcid", Description = "Free Company ID")]
        string fcid,
        [SlashCommandParameter(Name = "adminroleid", Description = "Free Company Name")]
        string adminroleid)
    {
        try
        {
            _logger.LogActionTraceStart(Context, "ReturnRegisterFCGuild");
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
            
            var webResult = await _lodestoneApi.GetLodestoneFreeCompanyMembers(fcid);
            
            if (webResult.Success == false)
            {
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = $"Error occured when running registerfcguild command"
                });
            }
            else
            {
                var context = Context;
                var something = "";
                var discordGuildId = Context.Guild.Id;
                
                RegisterFCGuildServerDTO registerFcGuildServerDto = new RegisterFCGuildServerDTO
                {
                    AdminRoleId = adminroleid,
                    DiscordGuildUid = discordGuildId.ToString(),
                    DiscordGuildName = Context.Guild.Name,
                    FreeCompanyName = webResult.FreeCompanyName,
                    FcMembers = await _mappingHelper.MapLodestoneFCMembersToFCMembers(webResult.Members.ToList())
                };

                var registerResult = _domainService.RegisterFCGuildServer(registerFcGuildServerDto, webResult.Members.ToList());

            }
            
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(Context, "ReturnRegisterFCGuild", e);
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"Error occured when running registerfcguild command"
            });
        }
    }

}