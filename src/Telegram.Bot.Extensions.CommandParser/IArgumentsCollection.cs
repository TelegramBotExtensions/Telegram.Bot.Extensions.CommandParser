using System;
using System.Collections.Generic;

namespace Telegram.Bot.Extensions.CommandParser
{
    public interface IArgumentsCollection : IReadOnlyDictionary<string, string>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="format"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        TValue Get<TValue>(string key, string? format = default);
    }
}
