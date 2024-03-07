using System.Numerics;
using System.Xml;

namespace DBHandlers
{
    public struct MeaningAndImage
    {
        public string Meaning;
        public string ImagePath;
    }

    internal class WordsDBHandler
    {
        private Dictionary<string, MeaningAndImage> wordDictionary;
        private Dictionary<string, List<string>> categoryDictionary;
        public WordsDBHandler(string path)
        {
            wordDictionary = new();
            categoryDictionary = new();

            XmlDocument xmlDoc = new();
            xmlDoc.Load(path);

            XmlNode root = xmlDoc.DocumentElement;

            if (root?.ChildNodes == null) return;
            foreach (XmlNode node in root.ChildNodes)
            {
                string name = node.Attributes?["name"].Value.ToLower();
                string meaning = node.Attributes?["meaning"].Value.ToLower();
                string imagePath = node.Attributes?["imagePath"].Value.ToLower();
                string category = node.Attributes?["category"].Value.ToLower();
                if (imagePath == "")
                {
                    //TBA: default image
                    imagePath = "noImage.png";
                }
                MeaningAndImage meaningAndImage = new()
                {
                    Meaning = meaning,
                    ImagePath = imagePath
                };

                wordDictionary.Add(name, meaningAndImage);
                if (!categoryDictionary.ContainsKey(category))
                {
                    categoryDictionary.Add(category, []);
                }
                categoryDictionary[category].Add(name);
            }
        }

        public MeaningAndImage GetMeaningAndImage(string word)
        {
            return wordDictionary[word];
        }
        public List<string> GetSimilarWordsWithoutCategory(string prefix)
        {
            return wordDictionary.Keys.Where(word => word.StartsWith(prefix)).ToList();
        }
        public List<string> GetSimilarWordsWithCategory(string prefix, string category)
        {
            return wordDictionary.Keys.Where(word => word.StartsWith(prefix) && categoryDictionary[category].Contains(word)).ToList();
        }
        public List<string> GetCategories()
        {
            return categoryDictionary.Keys.ToList();
        }
    }
}

