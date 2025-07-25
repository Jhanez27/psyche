using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkDialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> variables;

    public InkDialogueVariables(Story story)
    {
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach(string name in story.variablesState)
        {
            Ink.Runtime.Object value = story.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
        }
    }


    public void SyncVariablesAndStartListening(Story story)
    {
        SyncVariableToStory(story);
        story.variablesState.variableChangedEvent += UpdateVariableState;
    }
    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= UpdateVariableState;
    }
    public void UpdateVariableState(string name, Ink.Runtime.Object value)
    {
        if(variables.ContainsKey(name))
        {
            variables[name] = value;


            Debug.Log("Updated dialogue variable: " + name + " to " + value.ToString());
        }
    }

    private void SyncVariableToStory(Story story)
    {
        foreach(KeyValuePair<string, Ink.Runtime.Object> kvp in variables)
        {
            story.variablesState.SetGlobal(kvp.Key, kvp.Value);
        }
    }    

    public bool GetBoolVariableState(string name)
    {
        if (variables.ContainsKey(name) && variables[name] is BoolValue boolValue) // Checks if the key is in variables and value is a boolean
        {
            return boolValue.value; // Access the actual boolean value
        }
        else
        {
            Debug.LogWarning($"Variable '{name}' is not a boolean or does not exist.");
            return false; // Default value if the variable is not found or not a boolean
        }
    }
}
