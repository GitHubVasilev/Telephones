using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Windows.Controls;
using Telephones.Wpf.Properties;
using Telephones.Wpf.TokenHelpers;
using Telephones.Wpf.ViewModels.User;
using System.Linq;

namespace Telephones.Wpf.ViewModels.Auth
{
    /// <summary>
    /// Модель представления для диалога окна авторизации на сервере
    /// </summary>
    public class LoginDialogViewModel : BindableBase, IDialogAware
    {
        private UserViewModel _user;

        public LoginDialogViewModel(UserViewModel user)
        {
            _user = user;
            _errorMessage = "";
            _userName = "";
        }

        private string _errorMessage;

        /// <summary>
        /// Сообщение об ошибке авторизации
        /// </summary>
        public string ErrorMessage 
        {
            get => _errorMessage ?? "";
            set => SetProperty(ref _errorMessage, value);   
        }

        private string _userName;

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string UserName
        {
            get => _userName ?? "";
            set => SetProperty(ref _userName, value); 
        }

        private DelegateCommand<string> _closeDialogCommand = null!;
        /// <summary>
        /// Команда закрытия окна
        /// </summary>
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        private DelegateCommand<PasswordBox> _loginAndCloseCommand = null!;
        /// <summary>
        /// Команда для отправки запроса авторизации/(выхода из системы) и авторизация/(выхода из системы) пользователя в системе приложения
        /// </summary>
        public DelegateCommand<PasswordBox> LoginAndCloseCommand 
        {
            get => _loginAndCloseCommand; 
            private set => SetProperty(ref _loginAndCloseCommand, value);
        }

        private string _title = "Authorization";
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
        /// <param name="parameter">Параметр вызова команды</param>
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
        /// Вызывает событие для закрытия диалогового окна
        /// </summary>
        /// <param name="dialogResult">Результат диалога</param>
        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        /// <summary>
        /// Метод проверяет может ли быть закрыто диалоговое окно
        /// </summary>
        public virtual bool CanCloseDialog()
        {
            return true;
        }

        /// <summary>
        /// Выполняет действия после закрытием диалогового окна
        /// </summary>
        public virtual void OnDialogClosed()
        {

        }

        /// <summary>
        /// Выполняет действия после открытия окна
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            LoginAndCloseCommand = new DelegateCommand<PasswordBox>((passwordBox) =>
            {
                string password = passwordBox.Password;
                try
                {
                    SecurityToken? token = GetToken(UserName!, password, $"{Resources.UrlServerAuth}/connect/token");
                    if (token is null)
                    {
                        return;
                    }
                    _user.IsAuthenticated = true;
                    _user.UserName = UserName;
                    _user.SecretToken = token;
                    IEnumerable<Claim> claims = ((JwtSecurityToken)new JwtSecurityTokenHandler().ReadToken(token.AccessToken)).Claims;
                    _user.Roles = claims.Where(m => m.Type == ClaimTypes.Role);
                    RaiseRequestClose(new DialogResult());
                }
                catch (Exception e)
                {
                    ErrorMessage = e.Message;
                }
            },
            (passwordBox) =>
            {
                bool result = !string.IsNullOrEmpty(UserName);
                return true;
            });
        }

        /// <summary>
        /// Получает токен авторизации
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="serverUrl">Адрес для авторизации пользователя на сервере</param>
        /// <returns></returns>
        private SecurityToken? GetToken(string userName, string password, string serverUrl) 
        {
            SecurityToken? token = TokenLoader.RequestToken(userName, password, serverUrl);

            if (token != null && !string.IsNullOrWhiteSpace(token.AccessToken)) 
            {
                return token;
            }

            ErrorMessage = "Ошибка авторизации";
            return null;
        }
    }
}
