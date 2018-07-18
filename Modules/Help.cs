using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace GreenClover.Modules
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("pomoc")]
        public async Task HelpAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder
            .AddField("- GreenClover - pomoc", "~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~")
            .AddField("Komendy", "Lista dostępnych komend. Aby dowiedzieć się co robi komenda możesz również napisać help [komenda]. Na przykład help ping. // aktualnie nie działa lol")
            .AddField("Prefix", "Domyślny prefix to *")
            .AddField("[*ping]", "Sprawdź jak bardzo dzisiaj jestem opóźniony")
            .AddField("[*profil]", "Sprawdź swój lub kogoś profil, na którym raczej nie ma niczego ciekawego")
            .AddField("[*przytulas]", "Dotknij kogoś obiema rękami")
            .AddField("[*wybierz]", "Nie możesz się zdecydować co wybrać? Pozwól botowi wybrać za ciebie")
            .WithColor(new Color(90, 50, 165));

            await Context.Channel.SendMessageAsync(" ", false, builder.Build());
        }
    }
}
