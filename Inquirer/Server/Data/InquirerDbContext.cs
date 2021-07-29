using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Inquirer.Data
{
    public class InquirerDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public InquirerDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) 
            : base(options, operationalStoreOptions) { }

        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionGroup> QuestionGroups { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().HasIndex(t => t.Email).IsUnique();

            modelBuilder.Entity<Questionnaire>().HasMany(t => t.Groups).WithOne(t => t.Questionnaire).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Questionnaire>().HasMany(t => t.Questions).WithOne(t => t.Questionnaire).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Questionnaire>().HasMany(t => t.Surveys).WithOne(t => t.Questionnaire).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuestionGroup>().HasMany(t => t.Questions).WithOne(t => t.Group).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Question>().HasMany(t => t.Answers).WithOne(t => t.Question).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<QuestionAnswer>().HasMany(t => t.SurveyAnswers).WithOne(t => t.QuestionAnswer).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Survey>().HasMany(t => t.Answers).WithOne(t => t.Survey).OnDelete(DeleteBehavior.NoAction);
        }
        internal void Remove<TEntity, TIdentity>(TIdentity id, Action<TIdentity> removeDependencies) 
            where TEntity : class, IEntity<TIdentity>, new()
        {
            TEntity entity = Set<TEntity>().Local.FirstOrDefault(t => t.Id.Equals(id));
            if (entity is null)
            {
                Set<TEntity>().Attach(entity = new TEntity { Id = id });
            }
            removeDependencies(id);
            Set<TEntity>().Remove(entity);
        }
        public void SwapOrdered<T>(T entity1, T entity2) where T : Entity, IOrderedEntity
        {
            int number = entity1.Number;
            entity1.Number = entity2.Number;
            entity2.Number = number;
            Set<T>().UpdateRange(entity1, entity2);
            SaveChanges();
        }
    }
}