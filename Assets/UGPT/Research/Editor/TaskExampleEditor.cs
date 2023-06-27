using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;

public class TaskExampleEditor : EditorWindow
{
    private Task<string> task;
    private bool isTaskCompleted;

    [MenuItem("Window/Task Example")]
    private static void ShowWindow()
    {
        TaskExampleEditor window = GetWindow<TaskExampleEditor>();
        window.titleContent = new GUIContent("Task Example");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(20f);

        if (GUILayout.Button("Start Task"))
        {
            // Start the asynchronous task
            task = GetStringAsync();
            isTaskCompleted = false;

            // Start the coroutine to handle the completion of the task
            EditorApplication.update += UpdateCoroutine;
        }

        GUILayout.Space(20f);

        // Display the task result if it has completed
        if (isTaskCompleted)
        {
            if (task.IsFaulted)
            {
                EditorGUILayout.HelpBox("An error occurred: " + task.Exception.InnerException.Message, MessageType.Error);
            }
            else if (task.IsCanceled)
            {
                EditorGUILayout.HelpBox("Task was canceled.", MessageType.Warning);
            }
            else
            {
                string result = task.Result;
                EditorGUILayout.HelpBox("Result: " + result, MessageType.Info);
            }
        }
    }

    private async Task<string> GetStringAsync()
    {
        await Task.Delay(1000); // Simulating an asynchronous operation
        return "Hello, world!";
    }

    private void UpdateCoroutine()
    {
        // Check if the task has completed
        if (task != null && task.IsCompleted)
        {
            // Set the flag to indicate task completion
            isTaskCompleted = true;

            // Stop the coroutine by removing the update delegate
            EditorApplication.update -= UpdateCoroutine;

            // Repaint the Editor window to update the GUI
            Repaint();
        }
    }
}
