using Prism.Mvvm;
using System.Collections.Generic;
using System.Security.Claims;
using Telephones.Wpf.TokenHelpers;
using System.Linq;
using Telephones.Wpf.Properties;

namespace Telephones.Wpf.ViewModels.User
{
    public class UserViewModel : BindableBase
    {
        private string? _userName;

        public string? UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private SecurityToken? _secretToken;
        public SecurityToken? SecretToken
        {
            get => _secretToken;
            set => SetProperty(ref _secretToken, value);
        }

        private IEnumerable<Claim> _roles;

        public IEnumerable<Claim> Roles
        {
            get => _roles ?? new List<Claim>();
            set => SetProperty(ref _roles, value);
        }

        private bool _isAuthenticated;
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                SetProperty(ref _isAuthenticated, value);
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(CanCreateRecord)));
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(CanDeleteRecord)));
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(CanUpdateRecord)));
            }
        }

        public bool CanCreateRecord
        {
            get => Roles
                .Where(m => m.Value == Resources.RoleAdmin || m.Value == Resources.RoleUser)
                .Count() > 0;
        }

        public bool CanDeleteRecord
        {
            get => Roles
                .Where(m => m.Value == Resources.RoleAdmin)
                .Count() > 0;
        }

        public bool CanUpdateRecord
        {
            get => Roles
                .Where(m => m.Value == Resources.RoleAdmin)
                .Count() > 0;

        }
    }
}