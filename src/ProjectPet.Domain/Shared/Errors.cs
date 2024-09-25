﻿namespace ProjectPet.Domain.Shared
{
    public static class Errors
    {
        public static class General
        {
            public static Error ValueIsEmptyOrNull<T>(T? value, string valueName)
            {
                string emptyOrNull = value == null ? "null" : "empty";

                return Error.Validation("value.is.invalid", $"Value: {valueName ?? "name"} of type {typeof(T).Name} cannot be {emptyOrNull}!");
            }

            public static Error ValueIsInvalid<T>(T value, string valueName) =>
                Error.Validation
                ("value.is.invalid", $"Value: {valueName ?? "name"} of type {typeof(T).Name} is invalid!");

            public static Error NotFound(Type type, Guid? id = null) =>
                Error.NotFound
                ("record.not.found", $"Record with id: {id} of type {type.Name} was not found!");
        }
    }
}
