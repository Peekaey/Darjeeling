using Darjeeling.Helpers;
using Darjeeling.Interfaces;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Darjeeling.CommandModules;

public class GetGuildMembersList : ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<GetGuildMembersList> _logger;
    private readonly ICsvHelper _csvHelper;
    private readonly IDiscordBackendApiService _discordBackendApiService;
    
    public GetGuildMembersList(ILogger<GetGuildMembersList> logger, ICsvHelper csvHelper, IDiscordBackendApiService discordBackendApiService)
    {
        _logger = logger;
        _csvHelper = csvHelper;
        _discordBackendApiService = discordBackendApiService;
    }

    public async Task ReturnGetGuildMembersList()
    {
        try
        {
            _logger.LogActionTraceStart(Context, "ReturnGetGuildMembersList");
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
            var guildMembers = await _discordBackendApiService.GetGuildMembers(Context.Guild.Id);
            
            if (guildMembers.Count > 0)
            {
                var memoryStream = await _csvHelper.CreateTableCsv(guildMembers);
                var attachment = new AttachmentProperties("GuildMembersList.csv", memoryStream);
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = "List of Guild Members",
                    Attachments = new List<AttachmentProperties> {attachment}
                });
            }
            else
            {
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = "No Guild Members found"
                });
            }


        }
        catch (Exception e)
        {
            _logger.LogExceptionError(Context, "ReturnGetGuildMembersList", e);
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"Unexpected error occured when running getguildmemberslist command"
            });
            
        }
    }

}