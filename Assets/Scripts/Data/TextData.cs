using System;
using System.Collections.Generic;
using UnityEngine;
using DDTest.Extensions;

[CreateAssetMenu(fileName = "TextData", menuName = "TextData")]
public class TextData : ScriptableObject
{
    public List<TextTemplate> buttonTexts;

    public string GetTooltipText(ButtonShape button)
    {
        return buttonTexts.Find(x => button == x.buttonShape).tooltipText;
    }

    public string GetPopupText(ButtonShape button)
    {
        return buttonTexts.Find(x => button == x.buttonShape).popupText;
    }
}

[Serializable]
public class TextTemplate
{
    public ButtonShape buttonShape;
    public string tooltipText;
    public string popupText;
}

[Serializable]
public enum ButtonShape
{
    Square,
    Circle,
    Hexagon
}
