using Prism.Mvvm;

namespace Telephones.Wpf.ViewModels.DataViewModel
{
    /// <summary>
    /// Модель представления абонента в общем списке контактов
    /// </summary>
    public class ShortRecordViewModel: BindableBase
    {
        private int _id;
        /// <summary>
        /// Идентификатор абонента
        /// </summary>
        public int Id 
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _firstName = null!;
        /// <summary>
        /// Имя абонента
        /// </summary>
        public string FirstName
        { 
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName = null!;
        /// <summary>
        /// Фамилия абонента
        /// </summary>
        public string LastName 
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string _fatherName = null!;
        /// <summary>
        /// Отчество абонента
        /// </summary>
        public string FatherName 
        {
            get => _fatherName;
            set => SetProperty(ref _fatherName, value);
        }
    }
}
