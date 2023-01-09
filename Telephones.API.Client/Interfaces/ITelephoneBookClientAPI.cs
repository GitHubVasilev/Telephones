using Telephones.API.Client.DTO;

namespace Telephones.API.Client.Interfaces
{
    public interface ITelephoneBookClientAPI
    {
        public Task<WrapperResultDTO<IEnumerable<ShortRecordDTO>>> GetRecordsAsync();
        public Task<WrapperResultDTO<RecordDTO>> GetRecordAsync(int? id);
        public Task<WrapperResultDTO<int>> CreateRecordAsync(CreateRecordDTO viewModel);
        public Task<WrapperResultDTO<int>> UpdateRecordAsync(UpdateRecordDTO viewModel);
        public Task<WrapperResultDTO<int>> DeleteRecordAsync(int? id);
    }
}
