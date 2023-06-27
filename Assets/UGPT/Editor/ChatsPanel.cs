using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using OpenAI_API.Chat;

public class ChatsPanel : VisualElement
{
    public ChatsPanel()
    {
        styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UGPT/Editor/chats_panel.uss"));
        name = "chats_panel";

        // Create Chat Button
        var addChatButton = new Button(AddChat);
        addChatButton.text = "New Chat";
        Add(addChatButton);

        // Create Chats ListView
        var chatsList = new ListView();
        chatsList.name = "chats_list";

        chatsList.makeItem = () => new Label();
        chatsList.bindItem = (item, idx) => { (item as Label).text = idx.ToString(); };
        chatsList.itemsSource = chatData;
        Add(chatsList);
    }

    void AddChat()
    {
    }

    List<Conversation> chatData = new List<Conversation>();
}
