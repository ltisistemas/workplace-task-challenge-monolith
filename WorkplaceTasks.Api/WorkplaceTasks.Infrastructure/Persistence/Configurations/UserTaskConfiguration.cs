
using Microsoft.EntityFrameworkCore;
using WorkplaceTasks.Domain.Entities;

namespace WorkplaceTasks.Infrastructure.Persistence.Configurations;

public class UserTaskConfiguration : IEntityTypeConfiguration<UserTask>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserTask> builder)
    {
        builder.ToTable("users");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.Username).IsRequired().HasMaxLength(150);
        builder.Property(t => t.UserEmail).IsRequired().HasMaxLength(250);
        builder.Property(t => t.UserPassword).IsRequired();
        builder.Property(t => t.CreatedAt).IsRequired();
        builder.Property(t => t.UpdatedAt).IsRequired();
        builder.Property(t => t.DeletedAt).IsRequired(false);
    }
}