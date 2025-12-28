using Discord;
using Discord.WebSocket;


class Program
{
    private DiscordSocketClient ? _client;

    static void Main(string[] args)
        => new Program().RunBotAsync().GetAwaiter().GetResult();

    public async Task RunBotAsync()
    {
        // Configuramos los intents necesarios
        var config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds
                           | GatewayIntents.GuildMessages
                           | GatewayIntents.MessageContent
        };

        _client = new DiscordSocketClient(config);

        _client.Log += Log;
        _client.MessageReceived += MessageReceived;

        // Token de tu bot (¡no lo compartas públicamente!) mui importante
        string token = "TU_TOKEN_A";

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        Console.WriteLine("🤖 Bot conectado y listo.");
        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg);
        return Task.CompletedTask;
    }

    private async Task MessageReceived(SocketMessage message)
    {
        // Ignrar mensajes de otros bots
        if (message.Author.IsBot) return;

        // Depuraciòn: ver si el bot recibe mensajes
        Console.WriteLine($"Mensaje recibido: {message.Content}");

        if (message.Content.StartsWith("!clima"))
        {
            string ciudad = message.Content.Replace("!clima", "").Trim();

            if (string.IsNullOrEmpty(ciudad))
            {
                await message.Channel.SendMessageAsync("Escribe una ciudad. Ejemplo: `!clima Madrid`");
                return;
            }

            string clima = await ClimaService.ObtenerClima(ciudad);
            await message.Channel.SendMessageAsync(clima);
        }
    }
}
