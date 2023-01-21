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
using Telephones.Wpf.ViewModels.User;

namespace Telephones.Wpf.ViewModels
{
    /// <summary>
    /// Модель представления главного окна
    /// </summary>
    public class ShellViewModel : BindableBase
    {

        private readonly ITelephoneBookClientAPI _clientAPI;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;

        public ShellViewModel(ITelephoneBookClientAPI clientAPI,
                            IMapper mapper,
                            IDialogService dialogService,
                            UserViewModel user)
        {
            _user = user;
            DisplayName = "Telephones Book";
            _clientAPI = clientAPI;
            _mapper = mapper;
            _dialogService = dialogService;

            Initialize();
        }

        private UserViewModel _user;
        /// <summary>
        /// Пользователь
        /// </summary>
        public UserViewModel User 
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private ObservableCollection<ShortRecordViewModel> _telephones;

        /// <summary>
        /// Список записей в телефонной книге
        /// </summary>
        public ObservableCollection<ShortRecordViewModel> Telephones 
        {
            get => _telephones;
            set => SetProperty(ref _telephones, value);
        }

        private string _displayName;

        /// <summary>
        /// Заголовок окна
        /// </summary>
        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }
        
        /// <summary>
        /// Команда вызова окна предоставления данных записи в телефонной книге
        /// </summary>
        public DelegateCommand<int?> ShowBrowserRecordCommand
        { 
            get;
            private set;
        }

        /// <summary>
        /// Команда вызова кона обновления данных записи в телефонной книге
        /// </summary>
        public DelegateCommand<int?> ShowUpdateRecordCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Команда вызова окна создания новой записи в телефонной книге
        /// </summary>
        public DelegateCommand ShowCreateRecordCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Команда для удаления записи в телефонной книге
        /// </summary>
        public DelegateCommand<int?> DeleteRecordCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Команда обновления данных записей в телефонной книге на окне
        /// </summary>
        public DelegateCommand ReloadDataCommand 
        {
            get;
            private set;
        }

        /// <summary>
        /// Обновляет данные записей в телефонной книге на окне
        /// </summary>
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

        /// <summary>
        /// Инициализирует окно первоначальными данными
        /// </summary>
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
            }).ObservesCanExecute(() => User.CanUpdateRecord);

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
            }).ObservesCanExecute(() => User.CanCreateRecord);

            DeleteRecordCommand = new DelegateCommand<int?>((id) =>
            {
                _dialogService.ShowDialog("YesOrNoDialog",
                    new DialogParameters($"message=Вы действительно хотите удалить запись #{id}"),
                    r =>
                    {
                        if (r.Result == ButtonResult.OK)
                        {
                            WrapperResultDTO<int> resultQuery = _clientAPI.DeleteRecordAsync(id, _user.SecretToken.AccessToken).Result;

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
            }).ObservesCanExecute(() => User.CanDeleteRecord);

            ReloadData();
        }
    }
}
