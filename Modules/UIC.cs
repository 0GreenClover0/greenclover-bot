using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace GreenClover.Modules
{
    public class UIC : ModuleBase<SocketCommandContext>
    {
        [Command("paulinko")]
        public async Task UICAsync([Remainder] string message = null)
        {
            if (message == null)
            {
                Random r = new Random();
                int random = r.Next(1, 3);

                if (random == 1)
                    await Context.Channel.SendMessageAsync("Słucham?");

                if (random == 2)
                    await Context.Channel.SendMessageAsync("?");
                return;
            }

            message = message.ToLower();

            if (message.Contains("cześć") || message.Contains("hej") || message.Contains("witaj") || message.Contains("wróciłem")
                || message.Contains("siema") || message.Contains("dzień dobry") || message.Contains("hello"))
            {
                Random r = new Random();
                int random = r.Next(1, 8);

                if (random == 1)
                    await Context.Channel.SendMessageAsync("Witaj :)");

                if (random == 2)
                    await Context.Channel.SendMessageAsync("Jak mija ci dzień?");

                if (random == 3)
                    await Context.Channel.SendMessageAsync("Witaj z powrotem");

                if (random == 4)
                    await Context.Channel.SendMessageAsync("Miło mi cię widzieć");

                if (random == 5)
                    await Context.Channel.SendMessageAsync("Hi :3");

                if (random == 6)
                    await Context.Channel.SendMessageAsync("Jestem tu by ci pomóc");

                if (random == 7)
                    await Context.Channel.SendMessageAsync("Słucham");
            }

            else if (message.Contains("pokaż") && message.Contains("książki")
                || message.Contains("znasz") && message.Contains("książki")
                || message.Contains("polecasz") && message.Contains("książki")
                || message.Contains("dobre") && message.Contains("książki"))
            {
                Random r = new Random();
                int random = r.Next(1, 6);

                if (random == 1)
                    await Context.Channel.SendMessageAsync("Sprawdź to https://www.wattpad.com/story/134343836-verta");

                if (random == 2)
                    await Context.Channel.SendMessageAsync("Możesz obaczaić to https://www.wattpad.com/story/134343836-verta");

                if (random == 3)
                    await Context.Channel.SendMessageAsync("Może to ci się spodoba? https://www.wattpad.com/story/134343836-verta");

                if (random == 4)
                    await Context.Channel.SendMessageAsync("https://www.wattpad.com/story/134343836-verta");

                if (random == 5)
                    await Context.Channel.SendMessageAsync("Wattpad to *** ale jest jedna taka https://www.wattpad.com/story/134343836-verta");
            }

            else if (message.Contains("książka"))
            {
                await Context.Channel.SendMessageAsync("Chcesz abym poleciła ci jakąś książkę?");
            }

            else
            {
                await Context.Channel.SendMessageAsync(Utilities.GetGoogleUrl(message));
                return;
            }
        }
    }
}