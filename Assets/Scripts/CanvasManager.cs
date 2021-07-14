using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EUIElements
{
    LevelTitle,
    LevelDescription,
    TimeCounter,
    LineText,
    PlayerMassText,
    TargetText
}

public class CanvasManager : MonoBehaviour
{
    [System.Serializable]
    public struct UIElement
    {
        public Text UIText;
        public Vector2 UIOffset;
    }

    [Header("Text Scene Objects")]
    GameManager manager;
    public UIElement LevelTitle, LevelDescription, TimeCounter, LineText, PlayerMassText, TargetText;


    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SetLevelTitle((manager.levelIntroNr+1).ToString());
        SetLevelDescription(LevelIntroDisplays.LevelIntroText[manager.levelIntroNr]);
    }

    public void SetLevelTitle(string newID)
    {
        LevelTitle.UIText.text = "LEVEL " + newID;
    }

    public void SetLevelDescription(string newDescription)
    {
        LevelDescription.UIText.text = newDescription;
    }

    public void SetTimeCounter(float time)
    {
        TimeCounter.UIText.text = Mathf.FloorToInt(time).ToString();
    }

    public void SetLineText(string newLineText) 
    {
        LineText.UIText.text = newLineText;
    }

    public void SetPlayerMassText(float mass) 
    {
        PlayerMassText.UIText.text = "m = " + mass + " kg";
    }

    public void SetTargetText(float mass, float velocity) 
    {
        TargetText.UIText.text = "m = " + mass + " kg\nv = " + velocity + " m/s";
    }

    public void AttachTextToObject(EUIElements targetedUIElement, GameObject gameObject) 
    {
        UIElement uiElement = GetUIElement(targetedUIElement);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), screenPosition, null, out canvasPosition);
        
        SetPositionOfTextObject(uiElement, canvasPosition + uiElement.UIOffset);
    }

    public void SetPositionOfTextObject(UIElement gameObjectElement, Vector2 newPosition) 
    {
        gameObjectElement.UIText.gameObject.GetComponent<RectTransform>().localPosition = newPosition;
    }

    UIElement GetUIElement(EUIElements pendingElement) 
    {
        switch (pendingElement) {
            default:
            case EUIElements.LevelTitle:
                return LevelTitle; 
            case EUIElements.LevelDescription:
                return LevelDescription;
            case EUIElements.TimeCounter:
                return TimeCounter;
            case EUIElements.LineText:
                return LineText;
            case EUIElements.PlayerMassText:
                return PlayerMassText;
            case EUIElements.TargetText:
                return TargetText;
        }
    }
}
