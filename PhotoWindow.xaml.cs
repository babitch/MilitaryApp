using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace MillitaryApp
{
    /// <summary>
    /// Логика взаимодействия для PhotoWindow.xaml
    /// </summary>
    public partial class PhotoWindow : Window
    {
        /// <summary> Ссылки на фотографии аппаратуры связи </summary>
        Image[] photos;
        /// <summary> Ссылка на выбранную фотографию </summary>
        Image current_photo;
        /// <summary> Ссылка на объект управления анимацией </summary>
        public PhotoWindow()
        {
            InitializeComponent();

            var list = new List<Image>();
            foreach (UIElement element in ((Panel)Content).Children)
            {
                if (element is Image)
                {
                    list.Add((Image)element);
                }
            }

            photos = list.ToArray();
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
            if (id >= 0 && id < photos.Length)
            {
                current_photo = photos[id];
                current_photo.Visibility = Visibility.Visible;
                await Task.Delay(50); Show();
            }
            else MessageBox.Show("Невозможно загрузить изображение!", "",
              MessageBoxButton.OK, MessageBoxImage.Error);
        }


        /// <summary> Вернуться в основное меню </summary>
        private async void BackToMenu()
        {
            Hide(); await Task.Delay(100);

            if (current_photo != null)
                current_photo.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (current_photo != null && photos.Length > 0)
            {
                // прячем текущее из видимых
                current_photo.Visibility = Visibility.Hidden;

                var index = 0;
                var button = sender as Button;
                int.TryParse(button?.Tag.ToString(), out index);

                // назначаем текущему новый стиль из индекса
                current_photo = photos[index];
                current_photo.Visibility = Visibility.Visible;
                this.UpdateLayout();
            }
        }
    }
}
