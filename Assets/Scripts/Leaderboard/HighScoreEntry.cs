public class HighScoreEntry
{
    private string name;
    private int score;

    public string Name { get { return name; } set { name = value; } }
    public int Score { get { return score; } set { score = value; } }

    public HighScoreEntry() { }

    public HighScoreEntry(string name, int score)
    {
        Name = name;
        Score = score;
    }
}
