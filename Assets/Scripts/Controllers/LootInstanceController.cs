using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootInstanceController : MonoBehaviour
{
    [Header("Refereces")]
    [SerializeField] private Sprite _arrowSpriteReference;
    [SerializeField] private Sprite _lifeSpriteReference;
    [SerializeField] private Image _imageReference;
    [SerializeField] private TMP_Text _howMuchText;
    private SphereCollider _trigger;
    private Canvas _canvas;
    private LootAudioController _audioController;

    [Header("SetUp")]
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _height;
    [Range(1,10)]
    [SerializeField] private int _maxArrowPerLoot;

    private LootController.LootType _lootType;
    private int _arrowsPerLoot;

    public void Initialize(LootController.LootType type, Vector3 position)
    {
        Awake();

        this.transform.position = position;
        _lootType = type;

        switch (_lootType)
        {
            case LootController.LootType.ARROW:
                _imageReference.sprite = _arrowSpriteReference;
                _howMuchText.gameObject.SetActive(true);
                _arrowsPerLoot = Random.Range(1, _maxArrowPerLoot + 1);
                _howMuchText.text = "x" + _arrowsPerLoot;
                break;
            case LootController.LootType.LIFE:
                _howMuchText.gameObject.SetActive(false);
                _imageReference.sprite = _lifeSpriteReference;
                break;
        }

        Enable(true);
        _audioController.PopUp();
        StartCoroutine(PopUp());
    }

    public void Enable(bool enable)
    {
        _trigger.enabled = enable;
        _canvas.enabled = enable;
    }

    private void Awake()
    {
        _trigger = GetComponent<SphereCollider>();
        _canvas = GetComponentInChildren<Canvas>();
        _audioController = GetComponent<LootAudioController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            _audioController.PickUp();
            Enable(false);

            switch (_lootType)
            {
                case LootController.LootType.ARROW:
                    playerController.AddArrow(_arrowsPerLoot);
                    break;
                case LootController.LootType.LIFE:
                    playerController.AddLife(1);
                    break;
            }
        }
    }

    private IEnumerator PopUp()
    {

        float totalTime = _animationCurve.keys.Last().time;
        Vector3 startPosition = this.transform.position;

        for (float t = 0; t < totalTime; t += Time.deltaTime)
        {
            this.transform.position = startPosition + (Vector3.up * _height * _animationCurve.Evaluate(t));
            yield return null;
        }
        this.transform.position = startPosition;
    }
}
