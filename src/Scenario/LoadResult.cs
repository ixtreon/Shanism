using System;
using System.Collections.Generic;

namespace Shanism.ScenarioLib
{
    /// <summary>
    /// A result from the loading of a scenario.
    /// </summary>
    public class LoadResult<T>
        where T : class
    {

        /// <summary>
        /// Gets the object that was (possibly) loaded, or null if loading failed.
        /// See <see cref="IsSuccessful"/>.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Gets whether the loading operation was completed successfully.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets the errors from the loading operation, or null if loading succeeded.
        /// See <see cref="IsSuccessful"/>.
        /// </summary>
        public IEnumerable<string> Errors { get; set; }


        LoadResult(T value, bool success, IEnumerable<string> errors)
        {
            Value = value;
            IsSuccessful = success;
            Errors = errors;
        }

        public static LoadResult<T> Success(T value)
            => new LoadResult<T>(value ?? throw new ArgumentNullException(nameof(value)), true, null);

        public static LoadResult<T> Fail(IEnumerable<string> errors)
            => new LoadResult<T>(null, false, errors);

        public static LoadResult<T> Fail(params string[] errors)
            => new LoadResult<T>(null, false, errors);
    }
}