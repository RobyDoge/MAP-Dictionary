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
                string imagePath = node.Attributes?["imagePath"].Value;
                string category = node.Attributes?["category"].Value.ToLower();


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

        public KeyValuePair<string, MeaningAndImage> GetWord(string? word)
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
                imagePath = "Images/DEFAULT.PNG";
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
                imagePathAttribute.Value = word.Value.ImagePath[0] == '/' ? word.Value.ImagePath[1..] : word.Value.ImagePath;
                wordNode.Attributes.Append(imagePathAttribute);

                XmlAttribute categoryAttribute = xmlDoc.CreateAttribute("category");
                categoryAttribute.Value = categoryDictionary.FirstOrDefault(pair => pair.Value.Contains(word.Key)).Key;
                wordNode.Attributes.Append(categoryAttribute);

                rootNode.AppendChild(wordNode); 
            }

            xmlDoc.Save("../../../WordsDB.xml");
            return true;

        }

        public bool WordExists(string word)
        {
            return wordDictionary.ContainsKey(word);
        }

        public List<string> GetWordInfo(string word)
        {
            List<string> wordInfo =
            [
                word,
                categoryDictionary.FirstOrDefault(pair => pair.Value.Contains(word)).Key,
                wordDictionary[word].Meaning,
                wordDictionary[word].ImagePath
            ];

            return wordInfo;
        }

        public bool ModifyWord(string initialWord, List<string> wordInfo)
        {
            if (initialWord != wordInfo[0] && wordDictionary.ContainsKey(wordInfo[0]))
            {
                return false;
            }

            wordDictionary.Remove(initialWord);
            wordDictionary.Add(wordInfo[0], new MeaningAndImage()
            {
                Meaning = wordInfo[2],
                ImagePath = wordInfo[3]
            });

            categoryDictionary[categoryDictionary.FirstOrDefault(pair => pair.Value.Contains(initialWord)).Key].Remove(initialWord);
            if (categoryDictionary.ContainsKey(wordInfo[1]))
                categoryDictionary[wordInfo[1]].Add(wordInfo[0]);
            else categoryDictionary.Add(wordInfo[1], [wordInfo[0]]);

            return UpdateXML();
        }

        public bool DeleteWord(string word)
        {
            if (!wordDictionary.ContainsKey(word))
            {
                return false;
            }
            wordDictionary.Remove(word);
            categoryDictionary[categoryDictionary.FirstOrDefault(pair => pair.Value.Contains(word)).Key].Remove(word);
            return UpdateXML();
        }

        public string[][] GetWordsForGame()
        {
            var words = wordDictionary.Keys.ToArray();
            var random = new Random();
            var randomWords = words.OrderBy(x => random.Next()).Take(5).ToArray();

            string[][] wordsForGame = new string[5][];
            for (int i = 0; i < 5; i++)
            {
                if (wordDictionary[randomWords[i]].ImagePath== "Images/DEFAULT.PNG")
                    wordsForGame[i] = new string[2];
                else
                {
                    wordsForGame[i] = new string[3];
                    wordsForGame[i][2] = wordDictionary[randomWords[i]].ImagePath;
                }
                wordsForGame[i][0] = randomWords[i];
                wordsForGame[i][1] = wordDictionary[randomWords[i]].Meaning;
            }

            return wordsForGame;
        }
    }
}

