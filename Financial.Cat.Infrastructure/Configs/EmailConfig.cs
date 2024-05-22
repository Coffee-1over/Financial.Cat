namespace Financial.Cat.Infrustructure.Configs;

/// <summary>
/// Email config struct
/// </summary>
public class EmailConfig
{
    /// <summary>
    /// Bearer api key twilio for email sending
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// Email from which they are sent
    /// </summary>
    public string EmailFrom { get; set; }

    /// <summary>
    /// Name company
    /// </summary>
    public string NameFrom { get; set; }
}