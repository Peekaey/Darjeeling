using Darjeeling.Helpers;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Darjeeling.CommandModules.Interactions;

public class Greeting : ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<Greeting> _logger;
    public Greeting(ILogger<Greeting> logger)
    {
        _logger = logger;
    }
        
    [SlashCommand("greet", "Greet someone!")]
    public async Task ReturnGreeting(User user)
    {
        try {
            _logger.LogActionTraceStart(Context, "ReturnGreeting");
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
            
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"{Context.User} greets {user}!"
            });
            
            _logger.LogActionTraceFinish(Context, "ReturnGreeting");
        } catch (Exception error) {
            _logger.LogExceptionError(Context, "ReturnGreeting", error);
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"Error occured when running running greet command"
            });
        }
    }
}
