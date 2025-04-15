using Newtonsoft.Json;
using GameGather.Domain.Aggregates.Users.Enums;
using GameGather.Domain.Aggregates.Users.ValueObjects;
using GameGather.Domain.Common.Primitives;
using GameGather.Domain.DomainEvents;

namespace GameGather.Domain.Aggregates.Users;

public sealed class User : AggregateRoot<UserId>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Password Password { get; set; }
    public DateTime Birthday { get; set; }
    
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? VerifiedOnUtc { get; private set; }
    public DateTime LastModifiedOnUtc { get; private set; }
    public VerificationToken VerificationToken { get; private set; }
    public ResetPasswordToken? ResetPasswordToken { get; private set; }
    public Ban? Ban { get; private set; }
    public Role Role { get; private set; }
    
    private User(UserId id) : base(id)
    {
    }

    [JsonConstructor]
    private User(
        UserId id,
        string firstName,
        string lastName,
        string email,
        string password,
        DateTime birthday) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = Password.Create(password);
        Birthday = birthday;
        CreatedOnUtc = DateTime.UtcNow;
        LastModifiedOnUtc = DateTime.UtcNow;
        VerificationToken = VerificationToken.Create();
        Role = Role.User;
    }

    public static User Create(
        string firstName,
        string lastName,
        string email,
        string password,
        DateTime birthday
    )
    {
        var user = new User(
            default,
            firstName,
            lastName,
            email,
            password,
            birthday);
        
        user.RaiseDomainEvent(new UserRegistered(
            firstName,
            lastName,
            email,
            user.VerificationToken.Value));

        return user;
    }
    
    public User Load(
        UserId id,
        string firstName,
        string lastName,
        string email,
        Password password,
        DateTime birthday,
        DateTime createdOnUtc,
        DateTime? verifiedOnUtc,
        DateTime lastModifiedOnUtc,
        Ban ban,
        Role role,
        VerificationToken verificationToken,
        ResetPasswordToken resetPasswordToken
    )
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Birthday = birthday;
        CreatedOnUtc = createdOnUtc;
        VerifiedOnUtc = verifiedOnUtc;
        LastModifiedOnUtc = lastModifiedOnUtc;
        Ban = ban;
        Role = role;
        VerificationToken = verificationToken;
        ResetPasswordToken = resetPasswordToken;
        return this;
    }
    
    
}