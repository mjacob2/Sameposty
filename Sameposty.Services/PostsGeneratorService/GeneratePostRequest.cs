﻿namespace Sameposty.Services.PostsGeneratorService;
public class GeneratePostRequest
{
    public required int UserId { get; set; }

    public string BrandName { get; set; }

    public string Audience { get; set; }

    public string Mission { get; set; }

    public string ProductsAndServices { get; set; }

    public string Goals { get; set; }

    public string Assets { get; set; }

    public DateTime ShedulePublishDate { get; set; }

    public bool GenerateImage { get; set; }

    public bool GenerateText { get; set; }
}
