using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace MillitaryApp
{
    public partial class App : Application
    {
        public App() : base() {
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        /// <summary> Отображение сообщения об ошибке </summary>
        void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Произошла неизвестная ошибка! Программа будет завершена. Установка «.Net Framework 4.5» возможно решит проблему.", "Произошла неизвестная ошибка!",
                MessageBoxButton.OK, MessageBoxImage.Error);

            Current.Shutdown();
            e.Handled = true;
        }
    }
}
