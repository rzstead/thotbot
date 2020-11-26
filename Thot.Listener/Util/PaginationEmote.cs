using System.Collections.Generic;

public class PaginationEmote
{
    public string EmoteValue { get; }
    private PaginationEmote(string emoteValue)
    {
        EmoteValue = emoteValue;
    }

    public static PaginationEmote Back => new PaginationEmote("\u25C0");
    public static PaginationEmote Forward => new PaginationEmote("\u25B6");
    public static List<PaginationEmote> Emotes = new List<PaginationEmote> { Back, Forward };
}