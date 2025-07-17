using Microsoft.EntityFrameworkCore;
using SmartLearn.Domain.Entities;

namespace SmartLearn.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Course> Courses { get; }
    DbSet<Module> Modules { get; }
    DbSet<Lesson> Lessons { get; }
    DbSet<Quiz> Quizzes { get; }
    DbSet<QuizQuestion> QuizQuestions { get; }
    DbSet<QuizAttempt> QuizAttempts { get; }
    DbSet<QuizAnswer> QuizAnswers { get; }
    DbSet<Enrollment> Enrollments { get; }
    DbSet<LessonProgress> LessonProgress { get; }
    DbSet<Category> Categories { get; }
    DbSet<Review> Reviews { get; }
    DbSet<Certificate> Certificates { get; }
    DbSet<Payment> Payments { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}