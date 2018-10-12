using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace MillitaryApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary> Иконки </summary>
        IconButtonsClass Icons;
        /// <summary> Режимы работы </summary>
        ModeButtonsClass Modes;

        /// <summary> Ссылка на окно с фотографиями </summary>
        PhotoWindow Photo;
        /// <summary> Ссылка на окно с анимацией </summary>
        SchemeWindow Scheme;
        /// <summary> Ссылка на окно с общей схемой </summary>
        MainSchemeWindow MainScheme;
        /// <summary> Прошлое состояние окна </summary>
        WindowState MainWindowState;

        /// <summary> Определяет, включен ли режим проектора </summary>
        bool IsFullScreen;

        /// <summary> Определяет, включен ли режим со звком </summary>
        bool IsSoundEnable;


        public MainWindow()
        {
            InitializeComponent();

            Icons = new IconButtonsClass(IconButtons.Children, CommunicationTitle, CommunicationDescription, CommunicationModes.Children,
                (ComboBox)FindResource("App.Title"), (ComboBox)FindResource("App.Description"), (ComboBox)FindResource("App.Modes"), IconsImages.Children);

            Modes = new ModeButtonsClass(CommunicationModes.Children, Icons);
        }

        private void FocusChanged(object sender, EventArgs e) { if (IsFullScreen) Topmost = IsActive; }

        private void Escape(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Escape && IsFullScreen)
                    ProjectorMode(ProjectorButton, null);
            }
            catch { }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Scheme = CreateWindow<SchemeWindow>();
            Photo = CreateWindow<PhotoWindow>();
            MainScheme = CreateWindow<MainSchemeWindow>();

            Width = (int)(SystemParameters.PrimaryScreenWidth / 1.5) + 8;
            Height = (Width - 8) * 9 / 16 + 31;
        }

        /// <summary> Открытие окна и возврат ссылки на него </summary>
        private T CreateWindow<T>() where T : Window, new()
        {
            T window = new T();
            window.Owner = this;
            return window;
        }

        /// <summary> Переключение режима отображения </summary>
        private void ProjectorMode(object sender, MouseButtonEventArgs e)
        {
            TextBlock button = (TextBlock)sender;

            if (IsFullScreen = !IsFullScreen)
            {
                button.Text = "Проектор";
                ResizeMode = ResizeMode.NoResize;
                WindowStyle = WindowStyle.None;

                if ((MainWindowState = WindowState) == WindowState.Maximized)
                    WindowState = WindowState.Normal;

                WindowState = WindowState.Maximized;
            }
            else
            {
                button.Text = "Монитор";
                WindowStyle = WindowStyle.SingleBorderWindow;
                ResizeMode = ResizeMode.CanResize;

                if (MainWindowState == WindowState.Maximized)
                    WindowState = WindowState.Normal;

                WindowState = MainWindowState;
            }

            Topmost = IsFullScreen;
            Scheme.SetProjectMode(IsFullScreen);
        }

        /// <summary> Переключение режима озвучки </summary>
        private void SoundMode(object sender, MouseButtonEventArgs e)
        {
            TextBlock button = (TextBlock)sender;

            if (button.Tag == null) button.Tag = false;

            if ((bool)button.Tag)
            {
                button.Text = "Без звука";
                button.Tag = false;
                IsSoundEnable = false;
            }
            else
            {
                button.Text = "Со звуком";
                button.Tag = true;
                IsSoundEnable = true;
            }
        }


        /// ---------- Работа с окнами ---------- ///

        /// <summary> Отобразить окно со схемой </summary>
        private void OpenScheme(object sender, MouseButtonEventArgs e) { Scheme.Show(Icons.CheckedID, Modes.SelectID, Icons.Title, Modes.Mode, IsSoundEnable); }
        /// <summary> Отобразить окно с фотографиями </summary>
        private void OpenPhoto(object sender, MouseButtonEventArgs e) { Photo.Show(Icons.PhotoID); }
        /// <summary> Отобразить окно с общей схемой </summary>
        private void OpenMainScheme(object sender, MouseButtonEventArgs e) { MainScheme.Show(Icons.MainSchemeId); }


        /// ---------- Наведение и нажатие на иконки ---------- ///

        private void OnAppIconMouseEnter(object sender, MouseEventArgs e) { Icons.Select(sender); }
        private void OnAppIconMouseLeave(object sender, MouseEventArgs e) { Icons.Deselect(sender); }
        private void OnAppIconMouseClick(object sender, MouseButtonEventArgs e) { Icons.Check(sender); }



        /// ---------- Наведение на кнопки режимов  ---------- ///

        private void OnModeButtonMouseEnter(object sender, MouseEventArgs e) { Modes.Select(sender); }
        private void OnModeButtonMouseLeave(object sender, MouseEventArgs e) { Modes.Deselect(); }



        /// ---------- Пользовательские классы ---------- ///

        /// <summary>
        /// Иконки аппаратуры связи
        /// </summary>
        class IconButtonsClass
        {
            /// <summary> Области для нажатия </summary>
            UIElement[] Buttons;
            /// <summary> Картинки "выбрать" </summary>
            UIElement[] SelectImage;
            /// <summary> Картинки "выбрано" </summary>
            UIElement[] SelectedImage;

            /// <summary> Режимы </summary>
            UIElement[] ModesButton;
            /// <summary> Названия режимов </summary>
            TextBlock[] ModesTitle;
            /// <summary> Кнопки выбора режима </summary>
            TextBlock[] ModesCheckButtons;

            Run TitleRun, DescriptionRun;

            string[] Titles, Descriptions;
            public string[][] Modes;

            public string Title { get { return Titles[CheckedID]; } }
            public string[] Mode { get { return Modes[CheckedID]; } }


            /// <summary> Номер выбранного элемента </summary>
            public int CheckedID { private set; get; }
            /// <summary> Номер наведенного элемента </summary>
            public int SelectID { private set; get; }
            /// <summary> Отображение фотографии, не привязанной к режиму </summary>
            public int PhotoID { private set; get; }
            /// <summary> Отображение основной схемы, не привязанной к режиму </summary>
            public int MainSchemeId { private set; get; }


            public IconButtonsClass(UIElementCollection icons, Run title, Run description, UIElementCollection modes, ComboBox titles, ComboBox descriptions,
                ComboBox steps, UIElementCollection icons_images)
            {
                TitleRun = title;
                DescriptionRun = description;

                Titles = new string[titles.Items.Count];
                for (int id = 0; id < Titles.Length; ++id)
                    Titles[id] = ((Run)titles.Items[id]).Text;

                Descriptions = new string[descriptions.Items.Count];
                for (int id = 0; id < Descriptions.Length; ++id)
                    Descriptions[id] = ((Run)descriptions.Items[id]).Text;

                Modes = new string[steps.Items.Count][];
                for (int id = 0; id < Modes.Length; ++id)
                {
                    Modes[id] = new string[((ComboBox)steps.Items[id]).Items.Count];
                    for (int mode_id = 0; mode_id < Modes[id].Length; ++mode_id)
                        Modes[id][mode_id] = ((Run)((ComboBox)steps.Items[id]).Items[mode_id]).Text;
                }

                List<UIElement> panels = new List<UIElement>();
                List<UIElement> select_img = new List<UIElement>();
                List<UIElement> selected_img = new List<UIElement>();

                int index = 0;

                foreach (Panel panel in icons)
                {
                    panels.Add(panel);
                    panel.Opacity = 0;

                    foreach (FrameworkElement element in panel.Children)
                    {
                        switch ((string)element.Tag)
                        {
                            case "select":
                                select_img.Add(element);
                                element.Visibility = Visibility.Visible;
                                break;
                            case "selected":
                                selected_img.Add(element);
                                element.Visibility = Visibility.Hidden;
                                break;
                        }
                    }

                    // Отключить иконки для несуществующих
                    if (index++ >= Titles.Length)
                        panel.IsEnabled = false;
                }

                index = 0;

                foreach (Image image in icons_images)
                {
                    // Отключить иконки для несуществующих
                    if (index++ >= Titles.Length)
                        image.Visibility = Visibility.Hidden;
                }

                Buttons = panels.ToArray();
                SelectImage = select_img.ToArray();
                SelectedImage = selected_img.ToArray();

                List<UIElement> mode_buttons = new List<UIElement>();
                List<TextBlock> mode_titles = new List<TextBlock>();
                List<TextBlock> mode_check_buttons = new List<TextBlock>();

                foreach (Panel panel in modes)
                {
                    mode_buttons.Add(panel);
                    panel.Visibility = Visibility.Hidden;
                    foreach (FrameworkElement element in panel.Children)
                    {
                        switch (element.Tag.ToString())
                        {
                            case "title": mode_titles.Add(element as TextBlock); break;
                            case "button": mode_check_buttons.Add(element as TextBlock); break;
                        }
                    }
                }

                ModesButton = mode_buttons.ToArray();
                ModesTitle = mode_titles.ToArray();
                ModesCheckButtons = mode_check_buttons.ToArray();

                Select(0);
                Check(0);
            }


            /// <summary>
            /// Выбрать элемент по номеру
            /// </summary>
            public int Check(int id)
            {
                if (id < 0 || id >= Buttons.Length)
                    throw new ArgumentOutOfRangeException();

                Buttons[CheckedID].Opacity = 0;
                SelectImage[CheckedID].Visibility = Visibility.Visible;
                SelectedImage[CheckedID].Visibility = Visibility.Hidden;

                Buttons[id].Opacity = 100;
                SelectImage[id].Visibility = Visibility.Hidden;
                SelectedImage[id].Visibility = Visibility.Visible;

                TitleRun.Text = Titles[id];
                DescriptionRun.Text = Descriptions[id];

                CheckedID = id;

                DisplayModes(id);

                return CheckedID;
            }
            /// <summary> Выбрать элемент по ссылке </summary>
            public int Check(object sender) { return Check(FindElement(sender)); }


            /// <summary>
            /// Выделить элемент по номеру
            /// </summary>
            public void Select(int id)
            {
                if (id < 0 || id >= Buttons.Length)
                    throw new ArgumentOutOfRangeException();

                for (int index = 0; index < Buttons.Length; ++index)
                    if (index != CheckedID) Buttons[index].Opacity = 0;

                Buttons[id].Opacity = 100;
                SelectID = id;
            }
            /// <summary> Выделить элемент по номеру </summary>
            public void Select(object sender) { Select(FindElement(sender)); }


            /// <summary>
            /// Снять выделение с элемента по номеру
            /// </summary>
            public void Deselect(int id)
            {
                if (id < 0 || id >= Buttons.Length)
                    throw new ArgumentOutOfRangeException();

                for (int index = 0; index < Buttons.Length; ++index)
                    if (index != CheckedID) Buttons[index].Opacity = 0;

                SelectID = CheckedID;
            }
            /// <summary> Выделить элемент по номеру </summary>
            public void Deselect(object sender) { Deselect(FindElement(sender)); }


            /// <summary>
            /// Отобразить выбранные режимы
            /// </summary>
            void DisplayModes(int id)
            {
                if (id < 0 || id >= Buttons.Length)
                    throw new ArgumentOutOfRangeException();

                foreach (UIElement button in ModesButton)
                    button.Visibility = Visibility.Hidden;

                for (int i = 0; i < Modes[id].Length; ++i)
                {
                    ModesButton[i].Visibility = Visibility.Visible;
                    bool check = Modes[id][i].Contains("[x]");
                    ModesTitle[i].Text = (!check ? (i + 1) + ". " : "") + Modes[id][i];
                    ModesCheckButtons[i].IsEnabled = !check;
                    ModesCheckButtons[i].Opacity = check ? 0.6 : 1;
                }
            }


            /// <summary>
            /// Поиск ссылки в массиве и возврат номера
            /// </summary>
            private int FindElement(object element)
            {
                for (int index = 0; index < Buttons.Length; ++index)
                    if (Buttons[index] == element)
                        return index;
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Иконки аппаратуры связи
        /// </summary>
        class ModeButtonsClass
        {
            /// <summary> Режимы работы аппаратуры </summary>
            TextBlock[] title_blocks;
            /// <summary> Цвет текста </summary>
            Brush text_color, button_color;

            /// <summary> Номер наведенного режима </summary>
            public int SelectID { private set; get; }
            /// <summary> Название выбранного режима </summary>

            public string Mode
            {
                get
                {
                    try { return Icons.Modes[Icons.CheckedID][SelectID]; }
                    catch { return ""; }
                }
            }

            /// <summary> Ссылка на выбор аппаратуры связи </summary>
            IconButtonsClass Icons;


            public ModeButtonsClass(UIElementCollection modes, IconButtonsClass icons)
            {
                Icons = icons;

                List<TextBlock> titles = new List<TextBlock>();

                int button_id = 0;

                foreach (Panel panel in modes)
                    foreach (FrameworkElement element in panel.Children)
                        switch (element.Tag.ToString())
                        {
                            case "title":
                                titles.Add((TextBlock)element);
                                if (text_color == null)
                                    text_color = ((TextBlock)element).Foreground;
                                break;
                            case "button":
                                element.Tag = button_id++;
                                if (button_color == null)
                                    button_color = ((TextBlock)element).Foreground;
                                break;
                        }

                title_blocks = titles.ToArray();
                SelectID = -1;
            }

            /// <summary> Выделить элемент по номеру </summary>
            public void Select(int id) { title_blocks[SelectID = id].Foreground = button_color; }
            public void Select(object sender)
            {
                try { Select(int.Parse(((TextBlock)sender).Tag.ToString())); }
                catch { }
            }

            /// <summary> Снять выделение с элемента по номеру </summary>
            public void Deselect()
            {
                title_blocks[SelectID].Foreground = text_color;
                SelectID = -1;
            }
        }
    }
}
