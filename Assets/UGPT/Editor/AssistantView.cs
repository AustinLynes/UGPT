using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEngine.UIElements;
using UnityEditor.UIElements;

using OpenAI_API;
using System.Threading.Tasks;
using OpenAI_API.Chat;


public struct ConversationBubble
{
    public bool isUser;
    public string message;
}

public class AssistantView : EditorWindow
{
    [MenuItem("Open AI/GPT Assistant")]
    public static void Open()
    {
        if(instance != null)
        {
            Debug.LogError("Cannot Open Multiple Instances of GPT assistant view");
            return;
        }

        instance = GetWindow<AssistantView>();
    }

    OpenAIAPI api;

    private void OnEnable()
    {
        api = new OpenAIAPI("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");

        if (api == null)
            Debug.Log("<color=red>FATAL ERROR</color> OpenAI Connection Could not be made...");
    }

    private void OnDisable()
    {
        
    }


    private void CreateGUI()
    {

#if DEBUG
        var dbg_elements = new VisualElement();

        dbg_elements.Add(new Label("_DEBUG_")); 
        var resetButton = new Button(() => { 
            all_prompts.Clear(); 
            prompts.Rebuild();
            chats.Clear();
            all_chats.Clear();
        });
        resetButton.text = "Reset";
        dbg_elements.Add(resetButton);

        rootVisualElement.Add(dbg_elements);
        
#endif

        rootVisualElement.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UGPT/Editor/assistant_window.uss"));

        topBottom = new TwoPaneSplitView(0, position.height * 0.95f, TwoPaneSplitViewOrientation.Vertical);
        
        topBottom.Add(ChatsView());
        topBottom.Add(PromptSubmissionField());
       
        rootVisualElement.Add(topBottom);    
    }

    VisualElement ChatsView()
    {   
        var leftRight = new TwoPaneSplitView(0, position.width * 0.2f, TwoPaneSplitViewOrientation.Horizontal);

        //all_chats.Add();


        chats = new ListView();
        chats.makeItem = () => new Label();
        chats.bindItem = (item, idx) => { (item as Label).text = idx.ToString(); };
        chats.itemsSource = all_prompts;

        var addChatButton = new Button(() => {
            //all_chats.Add();
            chats.Rebuild();
        });


        addChatButton.text = "new chat";
        var chatsContainer = new VisualElement();

        chatsContainer.Add(addChatButton);
        chatsContainer.Add(chats);

        leftRight.Add(chatsContainer);

        prompts = new ListView();
        //prompts.fixedItemHeight = 100; 
        prompts.name = "current_chat";
        prompts.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
        prompts.makeItem = () => {
            return new ChatBubble();
        };

        prompts.bindItem = (item, idx) => { 
        
            (item as ChatBubble).message.text = all_prompts[idx].message;
            (item as ChatBubble).name = all_prompts[idx].isUser ? "ChatBubble_User" : "ChatBubble_Assistant"; 
          
        };
        prompts.itemsSource = all_prompts;
        
        leftRight.Add(prompts);

        return leftRight;
    }


    VisualElement PromptSubmissionField()
    {
        var inputArea = new VisualElement();
        inputArea.name = "inputArea";

        // Create the Text Field to Submit Questions and Statements to the GPT Client
        var iarea_tf = new TextField("", 250, true, false, '%');
        iarea_tf.value = "message";
       
        iarea_tf.name = "prompt_field";
        inputArea.Add(iarea_tf);
        iarea_tf.RegisterValueChangedCallback(
            (message) => {
                currentPrompt = message.newValue;
            });
        
        var iarea_btn = new Button();
        iarea_btn.text = "Submit";
        iarea_btn.name = "prompt_submit_btn";
        iarea_btn.clicked += async () =>
        {
            all_prompts.Add(new ConversationBubble(){ isUser=true, message=currentPrompt, });
            prompts.Rebuild();

            var curChat = api.Chat.CreateConversation();
            curChat.AppendUserInput(currentPrompt);
            var res = await curChat.GetResponseFromChatbot();

            all_prompts.Add(new ConversationBubble(){ isUser=false, message=res, });
            prompts.Rebuild();
            
            
            currentPrompt = string.Empty;


        };
        inputArea.Add(iarea_btn);
        
        return inputArea;
    }


    TwoPaneSplitView topBottom;

    string currentPrompt = string.Empty;
    
    ListView chats;
    List<Conversation> all_chats = new List<Conversation>();
    
    ListView prompts;
    List<ConversationBubble> all_prompts = new List<ConversationBubble>();

    static AssistantView instance;// only one instance of assistant view can be opened at a time
}
