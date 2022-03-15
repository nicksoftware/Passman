using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Passman.Commands.GetPassword
{
    public class GetPasswordCommand : Command<GetPasswordCommand.GetPasswordSettings>
    {
        public class GetPasswordSettings : CommandSettings
        {
            [CommandArgument(0, "[WEBSITE_NAME]")]
            public string WebsiteName { get; set; } = String.Empty;

            [CommandOption( "-a| --all")]
            public bool All { get; set; }

            [CommandOption( "-c| --clear")]
            [DefaultValue(false)]
            public bool IsClear { get; set; }
        }
        public override int Execute([NotNull] CommandContext context, [NotNull] GetPasswordSettings settings)
        {
            PassManService service = new();
            Table table = new();
            _ = table.AddColumn(new TableColumn(new Markup("[yellow]Website[/]")));
            _ = table.AddColumn(new TableColumn(new Markup("[yellow]Username[/]")));
            _ = table.AddColumn(new TableColumn(new Markup("[yellow]Password[/]")));

            if (!settings.All && settings.WebsiteName != string.Empty)
            {
                table.Title($"Your {settings.WebsiteName} username and password ");

                //GET PASSWORD FROM DATABASE
                Password password = Task.Run(async () => await service.GetAsync(settings.WebsiteName)).Result;

                //COPY TO CLIPBOARD
                TextCopy.ClipboardService.SetText($"{password.Username} {password.SecretPassword}");

                //DISPLAY ON CONSOLE
                _ = table.AddRow(password.Website, password.Username,  ProcessPassword(settings.IsClear,password.SecretPassword));
            }
            else
            {
                table.Title($"All your saved Passwords");

                List<Password> passwords = Task.Run(async () => await service.GetAll()).Result;
                passwords.ForEach(password =>
                {
                    _ = table.AddRow(
                        password.Website,
                        password.Username,
                        ProcessPassword(settings.IsClear,password.SecretPassword)
                        );
                });

            }
            AnsiConsole.Write(table);
            return 0;
        }

        private string ProcessPassword(bool isClear, string password)
        {
            return isClear ?  password:"*******" ;
        }
    }
}