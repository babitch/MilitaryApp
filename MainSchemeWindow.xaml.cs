using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MillitaryApp
{
    /// <summary>
    /// Логика взаимодействия для MainSchemeWindow.xaml
    /// </summary>
    public partial class MainSchemeWindow : Window
    {
        Image[] mainscheme;
        Image current_main_scheme;

        public MainSchemeWindow()
        {
            InitializeComponent();
            List<Image> list = new List<Image>();
            foreach (UIElement element in ((Panel)Content).Children)
                list.Add(element as Image);
            list.Remove(null);

            mainscheme = list.ToArray();
        }
        private void FocusChanged(object sender, EventArgs e) { Topmost = IsActive; }

        private void Escape(object sender, KeyEventArgs e) { if (e.Key == Key.Escape) BackToMenu(); }
        private void BackToMenu(object sender, RoutedEventArgs e) { BackToMenu(); }
        private void OnClosed(object sender, EventArgs e) { Application.Current.Shutdown(); }

        public new async void Show() { base.Show(); await Task.Delay(50); Owner.Hide(); base.Activate(); }
        public new async void Hide() { Owner.Show(); await Task.Delay(50); base.Hide(); Owner.Activate(); }


        /// <summary> Отображение заданной фотографии </summary>
        public async void Show(int id)
        {
            if (id >= 0 && id < mainscheme.Length)
            {
                current_main_scheme = mainscheme[id];
                current_main_scheme.Visibility = Visibility.Visible;
                await Task.Delay(50); Show();
            }
            else MessageBox.Show("Невозможно загрузить изображение!", "",
              MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary> Вернуться в основное меню </summary>
        private async void BackToMenu()
        {
            Hide(); await Task.Delay(100);

            if (current_main_scheme != null)
                current_main_scheme.Visibility = Visibility.Hidden;
        }
    }
}
