using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

namespace pkNX.Game;

public class PokeEditor8a : IDataEditor
{
    public IPersonalTable Personal { get; set; }
    public TableCache<PokeMiscTable8a, PokeMisc8a> PokeMisc { get; set; }
    public TableCache<PokeAIArchive8a, PokeAI8a> SymbolBehave { get; set; }
    public TableCache<EvolutionTable8, EvolutionSet8a> Evolve { get; set; }
    public TableCache<Learnset8a, Learnset8aMeta> Learn { get; set; }
    public TableCache<PokeDropItemArchive8a, PokeDropItem8a> FieldDropTables { get; set; }
    public TableCache<PokeDropItemBattleArchive8a, PokeDropItemBattle8a> BattleDropTabels { get; set; }
    public TableCache<PokedexResearchTable, PokedexResearchTask> DexResearch { get; set; }
    public TableCache<PokeInfoList8a, PokeInfo8a> PokeResourceList { get; set; }
    public TableCache<PokeResourceTable8a, PokeModelConfig8a> PokeResourceTable { get; set; }

    public void CancelEdits() { }

    public void Initialize() { }

    public void Save()
    {
        Personal.Save();
        PokeMisc.Save();
        SymbolBehave.Save();
        Learn.Save();
        Evolve.Save();
        FieldDropTables.Save();
        BattleDropTabels.Save();
        DexResearch.Save();
        PokeResourceList.Save();
        PokeResourceTable.Save();
    }
}