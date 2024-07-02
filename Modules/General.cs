namespace Watermelon.Modules
{
    using System;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Watermelon.Common;
    using Watermelon.Models;
    using Watermelon.GoogleSheetAPI;
    using Discord.Addons.Hosting;
    using Discord.Commands;
    using Discord.WebSocket;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The general module containing commands like ping.
    /// </summary>
    public class General : ModuleBase<SocketCommandContext>
    {


        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="General"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/> to be used.</param>
        public General(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        [Command("hello", RunMode = RunMode.Async)]
        public async Task Hello()
        {
            await Context.Message.ReplyAsync($"Hello {Context.User.Username}. Nice to meet you!");
        }

        [Command("ping")]
        [Summary("Show current latency.")]
        public async Task Ping()
        {
            await ReplyAsync($"Latency: {Context.Client.Latency} ms roundtrip.");
        }

        [Command("register")]
        public async Task RoleTask()
        {
            ulong roleId = 486599202093269012;
            var role = Context.Guild.GetRole(roleId);
            await ((SocketGuildUser)Context.User).AddRoleAsync(role);
        }

        [Command("testSheet")]
        public async Task setRows()
        {
            googleAuth Authenticate = new googleAuth();
            Authenticate.sheetAuth();
            await ReplyAsync($"Authenticated credentials & started service...");
            Authenticate.getValues();
            await ReplyAsync($"{Authenticate._catNum} is the catalogue number.");
        }

        [Command("CTA")]
        public async Task newAnnounce()
        {
            //Instantiate Auth Method
            googleAuth getReleaseInfo = new googleAuth();
            getReleaseInfo.sheetAuth();
            await ReplyAsync($"CALL TO ACTION");
            getReleaseInfo.getValues();

            //Store array entries in local strings
            string releaseID = getReleaseInfo._catNum;
            string artistName = getReleaseInfo._artistName;
            string releaseName = getReleaseInfo._title;
            string releaseDate = getReleaseInfo._releaseDate;
            string remixerName = getReleaseInfo._mixes;
            string socialShare = getReleaseInfo._socialShare;
            string releaseArtwork = getReleaseInfo._imgThumb;
            string labelID = "<@&908909123196841994>, ";

            //Build & display embed
            var embed = new WatermelonEmbedBuilder()
                .WithTitle($"{releaseID}")
                .AddField("Release:", $"{artistName}" + " - " + $"{releaseName}", true)
                .AddField("Remixer:", $"{remixerName}", true)
                .AddField("Released on", releaseDate, true)
                .WithThumbnailUrl(releaseArtwork)
                .WithDescription(labelID + releaseName + " is out now! Please share the label post linked below to your social media accounts \n" + socialShare)
                .Build();
            await ReplyAsync(embed: embed);
        }

        //print preservation to all threads every 7 days / 160 Hours

        //get channel (thread) ID as var for later
        //construct interval (dbl) TimeSpan instance
        //Parse interval into int (days)
        //If timeInteger < 7 days, sleep & return
        //If aSync message guild ID to preserve, reset interval to 0, sleep & return to nested loop

        [Command ("Unarchive")]
        public async Task ArchiveGuard(SocketMessage socketMessage)
        {
            TimeSpan interval = new TimeSpan(7);
            int timeInteger = (int)interval.TotalDays;
            Type thisClient = typeof(DiscordSocketClient);
            PropertyInfo clientProperties = thisClient.GetProperty("_client");


            Console.WriteLine($"{interval} Days");
            Console.WriteLine(clientProperties);
        }

     /*   public async Task archiveGuard(SocketMessage socketMessage)
            {
                TimeSpan interval = new TimeSpan(7);
                int timeInteger = (int)interval.TotalDays;
                Type thisClient = typeof(DiscordSocketClient);
                PropertyInfo clientProperties = thisClient.GetProperty("_client");
                var client = thisClient.TypeInitializer.Name.GetChannel(909756409749598258) as SocketTextChannel;

                if (timeInteger < 7)
                {
                    Thread.Sleep(500);

                    if (timeInteger >= 7)
                    {
                        await channel.SendMessageAsync("Archival imminent. Deploying archive counter-measures.");
                        Thread.Sleep(500);
                        return;
                    }
                }
            }

        private async Task PreserveThread(SocketMessage socketMessage)
        {
            TimeSpan interval = new TimeSpan();
            int timeInteger = (int)interval.TotalDays;
            var channel = _client.GetChannel(909756409749598258) as SocketTextChannel;

            if (timeInteger < 7)
            {
                Thread.Sleep(500);

                if (timeInteger >= 7)
                {
                    await channel.SendMessageAsync("Archival imminent. Deploying archive counter-measures.");
                    Thread.Sleep(500);
                    return;
                }
            }
        }*/

        [Command("howto", RunMode = RunMode.Async)]
        public async Task privateThreadQuery()
        {
            await Context.Message.ReplyAsync($"Hello {Context.User.Username}. To create a Private Thread that only Borderline artists can see for track feedback, please follow this guide and mention '@artists' (without single quotes) at the start of your message.");
                 await ReplyAsync("https://www.mava.app/blog/discord-private-threads-everything-you-need-to-know");
        }
    }
};
