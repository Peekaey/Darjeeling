using Darjeeling.Helpers;
using Darjeeling.Interfaces;
using Darjeeling.Models.DTOs;
using Microsoft.Extensions.Logging;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Darjeeling.CommandModules.FCUtilities;

public class UpdateMemberData : ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<UpdateMemberData> _logger;
    private readonly IDomainService _domainService;
    private readonly IPermissionHelpers _permissionHelpers;

    
    public UpdateMemberData(ILogger<UpdateMemberData> logger, IDomainService domainService, IPermissionHelpers permissionHelpers)
    {
        _logger = logger;
        _domainService = domainService;
        _permissionHelpers = permissionHelpers;
    }
    
    [SlashCommand("updatememberdata", "Updates the member data for the current guild")]
    public async Task ReturnUpdateMemberData()
    {
        try
        {
            _logger.LogActionTraceStart(Context, "ReturnUpdateMemberData");
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
            
            var isGuildSetup = await _permissionHelpers.CheckRegisteredGuildPermissions(Context.Guild.Id, Context.User.Id);
            
            if (isGuildSetup.Success == false)
            {
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = isGuildSetup.ErrorMessage
                });
                return;
            }
            
            var updateResult = await _domainService.UpdatedMemberData(Context.Guild.Id);

            if (updateResult.Success)
            {
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = $"Successfully updated member data for {Context.Guild.Name}"
                });
            }
            else
            {
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = $"Error when updating member data for {Context.Guild.Name}"
                });
            }
            _logger.LogActionTraceFinish(Context, "ReturnUpdateMemberData");

        }
        catch (Exception e)
        {
            _logger.LogExceptionError(Context, "UpdateMemberData", e);
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"Unexpected Error when running registerfcguild command"
            });
        }

    }

}