using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace BatchBook
{
    public class Communication
    {
        internal static Communication[] BuildList(XmlReader rdr)
        {
            if (rdr.IsStartElement("communications") && !rdr.IsEmptyElement)
            {
                List<Communication> communications = new List<Communication>();
                rdr.ReadStartElement("communications");
                while (rdr.Name == "communication")
                {
                    communications.Add(new Communication(rdr));
                }
                rdr.ReadEndElement();
                return communications.ToArray();
            }
            else
            {
                rdr.Skip();
                return new Communication[0];
            }
        }
        
        public static Communication Get(string apiKey, int communicationId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/communications/{1}.xml", Util.Subdomain, communicationId);
            return Util.Get(
                url,
                apiKey,
                rdr => new Communication(rdr));
        }

        public static Communication Get(int communicationId)
        {
            return Get(Util.DefaultApiKey, communicationId);
        }

        public static Communication[] List(string apiKey, int limit, int offset)
        {
            string url = String.Format("https://{0}.batchbook.com/service/communications.xml?offset={1}&limit={2}", Util.Subdomain, offset, limit);
            return Util.Get(url, apiKey, BuildList);
        }

        public static Communication[] List(int limit, int offset)
        {
            return List(Util.DefaultApiKey, limit, offset);
        }

        public static Communication[] ListByPerson(string apiKey, int personId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/people/{1}/communications.xml", Util.Subdomain, personId);
            return Util.Get(url, apiKey, BuildList);
        }

        public static Communication[] ListByPerson(int personId)
        {
            return ListByPerson(Util.DefaultApiKey, personId);
        }

        public static Communication[] ListByCompany(string apiKey, int companyId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/companies/{1}/communications.xml", Util.Subdomain, companyId);
            return Util.Get(url, apiKey, BuildList);
        }

        public static Communication[] ListByCompany(int companyId)
        {
            return ListByCompany(Util.DefaultApiKey, companyId);
        }

        public static Communication[] Search(
            string apiKey,
            string ctype, 
            int? contactId,
            DateTime? updatedSince,
            DateTime? updatedBefore,
            DateTime? createdSince,
            DateTime? createdBefore)
        {
            List<string> parameters = new List<string>();
            if (ctype != null)
                parameters.Add(String.Format("ctype={0}", HttpUtility.UrlEncode(ctype)));
            if (contactId != null)
                parameters.Add(String.Format("contactId={0}", contactId));
            if (updatedSince != null)
                parameters.Add(String.Format("updated_since={0}", HttpUtility.UrlEncode(updatedSince.Value.ToBatchBookFormat())));
            if (updatedBefore != null)
                parameters.Add(String.Format("updated_before={0}", HttpUtility.UrlEncode(updatedBefore.Value.ToBatchBookFormat())));
            if (createdSince != null)
                parameters.Add(String.Format("created_since={0}", HttpUtility.UrlEncode(createdSince.Value.ToBatchBookFormat())));
            if (createdBefore != null)
                parameters.Add(String.Format("created_before={0}", HttpUtility.UrlEncode(createdBefore.Value.ToBatchBookFormat())));

            string url = String.Format("https://{0}.batchbook.com/service/communications.xml?{1}", Util.Subdomain, String.Join("&", parameters.ToArray()));

            return Util.Get(url, apiKey, BuildList);
        }

        public static Communication[] Search(
            string ctype,
            int? contactId,
            DateTime? updatedSince,
            DateTime? updatedBefore,
            DateTime? createdSince,
            DateTime? createdBefore)
        {
            return Search(Util.DefaultApiKey, ctype, contactId, updatedSince, updatedBefore, createdSince, createdBefore);
        }

        public static int Create(
            string apiKey,
            string subject,
            string body,
            DateTime date,
            string ctype)
        {
            string url = String.Format("https://{0}.batchbook.com/service/communications.xml", Util.Subdomain);
            string location = Util.Post(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("communication");
                    wrtr.WriteElementString("subject", subject);
                    wrtr.WriteElementString("body", body);
                    wrtr.WriteElementString("date", date.ToBatchBookFormat());
                    wrtr.WriteElementString("ctype", ctype);
                    wrtr.WriteEndElement();
                });

            Match match = Regex.Match(location, @"\.batchbook\.com/service/communications/(\d+)\.xml");
            if (match.Success)
                return Convert.ToInt32(match.Groups[1].Captures[0].Value);
            else
                throw new Exception("Unable to parse location string.");
        }

        public static int Create(
            string subject,
            string body,
            DateTime date,
            string ctype)
        {
            return Create(Util.DefaultApiKey, subject, body, date, ctype);
        }

        public static void Update(string apiKey, int communcationId, string subject, string body, DateTime date, string ctype)
        {
            string url = String.Format("https://{0}.batchbook.com/service/communications/{1}.xml", Util.Subdomain, communcationId);
            Util.Put(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("communication");
                    wrtr.WriteElementString("subject", subject);
                    wrtr.WriteElementString("body", body);
                    wrtr.WriteElementString("date", date.ToBatchBookFormat());
                    wrtr.WriteElementString("ctype", ctype);
                    wrtr.WriteEndElement();
                });
        }

        public static void Update(int communcationId, string subject, string body, DateTime date, string ctype)
        {
            Update(Util.DefaultApiKey, communcationId, subject, body, date, ctype);
        }
        
        public static void Destroy(string apiKey, int communcationId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/communications/{1}.xml", Util.Subdomain, communcationId);
            Util.Delete(url, apiKey);
        }

        public static void Destroy(int communcationId)
        {
            Destroy(Util.DefaultApiKey, communcationId);
        }

        public static Participant[] ListParticipants(string apiKey, int communicationId, string role)
        {
            string url = String.Format(
                "https://{0}.batchbook.com/service/communications/{1}/participants.xml{2}", 
                Util.Subdomain, 
                communicationId,
                (role != null ? String.Format("?role={0}", role) : null));
            return Util.Get(url, apiKey, Participant.BuildList);
        }

        public static Participant[] ListParticipants(int communicationId, string role)
        {
            return ListParticipants(Util.DefaultApiKey, communicationId, role);
        }

        public static Participant[] ListParticipants(string apiKey, int communicationId)
        {
            return ListParticipants(apiKey, communicationId, null);
        }

        public static Participant[] ListParticipants(int communicationId)
        {
            return ListParticipants(Util.DefaultApiKey, communicationId);
        }
        
        public static void AddParticipant(string apiKey, int communicationId, int contactId, string role)
        {
            string url = String.Format("https://{0}.batchbook.com/service/communications/{1}/add_participant.xml?contact_id={2}&role={3}", Util.Subdomain, communicationId, contactId, role);
            Util.Put(
                url, 
                apiKey,                 
                null);
        }

        public static void AddParticipant(int communicationId, int contactId, string role)
        {
            AddParticipant(Util.DefaultApiKey, communicationId, contactId, role);
        }
        
        public static void RemoveParticipant(string apiKey, int communicationId, int contactId, string role)
        {
            string url = String.Format("https://{0}.batchbook.com/service/communications/{1}/remove_participant.xml?contactId={2}&role={3}", Util.Subdomain, communicationId, contactId, role);
            Util.Delete(
                url,
                apiKey);
        }

        public static void RemoveParticipant(int communicationId, int contactId, string role)
        {
            RemoveParticipant(Util.DefaultApiKey, communicationId, contactId, role);
        }

        internal Communication(XmlReader rdr)
        {
            rdr.ReadStartElement("communication");
            this.Id = rdr.ReadElementContentAsInt("id", "");
            this.Subject = rdr.ReadElementString("subject");
            this.Body = rdr.ReadElementString("body");
            this.Date = rdr.ReadElementContentAsDateTime("date", "ctype");
            this.Ctype = rdr.ReadElementString("");
            this.Tags = Tag.BuildList(rdr);
            this.Comments = MegaComment.BuildList(rdr);
            this.CreateAt = rdr.ReadElementContentAsDateTime("created_at", "");
            this.UpdatedAt = rdr.ReadElementContentAsDateTime("updated_at", "");
            rdr.ReadEndElement();
        }
        
        public int Id
        {
            get;
            protected set;
        }

        public string Subject
        {
            get;
            protected set;
        }

        public string Body
        {
            get;
            protected set;
        }

        public DateTime Date
        {
            get;
            protected set;
        }

        public string Ctype
        {
            get;
            protected set;
        }

        public Tag[] Tags
        {
            get;
            protected set;
        }

        public MegaComment[] Comments
        {
            get;
            protected set;
        }

        public DateTime CreateAt
        {
            get;
            protected set;
        }

        public DateTime UpdatedAt
        {
            get;
            protected set;
        }

        public Participant[] Participants
        {
            get { return ListParticipants(this.Id); }
        }

        public Participant[] GetParticipants(string apiKey)
        {
            return ListParticipants(apiKey, this.Id);
        }

        public class Participant
        {
            internal static Participant[] BuildList(XmlReader rdr)
            {
                List<Participant> participants = new List<Participant>();
                rdr.ReadStartElement("participants");
                while (rdr.ReadToFollowing("participant"))
                {
                    participants.Add(new Participant(rdr));
                }
                rdr.ReadEndElement();
                return participants.ToArray();
            }

            internal Participant(XmlReader rdr)
            {
                rdr.ReadStartElement("participant");
                this.Name = rdr.ReadElementString("name");
                this.ContactId = rdr.ReadElementContentAsInt("contact_id", "");
                this.Role = rdr.ReadElementString("role");
                rdr.ReadEndElement();
            }

            public string Name
            {
                get;
                protected set;
            }

            public int ContactId
            {
                get;
                protected set;
            }

            public string Role
            {
                get;
                set;
            }
        }
    }
}
