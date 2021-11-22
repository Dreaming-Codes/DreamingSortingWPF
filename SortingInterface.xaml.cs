using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace updatedDreamingSortingWPF;

public abstract partial class SortingInterface : Window {
    protected List<Border> lines = new();

    public SortingInterface(List<int> numbers)
    {
        InitializeComponent();

        int min = numbers.Min();
        double scale = 480.0 / (numbers.Max() - min);
        foreach (int number in numbers) {
            Border line = new() {
                Height = (number - min) * scale,
                Width = 10,
                Background = Brushes.White,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(10, 0, 0, 0),
                Tag = number
            };

            lines.Add(line);

            lineInterface.Children.Add(line);
        }
    }

    public abstract void sort();

    void SortingInterface_OnKeyDown(object sender, KeyEventArgs e)
    {
        sort();
    }
}