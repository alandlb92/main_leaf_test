using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootInstanceController : MonoBehaviour
{

    [SerializeField] private LootController.LootType _lootType;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _height;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            StartCoroutine(PopUp());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            switch (_lootType)
            {
                case LootController.LootType.ARROW:
                    playerController.AddArrow(1);
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
