using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using DreamingSortingWPF.utils;
using updatedDreamingSortingWPF;
using updatedDreamingSortingWPF.algorithms;
using static DreamingSortingWPF.utils.GeneralUtils;


namespace DreamingSortingWPF;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow {
    Duration animDuration = new(new TimeSpan(0, 0, 0, 0, 200));
    bool isAlgoView;
    Random randomSeeded = new();

    public MainWindow()
    {
        InitializeComponent();
        fillAlgorithmsList();
    }
    void fillAlgorithmsList()
    {
        Dictionary<string, Type> algorithms = new() {
            { "QuickSort", typeof(QuickSort) },
            { "SelectionSort", typeof(SelectionSort) }
        };

        foreach (KeyValuePair<string, Type> keyValuePair in algorithms) {
            Button button = new() {
                Content = keyValuePair.Key,
                Width = 100,
                Height = 30,
                Margin = new Thickness(10, 10, 10, 10),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Background = new BrushConverter().ConvertFrom("#202020") as Brush,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                Tag = keyValuePair.Value
            };


            button.Click += (_, _) => {
                SortingInterface sortingInterfaceInstance = (SortingInterface)Activator.CreateInstance(keyValuePair.Value, getNumbers(nList))!;
                sortingInterfaceInstance.Show();
            };

            mySelectList.Children.Add(button);
        }


    }

    void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed) {
            DragMove();
        }
    }
    void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }


    void Nick_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed) {
            OpenURL("https://dreaming.codes");
        }
    }

    async Task doAnimationAndAddValue()
    {
        if (!int.TryParse(nInput.Text, out _)) {
            Brush originalBrush = addButton.Background;
            if (originalBrush == Brushes.Red) {
                return;
            }

            addButton.Background = Brushes.Red;
            await AnimationUtils.doErrorAnimOnElement(addButton);
            addButton.Background = originalBrush;
            return;
        }

        //Calculation number width to create the containing box
        Size textSize = MeasureTextSize(nInput);

        Size targetSize = textSize;
        targetSize.Width += 20;
        targetSize.Height += 20;

        //Creating a new object wich is the one that will remain after the animation
        TextBox targetInput = CloneXaml(nInput);
        targetInput.Visibility = Visibility.Hidden;
        targetInput.Width = targetSize.Width;
        targetInput.Height = targetSize.Height;
        nList.Children.Add(targetInput);

        //I clone the input box to perform the animation
        TextBox cloneInputForAnimation = CloneXaml(nInput);
        UserInteraction.Children.Add(cloneInputForAnimation);

        //Randomizing next number
        nInput.Text = randomSeeded.Next(0, 1000).ToString();

        AsyncEventListener itChanged = new();
        targetInput.Loaded += itChanged.Listen;

        await itChanged.Successfully;

        myScrollViewer.ScrollToEnd();


        nInput.SelectAll();

        //Obtaining the destination point
        Point targetPosition = targetInput.TransformToAncestor(rootView).Transform(new Point(0, 0));

        //Obtaining the current point
        Point currentPos = cloneInputForAnimation.TransformToAncestor(rootView).Transform(new Point(0, 0));

        //Calculating the translation to do to reach targetPosition
        Vector translateToDo = targetPosition - currentPos;

        TranslateTransform myTranslateTransform = new();
        cloneInputForAnimation.RenderTransform = myTranslateTransform;

        DoubleAnimation myAnimX = new() {
            Duration = animDuration,
            To = translateToDo.X
        };

        DoubleAnimation myAnimY = new() {
            Duration = animDuration,
            To = translateToDo.Y
        };

        DoubleAnimation myAnimHeight = new() {
            Duration = animDuration,
            To = targetSize.Height
        };

        DoubleAnimation myAnimWidth = new() {
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
        e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
    }
    void AddSortButton_OnClick(object sender, RoutedEventArgs e)
    {
    #pragma warning disable CS4014
        doAnimationAndAddValue();
    #pragma warning restore CS4014
    }
    void NInput_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) {
        #pragma warning disable CS4014
            doAnimationAndAddValue();
        #pragma warning restore CS4014
        }
    }
    void randomEnterHandler(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) {
            randomClickHandler();
        }
    }
    async void randomClickHandler(object? sender = null, RoutedEventArgs? e = null)
    {
        if (!uint.TryParse(randomNv.Text, out uint randomQuantity)) {
            randomNv.Text = "";
            Brush originalBrush = randomNv.Background;
            if (originalBrush == Brushes.Red) {
                return;
            }

            randomNv.Background = Brushes.Red;
            randomButton.Background = Brushes.Red;
            await AnimationUtils.doErrorAnimOnElement(randomGrid);
            randomNv.Background = originalBrush;
            randomButton.Background = originalBrush;
            return;
        }

        randomNv.Text = "";
        insertRandomValues(randomQuantity);
    }

    void insertRandomValues(uint nV)
    {
        int delay = 1000 / (int)nV;
        if (delay == 0) {
            delay = 1;
        }

        Timer? myTimer = null;
        myTimer = SetIntervalThread(() => {
            if (nV <= 0) {
                // ReSharper disable once AccessToModifiedClosure
                myTimer?.Dispose();
                return;
            }

            nInput.Dispatcher.BeginInvoke(() => {
                nInput.Text = randomSeeded.Next(0, 1000).ToString();
            #pragma warning disable CS4014
                doAnimationAndAddValue();
            #pragma warning restore CS4014
            });

            nV--;
        }, delay);

    }
    void switchnListPage(object sender, MouseButtonEventArgs e)
    {
        if (isAlgoView) {
            DoubleAnimation reset = new() {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)),
                To = 0
            };

            DoubleAnimation resetAngle = new() {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)),
                To = -90
            };

            userInteractionGrid.RenderTransform.BeginAnimation(TranslateTransform.YProperty, reset);
            switchPageButton.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, resetAngle);
            isAlgoView = false;
        }
        else {
            DoubleAnimation goUp = new() {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)),
                To = -480
            };


            userInteractionGrid.RenderTransform.BeginAnimation(TranslateTransform.YProperty, goUp);


            DoubleAnimation rotateAngle = new() {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)),
                To = 90
            };

            switchPageButton.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAngle);
            isAlgoView = true;
        }
    }
}