using Calculator.Parser.Expressions;

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

    private void OnButtonClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        string value = button.Text;

        value = value switch
        {
            "×" => "*",
            "÷" => "/",
            _ => value
        };

        _content += value;

        DisplayLabel.Text = _content;
    }

    private void OnEqualsClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_content))
            return;

        try
        {
            decimal result = _calculator.Calculate(_content);

            _content = result.ToString();
            DisplayLabel.Text = _content;
        }
        catch
        {
            DisplayLabel.Text = "Error";
        }
    }

    private void OnClearClicked(object? sender, EventArgs e)
    {
        _content = "";
        DisplayLabel.Text = "0";
    }

    private async void OnBackspacePressed(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_content))
            return;

        _isDeleting = true;

        int delay = 200;

        while (_isDeleting && _content.Length > 0)
        {
            _content = _content[..^1];

            DisplayLabel.Text =
                string.IsNullOrEmpty(_content)
                    ? "0"
                    : _content;

            await Task.Delay(delay);

            if (delay > 100)
                delay -= 10;
        }
    }

    private void OnBackspaceReleased(object? sender, EventArgs e)
    {
        _isDeleting = false;
    }
}