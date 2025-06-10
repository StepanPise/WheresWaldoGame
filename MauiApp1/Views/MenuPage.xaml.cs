namespace MauiApp1;

public partial class MenuPage : ContentPage
{
    private double _currentScale = 1.0;
    private const double ScaleStep = 0.1;
    private const double MinScale = 0.5;
    private const double MaxScale = 3.0;

    public MenuPage()
    {
        InitializeComponent();
    }

    private async void OnStartClicked(object sender, EventArgs e)
    {
        if (smileyCountPicker.SelectedItem != null)
        {
            int selectedCount = (int)smileyCountPicker.SelectedItem;
            await Shell.Current.GoToAsync($"//MainPage?count={selectedCount}");
        }
        else
        {
            await DisplayAlert("Error", "Please select a count before starting.", "OK");
        }
    }
}
