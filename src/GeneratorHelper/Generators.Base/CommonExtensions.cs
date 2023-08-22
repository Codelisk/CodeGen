namespace Generators.Base
{
    public static class CommonExtensions
    {
        public static string Pluralize(this string singular)
        {
            if (string.IsNullOrEmpty(singular))
                return singular;

            // Handle some general pluralization rules
            if (singular.EndsWith("s") || singular.EndsWith("x") || singular.EndsWith("z") ||
                singular.EndsWith("ch") || singular.EndsWith("sh"))
            {
                return singular + "es";
            }

            if (singular.EndsWith("y") && !IsVowel(singular[singular.Length - 2]))
            {
                return singular.Remove(singular.Length - 1) + "ies";
            }

            return singular + "s";

        }
        // Helper method to check if a character is a vowel
        private static bool IsVowel(char ch)
        {
            return "AEIOUaeiou".IndexOf(ch) != -1;
        }
    }
}
