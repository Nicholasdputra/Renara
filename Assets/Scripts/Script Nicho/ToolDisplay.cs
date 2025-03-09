using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolDisplay : MonoBehaviour
{
    public ToolSO tool;
    public Image image;

    public string toolName;
    public string toolDescription;
    public Sprite toolSprite;

    void Start()
    {
        name = tool.toolName;
        toolName = tool.toolName;
        toolDescription = tool.toolDescription;
        toolSprite = tool.toolSprite;
        image = GetComponent<Image>();
        image.sprite = toolSprite; 
    }
}
