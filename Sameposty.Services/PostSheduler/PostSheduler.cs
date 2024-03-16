using Hangfire;
using Sameposty.DataAccess.Entities;
using Sameposty.Services.PostsPublishers.Orhestrator;

namespace Sameposty.Services.PostSheduler;
public class PostSheduler(IPostPublishOrhestrator postPublisher) : IPostSheduler
{
    public void SchedulePublishPostToAll(Post post, List<SocialMediaConnection> socialMediaConnections)
    {
        var jobPublishId = BackgroundJob.Schedule(() => postPublisher.PublishPostToAll(post, socialMediaConnections), new DateTimeOffset(post.ShedulePublishDate));
    }

}
