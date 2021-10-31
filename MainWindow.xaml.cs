﻿using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using DreamingSortingWPF.utils;
using static DreamingSortingWPF.utils.GeneralUtils;


namespace DreamingSortingWPF; 

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow {
    Duration animDuration = new(new TimeSpan(0, 0, 0, 0, 200));

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
            Process.Start("https://dreaming.codes");
        }
    }

    async Task<bool> doAnimationAndAddValue()
    {
        if (!int.TryParse(nInput.Text, out _)) {
            return false;
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
        nInput.Text = new Random().Next(0, 1000).ToString();

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

        return true;
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
    void randomClickHandler(object sender = null, RoutedEventArgs e = null)
    {
        if (!uint.TryParse(randomNv.Text, out uint randomQuantity)) {
            randomNv.Text = "";
            return;
        }

        randomNv.Text = "";
        insertRandomValues(randomQuantity);
    }

    async void insertRandomValues(uint nV)
    {
        int delay = 5000 / (int)nV;
        if (delay == 0) {
            delay = 1;
        }

        for (int i = 0; i < nV; i++) {
            nInput.Text = new Random().Next(0, 1000).ToString();
            doAnimationAndAddValue();
            await Task.Delay(delay);
        }
    }
}