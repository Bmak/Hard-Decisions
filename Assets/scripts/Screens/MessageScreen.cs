using System;
using UnityEngine;
using UnityEngine.UI;

public class MessageScreen : MonoBehaviour
{
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _captionText;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Text _closeButtonText;

    private MessageDef _def;

    public void SetData(MessageDef def)
    {
        _def = def;
        _titleText.text = def.Title;
        _captionText.text = def.Caption;
        _closeButtonText.text = def.ButtonTextTitle;

        _closeButton.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        _def.ButtonAction();
        Gui.Close(this);
    }
}

public class MessageDef
{
    public string Title = "Title";
    public string Caption = "Caption";
    public Action ButtonAction = () => { };
    public string ButtonTextTitle = "Close";
}
