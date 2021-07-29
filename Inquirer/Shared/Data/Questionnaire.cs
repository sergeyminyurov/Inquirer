using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inquirer.Data
{
    public sealed class Questionnaire : Entity<Questionnaire>
    {
        public Questionnaire()
        {
            Groups = new List<QuestionGroup>();
            Questions = new List<Question>();
            Surveys = new List<Survey>();
        }

        [Required(ErrorMessage = "Введите наименование опросника")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1, 100, ErrorMessage = "Введите значение от 1 до 100 для баллов")]
        public decimal Score { get; set; }
        public DateTime CreateTime { get; set; }

        [Required(ErrorMessage = "Выберите автора опросника")]
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public IList<QuestionGroup> Groups { get; set; }
        public IList<Question> Questions { get; set; }
        public IList<Survey> Surveys { get; set; }

        public override Questionnaire ForDatabase() => new()
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Score = Score,
            CreateTime = CreateTime,
            AuthorId = AuthorId,
        };
   }
}