using System.Collections.Generic;

namespace pkNX.Structures;

public class VsTrainer
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public TrainerData Self { get; set; }
    public readonly List<TrainerPoke> Team = new(6);

    public VsTrainer(TrainerData self) => Self = self;

    public TrainerClass GetClass(IList<TrainerClass> list) => list[Self.Class];
}
