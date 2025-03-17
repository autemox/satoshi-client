using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


public class SpriteSheetAsset
{
    public string name { get; private set; }
    public Texture2D texture { get; private set; }
    public Sprite[] sprites { get; private set; }
    public int rows { get; private set; }
    public int columns { get; private set; }
    
    public SpriteSheetAsset(string name, Texture2D texture, Sprite[] sprites, int rows, int columns)
    {
        this.name = name;
        this.texture = texture;
        this.sprites = sprites;
        this.rows = rows;
        this.columns = columns;
    }
}