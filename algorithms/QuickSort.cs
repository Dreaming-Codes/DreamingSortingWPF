using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using DreamingSortingWPF.utils;

namespace updatedDreamingSortingWPF.algorithms;

public class QuickSort : SortingInterface {
    int endIndex;
    Stack<KeyValuePair<int, int>> indexToOrder = new();
    int startIndex;

    public QuickSort(List<int> numbers) : base(numbers)
    {
        endIndex = lines.Count - 1;
        indexToOrder.Push(new KeyValuePair<int, int>(startIndex, endIndex));
    }

    public override void sort()
    {

        if (indexToOrder.Count != 0) {
            (startIndex, endIndex) = indexToOrder.Pop();

            foreach (Border border in lines) {
                border.Background = Brushes.White;
            }

            int pivotIndex = pivotSort(startIndex, endIndex);

            if (pivotIndex - 1 > startIndex) {
                indexToOrder.Push(new KeyValuePair<int, int>(startIndex, pivotIndex - 1));
            }

            if (pivotIndex + 1 < endIndex) {
                indexToOrder.Push(new KeyValuePair<int, int>(pivotIndex + 1, endIndex));
            }


        }
    }

    int pivotSort(int startIndex, int endIndex)
    {
        Border pivot = lines[endIndex];
        pivot.Background = Brushes.Red;

        int pivotIndex = startIndex;

        //Itero fra tutti gli elementi apparte l'ultimo elemento per evitare il pivot alla prima iterazione
        for (int i = startIndex; i < endIndex; i++) {
            if ((int)lines[i].Tag <= (int)pivot.Tag) {
                GeneralUtils.SwapBorderProperties(lines[i], lines[pivotIndex]);
                pivotIndex++;
            }
        }

        GeneralUtils.SwapBorderProperties(lines[pivotIndex], lines[endIndex]);
        return pivotIndex;
    }
}