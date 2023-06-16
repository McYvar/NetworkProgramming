using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class ChatBehaviour : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject scrollView;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private int maxMessageCount = 10; // Maximum number of messages in the chat
    [SerializeField] private float messagesFadoutTimer;
    private float timer;
    private LinkedList<RectTransform> messageList = new LinkedList<RectTransform>();

    [SerializeField] private TMP_InputField chatInput;
    [SerializeField] private float gapSize;

    private bool active = false;

    private void Start()
    {
        SessionVariables.instance.myGameClient.chatBehaviour = this;
    }

    public void SendMessageToServer()
    {
        if (chatInput.text.Length > 0)
        {
            string text = $"{SessionVariables.instance.myPlayerName}: {chatInput.text}";
            SessionVariables.instance.myGameClient.SendToServer(new Net_ChatMessage(text));
            chatInput.text = "";
        }
    }

    public void SendChatMessage(string messageText)
    {
        // Create a new message instance from the prefab
        GameObject newMessage = Instantiate(messagePrefab, content);

        // Set the text of the message
        TMP_Text newMessageText = newMessage.GetComponent<TMP_Text>();
        if (newMessageText != null)
        {
            newMessageText.text = messageText;
        }

        // Add the message to the list
        RectTransform messageTransform = newMessage.GetComponent<RectTransform>();
        if (messageTransform != null)
        {
            // If the message list is full, remove the oldest message
            if (messageList.Count >= maxMessageCount)
            {
                RectTransform oldestMessage = messageList.First.Value;
                messageList.RemoveFirst();
                Destroy(oldestMessage.gameObject);
            }

        }

        // Update the layout of the chat content
        StartCoroutine(UpdateChatLayout(messageTransform, newMessageText));
    }

    private IEnumerator UpdateChatLayout(RectTransform messageTransform, TMP_Text text)
    {
        // Wait for the next frame to ensure the layout is updated correctly
        yield return null;

        messageTransform.sizeDelta = new Vector2(messageTransform.sizeDelta.x, text.bounds.size.y);
        messageList.AddLast(messageTransform);

        LinkedListNode<RectTransform> current = messageList.Last;
        float currentHeight = 0;
        while (current != null)
        {
            current.Value.localPosition = new Vector2(current.Value.localPosition.x, currentHeight);
            currentHeight += current.Value.sizeDelta.y + gapSize;
            current = current.Previous;
        }

        content.sizeDelta = new Vector2(content.sizeDelta.x, currentHeight);
    }

    public void OpenChat()
    {
        scrollView.SetActive(true);
        eventSystem.SetSelectedGameObject(chatInput.gameObject);
        active = true;
    }

    public void CloseChat()
    {
        eventSystem.SetSelectedGameObject(null);
        timer = messagesFadoutTimer;
        active = false;
    }

    private void Update()
    {
        if (!active)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else scrollView.SetActive(false);
        }
    }
}