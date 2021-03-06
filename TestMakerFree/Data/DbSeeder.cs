﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestMakerFree.Data.Models;

namespace TestMakerFree.Data
{
    public class DbSeeder
    {
        public static void Seed(ApplicationDbContext dbContext)
        {
            if(!dbContext.Questions.Any())
            {
                CreateUsers(dbContext);
            }
            if(!dbContext.Quizzes.Any())
            {
                CreateQuizzes(dbContext);
            }
        }

        private static void CreateUsers(ApplicationDbContext dbContext)
        {
            DateTime createdDate = new DateTime(2016, 3, 1, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;
            var user_Admin = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Admin",
                Email = "maarten.deheegher@gmail.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };
            dbContext.Users.Add(user_Admin);

#if DEBUG
            var user_Ryan = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Ryan",
                Email = "ryan@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };
            var user_Solice = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Solice",
                Email = "solice@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };
            var user_Vodan = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Vodan",
                Email = "vodan@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };
            dbContext.Users.AddRange(user_Ryan, user_Solice, user_Vodan);
#endif

            dbContext.SaveChanges();
        }

        private static void CreateQuizzes(ApplicationDbContext dbContext)
        {
            DateTime createdDate = new DateTime(2016, 3, 1, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;

            var authorId = dbContext.Users.Where(u => u.UserName == "Admin").FirstOrDefault().Id;

#if DEBUG
            var num = 47;
            for(int i = 1; i <= num; ++i)
            {
                CreateSampleQuiz(dbContext, i, authorId, num - i, 3, 3, 3, createdDate.AddDays(-num));
            }
#endif
            EntityEntry<Quiz> e1 = dbContext.Quizzes.Add(new Quiz()
            {
                UserId = authorId,
                Title = "Are you more Light or Dark side of the Force?",
                Description = "Star Wars personality test",
                Text = "Choose wisely you mist, young padawan: this test will prove if your will is strong enough to adhere to the principles of the light side of the Force or if you're fated to embrace the dark side. No you want to become a true JEDI, you can't possibly miss this!",
                ViewCount = 2343,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });
            EntityEntry<Quiz> e2 = dbContext.Quizzes.Add(new Quiz()
            {
                UserId = authorId,
                Title = "GenX, GenY or GenZ",
                Description = "Find out what decade most presents you",
                Text = "Do you feel confortable in your generation? What year should you have been born in? Here's a bunch of questions that will help you to find out!",
                ViewCount = 4180,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });
            EntityEntry<Quiz> e3 = dbContext.Quizzes.Add(new Quiz()
            {
                UserId = authorId,
                Title = "Which Shinegeki No Kyojin character are you?",
                Description = "Attack On Titan personality test",
                Text = "Do you relentlessly seek revenge like Eren? Are you willing to put your like on the stake to protect your friends like Mikasa? Would uou trust your fighting skills like Levi or rely on your strategies ant tactics like Arwin? Unveil your true self with this Attac On Titan personality test!",
                ViewCount = 5203,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });

            dbContext.SaveChanges();
        }

        private static void CreateSampleQuiz(ApplicationDbContext dbContext, int num, string authorId, int viewCount, int numberOfQuestions, 
            int numberOfAnswersPerQuestion, int numberOfResults, DateTime createdDate)
        {
            var quiz = new Quiz()
            {
                UserId = authorId,
                Title = $"Quiz {num} Title",
                Description = $"This is a sample description for quiz {num}",
                Text = "This is a sample quiz created by the DbSeeder class for testing purposes. Alle the quiestions, answers & results are auto-generated as well",
                ViewCount = viewCount,
                CreatedDate = createdDate,
                LastModifiedDate = createdDate
            };
            dbContext.Quizzes.Add(quiz);
            dbContext.SaveChanges();

            for (int i = 0; i < numberOfQuestions; ++i)
            {
                var question = new Question()
                {
                    QuizId = quiz.Id,
                    Text = "This is a sample quiestion created by the DbSeeder class for testing purposes. All the childanswers are auto-generated as well",
                    CreatedDate = createdDate,
                    LastModifiedDate = createdDate
                };
                dbContext.Questions.Add(question);
                dbContext.SaveChanges();

                for(int i2 = 0; i2 < numberOfAnswersPerQuestion; ++i2)
                {
                    var e2 = dbContext.Answers.Add(new Answer()
                    {
                        QuestionId = question.Id,
                        Text = "This is a sample answer created by the DbSeeder class for testing purposes.",
                        Value = i2,
                        CreatedDate = createdDate,
                        LastModifiedDate = createdDate
                    });
                }
            }

            for(int i = 0; i < numberOfResults; ++i)
            {
                dbContext.Results.Add(new Result()
                {
                    QuizId = quiz.Id,
                    Text = "This is a sample result created by the DbSeeder class for testing purposes.",
                    MinValue = 0,
                    //max value should be equal to answers number * max answer value
                    MaxValue = numberOfAnswersPerQuestion * 2,
                    CreatedDate = createdDate,
                    LastModifiedDate = createdDate
                });
            }
            dbContext.SaveChanges();
        }
    }
}
