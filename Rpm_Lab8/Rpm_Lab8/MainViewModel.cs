using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rpm_Lab8
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IDialogService _dialogService;
        private string _newName;
        private string _newPhone;
        private string _errorMessage;
        private Contact _selectedContact;

        public ObservableCollection<Contact> Contacts { get; } = new();

        public string NewName
        {
            get => _newName;
            set { _newName = value; OnPropertyChanged(); }
        }
        public string NewPhone
        {
            get => _newPhone;
            set { _newPhone = value; OnPropertyChanged(); }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }
        public Contact SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            AddCommand = new RelayCommand(ExecuteAdd);
            DeleteCommand = new RelayCommand(ExecuteDelete, CanExecuteDelete);
        }

        public MainViewModel()
        {
        }

        private void ExecuteAdd(object parameter)
        {
            ErrorMessage = null;

            if (string.IsNullOrWhiteSpace(NewName))
            {
                ErrorMessage = "Имя не может быть пустым.";
                return;
            }

            if (!Regex.IsMatch(NewPhone, @"^(\+7|7|8)?\d{10}$"))
            {
                ErrorMessage = "Неверный формат телефона. Ожидается: +7XXXXXXXXXX или 10 цифр.";
                return;
            }
            if (Contacts.Any(c => c.Phone.Trim().Equals(NewPhone.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                _dialogService.ShowWarning("Контакт с таким номером телефона уже существует в телефонной книге.");
                return;
            }
            Contacts.Add(new Contact { Name = NewName.Trim(), Phone = NewPhone.Trim() });
            NewName = string.Empty;
            NewPhone = string.Empty;
            _dialogService.ShowInfo("Контакт успешно добавлен.");
        }

        private void ExecuteDelete(object parameter)
        {
            if (parameter is Contact contact && Contacts.Contains(contact))
            {
                if (_dialogService.ShowConfirmation($"Вы действительно хотите удалить контакт '{contact.Name}'?"))
                {
                    Contacts.Remove(contact);
                    SelectedContact = null;
                }
            }
        }

        private bool CanExecuteDelete(object parameter) => SelectedContact != null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }
}
