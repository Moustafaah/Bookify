namespace Application.Users.Events.HtmlTemplates;

internal record WelcomeEmailProps(
    string ToEmail,
    string Name,
    string AppName = "Bookify",
    string AppUrl = "https://bookify.example.com",
    string SupportEmail = "support@bookify.example.com",
    string PrivacyUrl = "https://bookify.example.com/privacy",
    string TermsUrl = "https://bookify.example.com/terms",
    string UnsubscribeUrl = "https://bookify.example.com/unsubscribe");
