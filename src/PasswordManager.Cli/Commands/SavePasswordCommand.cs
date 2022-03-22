using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Passman.Commands
{
    public class SavePasswordCommand : Command<SavePasswordCommand.SavePasswordSettings>
    {
        public class SavePasswordSettings : CommandSettings
        {
            [CommandArgument(0, "<WEBSITE>")]
            [Description("The websites url, (This should be unique)")]
            public string Website { get; set; } = String.Empty;

            [CommandArgument(1, "<USERNAME>")]
            [Description("Your username on the site")]
            public string Username { get; set; } = String.Empty;


            [CommandArgument(1, "<PASSWORD>")]
            [Description("Your password on the site")]
            public string Password { get; set; } = string.Empty;


        }
        public override int Execute([NotNull] CommandContext context, [NotNull] SavePasswordSettings settings)
        {
            Password newPassword = Password.Create(settings.Website, settings.Username, settings.Password);

            PassManService passwordService = new();
            try
            {
                passwordService.Create(newPassword);
            }
            catch (Exception exc)
            {
                AnsiConsole.WriteException(exc);
                return -1;
            }
            AnsiConsole.Status()
                .Start("Saving Password...", ctx =>
                {
                    // Simulate some work
                    ctx.Status("Encrypting Data...");
                    // AnsiConsole.MarkupLine("Encrypting Data...");
                    ctx.Spinner(Spinner.Known.Star);
                    Thread.Sleep(1000);

                    // Update the status and spinner
                    ctx.Status("Finalizing Encryption");
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));
                    Thread.Sleep(1000);

                });
            AnsiConsole.MarkupLine("[green]Password Saved 😁[/]");

            return 0;

        }
    }

}
