using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using DreamingSortingWPF.utils;

namespace updatedDreamingSortingWPF.algorithms;

public class SelectionSort : SortingInterface {
    int currentIteration = 0;

    public SelectionSort(List<int> numbers) : base(numbers)
    {
    }

    public override void sort()
    {   
        if(currentIteration >= lines.Count) {
            return;
        }
        
        Border min = lines[currentIteration];
        foreach (Border line in lines)  {
            line.Background = Brushes.White;
        }
        lines[currentIteration].Background = Brushes.Red;
        foreach (Border line in lines.Skip(currentIteration+1)) {
            line.Background = Brushes.Red;
            if ((int)line.Tag < (int)min.Tag) {
                min = line;
            }
        }
        GeneralUtils.SwapBorderProperties(lines[currentIteration], min);
        currentIteration++;
    }
}