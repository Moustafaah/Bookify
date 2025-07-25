using Domain.Bookings.ValueObjects;
using Domain.Shared.Errors;

using LanguageExt.Common;

namespace Domain.Shared;

public static class ValidationErrors
{
    public static class Domain
    {
        public static class String
        {
            public static BadRequestError IsNullOrEmpty => BadRequestError.New($"Value is required");
            public static BadRequestError IsNullOrWhiteSpace => BadRequestError.New($"Value is required");
            public static BadRequestError MaxLength(string repr, int maxLength) => BadRequestError.New($"Maxlength is {maxLength} characters, but got '{repr.Length}'.");

            public static BadRequestError MinLength(string repr, int minLength) => BadRequestError.New($"Minlength is {minLength} characters, but got '{repr.Length}'.");
        }
        public static class Enum
        {
            public static BadRequestError ParseFailure<T>(string repr) => BadRequestError.New($"Value {repr} Could not be parsed into one of the following valid values, '{System.Enum.GetValues(typeof(T))}'.");
        }

        public static class Amenity
        {

            public static class State
            {
                public static BadRequestError Invalid(int value) => BadRequestError.New($"Value {value} of amenity is invalid.");
                public static BadRequestError Invalid(string value) => BadRequestError.New($"Value {value} of amenity is invalid.");
                public static BadRequestError AlreadyExists(string repr) => BadRequestError.New($"Amenity with state '{repr}' already exists.");
            }

            public static class Name
            {
                public static BadRequestError Invalid(string repr) => BadRequestError.New($"Amenity with value '{repr}' was not found");

                public static BadRequestError Invalid(int value) => BadRequestError.New($"Amenity with value '{value}' was not found");

                public static BadRequestError AlreadyExists(string repr) => BadRequestError.New($"Amenity with amenityName '{repr}' already exists.");

            }


            public static class Percentage
            {
                public static BadRequestError Invalid(string message)
                {
                    return BadRequestError.New(message);
                }
            }

            public static class Cost
            {
                public static BadRequestError Invalid(string message)
                {
                    return BadRequestError.New(message);
                }
            }
        }

        public static class Money
        {
            public static class Cent
            {
                public static BadRequestError Invalid(int cent) => BadRequestError.New($"Value over 100 is invalid, got '{cent}'.");
            }
        }
        public static class Currency
        {
            public static BadRequestError Invalid(string code, string currencyName) =>
                throw new NotImplementedException();

            public static BadRequestError AlreadyExists(string repr, string currencyName) =>
                throw new NotImplementedException();
        }

        public static class Users
        {
            public static class Email
            {
                public static BadRequestError Invalid(string repr) =>
                    BadRequestError.New($"Value '{repr}' is invalid");
            }
        }

        public static class Date
        {
            public static BadRequestError ShouldNotBeInPast(string message) => BadRequestError.New(message);

            public static BadRequestError AtLeastOneDayDiff(string message) => BadRequestError.New(message);

            public static BadRequestError IsOverlappingStart(DateRange dateRange, DateRange check) => BadRequestError.New($"Start date {check.FromDate.ToShortDateString()} is overlapping with range : from {dateRange.FromDate.ToShortDateString()} to {dateRange.ToDate.ToShortDateString()}");

            public static BadRequestError IsOverlappingEnd(DateRange dateRange, DateRange check) => BadRequestError.New($"End date {check.ToDate.ToShortDateString()} is overlapping with range : from {dateRange.FromDate.ToShortDateString()} to {dateRange.ToDate.ToShortDateString()}");
        }

        public static class Bookings
        {
            public static class Status
            {
                public static BadRequestError Invalid(string message) => BadRequestError.New(message);

                public static BadRequestError InvalidVariant(string message) => BadRequestError.New(message);

                public static Error InvalidStatusChange(string message) => BadRequestError.New(message);
            }
        }
    }
}