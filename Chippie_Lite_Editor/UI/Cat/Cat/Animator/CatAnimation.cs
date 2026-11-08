using System.Windows.Media;

namespace Chippie_Lite_WPF.UI.Cat.Animator;

public struct CatAnimation
{
    public string Name { get; private set; }
    public int Chance { get; private set; }
    public double Duration { get; private set; }
    public double PlaybackSpeed { get; private set; }
    public ImageSource Source { get; private set; }


    public CatAnimation(string name, double duration, double playbackSpeed, int chance, ImageSource source)
    {
        Name = name;
        Duration = duration;
        PlaybackSpeed = playbackSpeed;
        Chance = chance;
        Source = source;
    }

    public bool GetChance()
    {
        Random random = new Random(DateTime.Now.Second);
        return random.Next(0, 101) < Chance;
    }
}