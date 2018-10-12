using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace MillitaryApp
{
    /// <summary>
    /// Логика взаимодействия для SchemeWindow.xaml
    /// </summary>
    public partial class SchemeWindow : Window
    {
        /// <summary> Ссылка на объект управления анимацией </summary>
        AnimationControl Control;

        /// <summary> Ссылка для доступа к управленияю схемами </summary>
        NavigationService Scheme;
        NavigationService Elements;

        public bool IsSoundEnable;
        public bool IsPauseEnable;


        public SchemeWindow()
        {
            InitializeComponent();
            Scheme = SchemeFrame.NavigationService;
            Elements = SchemeElementsFrame.NavigationService;
            Control = new AnimationControl(this);
            Scheme.LoadCompleted += SchemeLoadCompleted;
        }

        private void FocusChanged(object sender, EventArgs e) { Topmost = IsActive; }

        private void Escape(object sender, KeyEventArgs e) { if (e.Key == Key.Escape) BackToMenu(); }
        private void BackToMenu(object sender, MouseButtonEventArgs e) { BackToMenu(); }
        private void OnClosed(object sender, EventArgs e) { Application.Current.Shutdown(); }

        public new async void Show() { base.Show(); await Task.Delay(50); Owner.Hide(); base.Activate(); }
        public new async void Hide() { Owner.Show(); await Task.Delay(50); base.Hide(); Owner.Activate(); }


        /// <summary> Отображение заданной схемы или анимации </summary>
        public void Show(int app_id, int mode_id, string title = null, string mode = null, bool sound_enable = false)
        {
            Control.SetAll(false);   // Выключение кнопок и отображения текста

            AppTitle.Text = title != null ? title : "";     // Название аппаратуры связи
            ModeTitle.Text = mode != null ? mode : "";      // Название выбранного режима      

            IsSoundEnable = sound_enable;

            // Путь до заданного режима работы или схему
            string mode_path = ++mode_id > 0 ? "Mode_" + mode_id : "Scheme";
            string app_id_folder = (app_id + 1).ToString();
            Scheme.Source = new Uri("/Анимации режимов работы/" + app_id_folder + "/" + mode_path + ".xaml", UriKind.Relative);
            Elements.Source = new Uri("/Анимации режимов работы/" + app_id_folder + "/Elements.xaml", UriKind.Relative);

            PauseButton.Visibility = IsSoundEnable ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary> Вызывается при полной загрузке выбранной схемы. </summary>
        private async void SchemeLoadCompleted(object sender, NavigationEventArgs e)
        {
            if (Scheme.Content != null)
            {
                try { Control.Set(((Page)Scheme.Content).Resources); } catch { }
                await Task.Delay(50); Show();
            }
        }

        /// <summary> Вернуться в основное меню </summary>
        private async void BackToMenu()
        {
            Hide(); await Task.Delay(50);

            NextStepMessage.Visibility = Visibility.Hidden;

            AppTitle.Text = ModeTitle.Text = null;
            Scheme.Source = null;
            Elements.Source = null;
            Control.SetAll(false);
        }


        /// <summary> Следующий шаг </summary>
        public void NextStep(object sender, MouseButtonEventArgs e) { Control.NextStep(); }
        /// <summary> Предыдущий шаг </summary>
        private void BackStep(object sender, MouseButtonEventArgs e) { Control.BackStep(); }
        /// <summary> Повторить анимацию шага </summary>
        private void RepeatStep(object sender, MouseButtonEventArgs e) { Control.RepeatStep(); }
        /// <summary> Показать текст </summary>
        private void DescriptionStep(object sender, MouseButtonEventArgs e) { Control.DescriptionStep(); }
        /// <summary> Показать анимацию </summary>
        private void AnimationStep(object sender, MouseButtonEventArgs e) { Control.AnimationStep(); }

        /// <summary> Поставить воспроизведение режима со звуком на паузу. </summary>
        private void Pause(object sender, MouseButtonEventArgs e)
        {
            if (IsPauseEnable = !IsPauseEnable)
                PauseButton.Background = (SolidColorBrush)Resources["Main.Color.Red"];
            else PauseButton.ClearValue(TextBlock.BackgroundProperty);

            Control.DescriptionStep();
        }


        /// <summary> Режим отображения </summary>
        public void SetProjectMode(bool is_project_mode)
        {
            if (is_project_mode)
            {
                AppTitle.FontSize = 38;
                ModeTitle.FontSize = 32;

                ModeTitle.Style = (Style)Resources["Description.ModeTitle.Screen"];
                StepDescription.Style = (Style)Resources["Description.StepDescription.Screen"];
                DescriptionTitle.Style = (Style)Resources["Description.Title.Screen"];
                StepDescriptionPanel.Style = (Style)Resources["Description.Panel.Screen"];
            }
            else
            {
                AppTitle.FontSize = 28;
                ModeTitle.FontSize = 22;

                ModeTitle.Style = (Style)Resources["Description.ModeTitle"];
                StepDescription.Style = (Style)Resources["Description.StepDescription"];
                DescriptionTitle.Style = (Style)Resources["Description.Title"];
                StepDescriptionPanel.Style = (Style)Resources["Description.Panel"];
            }
        }
    }


    /// <summary>
    /// Класс управления анимацией заданного режима
    /// </summary>
    public class AnimationControl
    {
        /// <summary> Окно управления анимацией </summary>
        SchemeWindow Scheme;
        /// <summary> Описание этапа </summary>
        TextBlock Description;
        /// <summary> Номер эатпа </summary>
        TextBlock DescriptionSteps;
        /// <summary> Описание этапа (панель) </summary>
        Grid DescriptionPanel;

        /// <summary> Состояние этапов режима </summary>
        ModeState State;
        /// <summary> Этапы режима </summary>
        ModeStepList Steps;


        public AnimationControl(SchemeWindow window)
        {
            Scheme = window;
            Description = Scheme.StepDescription;
            DescriptionSteps = Scheme.StepSteps;
            DescriptionPanel = (Grid)Description.Parent;
        }

        /// <summary> Установить заданный режим в качестве активного </summary>
        public void Set(ResourceDictionary dictionary)
        {
            Steps = new ModeStepList(dictionary, OnAnimationStop);
            State = new ModeState(Steps);
            SetAll(true);
            UpdateStepState();
        }

        /// <summary> Отключить всё (отображение простой схемы) </summary>
        public void SetAll(bool check)
        {
            DescriptionPanel.Visibility = check ? Visibility.Visible : Visibility.Hidden;

            Scheme.NextStepButton.IsEnabled = check;
            Scheme.BackStepButton.IsEnabled = check;
            Scheme.DescriptionButton.IsEnabled = check;
            Scheme.RepeatButton.IsEnabled = check;

            Scheme.NextStepButton.Visibility = check ? Visibility.Visible : Visibility.Hidden;
            Scheme.BackStepButton.Visibility = check ? Visibility.Visible : Visibility.Hidden;
            Scheme.DescriptionButton.Visibility = check ? Visibility.Visible : Visibility.Hidden;
            Scheme.RepeatButton.Visibility = check ? Visibility.Visible : Visibility.Hidden;

            Scheme.StepSteps.Text = string.Empty;

            if (Steps != null && !check) Steps.StopAll();
        }


        /// <summary> Вызывается при завершении анимации </summary>
        async void OnAnimationStop(object sender, EventArgs e)
        {
            if (Scheme.IsVisible && State.IsAnimation && !State.IsEnd)
                if (Steps[State.ID].IsAutoNext || Scheme.IsSoundEnable)
                {
                    int id = State.ID;
                    await Task.Delay(1000);
                    if (State.ID == id && State.IsAnimation)
                        NextStep();
                }
                else Scheme.NextStepMessage.Visibility = Visibility.Visible;
        }

        /// <summary> Обновить состояние шага </summary>
        void UpdateStepState()
        {
            Steps.StopAllDescriptions();

            if (State.IsDescription)
            {
                Steps.Set(State.ID);
                if (Scheme.IsSoundEnable)
                {
                    DescriptionPanel.Visibility = Visibility.Hidden;
                    if (!Scheme.IsPauseEnable)
                    {
                        Steps.Description(State.ID).Play();
                    }
                }
                else
                {
                    DescriptionPanel.Visibility = Visibility.Visible;
                    Description.Text = Steps[State.ID].Description;
                }
                DescriptionSteps.Text = "Шаг " + (State.ID + 1) + " из " + (State.Last + 1);
            }
            else
            {
                DescriptionPanel.Visibility = Visibility.Hidden;
                Steps[State.ID].Play();
            }
            UpdateButtonState();
            Scheme.NextStepMessage.Visibility = Visibility.Hidden;
        }


        /// <summary> Следующий шаг или переход к анимации </summary>
        public void NextStep() { State.Next(); UpdateStepState(); }
        /// <summary> Предыдущий шаг </summary>
        public void BackStep() { State.Back(); UpdateStepState(); }
        /// <summary> Повторить шаг </summary>
        public void RepeatStep() { State.Animation(); UpdateStepState(); }
        /// <summary> Описание шага </summary>
        public void DescriptionStep() { State.Text(); UpdateStepState(); }
        /// <summary> Анимация шага </summary>
        public void AnimationStep() { State.Animation(); UpdateStepState(); }

        /// <summary> Обновление состояния кнопок </summary>
        void UpdateButtonState()
        {
            Scheme.NextStepButton.IsEnabled = State.IsNext;
            Scheme.BackStepButton.IsEnabled = State.IsBack;
            Scheme.DescriptionButton.IsEnabled = Scheme.RepeatButton.IsEnabled = State.IsAnimation;
        }



        /// <summary>
        /// Структура описания и анимации этапа
        /// </summary>
        struct ModeStep
        {
            /// <summary> Анимация этапа </summary>
            public readonly Storyboard Animation;
            /// <summary> Описание этапа </summary>
            public readonly string Description;
            /// <summary> Автоматический переход к следующему шагу </summary>
            public readonly bool IsAutoNext;
            /// <summary> Есть анимация? </summary>
            public readonly bool IsAnimation;

            public ModeStep(string description, bool is_auto_next, Storyboard animation, EventHandler onAnimationStop)
            {
                Description = description;
                Animation = animation;
                IsAutoNext = is_auto_next;
                IsAnimation = animation != null;

                if (Animation != null) Animation.Completed += onAnimationStop;
            }

            public void Play() { if (Animation != null) { Animation.Stop(); Animation.Begin(); } }
            public void Stop() { if (Animation != null) Animation.Stop(); }
            public void Pause() { if (Animation != null) Animation.Pause(); }
            public void Resume() { if (Animation != null) Animation.Resume(); }
            public void End() { if (Animation != null) { Animation.Begin(); Animation.SkipToFill(); } }
        }

        struct DescStep
        {
            /// <summary> Анимация этапа </summary>
            public readonly Storyboard Animation;

            public DescStep(Storyboard animation) { Animation = animation; }

            public void Play() { if (Animation != null) { Animation.Stop(); Animation.Begin(); } }
            public void Stop() { if (Animation != null) Animation.Stop(); }
            public void Pause() { if (Animation != null) Animation.Pause(); }
            public void Resume() { if (Animation != null) Animation.Resume(); }
            public void End() { if (Animation != null) { Animation.Begin(); Animation.SkipToFill(); } }
        }

        /// <summary>
        /// Класс управления описаниями и анимацией этапов
        /// </summary>
        class ModeStepList
        {
            /// <summary> Список этапов </summary>
            readonly ModeStep[] steps;

            readonly DescStep[] descriptions;

            /// <summary> Количество этапов </summary>
            public int Count { get { return steps.Length; } }

            /// <summary> Создать массиов этапов на основании заданной страницы </summary>
            public ModeStepList(ResourceDictionary dictionary, EventHandler onAnimationStop)
            {
                ItemCollection descriptions = ((ComboBox)dictionary["Descriptions"]).Items;

                steps = new ModeStep[descriptions.Count];
                this.descriptions = new DescStep[descriptions.Count];

                for (int id = 0; id < steps.Length; ++id)
                {
                    string text = ((Run)descriptions[id]).Text;
                    steps[id] = new ModeStep(text, text.StartsWith("[auto]"),
                        (Storyboard)dictionary["Step_" + (id + 1)], onAnimationStop);
                }

                for (int id = 0; id < this.descriptions.Length; ++id)
                    this.descriptions[id] = new DescStep((Storyboard)dictionary["Step_" + (id + 1) + "_S"]);
            }

            /// <summary> Анимация в соответствии с заданным этапом </summary>
            public void Set(int step_id)
            {
                if (step_id > steps.Length) throw new ArgumentOutOfRangeException();
                for (int id = 0; id < step_id; ++id) steps[id].End();
                for (int id = step_id; id < steps.Length; ++id) steps[id].Stop();
            }

            /// <summary> Остановить все анимации </summary>
            public void StopAll()
            {
                foreach (ModeStep animation in steps)
                    animation.Stop();
            }

            /// <summary> Остановить все анимации </summary>
            public void StopAllDescriptions()
            {
                foreach (DescStep desc in descriptions)
                    desc.Stop();
            }

            public DescStep Description(int id) { return descriptions[id]; }

            /// Доступ по индексу
            public ModeStep this[int id] { get { return steps[id]; } }
        }


        /// <summary>
        /// Класс управления состоянием заданного режима
        /// </summary>
        class ModeState
        {
            /// <summary> Количество этапов режима </summary>
            public readonly int Last;
            /// <summary> Номер текущего этапа </summary>
            int current;
            /// <summary> Список этапов </summary>
            readonly ModeStepList Steps;

            /// <summary> Номер текущего этапа </summary>
            public int ID { get { return current; } }
            /// <summary> Тип текущего этапа </summary>
            public eMode Mode { private set; get; }


            /// <summary> Возможен переход на следующий шаг? </summary>
            public bool IsNext { get { return current < Last; } }
            /// <summary> Возможен переход на предыдущий шаг? </summary>
            public bool IsBack { get { return current > 0; } }
            /// <summary> Отображается анимация? </summary>
            public bool IsAnimation { get { return Mode == eMode.Animation; } }
            /// <summary> Отображается описание шага? </summary>
            public bool IsDescription { get { return Mode == eMode.Description; } }
            /// <summary> Последний этап режима работы? </summary>
            public bool IsEnd { get { return current == Last; } }


            public ModeState(ModeStepList steps)
            {
                Last = steps.Count - 1; current = 0;
                Steps = steps;
                Mode = eMode.Description;
            }

            /// <summary> Переход на следующий этап </summary>
            public void Next()
            {
                if (current < Last) ++current;
                Mode = eMode.Description;
            }
            /// <summary> Переход на предыдущий этап </summary>
            public void Back()
            {
                if (current > 0) --current;
                Mode = eMode.Description;
            }
            /// <summary> Переход к описанию </summary>
            public void Text() { Mode = eMode.Description; }
            /// <summary> Переход к анимации </summary>
            public void Animation() { Mode = eMode.Animation; }

            /// <summary> Типы этапов </summary>
            public enum eMode { Description, Animation }
        }
    }
}
