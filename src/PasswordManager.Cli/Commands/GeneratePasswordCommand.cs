using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Passman.Commands.GeneratePassword
{
    public class GeneratePasswordCommand : Command<GeneratePasswordCommand.GeneratePasswordSettings>
    {
        public class GeneratePasswordSettings : CommandSettings
        {
            [CommandArgument(0, "<WEBSITE_NAME>")]
            [Description("Website name")]
            public string Website { get; set; }

            [CommandArgument(1, "[USERNAME]")]
            [Description("Your site Username ")]
            [DefaultValue("")]
            public string Username { get; set; }

            [CommandOption("-l| --length")]
            [DefaultValue(0)]
            public int PasswordLength { get; set; }
        }


        public override int Execute([NotNull] CommandContext context, [NotNull] GeneratePasswordSettings settings)
        {
            var passwordLength = settings.PasswordLength;
            var generatedPassword = settings.PasswordLength > 0 ? GenerateRandomPassword(passwordLength) : GenerateRandomPassword();

            Password newPassword = Password.Create(
                site: settings.Website,
                username: settings.Username,
                password: generatedPassword);

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
        private string GenerateRandomPassword(int length = 6)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

}