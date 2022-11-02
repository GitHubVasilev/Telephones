using Telephones.Data.Models;
using Telephones.Infrastructure.Mappers.Base;
using Telephones.ViewModels;

namespace Telephones.Infrastructure.Mappers
{
    /// <summary>
    /// Конфигурации для маппинга
    /// </summary>
    public class RecordMapperConfiguration : MapperConfigurationBase
    {
        public RecordMapperConfiguration()
        {
            CreateMap<Record, RecordViewModel>();
            CreateMap<Record, ShortRecordViewModel>();
        }
    }
}
