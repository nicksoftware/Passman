using Passman.Commands;
using PasswordManager.Commands;
using Spectre.Console.Cli;

CommandApp app = new();

app.Configure(config =>
{
    _ = config.SetApplicationName("passman");
    _ = config.SetApplicationVersion("v1.0.0");


    _ = config.AddCommand<SavePasswordCommand>("add")
    .WithAlias("-d")
    .WithDescription("Add new Password")
    .WithExample(new[] { "add", "nicksoftware.co.za", "nick", "P@ssw0rd!" });

    _ = config.AddCommand<GetPasswordCommand>("get")
        .WithDescription("Get an existing password")
        .WithExample(new[] { "get", "nick.co.za" });

    _ = config.AddCommand<GeneratePasswordCommand>("generate")
        .WithAlias("-g")
        .WithDescription("Generate new password")
        .WithExample(new[] { "generate", "nick.co.za" });
});

app.Run(args);
