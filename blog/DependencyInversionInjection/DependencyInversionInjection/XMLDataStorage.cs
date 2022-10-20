using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DependencyInversionInjection
{
    public class XMLDataStorage : IDataStorage
    {
        public void Persist(List<UserData> users)
        {
            using (XmlWriter writer = XmlWriter.Create("users.xml"))
            {
                writer.WriteStartElement("UserData");
                foreach(UserData user in users)
                {
                    writer.WriteElementString("Id", user.ID.ToString());
                    writer.WriteElementString("Name", user.Name.ToString());
                    writer.WriteElementString("Phone", user.PhoneNumber.ToString());
                }
                writer.WriteEndElement();
                writer.Flush();
            }
        }
    }
}
