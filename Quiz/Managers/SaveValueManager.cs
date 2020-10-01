using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;


namespace Quiz.Managers
{
    public static class SaveValueManager
    {
        const string fileName = "savedValue.json";
        public static void SaveValues(Dictionary<string, int> values)
        {
            string json = JsonSerializer.Serialize<Dictionary<string, int>>(values);
            using (var streamWriter = new StreamWriter(fileName))
            {
                streamWriter.Write(json);
                streamWriter.Close();
            } 
        }

        public static Dictionary<string, int> LoadValues(int valueSize)
        {
            try
            {
                using (var streamReader = new StreamReader(fileName))
                {
                    var json = streamReader.ReadToEnd();
                    return JsonSerializer.Deserialize<Dictionary<string, int>>(json);
                }
            }
            catch
            {
                return Enumerable.Range(1, valueSize).ToDictionary(x => x.ToString(), x => -1);
            }
        }
    }
}