using Microsoft.Maui.Storage;

public class Score
{
    public double RecordTimeFor150 { get; set; }
    public double RecordTimeFor200 { get; set; }
    public double RecordTimeFor250 { get; set; }

    const string Key150 = "RecordTimeFor150";
    const string Key200 = "RecordTimeFor200";
    const string Key250 = "RecordTimeFor250";

    public bool scoreHasChanged = false;
    public Score()
    {
        Load();
    }

    public void updateRecordFor150(double time)
    {
        scoreHasChanged = false;
        if (RecordTimeFor150 == 0 || time < RecordTimeFor150)
        {
            scoreHasChanged = true;
            RecordTimeFor150 = time;
            Save(Key150, time);
        }
    }

    public void updateRecordFor200(double time)
    {
        scoreHasChanged = false;

        if (RecordTimeFor200 == 0 || time < RecordTimeFor200)
        {
            scoreHasChanged = true;
            RecordTimeFor200 = time;
            Save(Key200, time);
        }
    }

    public void updateRecordFor250(double time)
    {
        scoreHasChanged = false;

        if (RecordTimeFor250 == 0 || time < RecordTimeFor250)
        {
            scoreHasChanged = true;
            RecordTimeFor250 = time;
            Save(Key250, time);
        }
    }

    private void Save(string key, double value)
    {
        Preferences.Set(key, value);
    }

    public void Load()
    {
        RecordTimeFor150 = Preferences.Get(Key150, 0.0);
        RecordTimeFor200 = Preferences.Get(Key200, 0.0);
        RecordTimeFor250 = Preferences.Get(Key250, 0.0);
    }
}
