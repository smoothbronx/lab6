using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections;
using System.Windows;
using System.Linq;
using lab6.Stack;
using System;
using System.Collections.Generic;

namespace lab6;

public partial class MainWindow
{
    private readonly ArrayList _arrayList = new();
    private readonly SinglyStack<double> _stack = new();
    private readonly List<double> _list = new();
    private readonly Random _random = new();


    public MainWindow()
    {
        InitializeComponent();
        OutputArrayList.ItemsSource = _arrayList;
        _stack.Enqueue(46.5);
        _stack.Enqueue(3.4);
        _stack.Enqueue(32.4);
        _stack.Enqueue(-3.21);
        UpdateStack();
        _stack.Dequeue();
        _stack.Enqueue(5.0);
        UpdateStack();
    }

    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Add(sender, e);
        }
    }

    private void Delete(object sender, RoutedEventArgs e)
    {
        if (OutputArrayList.SelectedIndex < 0) return;

        _arrayList.RemoveAt(OutputArrayList.SelectedIndex);
        OutputArrayList.Items.Refresh();
        RefreshStatus();
    }

    private void RefreshStatus()
    {
        var copyOfNumbers = (double[])_arrayList.ToArray(typeof(double));

        Status.Visibility = copyOfNumbers.Any(number => number > 10)
            ? Visibility.Visible
            : Visibility.Hidden;
    }

    private void UpdateStack()
    {
        OutputStack.Items.Clear();
        foreach (var item in _stack)
        {
            var listItem = new ListBoxItem
            {
                Content = item
            };

            OutputStack.Items.Add(listItem);
        }

        UpdateSum();
    }

    private void Add(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(TextBoxInput.Text))
        {
            SetError("Необходимо ввести число");
            return;
        }


        var inputStringArray = TextBoxInput.Text.Split(";");

        foreach (var inputString in inputStringArray)
        {
            var success = double.TryParse(inputString, out var value);

            if (!success)
            {
                TextBoxInput.Clear();
                SetError("Данные некорректны");
                continue;
            }

            if (value is not (>= 2 and <= 13))
            {
                TextBoxInput.Clear();
                SetError("Число не лежит в интервале");
                continue;
            }

            _arrayList.Add(value);
        }

        TextBoxInput.Clear();
        OutputArrayList.Items.Refresh();
        RefreshStatus();
    }

    private void NumberInputGotFocus(object sender, RoutedEventArgs e)
    {
        ErrorOut.Text = "";
        TextBoxInput.BorderBrush = Brushes.Black;
        ErrorOut.Margin = new Thickness();
        ErrorOut.Padding = new Thickness();
        ErrorOut.Visibility = Visibility.Hidden;
    }

    private void SetError(string message)
    {
        ErrorOut.Text = message;
        TextBoxInput.BorderBrush = Brushes.DarkRed;
        ErrorOut.Margin = new Thickness(0, 0, 0, 10);
        ErrorOut.Padding = new Thickness(5);
        ErrorOut.Visibility = Visibility.Visible;
    }

    private void Clear(object sender, RoutedEventArgs e)
    {
        _arrayList.Clear();
        OutputArrayList.Items.Refresh();
        RefreshStatus();
    }

    private void PushToStack(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(TextBoxStackInput.Text))
        {
            TextBoxStackInput.BorderBrush = Brushes.DarkRed;
            TextBoxStackInput.Clear();
            return;
        }

        if (!double.TryParse(TextBoxStackInput.Text, out var number))
        {
            TextBoxStackInput.BorderBrush = Brushes.DarkRed;
            TextBoxStackInput.Clear();
            return;
        }

        _stack.Enqueue(number);
        UpdateStack();
        TextBoxStackInput.Clear();
    }

    private void UpdateSum()
    {
        var elementsSum = Array
            .ConvertAll(_stack.ToArray(), Math.Abs)
            .Where(number => number > 12)
            .Sum();
        ElementsSum.Text = elementsSum.ToString("F");
    }

    private void PopToStack(object sender, RoutedEventArgs e)
    {
        _stack.Dequeue();
        UpdateStack();
    }

    private void ClearList(object sender, RoutedEventArgs e)
    {
        for (var index = 0; index < _list.Count; index++)
        {
            var number = _list[index];

            if (number is <= 10 or >= 20) continue;
            if (index == 0) continue;

            _list.RemoveAt(index - 1);
            index--;
        }

        UpdateListTextBox();
    }

    private void InsertToList(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(TextBoxListInput.Text))
        {
            TextBoxListInput.BorderBrush = Brushes.DarkRed;
            TextBoxListInput.Clear();
            return;
        }

        if (!double.TryParse(TextBoxListInput.Text, out var number))
        {
            TextBoxListInput.BorderBrush = Brushes.DarkRed;
            TextBoxListInput.Clear();
            return;
        }

        _list.Add(number);
        UpdateListTextBox();
        TextBoxStackInput.Clear();
    }

    private void UpdateListTextBox()
    {
        OutputList.Items.Clear();
        foreach (var item in _list)
        {
            var listItem = new ListBoxItem
            {
                Content = item
            };

            OutputList.Items.Add(listItem);
        }
    }

    private void FillList(object sender, RoutedEventArgs e)
    {
        for (var _ = 0; _ < _random.Next(1, 10); _++)
        {
            var number = Math.Round(_random.NextDouble() * 13, 3);
            if (number is > 2 and < 13) _arrayList.Add(number);
        }

        RefreshStatus();
        OutputArrayList.Items.Refresh();
    }
}