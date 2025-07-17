using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLearn.Domain.Entities;

namespace SmartLearn.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);
            
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(u => u.PasswordHash)
            .IsRequired();
            
        builder.HasIndex(u => u.Email)
            .IsUnique();
            
        builder.HasMany(u => u.CreatedCourses)
            .WithOne(c => c.Instructor)
            .HasForeignKey(c => c.InstructorId);
            
        builder.HasMany(u => u.Enrollments)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);
    }
}