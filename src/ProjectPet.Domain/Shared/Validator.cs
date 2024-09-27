using CSharpFunctionalExtensions;

namespace ProjectPet.Domain.Shared
{
    public class Validator<T>
    {
        private Func<Result<T, Error>>? NotEmptyOrNullCheck = null;
        private Func<Result<T, Error>>? MaxLenCheck = null;

        private T _item = default!;
        private string _itemName = null!;
        private Validator(){}
        public static Validator<T> Create<T>()
        {
            return new Validator<T>();
        }

        public Result<T, Error> Check(T item, string valName)
        {
            _itemName = valName;
            _item = item;
            Result<T, Error> result = _item;

            foreach (var fun in ChecksQueue())
            {
                if (fun is null)
                    continue;

                result = fun.Invoke();

                if (result.IsFailure)
                    return result.Error;
            }

            return result.Value;
        }

        public Validator<T> SetNotEmptyOrNull()
        {
            NotEmptyOrNullCheck ??= CheckIfNullOrEmpty;
            return this;
        }

        public Validator<T> SetMaxLen(int maxLenInclusive) // ef config fluent api seems to be inclusive
        {
            MaxLenCheck = () => 
            {
                if (_item is string itemString && itemString.Length > maxLenInclusive)
                {
                    return Error.Validation(
                        "value.is.invalid",
                        $"Value {_itemName} of type {typeof(T).Name} exceeds maximum length ({maxLenInclusive})!");
                }
                return _item;
            };

            return this;
        }

        private Result<T, Error> CheckIfNullOrEmpty()
        {
            if (_item == null)
                return ValueIsEmptyOrNull(_item, _itemName);

            if (_item is string itemString && String.IsNullOrWhiteSpace(itemString))
                return ValueIsEmptyOrNull(_item, _itemName);

            if (_item is Guid itemGuid && itemGuid.Equals(Guid.Empty))
                return ValueIsEmptyOrNull(_item, _itemName);

            return _item;
        }

        private Error ValueIsEmptyOrNull<T>(T? value, string valueName)
        {
            return Errors.General.ValueIsEmptyOrNull(_item, _itemName);
        }

        private IEnumerable<Func<Result<T, Error>>?> ChecksQueue()
        {
            yield return NotEmptyOrNullCheck;
            yield return MaxLenCheck;
        }
    }

    public static class Validator
    {
        public static Validator<T> NewForType<T>()
        {
            return Validator<T>.Create<T>();
        }

        public static Validator<string> ValidatorString(int maxLen = Constants.STRING_LEN_SMALL)
        {
            return Validator.NewForType<string>()
                .SetNotEmptyOrNull()
                .SetMaxLen(maxLen);
        }

        public static Validator<T> ValidatorNull<T>()
        {
            return Validator.NewForType<T>()
                .SetNotEmptyOrNull();
        }
    }




}
