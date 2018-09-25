using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Addons.Interactive;

namespace GreenClover.Modules
{
    public class UIC : InteractiveBase
    {
        [Command("paulinko")]
        public async Task UICAsync([Remainder] string message = "")
        {
            // Jest to prototyp bota, z którym można rozmawiać
            // Im więcej if'ów tym lepiej działa
            // Zrobienie go tym sposobem jest wysoce nieefektywne i mało skuteczne

            // Jeśli użytkownik jedynie wpisze komendę
            if (message == "")
            {
                Random r = new Random();
                int random = r.Next(1, 3);

                if (random == 1)
                    await ReplyAsync("Słucham?");

                if (random == 2)
                    await ReplyAsync("?");
                return;
            }

            message = message.ToLower();

            // Przywitanie
            if (message.Contains("cześć") || message.Contains("hej") || message.Contains("witaj") || message.Contains("wróciłem")
                || message.Contains("siema") || message.Contains("dzień dobry") || message.Contains("hello"))
            {
                Random r = new Random();
                int random = r.Next(1, 8);

                if (random == 1)
                    await ReplyAsync("Witaj :)");

                if (random == 2)
                    await ReplyAsync("Jak mija ci dzień?");

                if (random == 3)
                    await ReplyAsync("Witaj z powrotem");

                if (random == 4)
                    await ReplyAsync("Miło mi cię widzieć");

                if (random == 5)
                    await ReplyAsync("Hi :3");

                if (random == 6)
                    await ReplyAsync("Jestem tu by ci pomagać");

                if (random == 7)
                    await ReplyAsync("Słucham");
            }

            // Pytanie o pokazanie/polecenie książek
            else if (message.Contains("pokaż") && message.Contains("książki")
                || message.Contains("znasz") && message.Contains("książki")
                || message.Contains("polecasz") && message.Contains("książki")
                || message.Contains("dobre") && message.Contains("książki"))
            {
                Random r = new Random();
                int random = r.Next(1, 6);

                if (random == 1)
                {
                    await ReplyAsync("Sprawdź to https://www.wattpad.com/story/134343836-verta");
                }

                if (random == 2)
                {
                    await ReplyAsync("Możesz obaczaić to https://www.wattpad.com/story/134343836-verta");
                }

                if (random == 3)
                {
                    await ReplyAsync("Może to ci się spodoba? https://www.wattpad.com/story/134343836-verta");
                }

                if (random == 4)
                {
                    await ReplyAsync("https://www.wattpad.com/story/134343836-verta");
                }

                if (random == 5)
                {
                    await ReplyAsync("Wattpad to *** ale jest jedna taka https://www.wattpad.com/story/134343836-verta");
                }
                return;
            }

            // Gdy wiadomość zawiera jedynie wyraz książka
            // W związku z tym, że bot zadaje pytanie, trzeba tutaj stworzyć komendę confirm
            // TO DO
            else if (message.Contains("książka"))
            {
                await ReplyAsync("Chcesz abym poleciła ci jakąś książkę?");

                var response = NextMessageAsync();
                string answer = response.ToString();

                if (answer == "tak" || answer == "no" || answer =="nom")
                {
                    Random r = new Random();
                    int random = r.Next(1, 6);

                    if (random == 1)
                    {
                        await ReplyAsync("Sprawdź to https://www.wattpad.com/story/134343836-verta");
                    }

                    if (random == 2)
                    {
                        await ReplyAsync("Możesz obaczaić to https://www.wattpad.com/story/134343836-verta");
                    }

                    if (random == 3)
                    {
                        await ReplyAsync("Może to ci się spodoba? https://www.wattpad.com/story/134343836-verta");
                    }

                    if (random == 4)
                    {
                        await ReplyAsync("https://www.wattpad.com/story/134343836-verta");
                    }

                    if (random == 5)
                    {
                        await ReplyAsync("Wattpad to *** ale jest jedna taka https://www.wattpad.com/story/134343836-verta");
                    }
                    return;
                }
            }

            // Jeżeli coś jest niezrozumiałe
            else
            {
                int searchType = 1;
                await ReplyAsync(GoogleService.GetGoogle(message, searchType));
                return;
            }
        }
    }
}