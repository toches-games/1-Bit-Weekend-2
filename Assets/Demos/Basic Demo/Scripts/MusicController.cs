class MusicController
{
    static MusicController _instance;

    public static MusicController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MusicController();
            }

            return _instance;
        }
    }

    FMOD.Studio.EventInstance AllMusicGame;

    MusicController()
    {
        AllMusicGame = FMODUnity.RuntimeManager.CreateInstance("event:/Music");
    }

    public void Play()
    {
        AllMusicGame.start();
    }

    public void Stop()
    {
        AllMusicGame.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayIntro()
    {
        AllMusicGame.setParameterByName("Music", 0);
    }

    public void PlayGame()
    {
        AllMusicGame.setParameterByName("Music", 1);
    }

    public void PlayWheel()
    {
        AllMusicGame.setParameterByName("Music", 2);
    }

    public void PlayGoodCard()
    {
        AllMusicGame.setParameterByName("Music", 3);
    }

    public void PlayNormalCard()
    {
        AllMusicGame.setParameterByName("Music", 4);

    }
    public void PlayBadCard()
    {
        AllMusicGame.setParameterByName("Music", 5);

    }
}