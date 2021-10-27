using System.Windows;
using System.Windows.Input;

namespace DreamingSortingWPF {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow()
        {
            InitializeComponent();
        }
        void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }
        void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        void Nick_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) {
                System.Diagnostics.Process.Start("https://dreaming.codes");
            }
        }
        void AddSortButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}