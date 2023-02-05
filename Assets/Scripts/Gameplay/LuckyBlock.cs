using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ItemType
{
    MUSHROOM,
    FIREFLOWER
}

public class LuckyBlock : TileBase
{
    public ItemType type = ItemType.MUSHROOM;

#if UNITY_EDITOR
    [MenuItem("Assets/Create/LuckyBlock")]
    public static void CreateLuckyBlock()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save LuckyBlock Tile", "New LuckyBlock Tile", "Asset", "Save LuckyBlock Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<LuckyBlock>(), path);
    }
#endif
}
