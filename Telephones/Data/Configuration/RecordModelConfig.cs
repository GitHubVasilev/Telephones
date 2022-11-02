using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Telephones.Data.Models;

namespace Telephones.Data.Configuration
{
    /// <summary>
    /// Конфигурации для настройки базы данных
    /// </summary>
    public class RecordModelConfig : IEntityTypeConfiguration<Record>
    {
        public void Configure(EntityTypeBuilder<Record> builder)
        {
            builder.ToTable("Records");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(x => x.FirstName).HasMaxLength(20).IsRequired();
            builder.Property(x => x.LastName).HasMaxLength(20).IsRequired();
            builder.Property(x => x.FatherName).HasMaxLength(20).IsRequired();
            builder.Property(x => x.PhoneNumber);
            builder.Property(x => x.Address).HasMaxLength(50);
            builder.Property(x => x.Discript).HasMaxLength(100);
        }
    }
}
