using UnityEngine;
using UnityEngine.UI;

public sealed class EntryModel
{
    public readonly string Rank, Player, Value;
    public EntryModel(string rank, string player, string value)
    {
        Rank = rank;
        Player = player;
        Value = value;
    }
}

public class Entry : MonoBehaviour
{
    [SerializeField] private Text rank;
    [SerializeField] private Text player;
    [SerializeField] private Text value;

    public void Set(EntryModel model)
    {
        rank.text = model.Rank;
        player.text = model.Player;
        value.text = model.Value;
    }
}
