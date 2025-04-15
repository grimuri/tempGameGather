using Password_ = GameGather.Domain.Aggregates.Users.ValueObjects.Password;

namespace GameGather.Domain.UnitTests.TestUtils.Constants;

public static partial class Constants
{
    public static class User
    {
        public const string FirstName = "John";
        public const string LastName = "Doe";
        public const string Email = "john.doe@gmail.com";

        public static readonly Password_ Password =
            Password_.Create(Constants.Password.Value);
        public static readonly DateTime Birthday = new DateTime(1990, 1, 1);
    }
}