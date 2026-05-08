using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Rpm_Lab8
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddTransient<MainWindow>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.DataContext = serviceProvider.GetService<MainViewModel>();
            mainWindow.Show();
        }
    }

}
