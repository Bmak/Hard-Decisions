using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitalView : MonoBehaviour
{
    [SerializeField] private Text _statTextPrefab;
    [SerializeField] private Transform _statContainer;
    [SerializeField] private VitalService _vitalService;

    private List<Text> _vitals = new List<Text>();

    private void Start()
    {
        _statTextPrefab.gameObject.SetActive(true);

        foreach (VitalStat stat in _vitalService.Stats)
        {
            Text vital = Instantiate(_statTextPrefab, _statContainer);
            int f = stat.GetCurrentAmount();
            vital.text = string.Format("{0}: {1}", stat.VType.ToString(), f);

            _vitals.Add(vital);
            stat.Text = vital;
        }

        _statTextPrefab.gameObject.SetActive(false);

        _vitalService.OnVitalUpdated += UpdateVitals;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var item = S.VitalService.Stats[Random.Range(0, S.VitalService.Stats.Count)];
            S.VitalService.AddBuff(item.VType, "123", 1);
            S.AnimManager.SetPhase(SolGamePhase.Checkpoint);
        }
    }

    public void UpdateVitals()
    {
        int i = 0;
        foreach (VitalStat stat in _vitalService.Stats)
        {
            Text vital = stat.Text;
            int f = stat.GetCurrentAmount();
            vital.text = string.Format("{0}: {1}", stat.VType.ToString(), f);
            i++;
        }
    }

}
