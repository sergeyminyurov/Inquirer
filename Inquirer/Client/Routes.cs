using System;

namespace Inquirer
{
    public static class Routes
    {
        public static class Questionnaire
        {
            public const string List = "/questionnaire/list";
            public const string Form = "/questionnaire/form/{Id:int}";
        }
        public static class Survey
        {
            public const string List = "/survey/list";
            public const string Conducting = "/survey/conducting/{Id:int}";
        }
        public static string Parameterization(string route, params object[] values)
        {
            foreach (var value in values)
            {
                int startPos = route.IndexOf('{');
                int endPos = route.IndexOf('}');
                if (startPos < 0 || endPos < startPos)
                {
                    throw new InvalidOperationException($"{nameof(Routes)}.{nameof(Parameterization)}: {route}");
                }
                route = route[0..startPos] + value.ToString() + route[(endPos + 1)..];
            }
            return route;
        }
    }
}