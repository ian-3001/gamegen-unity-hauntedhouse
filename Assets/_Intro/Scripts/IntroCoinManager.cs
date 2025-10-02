using TMPro;
using UnityEngine;

public class IntroCoinManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;

    private int _count = 0;
    
    public void AddCoin()
    {
        _count += 1;

        if (_countText != null)
        {
            _countText.text = $"{_count}";
        }
    }
}
