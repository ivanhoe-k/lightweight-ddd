// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Examples.Domain.Errors;
using LightweightDdd.Results;

namespace LightweightDdd.Examples.Domain.Models
{
    public sealed record PersonalInfo
    {
        private PersonalInfo(string firstName, string lastName, int age, string? bio)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Bio = bio;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public int Age { get; }

        public string? Bio { get; }

        public static Result<IProfileError, PersonalInfo> Create(string firstName, string lastName, int age, string? bio)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                return Result<IProfileError>.Fail<PersonalInfo>(PersonalInfoError.MissingFirstName());
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                return Result<IProfileError>.Fail<PersonalInfo>(PersonalInfoError.MissingLastName());
            }

            if (age is < 0 or > 150)
            {
                return Result<IProfileError>.Fail<PersonalInfo>(PersonalInfoError.InvalidAge());
            }

            return Result<IProfileError>.Success(new PersonalInfo(
                firstName: firstName.Trim(),
                lastName: lastName.Trim(),
                age: age,
                bio: string.IsNullOrWhiteSpace(bio) ? null : bio.Trim()));
        }

        public string FullName => $"{FirstName} {LastName}";
    }

}
