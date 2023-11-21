using System;

public class Session
{
    public string SessionName { get; private set; }
    public DateTime SessionStartDate { get; private set; }
    public string History { get; private set; }


    public Session(DateTime startDate, string sessionName)
    {
        SessionStartDate = startDate;
        SessionName = sessionName;
    }

    public void SaveHistory(string history)
    {
        History = history;
    }
}
