using UnityEngine;

[CreateAssetMenu(fileName = "ToolSprites", menuName = "ScriptableObjects/ToolSprites")]
public class ToolSpritesSO : ScriptableObject
{
    [Header("Make Unknown Tool Sprite As Last Index!!")]
    public Sprite[] sprites;
}