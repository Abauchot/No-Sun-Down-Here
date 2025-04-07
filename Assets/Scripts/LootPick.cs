using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class LootPick : MonoBehaviour
{
    public enum LootType
    {
        Food, 
        Utilities,
        Meds,
        Money,
    }
    
    public LootType lootType;
    public int lootAmount = 1;
    
    [Header("Possible sprites")]
    public List<Sprite> lootSprites;
    
    private void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (lootSprites.Count > 0 && sr != null)
        {
            sr.sprite = lootSprites[Random.Range(0, lootSprites.Count)];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Picked up {lootAmount} {lootType}");
            Destroy(gameObject);
        }
    }
}
