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
        [SlashCommandParameter(Name = "fcid", Description = "Lodestone Free Company ID")]
        string fcid,
        [SlashCommandParameter(Name = "adminroleid", Description = "Role ID of Discord Admin Role")]
        string adminroleid,
        [SlashCommandParameter(Name = "adminchannelid", Description = "Role ID of Discord Admin Channel")]
        string adminchannelid
        )
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
                RegisterFCGuildServerDTO registerFcGuildServerDto = new RegisterFCGuildServerDTO
                {
                    AdminRoleId = adminroleid,
                    DiscordGuildUid = Context.Guild.Id.ToString(),
                    DiscordGuildName = Context.Guild.Name,
                    FreeCompanyName = webResult.FreeCompanyName,
                    FcMembers = webResult.Members,
                    AdminChannelId = adminchannelid
                };

                var registerResult = await _domainService.RegisterFCGuildServer(registerFcGuildServerDto);
                if (registerResult.Success)
                {
                    await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                    {
                        Content = $"Free Company {webResult.FreeCompanyName} has been successfully registered"
                    });
                }
                else
                {
                    await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                    {
                        Content = $"Error occured when registering Free Company {webResult.FreeCompanyName}"
                    });
                }
                _logger.LogActionTraceFinish(Context, "ReturnRegisterFCGuild");
            }
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(Context, "ReturnRegisterFCGuild", e);
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"Unexpected error occured when running registerfcguild command"
            });
        }
    }

}