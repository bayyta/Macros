using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace project.main
{
    static class CustomUIElements
    {
        public static Grid CreateGrid(double width, double height)
        {
            Grid grid = new Grid();
            grid.Width = width;
            grid.Height = height;
            grid.HorizontalAlignment = HorizontalAlignment.Left;
            grid.VerticalAlignment = VerticalAlignment.Top;

            return grid;
        }

        public static Image CreateImageButton(double width, double height, string relativePath, MouseButtonEventHandler mouseDown, MouseButtonEventHandler mouseUp, MouseEventHandler mouseEnter, MouseEventHandler mouseLeave)
        {
            Image img = new Image();
            img.Width = width;
            img.Height = height;
            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.VerticalAlignment = VerticalAlignment.Center;
            if (mouseDown != null) img.MouseLeftButtonDown += new MouseButtonEventHandler(mouseDown);
            if (mouseUp != null) img.MouseLeftButtonUp += new MouseButtonEventHandler(mouseUp);
            if (mouseEnter != null) img.MouseEnter += new MouseEventHandler(mouseEnter);
            if (mouseLeave != null) img.MouseLeave += new MouseEventHandler(mouseLeave);
            img.Source = new BitmapImage(new Uri(relativePath, UriKind.Relative));

            return img;
        }

        public static Label CreateLabel(string text, double size, Brush brush, int column = 0, int row = 0)
        {
            Label l = new Label();
            Grid.SetColumn(l, column);
            Grid.SetColumnSpan(l, 1);
            Grid.SetRow(l, row);
            Grid.SetRowSpan(l, 1);
            l.Content = text;
            l.FontSize = size;
            l.Foreground = brush;
            l.FontFamily = Application.Current.Resources["main_font"] as FontFamily;
            l.HorizontalAlignment = HorizontalAlignment.Left;
            l.VerticalAlignment = VerticalAlignment.Center;
            l.HorizontalContentAlignment = HorizontalAlignment.Left;
            l.VerticalContentAlignment = VerticalAlignment.Center;

            return l;
        }

        public static Label CreateComboBoxLabel(string text, double width, double height, int column, int row)
        {
            Label l = new Label();
            Grid.SetColumn(l, column);
            Grid.SetColumnSpan(l, 1);
            Grid.SetRow(l, row);
            Grid.SetRowSpan(l, 1);
            l.Width = width;
            l.Height = height;
            l.Content = text;
            l.Foreground = (Brush)Application.Current.Resources["font_color"];
            l.HorizontalAlignment = HorizontalAlignment.Left;
            l.VerticalAlignment = VerticalAlignment.Center;
            l.HorizontalContentAlignment = HorizontalAlignment.Left;
            l.VerticalContentAlignment = VerticalAlignment.Center;
            l.IsHitTestVisible = false;
            l.Focusable = false;
            l.Padding = new Thickness(10, 0, 10, 0);
            l.FontFamily = Application.Current.Resources["main_font"] as FontFamily;
            return l;
        }

        public static ComboBox CreateComboBox(int selectedIndex, string title, System.Collections.IEnumerable source, double width, double height, int column, int row, SelectionChangedEventHandler selectionChangedHandler = null)
        {
            ComboBox cb = new ComboBox();
            Grid.SetColumn(cb, column);
            Grid.SetColumnSpan(cb, 1);
            Grid.SetRow(cb, row);
            Grid.SetRowSpan(cb, 1);            
            cb.Width = width;
            cb.Height = height;
            cb.HorizontalAlignment = HorizontalAlignment.Left;
            cb.VerticalAlignment = VerticalAlignment.Center;
            if (selectionChangedHandler != null) cb.SelectionChanged += new SelectionChangedEventHandler(selectionChangedHandler);
            cb.HorizontalContentAlignment = HorizontalAlignment.Left;
            cb.VerticalContentAlignment = VerticalAlignment.Center;
            ComboBoxItem firstItem = new ComboBoxItem();
            firstItem.Content = title;
            cb.Items.Add(firstItem);
            cb.SelectedIndex = 0;
            foreach (string s in source)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = s;
                cb.Items.Add(cbi);
            }
            cb.FontFamily = Application.Current.Resources["main_font"] as FontFamily;
            cb.SelectedIndex = selectedIndex;

            return cb;
        }

        public static TextBox CreateTextBox(Brush fontColor, Thickness padding, double fontSize, KeyEventHandler keyEventHandler)
        {
            TextBox tb = new TextBox();
            tb.FontSize = fontSize;
            tb.Background = null;
            tb.BorderBrush = null;
            tb.Foreground = fontColor;
            tb.BorderThickness = new Thickness(0);
            tb.Padding = padding;
            tb.FontFamily = Application.Current.Resources["main_font"] as FontFamily;
            tb.CaretBrush = Brushes.White;
            if (keyEventHandler != null) tb.KeyDown += new KeyEventHandler(keyEventHandler);

            return tb;
        }


        public static TextBox CreateActionTextBox(string text, KeyEventHandler keyEventHandler, RoutedEventHandler lostFocusHandler, RoutedEventHandler gotFocusHandler, TextChangedEventHandler textChangedEventHandlere = null)
        {
            TextBox tb = new TextBox();
            tb.Text = text;
            tb.Background = null;
            tb.BorderBrush = null;
            tb.Foreground = (Brush)Application.Current.Resources["accent_color"];
            tb.BorderThickness = new Thickness(0);
            tb.Padding = new Thickness(5, 0, 5, 0);
            tb.FontFamily = Application.Current.Resources["main_font"] as FontFamily;
            tb.CaretBrush = Brushes.White;
            tb.VerticalContentAlignment = VerticalAlignment.Center;
            tb.LostFocus += new RoutedEventHandler(lostFocusHandler);
            tb.GotFocus += new RoutedEventHandler(gotFocusHandler);
            if (textChangedEventHandlere != null) tb.TextChanged += new TextChangedEventHandler(textChangedEventHandlere);
            if (keyEventHandler != null) tb.KeyDown += new KeyEventHandler(keyEventHandler);

            return tb;
        }

        public static Border CreateStandardBackgroundBorder(UIElement child = null)
        {
            Border r = new Border();
            r.Background = (Brush)Application.Current.Resources["dark_color"];
            r.CornerRadius = new CornerRadius(5);
            if (child != null) r.Child = child;

            return r;
        }

        public static Border CreateBackgroundBorder(Brush backgroundBrush, Brush borderBrush, double width, double height, int radius, Thickness borderThickness, UIElement child = null)
        {
            Border r = new Border();
            if (width >= 0) r.Width = width;
            if (height >= 0) r.Height = height;
            if (backgroundBrush != null) r.Background = backgroundBrush;
            if (borderBrush != null) r.BorderBrush = borderBrush;
            r.BorderThickness = borderThickness;
            r.CornerRadius = new CornerRadius(radius);
            if (child != null) r.Child = child;

            return r;
        }

        public static DockPanel CreateDockPanel(int column, int row)
        {
            DockPanel dp = new DockPanel();
            Grid.SetColumn(dp, column);
            Grid.SetColumnSpan(dp, 1);
            Grid.SetRow(dp, row);
            Grid.SetRowSpan(dp, 1);
            dp.LastChildFill = true;

            return dp;
        }

        public static ColumnDefinition CreateColumnDefinition(double value, GridUnitType type)
        {
            ColumnDefinition cd = new ColumnDefinition();
            cd.Width = new GridLength(value, type);

            return cd;
        }

        public static RowDefinition CreateRowDefinition(double value, GridUnitType type)
        {
            RowDefinition rd = new RowDefinition();
            rd.Height = new GridLength(value, type);

            return rd;
        }

    }
}
