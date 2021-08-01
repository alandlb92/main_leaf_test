using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartCounterUI : MonoBehaviour
{
    [SerializeField] private Image _imageReference;
    [SerializeField] private Sprite _outlineHeartImageReferece;
    [SerializeField] private Sprite _heartImageReferece;
    private GridLayoutGroup _gridLayout;
    private List<Image> _hearts = new List<Image>();

    private void Awake()
    {
        _gridLayout = GetComponent<GridLayoutGroup>();
    }

    public void SetHealth(int maxHealth, int currentHealth)
    {
        BuildHealthList(maxHealth);
        for(int i = 0;i < _hearts.Count;i++)
        {
            if(currentHealth > i)
            {
                _hearts[i].sprite = _heartImageReferece;
            }
            else
            {
                _hearts[i].sprite = _outlineHeartImageReferece;
            }
        }
    }

    private void BuildHealthList(int maxHealth)
    {
        if (_hearts.Count == maxHealth)
            return;

        _hearts.ForEach(h => Destroy(h.gameObject));
        _hearts = new List<Image>();

        _imageReference.gameObject.SetActive(false);
        for (int i = 0; i < maxHealth; i++)
        {
            Image instance = Instantiate(_imageReference, this.transform);
            instance.gameObject.SetActive(true);
            _hearts.Add(instance);
        }
        _gridLayout.constraintCount = _hearts.Count;
    }
}
