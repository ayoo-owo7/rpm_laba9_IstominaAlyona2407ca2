using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rpm_Lab8
{
    public interface IDialogService
    {
        void ShowInfo(string message);
        void ShowWarning(string message);
        void ShowError(string message);
        bool ShowConfirmation(string message);
    }

    public class DialogService : IDialogService
    {
        public void ShowInfo(string message) =>
            MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

        public void ShowWarning(string message) =>
            MessageBox.Show(message, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);

        public void ShowError(string message) =>
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

        public bool ShowConfirmation(string message) =>
            MessageBox.Show(message, "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }
}
