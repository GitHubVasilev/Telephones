using AutoMapper;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using Telephones.API.Client.DTO;
using Telephones.API.Client.Interfaces;
using Telephones.Wpf.ViewModels.DataViewModel;

namespace Telephones.Wpf.ViewModels
{
    public class BrowserRecordViewModel : BindableBase, IDialogAware
    {
        private readonly ITelephoneBookClientAPI _clientAPI;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;

        public BrowserRecordViewModel(ITelephoneBookClientAPI clientAPI, IMapper mapper, IDialogService dialogService)
        {
            _clientAPI = clientAPI;
            _mapper = mapper;
            _dialogService = dialogService;
        }

        private DelegateCommand<string>? _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        private string _title = "Browse";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public event Action<IDialogResult>? RequestClose;

        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
                result = ButtonResult.OK;
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {

        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                WrapperResultDTO<RecordDTO> resultQuery = _clientAPI.GetRecordAsync(parameters.GetValue<int>("id")).Result;
                if (resultQuery.IsSuccess)
                {
                    RecordData = _mapper.Map<RecordDTO, RecordDataViewModel>(resultQuery.Result);
                }
                else
                {
                    string message = resultQuery.Message;
                    _dialogService.ShowDialog("NotificationDialog", new DialogParameters($"message={message}"), r =>
                    {
                    });
                }
            }
            catch (Exception e)
            {
                string message = e.Message;
                _dialogService.ShowDialog("NotificationDialog", new DialogParameters($"message={message}"), r =>
                {
                });
            }

        }

        private RecordDataViewModel? _recordData;

        public RecordDataViewModel? RecordData 
        {
            get => _recordData;
            set => SetProperty(ref _recordData, value);
        }
    }
}
