using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootController : MonoBehaviour
{
    public enum LootType
    {
        ARROW,
        LIFE
    }

    [SerializeField] private LootInstanceController _lootInstanceReference;
    [SerializeField] private int _maxLootSimultaneous = 10;
    private List<LootInstanceController> _loots = new List<LootInstanceController>();
    private int _lootsNextIndex = 0;

    public void DisableAllLoots()
    {
        _loots.ForEach(l =>
        {
            l.Enable(false);
        });
    }

    public void NewRandomLoot(Vector3 position)
    {
        int lootIndex = UnityEngine.Random.Range(0, Enum.GetValues(typeof(LootType)).Length);
        LootType lootType = (LootType) lootIndex;
        if(_loots.Count >= _maxLootSimultaneous)
        {
            _loots[_lootsNextIndex].Initialize(lootType, position);
            _lootsNextIndex++;
        }
        else
        {
            LootInstanceController instance = Instantiate(_lootInstanceReference);
            instance.Initialize(lootType, position);
            _loots.Add(instance);
        }
    }
}
