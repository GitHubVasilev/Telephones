using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Security.Claims;
using Telephones.Wpf.ViewModels.User;

namespace Telephones.Wpf.ViewModels.Auth
{
    /// <summary>
    /// Модель представления для элемента окна с данными авторизации пользователя
    /// </summary>
    public class LoginPanelViewModel : BindableBase
    {
        private UserViewModel _userViewModel;
        private readonly IDialogService _dialogService;

        public LoginPanelViewModel(UserViewModel user, IDialogService dialogService)
        {
            _userViewModel = user;
            _dialogService = dialogService;
            UpdateUserData();
            UpdateStateBtnName();
        }

        private string _userData = null!;
        /// <summary>
        /// Пользовательские данные в системе
        /// </summary>
        public string UserData 
        {
            get => _userData;
            set => SetProperty(ref _userData, value);
        }

        private string _btnName = null!;
        /// <summary>
        /// Название кнопки (вызова окна для авторизации)/(выхода из системы)
        /// </summary>
        public string BtnName
        {
            get=> _btnName;
            set => SetProperty(ref _btnName, value);
        }

        /// <summary>
        /// Команда для (вызова окна для авторизации)/(выхода из системы)
        /// </summary>
        public DelegateCommand LoginOrLogoutCommand 
        {
            get 
            {
                return new DelegateCommand(() => 
                {
                    if (_userViewModel.IsAuthenticated)
                    {
                        _dialogService.ShowDialog("YesOrNoDialog", new DialogParameters($"message=Вы действительно хотите выйти"),
                            r =>
                            {
                                if (r.Result == ButtonResult.OK)
                                {
                                    _userViewModel.IsAuthenticated = false;
                                    _userViewModel.Roles = null;
                                    _userViewModel.SecretToken = null;
                                    UpdateStateBtnName();
                                    UpdateUserData();
                                }
                            });
                    }
                    else 
                    {
                        _dialogService.ShowDialog("LoginDialog", r =>
                        {
                            UpdateStateBtnName();
                            UpdateUserData();
                        });
                    }
                });
            }
        }

        /// <summary>
        /// Обновляет пользовательские данные в системе
        /// </summary>
        private void UpdateUserData() 
        {
            if (_userViewModel.IsAuthenticated)
            {
                string roles = "";
                foreach (Claim role in _userViewModel.Roles ?? new List<Claim>())
                {
                    roles += $" {role.Value}";
                }
                UserData = $"Имя: {_userViewModel.UserName}  Роли:{roles}";
            }
            else
            {
                UserData = "";
            }
        }

        /// <summary>
        /// Обновляет название кнопки на панели
        /// </summary>
        private void UpdateStateBtnName() 
        {
            BtnName = _userViewModel.IsAuthenticated ? "Log Out" : "Log In";
        }

    }
}
