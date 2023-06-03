using System;
using System.Collections.Generic;
using UnityEngine;
using RS.Extensions;

[CreateAssetMenu(fileName = "ColorData", menuName = "ColorData")]
public class ColorData : ScriptableObject
{
    public List<ColorTemplate> colors;

    public ColorTemplate GetColorTemplate(ColorName currentColor)
    {
        return colors.Find(x => currentColor == x.colorName);
    }

    public ColorTemplate GetRandomColorTemplate()
    {
        return colors.GetRandomElement();
    }
}

[Serializable]
public class ColorTemplate
{
    public ColorName colorName;
    public ColorName nextColorName;
    public Color currentColor;
    public Color nextColor;
}

[Serializable]
public enum ColorName
{
    RED,
    GREEN,
    BLUE
}
