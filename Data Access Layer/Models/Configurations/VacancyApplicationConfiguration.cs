using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data_Access_Layer.Models.Configurations
{
    public class VacancyApplicationConfiguration : IEntityTypeConfiguration<VacancyApplication>
    {
        public void Configure(EntityTypeBuilder<VacancyApplication> builder)
        {
            builder.HasKey(e => new { e.VacancyId, e.UserId });
        }
    }
}
