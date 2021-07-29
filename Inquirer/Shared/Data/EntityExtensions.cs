using System;
using System.Linq;
using System.Security.Claims;

namespace Inquirer.Data
{
    public static class EntityExtensions
    {
        public static bool IsSingle(this Question question)
        {
            if (question.Answers is null)
            {
                throw new ArgumentNullException($"{nameof(Question)}.{nameof(Question.Answers)} (Id={question.Id})");
            }
            if (question.Answers.Count == 0)
            {
                throw new IndexOutOfRangeException($"{nameof(Question)}.{nameof(Question.Answers)}.Count == 0 (Id={question.Id})");
            }
            if (question.Answers.Count(t => t.Score > 0) == 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(Question)}.{nameof(Question.Answers)} (Id={question.Id}): no right answers");
            }
            return question.Answers.Count(t => t.Score > 0) == 1;
        }
        public static string GetId(this ClaimsPrincipal user, bool thrownIfUndefined = true)
        {           
            Claim claim = user.Claims.FirstOrDefault(t => t.Type.Equals(ClaimTypes.NameIdentifier));
            if (claim is null)
            {
                if (thrownIfUndefined)
                    throw new ArgumentNullException(ClaimTypes.NameIdentifier);
                return null;
            }
            return claim.Value;
        }
    }
}