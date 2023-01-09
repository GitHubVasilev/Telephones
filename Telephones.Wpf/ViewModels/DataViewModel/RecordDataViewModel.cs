using Prism.Mvvm;

namespace Telephones.Wpf.ViewModels.DataViewModel
{
    public class RecordDataViewModel : BindableBase
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string? _firstName;

        public string? FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string? _lastName;
        public string? LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string? _fatherName;
        public string? FatherName
        {
            get => _fatherName;
            set => SetProperty(ref _fatherName, value);
        }

        private string? _phoneNumber;
        public string? PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        private string? _address;
        public string? Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private string? _discript;
        public string? Discript
        {
            get => _discript;
            set => SetProperty(ref _discript, value);
        }
    }
}
