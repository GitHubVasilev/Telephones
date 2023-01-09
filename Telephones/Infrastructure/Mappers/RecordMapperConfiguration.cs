using Telephones.API.Client.DTO;
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
            CreateMap<RecordDTO, RecordViewModel>();
            CreateMap<ShortRecordDTO, ShortRecordViewModel>();
            CreateMap<CreateRecordViewModel, CreateRecordDTO>();
            CreateMap<RecordDTO, UpdateRecordViewModel>();
            CreateMap<UpdateRecordViewModel, UpdateRecordDTO>();
        }
    }
}
