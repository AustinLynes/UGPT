using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using OpenAI_API;

//public class ConversationBubble
//{
//    public bool isUser;
//    public string message;
//}

public class AssistantView : EditorWindow
{
    [MenuItem("Open AI/GPT Assistant")]
    public static void Open()
    {
        if (instance != null)
        {
            Debug.LogError("Cannot Open Multiple Instances of GPT assistant view");
            return;
        }

        instance = GetWindow<AssistantView>();
        instance.titleContent = new GUIContent("Unity|ChatGPT", AssetDatabase.LoadAssetAtPath<Texture>("Assets/ugpt_logo_white.png"));
    }

    OpenAIAPI api;

    private void CreateGUI()
    {
        InitChatGPT();

        rootVisualElement.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UGPT/Editor/assistant_window.uss"));

        rootVisualElement.Add(CreateView());
    }

    void InitChatGPT()
    {
        bool success = EnvLoader.LoadEnviormentVariables();
        if (!success)
        {
            Debug.LogError("NO ENV!");
            return;
        }

        string apiKey = EnvLoader.GetVariable("GPT_KEY");

        api = new OpenAIAPI(apiKey);

        if (api == null)
            Debug.Log("<color=red>FATAL ERROR</color> OpenAI Connection Could not be made...");

    }

    VisualElement CreateView()
    {
        var root = new TwoPaneSplitView(0, position.width * 0.2f, TwoPaneSplitViewOrientation.Horizontal);
        root.name = "chat_view";

        root.Add(new ChatsPanel());
        root.Add(new ConversationPanel(api, position.height * 0.95f));

        return root;
    }


    static AssistantView instance; // only one instance of assistant view can be opened at a time
}
