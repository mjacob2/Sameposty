﻿using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsGenerator;
public interface IPostsGenerator
{
    Task<List<Post>> GeneratePostsAsync(GeneratePostRequest originalRequest, int numberOfPostsToGenerate);
    Task<Post> GenerateSinglePost(GeneratePostRequest request);

    List<Post> GenerateStubbedPosts(GeneratePostRequest request);
}
