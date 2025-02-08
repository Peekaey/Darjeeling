using Darjeeling.Helpers;
using Darjeeling.Interfaces;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Darjeeling.CommandModules.Interactions;

public class GetLodestoneCharacterId : ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<GetLodestoneCharacterId> _logger;
    private readonly ILodestoneApi _lodestoneApi;
    private readonly IPermissionHelpers _permissionHelpers;
    public GetLodestoneCharacterId(ILogger<GetLodestoneCharacterId> logger, ILodestoneApi lodestoneApi,
        IPermissionHelpers permissionHelpers)
    {
        _logger = logger;
        _lodestoneApi = lodestoneApi;
        _permissionHelpers = permissionHelpers;
    }
    
    [SlashCommand("getcharacterid", "Returns the Lodestone Character ID of a user")]
    public async Task ReturnLodestoneCharacterId([SlashCommandParameter(Name = "firstname", Description = "First Name")] string firstName ,
        [SlashCommandParameter(Name = "lastname", Description = "Last Name")] string lastName,
        [SlashCommandParameter(Name = "world", Description = "World")] string world)
    {
        try {
            _logger.LogActionTraceStart(Context, "ReturnLodestoneCharacterId");
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
            
            var isGuildSetup = await _permissionHelpers.CheckRegisteredGuildPermissions(Context.Guild.Id, Context.User.Id, Context.Channel.Id);
            
            if (isGuildSetup.Success == false)
            {
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = isGuildSetup.ErrorMessage
                });
                return;
            }
            
            var webResult = await _lodestoneApi.GetLodestoneCharacterId(firstName, lastName, world);

            if (webResult.Success == false)
            {
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = webResult.ResultValue
                });
            }
            else
            {
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = $"Character ID for {firstName} {lastName} on {world} is {webResult.ResultValue}"
                });
            }
            
            _logger.LogActionTraceFinish(Context, "ReturnLodestoneCharacterId");
        } catch (Exception error) {
            _logger.LogExceptionError(Context, "ReturnLodestoneCharacterId", error);
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"Error occured when running getcharacterid command"
            });
        }
    }
}