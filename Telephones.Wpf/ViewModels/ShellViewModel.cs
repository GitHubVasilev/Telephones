using AutoMapper;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Telephones.API.Client.DTO;
using Telephones.API.Client.Interfaces;
using Telephones.Wpf.ViewModels.DataViewModel;

namespace Telephones.Wpf.ViewModels
{
    /// <summary>
    /// Shell ViewModel
    /// </summary>
    public class ShellViewModel : BindableBase
    {

        private readonly ITelephoneBookClientAPI _clientAPI;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;

        public ShellViewModel(ITelephoneBookClientAPI clientAPI,
                            IMapper mapper,
                            IDialogService dialogService)
        {
            DisplayName = "Telephone Book";
            _clientAPI = clientAPI;
            _mapper = mapper;
            _dialogService = dialogService;

            Initialize();
        }

        private ObservableCollection<ShortRecordViewModel> _telephones;

        public ObservableCollection<ShortRecordViewModel> Telephones 
        {
            get => _telephones;
            set => SetProperty(ref _telephones, value);
        }

        private string _displayName;

        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }
        
        public DelegateCommand<int?> ShowBrowserRecordCommand
        { 
            get;
            private set;
        }

        public DelegateCommand<int?> ShowUpdateRecordCommand
        {
            get;
            private set;
        }

        public DelegateCommand ShowCreateRecordCommand
        {
            get;
            private set;
        }

        public DelegateCommand<int?> DeleteRecordCommand
        {
            get;
            private set;
        }

        public DelegateCommand ReloadDataCommand 
        {
            get;
            private set;
        }

        private void ReloadData() 
        {
            try
            {
                WrapperResultDTO<IEnumerable<ShortRecordDTO>> resultQuery = _clientAPI.GetRecordsAsync().Result;
                if (resultQuery.IsSuccess)
                {
                    Telephones = new ObservableCollection<ShortRecordViewModel>
                        (_mapper
                        .Map<IEnumerable<ShortRecordDTO>, IEnumerable<ShortRecordViewModel>>(resultQuery.Result));
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

        private void Initialize()
        {
            ReloadDataCommand = new DelegateCommand(() =>
            {
                ReloadData();
            }, () => true);

            ShowBrowserRecordCommand = new DelegateCommand<int?>((id) =>
            {
                _dialogService.ShowDialog("BrowserRecord", new DialogParameters($"id={id}"), r => { });
            }, (id) => true);

            ShowUpdateRecordCommand = new DelegateCommand<int?>((id) =>
            {
                _dialogService.ShowDialog("UpdateRecord",
                    new DialogParameters($"id={id}"),
                    r =>
                    {
                        if (r.Result == ButtonResult.OK)
                        {
                            ReloadData();
                        }
                    });
            }, (id) => true);

            ShowCreateRecordCommand = new DelegateCommand(() =>
            {
                _dialogService.ShowDialog("CreateRecord",
                    new DialogParameters(),
                    r =>
                    {
                        if (r.Result == ButtonResult.OK)
                        {
                            ReloadData();
                        }
                    });
            }, () => true);

            DeleteRecordCommand = new DelegateCommand<int?>((id) =>
            {
                _dialogService.ShowDialog("YesOrNoDialog",
                    new DialogParameters($"message=Вы действительно хотите удалить запись #{id}"),
                    r =>
                    {
                        if (r.Result == ButtonResult.OK)
                        {
                            WrapperResultDTO<int> resultQuery = _clientAPI.DeleteRecordAsync(id).Result;

                            if (resultQuery.IsSuccess) 
                            {
                               ReloadData();
                            }
                            else 
                            {
                                string message = resultQuery.Message;
                                _dialogService.ShowDialog("NotificationDialog", new DialogParameters($"message={message}"), r =>
                                {
                                });
                            }
                        }
                    });
            }, (id) => true);

            ReloadData();
        }
    }
}
