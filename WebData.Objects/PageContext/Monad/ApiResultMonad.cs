using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebData.Objects.PageContext.Monad
{
    /// <summary>
    /// Definiert ein Monad-Objekt welches ermöglicht generisch serielle Wrapper-Funktionen auszuführen
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Gibt den Generischen Wert an
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gibt die Fehlermeldung an
        /// </summary>
        public string Error { get; }
        /// <summary>
        /// Gibt an ob die Monad-Funktion erfolgreich durchgelaufen ist
        /// </summary>
        public bool IsSuccess { get; }

        protected Result(T value, string error, bool isSuccess)
        {
            Value = value;
            Error = error;
            IsSuccess = isSuccess;
        }

        /// <summary>
        /// Definiert die Monad-Funktionalität um sie erfolgreich abzuschließen
        /// </summary>
        public static Result<T> Success(T value) 
            => new Result<T>(value, null, true);


        /// <summary>
        /// Definiert die Monad-Funktionalität um einen Fehler aufzuzeichen
        /// </summary>
        public static Result<T> Failure(string error) 
            => new Result<T>(default(T), error, false);


        /// <summary>
        /// Definiert die Monad-Funktionalität um Seriel eine weitere Funktion anzuhängen
        /// </summary>
        public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> func) 
            => !IsSuccess ? Result<TResult>.Failure(Error) : func(Value);

        /// <summary>
        /// Definiert die Monad-Funktionalität um einen Fehler zu fangen
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Result<T> OnFailure(Action<string> action)
        {
            if (!IsSuccess)
            {
                action(Error);
            }
            return this;
        }
    }

    /// <summary>
    /// Definiert eine Erweiterung des Monad-Objekts
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Erweiterung für das Monad-Objekt um Bindung Asyncron zuzulassen
        /// </summary>
        public static async Task<Result<TResult>> Bind<T, TResult>(
            this Task<Result<T>> task, Func<T, Task<Result<TResult>>> func)
        {
            var result = await task;
            return !result.IsSuccess 
                ? Result<TResult>.Failure(result.Error) 
                : await func(result.Value);
        }

        /// <summary>
        /// Erweiterung für das Monad-Objekt um Bindung Syncron zuzulassen
        /// </summary>
        public static async Task<Result<TResult>> Bind<T, TResult>(
            this Task<Result<T>> task, Func<T, Result<TResult>> func)
        {
            var result = await task;
            return !result.IsSuccess 
                ? Result<TResult>.Failure(result.Error) 
                : func(result.Value);
        }

        /// <summary>
        /// Erweiterung für das Monad-Objekt um Fehler zu fangen
        /// </summary>
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

        /// <summary>
        /// Erweiterung für das Monad-Objekt um das Ergebnis in ein IActionResult zu mappen
        /// </summary>
        public static async Task<IActionResult> Map<T>(
            this Task<Result<T>> task, Func<T, IActionResult> func)
        {
            var result = await task;
            return result.IsSuccess 
                ? func(result.Value) 
                : new BadRequestObjectResult(result.Error);
        }
    }


}
