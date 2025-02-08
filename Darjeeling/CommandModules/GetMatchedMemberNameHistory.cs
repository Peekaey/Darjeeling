using Darjeeling.Helpers;
using Darjeeling.Interfaces;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Darjeeling.CommandModules;

public class GetMatchedMemberNameHistory : ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<GetMatchedMemberNameHistory> _logger;
    private readonly IPermissionHelpers _permissionHelpers;
    private readonly ICsvHelper _csvHelpers;
    private readonly IDomainService _domainService;
    
    public GetMatchedMemberNameHistory(ILogger<GetMatchedMemberNameHistory> logger, IPermissionHelpers permissionHelpers, ICsvHelper csvHelpers, IDomainService domainService)
    {
        _logger = logger;
        _permissionHelpers = permissionHelpers;
        _csvHelpers = csvHelpers;
        _domainService = domainService;
    }


    [SlashCommand("getmatchedmembernamehistory", "Gets the matched member name history for the current guild")]
    public async Task ReturnGetMatchedMemberNameHistory(User user)
    {
        try
        {
            _logger.LogActionTraceStart(Context, "ReturnGetMatchedMemberNameHistory");
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());

            var isGuildSetup =
                await _permissionHelpers.CheckRegisteredGuildPermissions(Context.Guild.Id, Context.User.Id, Context.Channel.Id);

            if (isGuildSetup.Success == false)
            {
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = isGuildSetup.ErrorMessage
                });
                return;
            }

            var matchedMemberNameHistory = await _domainService.GetMatchedMemberNameHistoryList(Context.User.Id);

            if (matchedMemberNameHistory == null)
            {
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = $"User {Context.User.Username} has not been matched with a Lodestone character and has no stored name history"
                });
            }
            else
            {
                var memoryStream = await _csvHelpers.CreateGuildMemberNameHistoryDtoCsv(matchedMemberNameHistory);
                var attachment = new AttachmentProperties("MatchedMemberNameHistory.csv", memoryStream);
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = $"Matched member name history for {Context.User.Username}",
                    Attachments = new List<AttachmentProperties> { attachment }
                });
            }
        } catch (Exception e) {
            _logger.LogExceptionError(Context, "ReturnGetMatchedMemberNameHistory", e);
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"Error when getting matched member name history for {Context.User.Username}"
            });
        }         
    }
    
}