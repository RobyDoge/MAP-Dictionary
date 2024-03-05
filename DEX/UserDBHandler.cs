using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DBHandlers
{
    internal static class UserDBHandler
    {
        public static bool CheckUsernameAndPassword(string username,string password)
        {
            const string path = "../../../AdminDB.xml";
            XmlDocument xmlDoc = new();
            xmlDoc.Load(path);

            XmlNode root = xmlDoc.DocumentElement;

            if (root?.ChildNodes == null) return false;
            foreach (XmlNode node in root.ChildNodes)
            {
                string usernameXML = node.Attributes?["username"].Value;
                string passwordXML = node.Attributes?["password"].Value;
                if (password == passwordXML && username == usernameXML) return true;
            }
            return false;
        }
    }
}
