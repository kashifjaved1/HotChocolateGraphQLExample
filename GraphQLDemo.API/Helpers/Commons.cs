using System.Collections.Generic;

namespace GraphQLDemo.API.Helpers
{
    public static class Commons
    {
        public static string CapsFirstLetter(string input)
        {
            return char.ToUpper(input[0]) + input.Substring(1);
        }

        public static List<string> CapsFirstLetter(List<string> input)
        {
            List<string> result = new();
            foreach (var inputItem in input)
            {
                result.Add(CapsFirstLetter(inputItem));
            }

            return result;
        }
    }
}
