using System.Diagnostics.CodeAnalysis;

namespace blog_api.UnitTests;

public class PostDtoComparer : IEqualityComparer<PostDto>
{
    public bool Equals(PostDto? x, PostDto? y)
    {
        if (x is null || y is null)
            return false;
        if (ReferenceEquals(x, y))
            return true;

        return x.Id == y.Id &&
                x.Title == y.Title &&
                x.Body == y.Body &&
                x.UserFullName == y.UserFullName &&
                x.UserId == y.UserId;
    }

    public int GetHashCode([DisallowNull] PostDto obj)
    {
        return obj.Id.GetHashCode();
    }
}
