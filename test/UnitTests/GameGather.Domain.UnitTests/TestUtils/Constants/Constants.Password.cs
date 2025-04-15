namespace GameGather.Domain.UnitTests.TestUtils.Constants;

public static partial class Constants
{
    public static class Password
    {
        public const string Value = "P@ssw0rd";
        public static readonly DateTime ExpiresOnUtc = DateTime.UtcNow.AddDays(30);
        public static readonly DateTime LastModifiedOnUtc = DateTime.UtcNow;
    }
}