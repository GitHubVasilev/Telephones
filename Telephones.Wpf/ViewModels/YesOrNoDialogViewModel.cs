using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace Telephones.Wpf.ViewModels
{
    /// <summary>
    /// Модель представления окна подтверждения действия
    /// </summary>
    public class YesOrNoDialogViewModel : BindableBase, IDialogAware
    {
        private DelegateCommand<string> _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        private string _message = null!;

        /// <summary>
        /// Сообщение на окне
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _title = "Notification";
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
        public event Action<IDialogResult> RequestClose = null!;

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
            Message = parameters.GetValue<string>("message");
        }
    }
}

