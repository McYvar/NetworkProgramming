using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    private float timer;

    [SerializeField] private float fadeOutDuration;
    [SerializeField] private Image messageBackground;
    [SerializeField] private TMPro.TMP_Text messageText;
    [SerializeField] public RectTransform rectTransform;

    private float messageBackGroundAlpha;
    private float messageTextAlpha;

    private bool active;

    private void Start()
    {
        messageBackGroundAlpha = messageBackground.color.a;
        messageTextAlpha = messageText.color.a;
        timer = fadeOutDuration;
    }

    public void SetActive()
    {
        active = true;
        messageBackground.color = new Color(messageBackground.color.r, messageBackground.color.g, messageBackground.color.b, messageBackGroundAlpha);
        messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, messageTextAlpha);
    }

    public void SetInactive(float time)
    {
        active = false;
        timer = time;
    }

    private void Update()
    {
        if (!active)
        {
            if (timer > 0) timer -= Time.deltaTime;
            messageBackground.color = new Color(messageBackground.color.r, messageBackground.color.g, messageBackground.color.b, Mathf.Clamp(Mathf.InverseLerp(0, fadeOutDuration, timer), 0, messageBackGroundAlpha));
            messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, Mathf.Clamp(Mathf.InverseLerp(0, fadeOutDuration, timer), 0, messageTextAlpha));
        }
    }


}
