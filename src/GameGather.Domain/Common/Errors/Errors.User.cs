using ErrorOr;

namespace GameGather.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail = Error.Conflict(
            code: "User.DuplicateEmail",
            description: "User with that email exists");

        public static Error NotFound = Error.NotFound(
            code: "User.NotFound",
            description: "User not found");

        public static Error InvalidCredentials = Error.Conflict(
            code: "User.InvalidCredentials",
            description: "Invalid credentials");
    }
}