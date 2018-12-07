using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace MillitaryApp
{
    /// <summary>
    /// Логика взаимодействия для SchemeElement.xaml
    /// </summary>
    public partial class SchemeElement : UserControl
    {
        private readonly Style Left, Right;

        public SchemeElement()
        {
            InitializeComponent();
            Left = (Style)Resources["Tooltip.Left"];
            Right = (Style)Resources["Tooltip.Right"];
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TooltipDescription.Text))
                TooltipDescription.Visibility = Visibility.Collapsed;
        }


        /// <summary> Изображение элемента. </summary>
        [Category("Элемент схемы")]
        [DisplayName("Картинка")]
        public System.Windows.Media.ImageSource Image
        {
            get { return TooltipImage.Source; }
            set { TooltipImage.Source = value; }
        }


        /// <summary> Название элемента. </summary>
        [Category("Элемент схемы")]
        [DisplayName("Название")]
        public string Title
        {
            get { return TooltipTitle.Text; }
            set { TooltipTitle.Text = value; }
        }

        /// <summary> Описание элемента. </summary>
        [Category("Элемент схемы")]
        [DisplayName("Описание")]
        public string Description
        {
            get { return TooltipDescription.Text; }
            set { TooltipDescription.Text = value; }
        }

        /// <summary> Расположение всплывающей подсказки. </summary>
        [Category("Элемент схемы")]
        [DisplayName("Расположение")]
        public EPlacement Placement
        {
            get
            {
                if (TooltipPlacement.Style == Right)
                    return EPlacement.Right;
                if (TooltipPlacement.Style == Left)
                    return EPlacement.Left;
                return EPlacement.Right;
            }
            set
            {
                switch (value)
                {
                    case EPlacement.Left: TooltipPlacement.Style = Left; break;
                    case EPlacement.Right: TooltipPlacement.Style = Right; break;
                }
            }
        }

        private async void OnMouseEnter(object sender, MouseEventArgs e)
        {
            await Task.Delay(200);
            if (IsMouseOver)
            {
                Border border = (Border)sender;
                border.Opacity = 1;
                border.Child.Visibility = Visibility.Visible;
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Border border = (Border)sender;
            border.Opacity = 0;
            border.Child.Visibility = Visibility.Collapsed;
        }

        /// <summary> Варианты расположение всплывающей подсказки. </summary>
        public enum EPlacement { Right, Left }
    }
}
