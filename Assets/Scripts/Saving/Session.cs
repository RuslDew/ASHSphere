public class Session
{
    public string SessionName { get; private set; }
    public float GameDuration;
    public string History;


    public Session(string sessionName, string history = "", float gameDuration = 0f)
    {
        GameDuration = gameDuration;
        SessionName = sessionName;
        History = history;
    }
}
