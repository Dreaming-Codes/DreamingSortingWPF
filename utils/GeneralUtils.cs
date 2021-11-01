using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace DreamingSortingWPF.utils;

public static class GeneralUtils {
    /// <summary>
    ///     Measure the size of a text
    /// </summary>
    public static Size MeasureTextSize(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch,
                                       double fontSize)
    {
        while (true) {
            Typeface typeface = new(fontFamily, fontStyle, fontWeight, fontStretch);

            if (!typeface.TryGetGlyphTypeface(out GlyphTypeface glyphTypeface)) {
                continue;
            }

            double totalWidth = 0;
            double height = 0;

            foreach (char t in text) {
                ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[t];

                double width = glyphTypeface.AdvanceWidths[glyphIndex] * fontSize;

                double glyphHeight = glyphTypeface.AdvanceHeights[glyphIndex] * fontSize;

                if (glyphHeight > height) {
                    height = glyphHeight;
                }

                totalWidth += width;
            }

            return new Size(totalWidth, height);
        }
    }

    /// <summary>
    ///     Measure the size of a text
    /// </summary>
    public static Size MeasureTextSize(TextBox element)
    {
        return MeasureTextSize(element.Text, element.FontFamily, element.FontStyle, element.FontWeight, element.FontStretch, element.FontSize);
    }

    /// <summary>
    ///     Gets the numbers within a WrapPanel
    /// </summary>
    public static List<int> getNumbers(WrapPanel panel)
    {
        return (from TextBox panelChild in panel.Children select int.Parse(panelChild.Text)).ToList();
    }

    /// <summary>
    ///     Clone a XAML element
    /// </summary>
    public static T CloneXaml<T>(T source)
    {
        string xaml = XamlWriter.Save(source);
        StringReader sr = new(xaml);
        XmlReader xr = XmlReader.Create(sr);
        return (T)XamlReader.Load(xr);
    }

    /// <summary>
    ///     Usage: var timer = SetIntervalThread(DoThis, 1000);
    ///     UI Usage: BeginInvoke((Action)(() =>{ SetIntervalThread(DoThis, 1000); }));
    /// </summary>
    /// <returns>Returns a timer object which can be disposed.</returns>
    public static Timer? SetIntervalThread(Action Act, int Interval)
    {
        return new Timer(_ => { Act(); }, null, 0, Interval);
    }
}