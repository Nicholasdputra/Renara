using UnityEngine;

[CreateAssetMenu(fileName = "ToolSprites", menuName = "ToolSprites")]
public class ToolSpritesSO : ScriptableObject
{
    [Header("Make Unknown Tool Sprite As Last Index!!")]
    public Sprite[] toolSprites;
}