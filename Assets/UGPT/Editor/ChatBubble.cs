using UnityEditor;
using UnityEngine.UIElements;

public class ChatBubble : VisualElement
{
    private Label messageLabel;

    public ChatBubble()
    {
        styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UGPT/Editor/chat_bubbles.uss"));
        AddToClassList("chat-bubble");

        messageLabel = new Label();
        Add(messageLabel);
    }

    public void UpdateMessage(string message)
    {
        messageLabel.text = message;
    }

    public void SetIsUser(bool isUser)
    {
        if (isUser)
        {
            AddToClassList("user");
            RemoveFromClassList("assistant");
        }
        else
        {
            AddToClassList("assistant");
            RemoveFromClassList("user");
        }
    }
}
