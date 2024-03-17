using System.Numerics;
using System.Xml;

namespace DBHandlers
{
    public struct MeaningAndImage
    {
        public string Meaning;
        public string ImagePath;
    }

    public class WordsDBHandler
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
                    imagePath = "tba";
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

        public KeyValuePair<string,MeaningAndImage> GetWord(string? word)
        {
            return wordDictionary.FirstOrDefault(pair => pair.Key == word);
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

        public bool AddWord(string word, string category, string definition, string imagePath)
        {
            word = word.ToLower();
            if (wordDictionary.ContainsKey(word))
            {
                return false;
            }

            if (imagePath == "")
            {
                //TBA: default image
                imagePath = "tba";
            }

            if (!categoryDictionary.ContainsKey(category))
            {
                categoryDictionary.Add(category, []);
            }
            categoryDictionary[category].Add(word);

            var meaningAndImage = new MeaningAndImage()
            {
                Meaning = definition,
                ImagePath = imagePath
            };
            wordDictionary.Add(word, meaningAndImage);
            return UpdateXML();
        }

        private bool UpdateXML()
        {
            XmlDocument xmlDoc = new();
            XmlNode rootNode = xmlDoc.CreateElement("Words");
            xmlDoc.AppendChild(rootNode);

            foreach (var word in wordDictionary)
            {
                XmlNode wordNode = xmlDoc.CreateElement("Word");
                XmlAttribute nameAttribute = xmlDoc.CreateAttribute("name");
                nameAttribute.Value = word.Key;
                wordNode.Attributes.Append(nameAttribute);

                XmlAttribute meaningAttribute = xmlDoc.CreateAttribute("meaning");
                meaningAttribute.Value = word.Value.Meaning;
                wordNode.Attributes.Append(meaningAttribute);

                XmlAttribute imagePathAttribute = xmlDoc.CreateAttribute("imagePath");
                imagePathAttribute.Value = word.Value.ImagePath[1..]; // Remove the first character of the image path
                wordNode.Attributes.Append(imagePathAttribute);

                XmlAttribute categoryAttribute = xmlDoc.CreateAttribute("category");
                categoryAttribute.Value = categoryDictionary.FirstOrDefault(pair => pair.Value.Contains(word.Key)).Key;
                wordNode.Attributes.Append(categoryAttribute);

                rootNode.AppendChild(wordNode);
            }

            xmlDoc.Save("../../../WordsDB.xml");
            return true;

        }
    }
}

