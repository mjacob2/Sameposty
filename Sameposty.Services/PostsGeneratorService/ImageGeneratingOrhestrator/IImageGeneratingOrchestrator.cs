﻿namespace Sameposty.Services.PostsGeneratorService.ImageGeneratingOrhestrator;
public interface IImageGeneratingOrchestrator
{
    Task<string> GenerateImage(string productsAndServices, int postId, bool generateImage);

    Task<string> GenerateImageFromUserPrompt(string prompt);
}
