using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Breakout
{
    public class GamesList
    {
        LinkedList<GameInfo> actualList;

        public GamesList()
        {
            actualList = new LinkedList<GameInfo>();
        }

        public void Add(GameInfo element)
        {
            actualList.AddFirst(element);
        }

        public GamesList ListFromFile()
        {
            GamesList list;
            XmlSerializer serializer;
            FileStream stream;
            serializer = new XmlSerializer(typeof(GamesList));
            try
            {
                stream = new FileStream("games.xml", FileMode.Open);
            }
            catch (FileNotFoundException e)
            {
                return new GamesList();
            }
            list = (GamesList)serializer.Deserialize(stream);
            actualList = list.actualList;
            stream.Close();
            return list;
        }

        public void ListToFile()
        {
            actualList = new LinkedList<GameInfo>(actualList.Take<GameInfo>(10).ToArray<GameInfo>());
            XmlSerializer serializer = new XmlSerializer(typeof(GamesList));
            StreamWriter writer = new StreamWriter("games.xml");
            serializer.Serialize(writer, this);
            writer.Close();
        }
    }
}
