using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatBubble : VisualElement
{
    public ChatBubble()
    {
        // Create a chat bubble container element
        var root = new VisualElement();
        root.name = "root";
        root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UGPT/Editor/chat_bubbles.uss"));


        var bubble = new VisualElement();
        bubble.name = "bubble";

        message = new Label();
        message.name = "label";
        message.text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas tincidunt nulla quam, ut faucibus lectus lobortis id. Sed non ipsum justo. Fusce id eros sed tellus tincidunt pulvinar et id sem. Donec fringilla, tortor a sagittis tincidunt, odio mauris elementum erat, in placerat tortor nisi et diam.";
        
        // build tree element
        bubble.Add(message);    
        root.Add(bubble);
        Add(root);

        
    }

    public Label message;

}
