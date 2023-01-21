using Prism.Mvvm;

namespace Telephones.Wpf.ViewModels.DataViewModel
{
    /// <summary>
    /// Медель представления обновления с данными об абоненте в телефонной книге
    /// </summary>
    public class UpdateRecordDataViewModel : BindableBase
    {
        private int _id;
        /// <summary>
        /// Идентификатор новой записи
        /// </summary>
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string? _firstName;
        /// <summary>
        /// Имя абонента
        /// </summary>
        public string? FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string? _lastName;
        /// <summary>
        /// Фамилия абонента
        /// </summary>
        public string? LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string? _fatherName;
        /// <summary>
        /// Отчество абонента
        /// </summary>
        public string? FatherName
        {
            get => _fatherName;
            set => SetProperty(ref _fatherName, value);
        }

        private string? _phoneNumber;
        /// <summary>
        /// Телефон абонента
        /// </summary>
        public string? PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        private string? _address;
        /// <summary>
        /// Адрес абонента
        /// </summary>
        public string? Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private string? _discript;
        /// <summary>
        /// Описание абонента
        /// </summary>
        public string? Discript
        {
            get => _discript;
            set => SetProperty(ref _discript, value);
        }
    }
}
