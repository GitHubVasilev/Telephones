using AutoMapper;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using Telephones.API.Client.DTO;
using Telephones.API.Client.Interfaces;
using Telephones.Wpf.ViewModels.DataViewModel;
using Telephones.Wpf.ViewModels.User;

namespace Telephones.Wpf.ViewModels
{
    /// <summary>
    /// Модель представления для создания новой записи в телефонной книге
    /// </summary>
    internal class CreateRecordViewModel : BindableBase, IDialogAware
    {
        private readonly ITelephoneBookClientAPI _clientAPI;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;
        private UserViewModel _user;

        public CreateRecordViewModel(ITelephoneBookClientAPI clientAPI, IMapper mapper, IDialogService dialogService, UserViewModel user)
        {
            _user = user;
            _clientAPI = clientAPI;
            _mapper = mapper;
            _dialogService = dialogService;
        }

        private DelegateCommand<string>? _closeDialogCommand;

        /// <summary>
        /// Команда для закрытия окна
        /// </summary>
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        private string _title = "Создание новой записи";

        /// <summary>
        /// Заголовок окна
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>
        /// Событие закрытия окна
        /// </summary>
        public event Action<IDialogResult>? RequestClose;

        /// <summary>
        /// Метод выполняется перед закрытием окна. Вызывает событие о закрытии
        /// </summary>
        /// <param name="parameter">Параметр закрытия окна</param>
        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
            {
                CreateRecord();
                result = ButtonResult.OK;
            }
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        /// <summary>
        /// Вызывает событие закрытия окна
        /// </summary>
        /// <param name="dialogResult">Результат работы диалога</param>
        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        /// <summary>
        /// Проверяет может ли быть закрыто окно
        /// </summary>
        /// <returns></returns>
        public virtual bool CanCloseDialog()
        {
            return true;
        }

        /// <summary>
        /// Метод выполняется после закрытия окна
        /// </summary>
        public virtual void OnDialogClosed()
        {

        }

        /// <summary>
        /// Метод выполняется после открытия окна
        /// </summary>
        /// <param name="parameters">Параметр для открытия окна</param>
        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            RecordData = new CreateRecordDataViewModel();

        }

        private CreateRecordDataViewModel? _recordData;

        /// <summary>
        /// Данные для создания записи в телефонной книге
        /// </summary>
        public CreateRecordDataViewModel? RecordData
        {
            get => _recordData;
            set => SetProperty(ref _recordData, value);
        }

        /// <summary>
        /// Метод отпраляет запрос на добавления новой записи в телефонную книгу
        /// </summary>
        private void CreateRecord()
        {
            try
            {
                WrapperResultDTO<int> resultQuery = _clientAPI
                    .CreateRecordAsync(_mapper.Map<CreateRecordDataViewModel, CreateRecordDTO>(RecordData!), _user.SecretToken.AccessToken)
                    .Result;
                if (resultQuery.IsSuccess)
                {
                    _dialogService.ShowDialog("NotificationDialog", new DialogParameters($"message=Данные успешно добавлены!"), r =>
                    {
                    });
                }
                else
                {
                    string message = resultQuery.Message!;
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
    }

}
