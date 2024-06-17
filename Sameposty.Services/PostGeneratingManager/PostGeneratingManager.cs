using Hangfire;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.Configurator;
using Sameposty.Services.PostsGenerator;
using Sameposty.Services.PostsPublishers.Orhestrator;
using Sameposty.Services.PostsPublishers.Orhestrator.Models;

namespace Sameposty.Services.PostGeneratingManager;
public class PostGeneratingManager(IPostsGenerator postsGenerator, ICommandExecutor commandExecutor, IConfigurator configurator, IPostPublishOrchestrator postPublishOrchestrator) : IPostGeneratingManager
{
    public async Task<Post> ManageGeneratingSinglePost(User user, DateTime date)
    {
        var generatePostRequest = new GeneratePostRequest()
        {
            UserId = user.Id,
            BrandName = user.BasicInformation.BrandName,
            Audience = user.BasicInformation.Audience,
            Mission = user.BasicInformation.Mission,
            ProductsAndServices = user.BasicInformation.ProductsAndServices,
            Goals = user.BasicInformation.Goals,
            Assets = user.BasicInformation.Assets,
            ShedulePublishDate = date,
        };

        var newPost = await postsGenerator.GenerateSinglePost(generatePostRequest);

        var addedPost = await commandExecutor.ExecuteCommand(new AddPostCommand() { Parameter = newPost });

        var publishRequest = new PublishPostToAllRequest()
        {
            BaseApiUrl = configurator.ApiBaseUrl,
            PostId = addedPost.Id,
        };
        DateTimeOffset localDateTimeOffset = new(addedPost.ShedulePublishDate, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time").GetUtcOffset(addedPost.ShedulePublishDate));
        DateTimeOffset utcDateTimeOffset = localDateTimeOffset.UtcDateTime;

        var jobPublishId = BackgroundJob.Schedule(() => postPublishOrchestrator.PublishPostToAll(publishRequest), utcDateTimeOffset);

        addedPost.JobPublishId = jobPublishId;

        var response = await commandExecutor.ExecuteCommand(new UpdatePostCommand() { Parameter = addedPost });

        if (user.Role != Roles.Admin)
        {
            user.ImageTokensUsed++;
            user.TextTokensUsed++;
        }

        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = user });

        return response;
    }

    public async Task<List<Post>> ManageGeneratingPosts(User user, int numberOfPostsToGenerate)
    {
        var generatePostRequest = new GeneratePostRequest()
        {
            UserId = user.Id,
            BrandName = user.BasicInformation.BrandName,
            Audience = user.BasicInformation.Audience,
            Mission = user.BasicInformation.Mission,
            ProductsAndServices = user.BasicInformation.ProductsAndServices,
            Goals = user.BasicInformation.Goals,
            Assets = user.BasicInformation.Assets,
        };

        var newPosts = await postsGenerator.GeneratePostsAsync(generatePostRequest, numberOfPostsToGenerate);

        var addedPosts = await commandExecutor.ExecuteCommand(new AddListOfPostsCommand() { Parameter = newPosts });

        foreach (var post in addedPosts)
        {
            var publishRequest = new PublishPostToAllRequest()
            {
                BaseApiUrl = configurator.ApiBaseUrl,
                PostId = post.Id,
            };

            DateTimeOffset localDateTimeOffset = new(post.ShedulePublishDate, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time").GetUtcOffset(post.ShedulePublishDate));
            DateTimeOffset utcDateTimeOffset = localDateTimeOffset.UtcDateTime;

            var jobPublishId = BackgroundJob.Schedule(() => postPublishOrchestrator.PublishPostToAll(publishRequest), utcDateTimeOffset);

            post.JobPublishId = jobPublishId;
        }

        var response = await commandExecutor.ExecuteCommand(new UpdateListOfPostsCommand() { Parameter = addedPosts });

        if (user.Role != Roles.Admin)
        {
            user.ImageTokensUsed += numberOfPostsToGenerate;
            user.TextTokensUsed += numberOfPostsToGenerate;
        }

        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = user });

        return response;
    }
}
