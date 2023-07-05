public class UiSoundEmitter : SoundEmitter
{
    public enum ClipEnum
    {
        ButtonTap
    }

    public void Play(ClipEnum type)
    {
        Play((int)type);
    }
}
