using Darjeeling.Helpers;
using Darjeeling.Interfaces;
using Darjeeling.Models.LodestoneEntities;
using Microsoft.Extensions.Logging;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Darjeeling.CommandModules.Interactions;

public class GetFreeCompanyMemberList : ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<GetFreeCompanyMemberList> _logger;
    private readonly ILodestoneApi _lodestoneApi;
    private readonly ICsvHelper _csvHelper;
    private readonly IPermissionHelpers _permissionHelpers;
    public GetFreeCompanyMemberList(ILogger<GetFreeCompanyMemberList> logger, ILodestoneApi lodestoneApi, ICsvHelper csvHelper,
        IPermissionHelpers permissionHelpers)
    {
        _logger = logger;
        _lodestoneApi = lodestoneApi;
        _csvHelper = csvHelper;
        _permissionHelpers = permissionHelpers;
    }
    

    [SlashCommand("getfreecompanylist", "Returns a list of the members within the FC from the lodestone")]
    public async Task ReturnFreeCompanyMemberList([SlashCommandParameter(Name = "fcid", Description = "Free Company ID")] string fcid)
    {
        try
        {
            _logger.LogActionTraceStart(Context, "ReturnFreeCompanyMemberList");
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
            
            
            var webResult = await _lodestoneApi.GetLodestoneFreeCompanyMembers(fcid);
            
            if (webResult.Success == false)
            {
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = $"Unable to get data from Lodestone"
                });
            }
            else
            {
                var memoryStream = await _csvHelper.CreateTableCsv(webResult.Members.ToList());
                var attachment = new AttachmentProperties("FreeCompanyMemberList.csv", memoryStream);
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = $"Free Company Member List for {webResult.FreeCompanyName}",
                    Attachments = new List<AttachmentProperties> {attachment}
                });
            }
            _logger.LogActionTraceFinish(Context, "ReturnFreeCompanyMemberList");
        } catch (Exception error) 
        {
            _logger.LogExceptionError(Context, "ReturnFreeCompanyMemberList", error);
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"Error occured when running getfreecompanylist command"
            });
        }
    }
}
