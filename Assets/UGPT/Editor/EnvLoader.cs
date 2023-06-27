using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class EnvLoader
{
    static Dictionary<string, string> variables = new Dictionary<string, string>();

    public static bool LoadEnviormentVariables()
    {
        string path = Path.Combine(Application.dataPath,"..", ".env");

        if (!File.Exists(path))
        {
            Debug.LogWarning("Env File Doesnt Exist!");
            return false;
        }

        string[] lines  = File.ReadAllLines(path);

        // clear anything in the table prev stored
        variables = new Dictionary<string, string>();

        foreach (string line in lines) {

            // ignore empty lines or '#' commented lines
            if (string.IsNullOrEmpty(line) || line.StartsWith('#'))
                continue;

            string[] parts = line.Split('=');

            if(parts.Length == 2)
            {
                variables[parts[0]] = parts[1];
            }

        }

        return true;
    }

    public static string GetVariable(string name)
    {
        if (variables.ContainsKey(name))
            return variables[name];
        else 
            return null;
    }

}
