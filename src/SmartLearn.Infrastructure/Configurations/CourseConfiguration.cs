using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLearn.Domain.Entities;

namespace SmartLearn.Infrastructure.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(c => c.Description)
            .HasMaxLength(2000);
            
        builder.Property(c => c.ShortDescription)
            .HasMaxLength(500);
            
        builder.OwnsOne(c => c.Price, price =>
        {
            price.Property(p => p.Amount)
                .HasColumnName("PriceAmount")
                .HasColumnType("decimal(18,2)");
                
            price.Property(p => p.Currency)
                .HasColumnName("PriceCurrency")
                .HasMaxLength(3);
        });
        
        builder.Property(c => c.Tags)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
            );
            
        builder.Property(c => c.LearningObjectives)
            .HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries)
            );
            
        builder.HasMany(c => c.Modules)
            .WithOne(m => m.Course)
            .HasForeignKey(m => m.CourseId);
            
        builder.HasMany(c => c.Enrollments)
            .WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId);
    }
}