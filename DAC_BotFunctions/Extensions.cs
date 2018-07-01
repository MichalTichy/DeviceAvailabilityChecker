﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Connector;

namespace DAC_BotFunctions
{
    public static class Extensions
    {
        public static IEnumerable<string> GetMessageParts(this IMessageActivity message)
        {
            return message
                .Text
                .Replace("<at>PcStatusBot</at>", "")
                .Trim()
                .Split(new[] {' ', (char) 160}, StringSplitOptions.RemoveEmptyEntries);   
        }

        public static string FixNewLines(this string text)
        {
            return text.Replace("\n", "\n\n");
        }
        public static string GetCorrectVerb<T>(this IEnumerable<T> collection,bool usePastTense=false)
        {
            if (collection.Count() == 1)
                return usePastTense ? "was" : "is";

            return usePastTense ? "were" : "are";
        }
    }
}
