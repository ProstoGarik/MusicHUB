using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ClassLibrary1
{
    public  class FileManager
    {
        private XmlSerializer serializer;

        public FileManager() {
            TrackList trackList = new TrackList();
            Serializer = new System.Xml.Serialization.XmlSerializer(trackList.GetType());
        }
        public XmlSerializer Serializer { get => serializer; set => serializer = value; }

        public void SaveFile(TrackList trackList)
        {
            using (StreamWriter streamWriter = new StreamWriter("Z:\\TempFolder\\TestXml.xml"))
            {
                Serializer.Serialize(streamWriter, trackList);
            }
        }

        public TrackList LoadFile()
        {
            using (StreamReader streamReader = new StreamReader("Z:\\TempFolder\\TestXml.xml"))
            {
                return (TrackList)serializer.Deserialize(streamReader);
            }
        }
    }
}
