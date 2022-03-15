using Passman.Commands.GeneratePassword;
using Passman.Commands.GetPassword;
using Passman.Commands.SavePasswords;
using Spectre.Console.Cli;

CommandApp app = new();

app.Configure(config =>
{
    config.SetApplicationName("passman");
    config.SetApplicationVersion("v1.0.0");


    config.AddCommand<SavePasswordCommand>("add")
    .WithAlias("-d")
    .WithDescription("Add new Password")
    .WithExample(new[] { "add", "nicksoftware.co.za", "nick", "P@ssw0rd!" });

    config.AddCommand<GetPasswordCommand>("get")
        .WithDescription("Get an existing password")
        .WithExample(new[] { "get", "nick.co.za" });

    config.AddCommand<GeneratePasswordCommand>("generate")
        .WithAlias("-g")
        .WithDescription("Get an existing password")
        .WithExample(new[] { "generate", "nick.co.za" });
});

app.Run(args);
