﻿namespace Sameposty.Services.Configurator;
public interface IConfigurator
{
    public string ApiBaseUrl { get; }
    public string WwwRoot { get; }
    public int NumberFirstPostsGenerated { get; }
    public int ImageTokensDefaultLimit { get; }
    public int TextTokensDefaultLimit { get; }
    string AngularClientBaseURl { get; }
}
