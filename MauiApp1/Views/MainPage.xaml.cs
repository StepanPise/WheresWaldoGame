using MauiApp1.Models;

namespace MauiApp1;

public partial class MainPage : ContentPage, IQueryAttributable
{
    public static MainPage Instance { get; private set; }

    int smileyCount = 150;
    const int columns = 10;
    List<Smiley> smileys = new();
    private DateTime startTime;
    public Smiley Waldo;
    public bool hintGiven = false;

    string[] colorCircleImages = new[]
    {
        "blue.png",
        "green.png",
        "orange.png",
        "pink.png",
        "red.png",
        "white.png",
        "yellow.png"
    };

    string[] hatImages = new[]
    {
        "hat1.png",
        "hat2.png",
        "hat3.png",
        "hat4.png",
        "hat5.png",
        "hat6.png",
        "hat7.png"
    };

    string[] accessoryImages = new[]
    {
        "clown_nose.png",
        "glasses.png",
        "monocle.png",
        "mustache.png",
        "nose.png",
        "mustache_b.png"
    };

    Dictionary<string, string> imageToColorName = new()
    {
        { "blue.png", "blue" },
        { "green.png", "green" },
        { "orange.png", "orange" },
        { "pink.png", "pink" },
        { "red.png", "red" },
        { "white.png", "white" },
        { "yellow.png", "yellow" }
    };

    public MainPage()
    {
        InitializeComponent();

        if (Instance == null)
        {
            Instance = this;
           // Preferences.Clear();
        }
    }
    private async void GiveHint(object sender, EventArgs e)
    {
        hintGiven = true;
        string fileName = Path.GetFileName(Waldo.BackgroundImage);
        imageToColorName.TryGetValue(fileName, out string color);

        var formatted = new FormattedString();
        formatted.Spans.Add(new Span { Text = $"The Dud is {color}!" });
        formatted.Spans.Add(new Span { Text = " (Run is no longer eligible for the scoreboard)", TextColor = Colors.Gray });
        hintLabel.FormattedText = formatted;
        hintLabel.IsVisible = true;

        await Task.CompletedTask;
    }

    private async void GiveUp(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MenuPage");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("count", out var countObj) && int.TryParse(countObj.ToString(), out var count))
        {
            smileyCount = count;
        }

        GameReset();
        GenerateSmileys();
        PopulateGrid();
    }

    void GameReset()
    {
        startTime = DateTime.Now;
        hintGiven = false;
        hintLabel.Text = "";
    }

    void GenerateSmileys()
    {
        smileys.Clear();
        var random = new Random();
        int waldoIndex = random.Next(smileyCount);

        for (int i = 0; i < smileyCount; i++)
        {
            bool isWaldo = i == waldoIndex;

            var bgImage = colorCircleImages[random.Next(colorCircleImages.Length)];
            string hat = hatImages[random.Next(hatImages.Length)];
            string accessory = accessoryImages[random.Next(accessoryImages.Length)];

            var smiley = new Smiley(isWaldo ? "waldo.png" : "generic.png", hat, accessory, isWaldo, bgImage);

            if (smiley.IsWaldo)
            {
                Waldo = smiley;
            }

            smileys.Add(smiley);
        }
    }

    void PopulateGrid()
    {
        smileyGrid.RowDefinitions.Clear();
        smileyGrid.ColumnDefinitions.Clear();
        smileyGrid.Children.Clear();

        for (int i = 0; i < columns; i++)
            smileyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        int rows = (int)Math.Ceiling((double)smileyCount / columns);
        for (int i = 0; i < rows; i++)
            smileyGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

        for (int i = 0; i < smileys.Count; i++)
        {
            var smiley = smileys[i];

            var grid = new Grid();

            var background = new Image
            {
                Source = smiley.BackgroundImage,
                Aspect = Aspect.AspectFit,

            };

            var face = new Image
            {
                Source = smiley.FaceImage,
                Aspect = Aspect.AspectFit,

            };

            grid.Children.Add(background);
            grid.Children.Add(face);

            if (!string.IsNullOrEmpty(smiley.AccessoryImage))
            {
                var accessory = new Image
                {
                    Source = smiley.AccessoryImage,
                    Aspect = Aspect.AspectFit,

                };
                grid.Children.Add(accessory);
            }

            if (!string.IsNullOrEmpty(smiley.HatImage))
            {
                var hat = new Image
                {
                    Source = smiley.HatImage,
                    Aspect = Aspect.AspectFit,
                };
                grid.Children.Add(hat);
            }

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) =>
            {
                if (smiley.IsWaldo)
                {
                    var elapsed = DateTime.Now - startTime;
                    var timeInSeconds = elapsed.TotalSeconds;
                    await Shell.Current.GoToAsync($"//ScorePage?time={timeInSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture)}&count={smileyCount}");
                }
                else
                {
                    await DisplayAlert("Wrong!", "That's not the Dud.", "Try again");
                }
            };
            grid.GestureRecognizers.Add(tapGesture);

            int row = i / columns;
            int col = i % columns;
            smileyGrid.Add(grid, col, row);
        }
    }
}
