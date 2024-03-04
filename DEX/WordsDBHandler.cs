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
        private Dictionary<string, MeaningAndImage> dictionary;
        public WordsDBHandler(string path)
        {
            dictionary = new();

            XmlDocument xmlDoc = new();
            xmlDoc.Load(path);

            XmlNode root = xmlDoc.DocumentElement;

            if (root?.ChildNodes == null) return;
            foreach (XmlNode node in root.ChildNodes)
            {
                string name = node.Attributes?["name"].Value.ToLower();
                string meaning = node.Attributes?["meaning"].Value.ToLower();
                string imagePath = node.Attributes?["imagePath"].Value.ToLower();
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

                dictionary.Add(name, meaningAndImage);
            }
        }

        public MeaningAndImage GetMeaningAndImage(string word)
        {
            return dictionary[word];
        }
        public List<string> GetSimilarWords(string prefix)
        {
            return dictionary.Keys.Where(word => word.StartsWith(prefix)).ToList();
        }
        

    }
}

