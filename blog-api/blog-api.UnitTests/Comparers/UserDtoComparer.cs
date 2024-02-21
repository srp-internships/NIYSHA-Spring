using System.Diagnostics.CodeAnalysis;

namespace blog_api.UnitTests;

public class UserDtoComparer : IEqualityComparer<UserDto>
{
    public bool Equals(UserDto? x, UserDto? y)
    {
        if (x is null || y is null)
            return false;
        if (ReferenceEquals(x, y))
            return true;

        return x.Id == y.Id &&
                x.FirstName == y.FirstName &&
                x.LastName == y.LastName &&
                x.Username == y.Username &&
                x.CompanyName == y.CompanyName &&
                x.Email == y.Email &&
                x.Address == y.Address &&
                x.Phone == y.Phone;
        ;
    }

    public int GetHashCode([DisallowNull] UserDto obj)
    {
        return obj.Id.GetHashCode();
    }
}
