using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DreamingSortingWPF.utils;

public static class AnimationUtils {
    public static async Task doErrorAnimOnElement(FrameworkElement element)
    {
        DoubleAnimation goRight = new() {
            Duration = new Duration(new TimeSpan(0, 0, 0, 0, 50)),
            To = 10
        };

        DoubleAnimation goLeft = new() {
            Duration = new Duration(new TimeSpan(0, 0, 0, 0, 50)),
            To = -10
        };

        DoubleAnimation returnToBase = new() {
            Duration = new Duration(new TimeSpan(0, 0, 0, 0, 50)),
            To = 0
        };

        TranslateTransform myAddTranslateTransform = new();
        element.RenderTransform = myAddTranslateTransform;
        myAddTranslateTransform.BeginAnimation(TranslateTransform.XProperty, goRight);
        await Task.Delay(50);
        myAddTranslateTransform.BeginAnimation(TranslateTransform.XProperty, goLeft);
        await Task.Delay(50);
        myAddTranslateTransform.BeginAnimation(TranslateTransform.XProperty, goRight);
        await Task.Delay(50);
        myAddTranslateTransform.BeginAnimation(TranslateTransform.XProperty, goLeft);
        await Task.Delay(50);
        myAddTranslateTransform.BeginAnimation(TranslateTransform.XProperty, returnToBase);
        await Task.Delay(50);
    }
}