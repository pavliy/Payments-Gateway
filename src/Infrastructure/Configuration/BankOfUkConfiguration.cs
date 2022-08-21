namespace Infrastructure.Configuration;

public class BankOfUkConfiguration
{
    public const string SettingsSectionName = "BankOfUk";

    public string Url { get; set; } = null!;
}