using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        
        double scale = 880.0 / (numbers.Max() - numbers.Min());
        foreach (int number in numbers) {
            Border line = new() {
                Height = number * scale,
                Width = 10,
                Background = Brushes.White,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(10, 0, 0, 0),
                DataContext = number
            };
            this.lines.Add(line);
            
            lineInterface.Children.Add(line);
        }
    }

    public abstract void sort();
    
    void SortingInterface_OnKeyDown(object sender, KeyEventArgs e)
    {
        sort();
    }
}