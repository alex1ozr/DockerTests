﻿using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Results;
using ValueOf;

namespace DockerTestsSample.Api.Domain.Common;

public class Email : ValueOf<string, Email>
{
    private static readonly Regex EmailRegex =
        new("^[\\w!#$%&’*+/=?`{|}~^-]+(?:\\.[\\w!#$%&’*+/=?`{|}~^-]+)*@(?:[a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

    protected override void Validate()
    {
        if (!EmailRegex.IsMatch(Value))
        {
            var message = $"{Value} is not a valid email address";
            throw new ValidationException(message, new []
            {
                new ValidationFailure(nameof(Email), message)
            });
        }
    }
}
