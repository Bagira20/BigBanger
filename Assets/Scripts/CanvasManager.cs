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
        public GameObject CanvasElement;
        [HideInInspector]
        public Text UIText;
        public Vector2 Offset;
    }

    [Header("Text Scene Objects")]
    GameManager manager;
    public UIElement LevelTitle, LevelDescription, TimeCounter, LineText, PlayerMassText, TargetText;
    public Image[] rocketCounts = new Image[5];
    public AudioClip[] poofRocketSounds, attachRocketSounds, CancelSounds, SelectSounds, TouchSounds;

    void SetUIElements()
    {
        LevelTitle.CanvasElement.GetComponent<Text>();
        if (LevelTitle.UIText == null)
            LevelTitle.UIText = LevelTitle.CanvasElement.GetComponentInChildren<Text>();

        LevelDescription.CanvasElement.GetComponent<Text>();
        if (LevelDescription.UIText == null)
            LevelDescription.UIText = LevelDescription.CanvasElement.GetComponentInChildren<Text>();

        TimeCounter.CanvasElement.GetComponent<Text>();
        if (TimeCounter.UIText == null)
            TimeCounter.UIText = TimeCounter.CanvasElement.GetComponentInChildren<Text>();

        LineText.CanvasElement.GetComponent<Text>();
        if (LineText.UIText == null)
            LineText.UIText = LineText.CanvasElement.GetComponentInChildren<Text>();

        PlayerMassText.CanvasElement.GetComponent<Text>();
        if (PlayerMassText.UIText == null)
            PlayerMassText.UIText = PlayerMassText.CanvasElement.GetComponentInChildren<Text>();

        TargetText.CanvasElement.GetComponent<Text>();
        if (TargetText.UIText == null)
            TargetText.UIText = TargetText.CanvasElement.GetComponentInChildren<Text>();
    }

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SetUIElements();
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

    public void AttachTextToObject(EUIElements targetedUIElement, GameObject attachGameObject) 
    {
        Vector3 gameObjectPos = attachGameObject.transform.position;
        AttachTextToPosition(targetedUIElement, gameObjectPos);
    }

    public void AttachTextToPosition(EUIElements targetedUIElement, Vector3 worldPosition) 
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), screenPosition, null, out canvasPosition);

        UIElement uiElement = GetUIElement(targetedUIElement);

        SetPositionOfTextObject(uiElement, canvasPosition + uiElement.Offset);
    }


    public void SetPositionOfTextObject(UIElement canvasUIElement, Vector2 newPosition) 
    {
        RectTransform textTransform = canvasUIElement.CanvasElement.GetComponent<RectTransform>();
        textTransform.localPosition = newPosition;
    }
    public void SetRocketCountUI(int rocketCount, float alphaValue) 
    {
        for (int i = 0; i < 5; i++)
        {
            if (i < rocketCount)
                rocketCounts[i].enabled = true;
            else
                rocketCounts[i].enabled = false;
            rocketCounts[i].color = new Color(rocketCounts[i].color.r, rocketCounts[i].color.g, rocketCounts[i].color.b, alphaValue);
        }

        Image rocketIcon = rocketCounts[0].rectTransform.parent.GetComponent<Image>();
        if (rocketIcon != null)
            rocketIcon.color = new Color(rocketIcon.color.r, rocketIcon.color.g, rocketIcon.color.b, alphaValue);
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

    bool IsTargetUIElementParentedByImage(EUIElements targetedUIElement) 
    {
        return targetedUIElement == EUIElements.PlayerMassText || targetedUIElement == EUIElements.TargetText;
    }
}
