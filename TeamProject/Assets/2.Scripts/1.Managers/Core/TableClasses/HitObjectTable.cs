using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class HitObjectTable : LowBase
{
    public override void Load(string jsonName)
    {
        TextAsset jsonFile = Resources.Load("Tables/" + jsonName, typeof(TextAsset)) as TextAsset;
        if (jsonFile != null)
        {
            string jsonContent = jsonFile.ToString();
            JsonData jsonData = JsonMapper.ToObject(jsonContent);
            if (jsonData != null)
            {
                foreach (string sheetName in jsonData.Keys)
                {
                    foreach (JsonData record in jsonData[sheetName])
                    {
                        foreach (string columnNmae in record.Keys)
                        {
                            if (columnNmae != "Index")
                            {
                                Add(record["Index"].ToString(), columnNmae, record[columnNmae].ToString());
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.LogError($"Failed to parse {jsonName} data.");
            }
        }
        else
        {
            Debug.LogError($"JSON file {jsonName} not found!");
        }
    }
}
