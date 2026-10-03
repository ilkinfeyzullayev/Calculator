using Calculator.Parser.Expressions;
using System.Text.RegularExpressions;

namespace Calculator;

public partial class MainPage : ContentPage
{
    private string _content = "";
    private readonly Calculator _calculator = new();

    private bool _isDeleting;

    public MainPage()
    {
        InitializeComponent();
    }

    private string OutputResult()
    {
        string expression = _content.Replace('×', '*').Replace('÷', '/');

        decimal result = _calculator.Calculate(expression);

        return result.ToString();
    }

    private async Task KeypadButtonClicked(object? sender)
    {
        if (sender is not Button button)
            return;

        Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(75));

        await button.ScaleToAsync(0.97, 50);
        await button.ScaleToAsync(1.0, 50);
    }

    private void OnButtonClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        string value = button.Text;

        _content += value;

        DisplayLabel.Text = _content;

        try
        {
            ResultLabel.Text = _content != "" ? $"= {OutputResult()}" : "";
        } catch
        {
            ResultLabel.Text = "";
        }

        _ = KeypadButtonClicked(button);
    }

    private void OnEqualsClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button || string.IsNullOrWhiteSpace(_content))
            return;

        try
        {
            _content = OutputResult();
            DisplayLabel.Text = _content;
        }
        catch
        {
            DisplayLabel.Text = "Error";
            _content = "";
        }

        _ = KeypadButtonClicked(button);
    }

    private async void OnBackspacePressed(object? sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        _isDeleting = true;

        int delay = 150;

        while (_isDeleting && _content.Length > 0)
        {
            _content = _content[..^1];

            DisplayLabel.Text =
                string.IsNullOrEmpty(_content)
                    ? "0"
                    : _content;

            try
            {
                ResultLabel.Text = _content != "" ? $"= {OutputResult()}" : "";
            }
            catch
            {
                ResultLabel.Text = "= ";
            }

            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(50));

            await Task.Delay(delay);

            if (delay > 50)
                delay -= 10;
        }

        _ = KeypadButtonClicked(button);
    }

    private void OnBackspaceReleased(object? sender, EventArgs e)
    {
        _isDeleting = false;
    }
}