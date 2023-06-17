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
    [SerializeField] private float messageFadoutTime;
    [SerializeField] private float messageHeight;
    private LinkedList<Message> messageList = new LinkedList<Message>();

    [SerializeField] private TMP_InputField chatInput;
    [SerializeField] private float gapSize;
    [SerializeField] private Image handle;

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
        TMP_Text newMessageText = newMessage.GetComponentInChildren<TMP_Text>();
        if (newMessageText != null)
        {
            newMessageText.text = messageText;
        }

        // Add the message to the list
        Message message = newMessage.GetComponent<Message>();
        if (message != null)
        {
            // If the message list is full, remove the oldest message
            if (messageList.Count >= maxMessageCount)
            {
                Message oldestMessage = messageList.First.Value;
                messageList.RemoveFirst();
                Destroy(oldestMessage.gameObject);
            }

        }

        // Update the layout of the chat content
        StartCoroutine(UpdateChatLayout(message, newMessageText));
    }

    private IEnumerator UpdateChatLayout(Message message, TMP_Text text)
    {
        // Wait for the next frame to ensure the layout is updated correctly
        yield return null;

        float bound = (text.bounds.size.y - (text.bounds.size.y % messageHeight));
        if (bound < messageHeight) bound = messageHeight;
        message.rectTransform.sizeDelta = new Vector2(message.rectTransform.sizeDelta.x, bound + 16);
        messageList.AddLast(message);
        message.SetInactive(messageFadoutTime);

        LinkedListNode<Message> current = messageList.Last;
        float currentHeight = 0;
        while (current != null)
        {
            current.Value.rectTransform.localPosition = new Vector2(current.Value.rectTransform.localPosition.x, currentHeight);
            currentHeight += current.Value.rectTransform.sizeDelta.y + gapSize;
            current = current.Previous;
        }

        content.sizeDelta = new Vector2(content.sizeDelta.x, currentHeight);
    }

    public void OpenChat()
    {
        handle.enabled = true;
        eventSystem.SetSelectedGameObject(chatInput.gameObject);
        foreach (var message in messageList)
        {
            message.SetActive();
        }
    }

    public void CloseChat()
    {
        handle.enabled = false;
        eventSystem.SetSelectedGameObject(null);
        foreach (var message in messageList)
        {
            message.SetInactive(messageFadoutTime);
        }
    }
}