using FluentValidation;
using GameGather.Application.Configurations;
using Microsoft.Extensions.Configuration;

namespace GameGather.Application.Features.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(r => r.FirstName)
            .NotEmpty()
            .WithMessage("Firstname is required")
            .MaximumLength(100)
            .WithMessage("Firstname is too long");

        RuleFor(r => r.LastName)
            .NotEmpty()
            .WithMessage("Lastname is required")
            .MaximumLength(100)
            .WithMessage("Lastname is too long");

        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be valid");

        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(8)
            .WithMessage("Minimum length of password is 8 characters");

        RuleFor(r => r.ConfirmPassword)
            .Equal(r => r.Password)
            .WithMessage("Password and ConfirmPassword must be the same");

        RuleFor(r => r.Birthday)
            .NotEmpty()
            .WithMessage("Birthday is required");
    }
}