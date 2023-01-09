using Telephones.API.Data.Models;
using Telephones.API.Infrastructure.Mappers.Base;
using Telephones.API.ViewModels;

namespace Telephones.API.Infrastructure.Mappers
{
    /// <summary>
    /// Конфигурации для маппинга
    /// </summary>
    public class RecordMapperConfiguration : MapperConfigurationBase
    {
        public RecordMapperConfiguration()
        {
            CreateMap<Record, RecordDTO>();
            CreateMap<Record, ShortRecordDTO>();
            CreateMap<CreateRecordDTO, Record>();
            CreateMap<Record, UpdateRecordDTO>();
            CreateMap<UpdateRecordDTO, Record>();
        }
    }
}
