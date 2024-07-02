namespace Watermelon.Services
{
    using System;
    using System.IO;
    using System.Text;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Addons.Hosting;
    using Discord.Commands;
    using Discord.WebSocket;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The class responsible for handling the commands and various events.
    /// </summary>
    public class CommandHandler : InitializedService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IServiceProvider"/> that should be injected.</param>
        /// <param name="client">The <see cref="DiscordSocketClient"/> that should be injected.</param>
        /// <param name="service">The <see cref="CommandService"/> that should be injected.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> that should be injected.</param>
        public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService service, IConfiguration configuration)
        {
            _provider = provider;
            _client = client;
            _service = service;
            _configuration = configuration;
        }

        /// <inheritdoc/>
        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += OnMessageReceived;
            _service.CommandExecuted += OnCommandExecuted;
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> commandInfo, ICommandContext commandContext, IResult result)
        {
            if (result.IsSuccess)
            {
                return;
            }

            await commandContext.Channel.SendMessageAsync(result.ErrorReason);
        }

        private async Task OnMessageReceived(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage message))
            {
                return;
            }

            if (message.Source != MessageSource.User)
            {
                return;
            }

            var argPos = 0;
            if (!message.HasStringPrefix(_configuration["Prefix"], ref argPos) && !message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);
            await _service.ExecuteAsync(context, argPos, _provider);


        }


        /* private async Task Shutupchris(SocketMessage socketMessage, string messageId, string msg, SocketGuildUser user)
         {

             var id = Convert.ToUInt64(messageId);
             var context = new SocketCommandContext(_client, msg);
             msg = (SocketUserMessage)await context.Channel.GetMessageAsync(id);

             if (socketMessage is IUserMessage msg)
             {
                 await msg.ModifyAsync(m => m.Content = "Fucking hell");
             }
         } */


        public async Task AnnounceUserJoin(SocketGuildUser user)
        {
            var embed = new EmbedBuilder();
            var sb = new StringBuilder();
            embed.WithColor(new Color(0, 255, 0));
            embed.Title = "Welcome To the Borderline Music Community";
            sb.AppendLine(user.Mention);
            sb.AppendLine("On your left, you'll find channels to get involved with chat and submit music for feedback from Borderline artists.");
            sb.AppendLine("Joined! Welcome.");
            embed.Description = sb.ToString();
            var channel = _client.GetChannel(981275243689689199) as SocketTextChannel;
            await channel.SendMessageAsync(null, false, embed.Build());
        }

        //print preservation to all threads every 7 days / 160 Hours

        //get channel (thread) ID as var for later
        //construct interval (dbl) TimeSpan instance
        //Parse interval into int (days)
        //If timeInteger < 7 days, sleep & return
        //If aSync message guild ID to preserve, reset interval to 0, sleep & return to nested loop




    }
};