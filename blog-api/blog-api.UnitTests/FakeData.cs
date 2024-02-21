namespace blog_api.UnitTests;

public static class FakeData
{
    public static List<User> GetUsers()
    {
        var users = new List<User>()
        {
            new User()
            {
                Id = 1,
                Address = "City Street1",
                CompanyName = "Company Name1",
                FirstName = "FirstName1",
                LastName = "LastName1",
                Email = "Email1",
                Phone = "Phone1",
                Username = "Username1"
            },
            new User()
            {
                Id = 2,
                Address = "City Street2",
                CompanyName = "Company Name2",
                FirstName = "FirstName2",
                LastName = "LastName2",
                Email = "Email2",
                Phone = "Phone2",
                Username = "Username2"
            }
        };

        return users;
    }

    public static List<Post> GetPosts()
    {
        var posts = new List<Post>();

        for (int i = 0; i < 10; i++)
        {
            posts.Add(new Post()
            {
                UserId = 1,
                Title = "Title",
                Body = "Body"
            });

            posts.Add(new Post()
            {
                UserId = 2,
                Title = "Title",
                Body = "Body"
            });
        }

        return posts;
    }
}
