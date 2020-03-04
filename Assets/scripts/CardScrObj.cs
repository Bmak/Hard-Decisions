using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 0)]
public class CardScrObj : ScriptableObject {
    
    public new string name;
    public string description;
    public Sprite artwork;

    public string affectedVital;
    public int affectAmount;

    public void CopyFrom(CardScrObj source)
    {
        name = source.name;
        description = source.description;
        artwork = source.artwork;

        affectedVital = source.affectedVital;
        affectAmount = source.affectAmount;
    }

    public CardScrObj DeepCopy()
    {
        CardScrObj obj = Object.Instantiate(this);
        return obj;
    }
}

