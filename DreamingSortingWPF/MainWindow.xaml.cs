using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml;

namespace DreamingSortingWPF {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        Duration animDuration = new Duration(new TimeSpan(0, 0, 0, 0, 800));

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

        static T CloneXaml<T>(T source)
        {
            string xaml = XamlWriter.Save(source);
            StringReader sr = new StringReader(xaml);
            XmlReader xr = XmlReader.Create(sr);
            return (T)XamlReader.Load(xr);
        }
        public static Size MeasureTextSize(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch,
                                           double fontSize)
        {
            FormattedText ft = new FormattedText(text,
                                                 CultureInfo.CurrentCulture,
                                                 FlowDirection.LeftToRight,
                                                 new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
                                                 fontSize,
                                                 Brushes.Black);

            return new Size(ft.Width, ft.Height);
        }

        void Nick_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) {
                Process.Start("https://dreaming.codes");
            }
        }
        
        
        async void doAnimationAndAddValue()
        {
            //I clone the input box to perform the animation
            TextBox cloneInputForAnimation = CloneXaml(nInput);
            UserInteraction.Children.Add(cloneInputForAnimation);

            //Removing text from the realInput
            nInput.Text = "";

            //nInput.IsEnabled = false;

            //Calculation number width to create the containing box
            Size textSize = MeasureTextSize(cloneInputForAnimation.Text,
                                            cloneInputForAnimation.FontFamily, cloneInputForAnimation.FontStyle, cloneInputForAnimation.FontWeight,
                                            cloneInputForAnimation.FontStretch, cloneInputForAnimation.FontSize);

            Size targetSize = textSize;
            targetSize.Width += 10;
            targetSize.Height += 10;

            //Creating a new object wich is the one that will remain after the animation
            TextBox targetInput = CloneXaml(cloneInputForAnimation);
            targetInput.Visibility = Visibility.Hidden;
            targetInput.Width = targetSize.Width;
            targetInput.Height = targetSize.Height;
            nList.Children.Add(targetInput);

            //Obtaining the destination point
            //TODO: Trovare un modo che funzioni per trovare le cordinate. Altrimenti ricorrere alla matematica
            Point targetPosition = targetInput.TransformToAncestor(rootView).Transform(new Point(0, 0));

            //Obtaining the current point
            Point currentPos = cloneInputForAnimation.TransformToAncestor(rootView).Transform(new Point(0, 0));

            //Calculating the translation to do to reach targetPosition
            Vector translateToDo = targetPosition - currentPos;

            TranslateTransform myTranslateTransform = new TranslateTransform();
            cloneInputForAnimation.RenderTransform = myTranslateTransform;

            DoubleAnimation myAnimX = new DoubleAnimation {
                Duration = animDuration,
                To = translateToDo.X
            };

            DoubleAnimation myAnimY = new DoubleAnimation {
                Duration = animDuration,
                To = translateToDo.Y
            };

            DoubleAnimation myAnimHeight = new DoubleAnimation {
                Duration = animDuration,
                To = targetSize.Height
            };

            DoubleAnimation myAnimWidth = new DoubleAnimation {
                Duration = animDuration,
                To = targetSize.Width
            };


            myTranslateTransform.BeginAnimation(TranslateTransform.YProperty, myAnimY);
            myTranslateTransform.BeginAnimation(TranslateTransform.XProperty, myAnimX);
            cloneInputForAnimation.BeginAnimation(WidthProperty, myAnimWidth);
            cloneInputForAnimation.BeginAnimation(HeightProperty, myAnimHeight);

            await Task.Delay(animDuration.TimeSpan);
            targetInput.Visibility = Visibility.Visible;
            UserInteraction.Children.Remove(cloneInputForAnimation);
        }


        void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        void AddSortButton_OnClick(object sender, RoutedEventArgs e)
        {
            doAnimationAndAddValue();
        }
    }
}