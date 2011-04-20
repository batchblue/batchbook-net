using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace BatchBook
{
    public class Deal
    {
        internal static Deal[] BuildList(XmlReader rdr)
        {
            if (rdr.IsStartElement("companies") && !rdr.IsEmptyElement)
            {
                List<Deal> deals = new List<Deal>();
                rdr.ReadStartElement("deals");
                while (rdr.Name == "deal")
                {
                    deals.Add(new Deal(rdr));
                }
                rdr.ReadEndElement();
                return deals.ToArray();
            }
            else
            {
                rdr.Skip();
                return new Deal[0];
            }
        }

        public static Deal Get(string apiKey, int dealId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/deals/{1}.xml ", Util.Subdomain, dealId);
            return Util.Get(
                url,
                apiKey,
                rdr => new Deal(rdr));
        }

        public static Deal Get(int dealId)
        {
            return Get(Util.DefaultApiKey, dealId);
        }

        public static Deal[] List(string apiKey, int limit, int offset)
        {
            string url = String.Format("https://{0}.batchbook.com/service/deals.xml?offset={1}&limit={2}", Util.Subdomain, offset, limit);
            return Util.Get(url, apiKey, BuildList);
        }

        public static Deal[] List(int limit, int offset)
        {
            return List(Util.DefaultApiKey, limit, offset);
        }

        public static Deal[] Search(
            string apiKey,
            string assignedTo,
            string status,
            DateTime? updatedSince)
        {
            List<string> parameters = new List<string>();
            if (assignedTo != null)
                parameters.Add(String.Format("assigned_to={0}", HttpUtility.UrlEncode(assignedTo)));
            if (status != null)
                parameters.Add(String.Format("status={0}", status));
            if (updatedSince != null)
                parameters.Add(String.Format("updated_since={0}", HttpUtility.UrlEncode(updatedSince.Value.ToBatchBookFormat())));

            string url = String.Format("https://{0}.batchbook.com/service/communications.xml?{1}", Util.Subdomain, String.Join("&", parameters.ToArray()));

            return Util.Get(url, apiKey, BuildList);
        }

        public static Deal[] Search(
            string assignedTo,
            string status,
            DateTime? updatedSince)
        {
            return Search(Util.DefaultApiKey, assignedTo, status, updatedSince);
        }

        public static int Create(
            string apiKey,
            string title,
            string description,
            float amount,
            string status,
            int dealWithId,
            string assignedTo)
        {
            string url = string.Format("https://{0}.batchbook.com/service/deals.xml", Util.Subdomain);
            string location = Util.Post(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                    {
                        wrtr.WriteStartElement("deal");
                        wrtr.WriteElementString("title", title);
                        wrtr.WriteElementString("description", description);
                        wrtr.WriteElementString("amount", amount.ToString());
                        wrtr.WriteElementString("deal_with_id", dealWithId.ToString());
                        wrtr.WriteElementString("assigned_to", assignedTo);
                    });

            Match match = Regex.Match(location, @"\.batchbook\.com/service/deals/(\d+)\.xml");
            if (match.Success)
                return Convert.ToInt32(match.Groups[1].Captures[0].Value);
            else
                throw new Exception("Unable to parse location string.");
        }

        public static int Create(
            string title,
            string description,
            float amount,
            string status,
            int dealWithId,
            string assignedTo)
        {
            return Create(
                Util.DefaultApiKey,
                title,
                description,
                amount,
                status,
                dealWithId,
                assignedTo);
        }

        public static void Update(
            string apiKey, 
            int dealId,
            string title,
            string description,
            float amount,
            string status,
            int dealWithId,
            string assignedTo)
        {
            string url = String.Format("https://{0}.batchbook.com/service/deals/{1}.xml", Util.Subdomain, dealId);
            Util.Put(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("deal");
                    wrtr.WriteElementString("title", title);
                    wrtr.WriteElementString("description", description);
                    wrtr.WriteElementString("amount", amount.ToString());
                    wrtr.WriteElementString("deal_with_id", dealWithId.ToString());
                    wrtr.WriteElementString("assigned_to", assignedTo);
                    wrtr.WriteEndElement();
                });
        }

        public static void Update(
            int dealId,
            string title,
            string description,
            float amount,
            string status,
            int dealWithId,
            string assignedTo)
        {
            Update(Util.DefaultApiKey,
                dealId,
                title,
                description,
                amount,
                status,
                dealWithId,
                assignedTo);
        }

        public static void Destroy(string apiKey, int dealId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/deals/{1}.xml", Util.Subdomain, dealId);
            Util.Delete(url, apiKey);
        }

        public static void Destroy(int dealId)
        {
            Destroy(Util.DefaultApiKey, dealId);
        }
        
        public static string[] GetStatuses(string apiKey)
        {
            string url = String.Format("https://{0}.batchbook.com/service/deals/statuses.xml", Util.Subdomain);
            return Util.Get(
                url, 
                apiKey, 
                delegate (XmlReader rdr)
                {
                    List<string> deals = new List<string>();
                    rdr.ReadStartElement("statuses");
                    while (rdr.ReadToFollowing("status"))
                    {
                        deals.Add(rdr.ReadElementString("status"));
                    }
                    rdr.ReadEndElement();
                    return deals.ToArray();
                });
        }

        public static string[] GetStatuses()
        {
            return GetStatuses(Util.DefaultApiKey);
        }

        internal Deal(XmlReader rdr)
        {
            this.Id = rdr.ReadElementContentAsInt("id", "");
            this.Title = rdr.ReadElementString("title");
            this.Description = rdr.ReadElementString("description");
            this.DealWith = rdr.ReadElementString("deal_with");
            this.DealWithId = int.Parse(rdr.ReadElementString("deal_with_id"));
            this.Status = rdr.ReadElementString("status");
            this.AssignedTo = rdr.ReadElementString("assigned_to");
            this.Amount = float.Parse(rdr.ReadElementString("amount"));
            this.AttachedCommunications = AttachedCommunication.BuildList(rdr);
            this.AttachedContacts = AttachedContact.BuildList(rdr);
            this.AttachedToDos = AttachedTodo.BuildList(rdr);
            this.Tags = Tag.BuildList(rdr);
            this.Comments = MegaComment.BuildList(rdr);
            this.CreatedAt = rdr.ReadElementString("created_at").FromBatchBookFormat();
            this.UpdatedAt = rdr.ReadElementString("updated_at").FromBatchBookFormat();
        }

        public int Id
        {
            get;
            protected set;
        }

        public string Title
        {
            get;
            protected set;
        }

        public string Description
        {
            get;
            protected set;
        }

        public string DealWith
        {
            get;
            protected set;
        }

        public int DealWithId
        {
            get;
            protected set;
        }

        public string Status
        {
            get;
            protected set;
        }

        public string AssignedTo
        {
            get;
            protected set;
        }

        public float Amount
        {
            get;
            protected set;
        }

        public AttachedCommunication[] AttachedCommunications
        {
            get;
            protected set;
        }

        public AttachedContact[] AttachedContacts
        {
            get;
            protected set;
        }

        public AttachedTodo[] AttachedToDos
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

        public DateTime CreatedAt
        {
            get;
            protected set;
        }

        public DateTime UpdatedAt
        {
            get;
            protected set;
        }
    }

    public class AttachedCommunication
    {
        internal static AttachedCommunication[] BuildList(XmlReader rdr)
        {
            List<AttachedCommunication> attachedCommunications = new List<AttachedCommunication>();
            rdr.ReadStartElement("attached_communications");
            while (rdr.ReadToFollowing("attached_communication_id"))
            {
                attachedCommunications.Add(new AttachedCommunication(rdr));
            }
            rdr.ReadEndElement();

            return attachedCommunications.ToArray();
        }

        internal AttachedCommunication(XmlReader rdr)
        {
            this.CommunicationId = rdr.ReadElementContentAsInt("attached_communication_id", "");
        }

        public int CommunicationId
        {
            get;
            protected set;
        }

        public Communication GetCommunication(string apiKey)
        {
            return Communication.Get(apiKey, this.CommunicationId);
        }
    }

    public class AttachedContact
    {
        internal static AttachedContact[] BuildList(XmlReader rdr)
        {
            List<AttachedContact> attachedContacts = new List<AttachedContact>();
            rdr.ReadStartElement("attached_communications");
            while (rdr.ReadToFollowing("attached_communication_id"))
            {
                attachedContacts.Add(new AttachedContact(rdr));
            }
            rdr.ReadEndElement();

            return attachedContacts.ToArray();
        }

        internal AttachedContact(XmlReader rdr)
        {
            this.ContactId = rdr.ReadElementContentAsInt("attached_contacts_id", "");
        }

        public int ContactId
        {
            get;
            protected set;
        }
    }

    public class AttachedTodo
    {
        internal static AttachedTodo[] BuildList(XmlReader rdr)
        {
            List<AttachedTodo> attachedToDo = new List<AttachedTodo>();
            rdr.ReadStartElement("todos");
            while (rdr.ReadToFollowing("todo_id"))
            {
                attachedToDo.Add(new AttachedTodo(rdr));
            }
            rdr.ReadEndElement();

            return attachedToDo.ToArray();
        }

        internal AttachedTodo(XmlReader rdr)
        {
            this.TodoId = rdr.ReadElementContentAsInt("todo_id", "");
        }
        
        public int TodoId
        {
            get;
            protected set;
        }
    }
}
