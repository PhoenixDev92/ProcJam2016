using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public enum TileType { Black, White, Green, Red };

    [Header("Sprite Settings")]
    [SerializeField]
    private Sprite blackTile;
    [SerializeField]
    private Sprite whiteTile;
    [SerializeField]
    private Sprite greenTile;
    [SerializeField]
    private Sprite redTile;

    private TileType type;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    public TileType Type
    {
        get { return type; }
        set
        {
            type = value;
            UpdateSprite();
        }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void UpdateSprite()
    {
        switch (Type)
        {
            case TileType.Black:
                spriteRenderer.sprite = blackTile;
                break;
            case TileType.White:
                spriteRenderer.sprite = whiteTile;
                break;
            case TileType.Green:
                spriteRenderer.sprite = greenTile;
                break;
            case TileType.Red:
                spriteRenderer.sprite = redTile;
                break;
            default:
                spriteRenderer.sprite = blackTile;
                break;
        }
    }
}
