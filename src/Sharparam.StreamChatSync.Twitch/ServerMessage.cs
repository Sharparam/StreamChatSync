namespace Sharparam.StreamChatSync.Twitch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class ServerMessage
    {
        private static readonly Regex MessageRegex =
            new Regex(@"^(?::([^ ]+) +)?([a-zA-Z\d]+) +([^:]*?)(?::(.+))?$", RegexOptions.Compiled);

        private ServerMessage(string prefix, string command, IEnumerable<string> parameters, string trailing)
        {
            Prefix = prefix;
            Command = command;
            Parameters = parameters;
            Trailing = trailing;
        }

        public string Command { get; }

        public IEnumerable<string> Parameters { get; }

        public string Prefix { get; }

        public string Trailing { get; }

        internal static ServerMessage Parse(string content)
        {
            var match = MessageRegex.Match(content);

            if (match == null || !match.Success)
                throw new FormatException("Content is not a valid IRC message");

            var prefix = match.Groups[1]?.Value?.Trim();
            var command = match.Groups[2]?.Value?.Trim();
            var parameters =
                match.Groups[3]
                    ?.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList() ?? new List<string>(0);
            var trailing = match.Groups[4]?.Value?.Trim();

            return new ServerMessage(prefix, command, parameters.AsReadOnly(), trailing);
        }
    }
}
