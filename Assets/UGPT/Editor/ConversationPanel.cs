using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using OpenAI_API;
using System.Threading.Tasks;
using OpenAI_API.Chat;

public class ConversationBubble
{
    public bool isUser;
    public string message;
}

public class ConversationPanel : VisualElement
{
    private ListView conversationListView;
    private List<ConversationBubble> conversationMessages;
    private TextField inputTextField;
    private OpenAIAPI api;

    public ConversationPanel(OpenAIAPI api, float height)
    {
        this.api = api;
        name = "conversation_panel";
        styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UGPT/Editor/conversation_panel.uss"));

        conversationMessages = new List<ConversationBubble>();

        conversationListView = new ListView(conversationMessages, 48, MakeChatBubble, BindChatBubble);
        conversationListView.name = "current_chat";
        conversationListView.selectionType = SelectionType.None;
        conversationListView.style.flexGrow = 1;
        conversationListView.style.overflow = Overflow.Visible;

        var root = new TwoPaneSplitView(0, height, TwoPaneSplitViewOrientation.Vertical);
        root.name = "conversation_panel";
        root.Add(conversationListView);
        root.Add(MessageSubmissionField());
        Add(root);
    }

    VisualElement MessageSubmissionField()
    {
        var inputArea = new VisualElement();
        inputArea.name = "inputArea";

        inputTextField = new TextField();
        inputTextField.multiline = false;
        inputTextField.RegisterCallback<KeyUpEvent>(OnInputKeyUp);

        inputArea.Add(inputTextField);
        inputArea.Add(new Button(SubmitMessage) { text = "Submit" });

        return inputArea;
    }

    void OnInputKeyUp(KeyUpEvent evt)
    {
        if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
        {
            SubmitMessage();
        }
    }

    async void SubmitMessage()
    {
        string message = inputTextField.value;
        if (!string.IsNullOrEmpty(message))
        {
            AddConversationBubble(true, message);
            inputTextField.value = "";

            var conversation = api.Chat.CreateConversation();
            conversation.AppendUserInput(message);

            string response = await conversation.GetResponseFromChatbot();
            AddConversationBubble(false, response);
        }
    }

    VisualElement MakeChatBubble()
    {
        return new ChatBubble();
    }

    void BindChatBubble(VisualElement element, int index)
    {
        if (element is ChatBubble chatBubble && index < conversationMessages.Count)
        {
            ConversationBubble conversationBubble = conversationMessages[index];
            chatBubble.UpdateMessage(conversationBubble.message);
            chatBubble.SetIsUser(conversationBubble.isUser);
        }
    }

    void AddConversationBubble(bool isUser, string message)
    {
        conversationMessages.Add(new ConversationBubble { isUser = isUser, message = message });
        conversationListView.Rebuild();
        conversationListView.ScrollToItem(conversationMessages.Count - 1);
    }
}
