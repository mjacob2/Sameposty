﻿namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;
public interface IImageGeneratingOrchestrator
{
    Task<string> GenerateImage(string companyDescription);
}
