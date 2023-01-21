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
    /// <summary>
    /// Модель представления для окна просмотра контакта
    /// </summary>
    public class BrowserRecordViewModel : BindableBase, IDialogAware
    {
        private readonly ITelephoneBookClientAPI _clientAPI;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;

        public BrowserRecordViewModel(ITelephoneBookClientAPI clientAPI,
            IMapper mapper,
            IDialogService dialogService)
        {
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

        private string _title = "Browse";
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
                result = ButtonResult.OK;
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
            try
            {
                WrapperResultDTO<RecordDTO> resultQuery = _clientAPI.GetRecordAsync(parameters.GetValue<int>("id"), null).Result;
                if (resultQuery.IsSuccess)
                {
                    if (resultQuery.Result is null) 
                    {
                        _dialogService.ShowDialog("NotificationDialog", new DialogParameters($"Result in responce is null"), r =>
                        {
                        });
                    }
                    RecordData = _mapper.Map<RecordDTO, RecordDataViewModel>(resultQuery.Result!);
                }
                else
                {
                    if (resultQuery.Message is null) 
                    {
                        _dialogService.ShowDialog("NotificationDialog", new DialogParameters($"Error message in responce is null"), r =>
                        {
                        });
                    }
                    _dialogService.ShowDialog("NotificationDialog", new DialogParameters($"Message={resultQuery.Message}"), r =>
                    {
                    });
                }
            }
            catch (Exception e)
            {
                _dialogService.ShowDialog("NotificationDialog", new DialogParameters($"message={e.Message}"), r =>
                {
                });
            }

        }

        private RecordDataViewModel? _recordData;
        /// <summary>
        /// Данные о записи в телефонной книге
        /// </summary>
        public RecordDataViewModel? RecordData 
        {
            get => _recordData;
            set => SetProperty(ref _recordData, value);
        }
    }
}
