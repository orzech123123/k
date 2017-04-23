using System.Collections.Generic;
using System.Linq;

namespace Kompostowanie
{
    public static class Notifier
    {
        private static readonly IList<string> errors = new List<string>();

        public static void AddError(string message)
        {
            errors.Add(message);
        }

        public static bool HasErrors()
        {
            return errors.Any();
        }

        public static IList<string> GetAndRemoveErrors()
        {
            var tmp = errors.ToList();
            errors.Clear();
            return tmp;
        }
    }
}