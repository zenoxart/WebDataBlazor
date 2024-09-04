using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebData.Objects.PageContext.Monad
{
    public class Result<T>
    {
        public T Value { get; }
        public string Error { get; }
        public bool IsSuccess { get; }

        protected Result(T value, string error, bool isSuccess)
        {
            Value = value;
            Error = error;
            IsSuccess = isSuccess;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value, null, true);
        }

        public static Result<T> Failure(string error)
        {
            return new Result<T>(default(T), error, false);
        }

        public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> func)
        {
            if (!IsSuccess)
                return Result<TResult>.Failure(Error);

            return func(Value);
        }

        public Result<T> OnFailure(Action<string> action)
        {
            if (!IsSuccess)
            {
                action(Error);
            }
            return this;
        }
    }

    public static class ResultExtensions
    {
        // Asynchronous Bind method for Task<Result<T>>
        public static async Task<Result<TResult>> Bind<T, TResult>(
            this Task<Result<T>> task, Func<T, Task<Result<TResult>>> func)
        {
            var result = await task;
            if (!result.IsSuccess)
                return Result<TResult>.Failure(result.Error);

            return await func(result.Value);
        }

        // Synchronous Bind method for Task<Result<T>>
        public static async Task<Result<TResult>> Bind<T, TResult>(
            this Task<Result<T>> task, Func<T, Result<TResult>> func)
        {
            var result = await task;
            if (!result.IsSuccess)
                return Result<TResult>.Failure(result.Error);

            return func(result.Value);
        }

        // OnFailure for Task<Result<T>>
        public static async Task<Result<T>> OnFailure<T>(
            this Task<Result<T>> task, Action<string> action)
        {
            var result = await task;
            if (!result.IsSuccess)
            {
                action(result.Error);
            }
            return result;
        }

        // Map method for transforming the result into an IActionResult
        public static async Task<IActionResult> Map<T>(
            this Task<Result<T>> task, Func<T, IActionResult> func)
        {
            var result = await task;
            if (result.IsSuccess)
            {
                return func(result.Value);
            }

            return new BadRequestObjectResult(result.Error);
        }
    }


}
