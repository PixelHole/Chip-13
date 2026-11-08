using System.Timers;
using System.Windows;
using Chippie_Lite_WPF.Computer.Utility;
using yt_unzipper;
using Timer = System.Timers.Timer;

namespace Chippie_Lite_WPF.UI.Cat;

public class CatContainerControl
{
    private CatContainer View { get; }
    private string[] Names = ["Unloaded"];
    
    private Cat?[] Cats { get; } = new Cat[12];
    private int Count { get; set; }
    private Timer UpdateTimer { get; } = new Timer(TimeSpan.FromMilliseconds(50));

    private bool Running = true;

    public CatContainerControl(CatContainer view)
    {
        View = view;
        ConnectEvents();
        LoadNames();
        StartTimers();
    }
    private void ConnectEvents()
    {
        Application.Current.Exit += CurrentOnExit;
    }
    private void CurrentOnExit(object sender, ExitEventArgs e)
    {
        UpdateTimer.Stop();
        UpdateTimer.Close();
        UpdateTimer.Dispose();
    }
    private void StartTimers()
    {
        UpdateTimer.Elapsed += UpdateTimerOnElapsed;
        UpdateTimer.Start();
    }
    private void UpdateTimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        UpdateTimer.Stop();
        ProcessCats();
        UpdateTimer.Start();
    }


    private void ProcessCats()
    {
        for (int i = 0; i < Count; i++)
        {
            var cat = Cats[i];
            if (cat == null) break;
            cat.Control.CatBrain();
        }
    }
    
    private void LoadNames()
    {
        var raw = EmbeddedResourceUtility.ReadEmbeddedResource("Chippie_Lite_WPF.Resources.Data.CatNames.csv");
        Names = raw.Split(',', StringSplitOptions.TrimEntries);
    }
    
    public void SpawnCat(string name = "")
    {
        if (Count == Cats.Length) return;
        Cat cat = new Cat(name == "" ? GetRandomName() : name, View);
        Cats[Count] = cat;
        Count++;
        WPFUtility.DoUiAction(View.CatCanvas, () => View.CatCanvas.Children.Add(cat));
    }
    public void DespawnCat()
    {
        if (Count == 0) return;
        var cat = Cats[Count - 1];
        Cats[Count - 1] = null;
        if (cat != null) Count--;
        WPFUtility.DoUiAction(View.CatCanvas, () => View.CatCanvas.Children.Remove(cat));
    }
    public void DespawnAllCats()
    {
        for (int i = 0; i < Count; i++)
        {
            if (Cats[i] == null) break;
            Cats[i]!.StopProcesses();
            WPFUtility.DoUiAction(View.CatCanvas, () => View.CatCanvas.Children.Remove(Cats[i]!));
        }
    }

    private string GetRandomName()
    {
        Random random = new Random(DateTime.Now.Millisecond);
        return Names[random.Next(0, Names.Length)];
    }
}