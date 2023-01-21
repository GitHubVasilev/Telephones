using AutoMapper;
using Telephones.API.Client.DTO;
using Telephones.Wpf.ViewModels.DataViewModel;

namespace Telephones.Wpf.Infrastructure.Mapper
{
    /// <summary>
    /// Конфигурация для automapper
    /// </summary>
    public class RecordMapperConfiguration : Profile
    {
        public RecordMapperConfiguration()
        {
            CreateMap<RecordDTO, RecordDataViewModel>();
            CreateMap<ShortRecordDTO, ShortRecordViewModel>();
            CreateMap<CreateRecordDataViewModel, CreateRecordDTO>();
            CreateMap<RecordDTO, UpdateRecordDataViewModel>();
            CreateMap<UpdateRecordDataViewModel, UpdateRecordDTO>();
        }
    }
}
