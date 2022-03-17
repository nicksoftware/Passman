using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Passman;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PasswordManager.Commands
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
            [DefaultValue(6)]
            public int PasswordLength { get; set; }

            [CommandOption("-s| --special-characters")]
            [DefaultValue(false)]
            public bool IncludeSpecialChar { get; set; }


            [CommandOption("-n| --numeric-characters")]
            [DefaultValue(false)]
            public bool IncludeNumerics { get; set; }
        }


        public override int Execute([NotNull] CommandContext context, [NotNull] GeneratePasswordSettings settings)
        {
            int passwordLength = settings.PasswordLength;

            string generatedPassword = GenerateRandomPassword(
                    passwordLength,
                    settings.IncludeSpecialChar,
                    settings.IncludeNumerics);

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
                    _ = ctx.Status("Encrypting Data...");
                    // AnsiConsole.MarkupLine("Encrypting Data...");
                    _ = ctx.Spinner(Spinner.Known.Star);
                    Thread.Sleep(1000);

                    // Update the status and spinner
                    _ = ctx.Status("Finalizing Encryption");
                    _ = ctx.Spinner(Spinner.Known.Star);
                    _ = ctx.SpinnerStyle(Style.Parse("green"));
                    Thread.Sleep(1000);

                });
            AnsiConsole.MarkupLine("[green]Password Saved 😁[/]");

            return 0;
        }
        private static string GenerateRandomPassword(
            int length,
            bool includeSpecialChar,
            bool includeNumerics)
        {
            Random random = new();
            StringBuilder builder = new();

            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            string specialSymbols = "!@#$%^&*-+=:?/.,';~`±§";
            string numerics = "0123456789";

            _ = builder.Append(alphabets.ToLower());
            _ = builder.Append(alphabets);

            if (includeSpecialChar)
            {
                _ = builder.Append(specialSymbols);
            }

            if (includeNumerics)
            {
                _ = builder.Append(numerics);
            }

            string chars = builder.ToString();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

}