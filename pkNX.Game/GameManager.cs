using System;
using pkNX.Containers;
using pkNX.Structures;
using static pkNX.Structures.GameVersion;

namespace pkNX.Game;

/// <summary>
/// Manages fetching of game data.
/// </summary>
public abstract class GameManager
{
    protected readonly GameLocation ROM;
    protected readonly TextManager Text; // GameText
    protected readonly GameFileMapping FileMap;
    public readonly GameInfo Info;
    private int _language;

    public string PathExeFS => ROM.ExeFS ?? throw new ArgumentException("ExeFS path not set.");
    public string PathRomFS => ROM.RomFS ?? throw new ArgumentException("RomFS path not set.");

    /// <summary>
    /// Language to use when fetching string &amp; graphic assets.
    /// </summary>
    public int Language
    {
        get => _language;
        set
        {
            if (value == _language)
                return;
            _language = value;
            Text.ClearCache();
        }
    }

    /// <summary>
    /// Current <see cref="GameVersion"/> the data represents.
    /// </summary>
    public GameVersion Game => ROM.Game;

    /// <summary>
    /// Initializes a new <see cref="GameManager"/> for the input <see cref="GameLocation"/> with initial <see cref="Language"/>.
    /// </summary>
    /// <param name="rom"></param>
    /// <param name="language"></param>
    protected GameManager(GameLocation rom, int language)
    {
        ROM = rom;
        FileMap = new GameFileMapping(rom);
        Text = new TextManager(Game);
        Info = new GameInfo(Game);
        Language = language;
    }

    /// <summary>
    /// Fetches a <see cref="GameFile"/> from the Game data.
    /// </summary>
    /// <param name="file">File type to fetch</param>
    /// <returns>Container that contains the game data requested.</returns>
    /// <remarks>Sugar for the other <see cref="GetFile"/> method.</remarks>
    public IFileContainer this[GameFile file] => GetFile(file);

    /// <summary>
    /// Fetches a <see cref="GameFile"/> from the Game data.
    /// </summary>
    /// <param name="file">File type to fetch</param>
    /// <returns>Container that contains the game data requested.</returns>
    public IFileContainer GetFile(GameFile file) => FileMap.GetFile(file, Language, this);

    /// <summary>
    /// Fetches strings for the input <see cref="TextName"/>.
    /// </summary>
    /// <param name="text">Text file to fetch</param>
    /// <returns>Array of strings from the requested text file.</returns>
    public string[] GetStrings(TextName text)
    {
        var arc = this[GameFile.GameText];
        var lines = Text.GetStrings(text, arc);
        return lines;
    }

    /// <summary>
    /// Saves all open files and finalizes the ROM data.
    /// </summary>
    /// <param name="closing">Skip re-initialization of game data.</param>
    public void SaveAll(bool closing)
    {
        Terminate();
        FileMap.SaveAll();
        if (!closing)
            Initialize();
    }

    public virtual void Initialize()
    {
        SetMitm();
    }

    protected abstract void Terminate();
    protected abstract void SetMitm();

    public FolderContainer GetFilteredFolder(GameFile type, Func<string, bool>? filter = null)
    {
        var c = (FolderContainer)this[type];
        c.Initialize(filter);
        return c;
    }

    public static GameManager GetManager(GameLocation loc, int language) => loc.Game switch
    {
        GP or GE or GG   => new GameManagerGG(loc, language),
        SW or SH or SWSH => new GameManagerSWSH(loc, language),
        PLA              => new GameManagerPLA(loc, language),
        SL or VL or SV   => new GameManagerSV(loc, language),
        _ => throw new ArgumentException(nameof(loc.Game)),
    };
}
