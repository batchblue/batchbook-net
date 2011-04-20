using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace BatchBook
{
    public class Activity
    {
        internal static Activity[] BuildList(XmlReader rdr)
        {
            rdr.ReadToFollowing("activity");
            List<Activity> todos = new List<Activity>();
            while (rdr.ReadToFollowing("activity"))
            {
                todos.Add(new Activity(rdr));
            }
            return todos.ToArray();
        }

        public static Activity[] List(string apiKey, int page)
        {
            string url = String.Format("https://{0}.batchbook.com/service/activities/recent.xml?page={1}", Util.Subdomain, page);
            return Util.Get(
                url,
                apiKey,
                BuildList);
        }

        public static Activity[] List(int page)
        {
            return List(Util.DefaultApiKey, page);
        }

        internal Activity(XmlReader rdr)
        {
            rdr.ReadStartElement("activity");
            this.Name = rdr.ReadElementString("name");
            this.Description = rdr.ReadElementString("description");
            this.RecordType = rdr.ReadElementString("record_type");
            int recordId;
            if (int.TryParse(rdr.ReadElementString("record_id"), out recordId))
                this.RecordId = recordId;
            this.UserName = rdr.ReadElementString("user_name");
            this.UserId = rdr.ReadElementContentAsInt("user_id", "");
            this.Date = Convert.ToDateTime(rdr.ReadElementString("date"));
            rdr.ReadEndElement();
        }
        
        public string Name
        {
            get;
            protected set;
        }

        public string Description
        {
            get;
            protected set;
        }

        public string RecordType
        {
            get;
            protected set;
        }

        public int? RecordId
        {
            get;
            protected set;
        }

        public string UserName
        {
            get;
            protected set;
        }

        public int UserId
        {
            get;
            protected set;
        }

        public DateTime Date
        {
            get;
            protected set;
        }
    }
}
