using UnityEngine;
using UnityEngine.UI;

public class ProfStatView : MonoBehaviour
{
    [SerializeField] private Image _bkg;
    [SerializeField] private Text _stat;

    public void SetStat(ProfessionType type, int stat)
    {
        _bkg.color = S.GetColor(type);
        _stat.text = stat.ToString();
        _stat.gameObject.SetActive(stat == 0 ? false : true);
    }

    public int GetAmount()
    {
        return int.Parse(_stat.text);
    }
    
}
