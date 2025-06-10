using Microsoft.Maui.Controls;
using System.Globalization;
using MauiApp1.Models;

namespace MauiApp1;

public partial class ScorePage : ContentPage, IQueryAttributable
{
    private Score _score;

    public ScorePage()
    {
        InitializeComponent();
        _score = new Score();
    }

    private async void RestartGame(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MenuPage");
        recordLabel.Text = "";
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        double time = 0;
        int count = 0;
        _score.scoreHasChanged = false;

        if (query.TryGetValue("time", out var timeObj))
            time = double.Parse(timeObj.ToString(), CultureInfo.InvariantCulture);

        if (query.TryGetValue("count", out var countObj))
            count = int.Parse(countObj.ToString());

        if (MainPage.Instance.hintGiven == false)
        {
            switch (count)
            {
                case 150:
                    _score.updateRecordFor150(time);
                    break;
                case 200:
                    _score.updateRecordFor200(time);
                    break;
                case 250:
                    _score.updateRecordFor250(time);
                    break;
            }
        }

        timeLabel.Text = $"Your time: {time:F2} s";

        if(_score.scoreHasChanged)
        {
            recordLabel.Text = "New record!";
        }
        else
        {
            recordLabel.Text = "";
        }

        recordTable.Text = GenerateScoreTable(time, count);
    }

    string GenerateScoreTable(double currentTime, int count)
    {
        var scores = new Dictionary<int, double>
        {
            { 150, _score.RecordTimeFor150 },
            { 200, _score.RecordTimeFor200 },
            { 250, _score.RecordTimeFor250 }
        };

        string result = "Record Times:\n";

        foreach (var kvp in scores)
        {
            int smileyCount = kvp.Key;
            double recordTime = kvp.Value;

            string recordText;

            if (recordTime == 0)
            {
                recordText = "empty";
            }
            else
            {
                recordText = $"{recordTime:F2} s";
            }

            result += $"{smileyCount} smileys: {recordText}\n";
        }

        return result;
    }
}
