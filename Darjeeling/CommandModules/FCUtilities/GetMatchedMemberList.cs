using Darjeeling.Helpers;
using Darjeeling.Interfaces;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Darjeeling.CommandModules.FCUtilities;

public class GetMatchedMemberList : ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<GetMatchedMemberList> _logger;
    private readonly IDomainService _domainService;
    private readonly ICsvHelper _csvHelper;
    private readonly IPermissionHelpers _permissionHelpers;
    
    public GetMatchedMemberList(ILogger<GetMatchedMemberList> logger, IDomainService domainService, ICsvHelper csvHelper
    , IPermissionHelpers permissionHelpers)
    {
        _logger = logger;
        _domainService = domainService;
        _csvHelper = csvHelper;
        _permissionHelpers = permissionHelpers;
    }
    
    [SlashCommand("getmatchedmemberlist", "Returns a list of registered FC Guild Members from the database in a csv file")]
    public async Task ReturnGetMatchedMemberList()
    {
        try
        {
            _logger.LogActionTraceStart(Context, "ReturnGetMatchedMemberList");
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
            
            
            var matchedMembers = await _domainService.GetRegisteredFCGuildMembers(Context.Guild.Id);

            if (matchedMembers.Count > 0)
            {
                var memoryStream = await _csvHelper.CreateTableCsv(matchedMembers);
                var attachment = new AttachmentProperties("RegisteredFCGuildMemberList.csv", memoryStream);
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = "List of Identified and Matched FC Guild Members",
                    Attachments = new List<AttachmentProperties> {attachment}
                });
            } else {
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = "No registered FC Guild Members found"
                });
            }
            
            _logger.LogActionTraceFinish(Context, "ReturnGetMatchedMemberList");
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(Context, "ReturnGetMatchedMemberList", e);
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"Unexpected error occured when running getmatchedmemberlist command"
            });
        }
    }

}