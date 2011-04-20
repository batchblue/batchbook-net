using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace BatchBook
{
    public class Todo
    {
        internal static Todo[] BuildList(XmlReader rdr)
        {
            if (rdr.IsStartElement("todos") && !rdr.IsEmptyElement)
            {
                List<Todo> todos = new List<Todo>();
                rdr.ReadStartElement("todos");
                while (rdr.Name == "todo")
                {
                    todos.Add(new Todo(rdr));
                }
                rdr.ReadEndElement();
                return todos.ToArray();
            }
            else
            {
                rdr.Skip();
                return new Todo[0];
            }
        }

        public static Todo Get(string apiKey, int todoId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/todos/{1}.xml", Util.Subdomain, todoId);
            return Util.Get(
                url,
                apiKey,
                rdr => new Todo(rdr));
        }

        public static Todo Get(int todoId)
        {
            return Get(Util.DefaultApiKey, todoId);
        }
        
        public static Todo[] List(string apiKey)
        {
            string url = String.Format("https://{0}.batchbook.com/service/todos.xml", Util.Subdomain);
            return Util.Get(url, apiKey, BuildList);
        }

        public static Todo[] List()
        {
            return List(Util.DefaultApiKey);
        }
        
        public static Todo[] ListByPerson(string apiKey, int personId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/todos.xml?person_id={1}", Util.Subdomain, personId);
            return Util.Get(url, apiKey, BuildList);
        }

        public static Todo[] ListByPerson(int personId)
        {
            return ListByPerson(Util.DefaultApiKey, personId);
        }

        public static Todo[] ListByCompany(string apiKey, int companyId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/todos.xml?company_id={1}", Util.Subdomain, companyId);
            return Util.Get(url, apiKey, BuildList);
        }

        public static Todo[] ListByCompany(int companyId)
        {
            return ListByCompany(Util.DefaultApiKey, companyId);
        }

        public static Todo[] ListByUpdatedSince(string apiKey, DateTime updatedSince)
        {
            string url = String.Format("https://{0}.batchbook.com/service/todos.xml?updated_since={1}", Util.Subdomain, HttpUtility.UrlEncode(updatedSince.ToBatchBookFormat()));
            return Util.Get(url, apiKey, BuildList);
        }

        public static Todo[] ListByUpdatedSince(DateTime updatedSince)
        {
            return ListByUpdatedSince(Util.DefaultApiKey, updatedSince);
        }

        public static Todo[] ListByAssignedTo(string apiKey, string assignedTo)
        {
            string url = String.Format("https://{0}.batchbook.com/service/todos.xml?assigned_to={1}", Util.Subdomain, HttpUtility.UrlEncode(assignedTo));
            return Util.Get(url, apiKey, BuildList);
        }

        public static Todo[] ListByAssignedTo(string assignedTo)
        {
            return ListByAssignedTo(Util.DefaultApiKey, assignedTo);
        }

        public static int Create(
            string apiKey,
            string title,
            string description,
            DateTime dueDate,
            bool flagged,
            bool complete,
            string assignedTo)
        {
            string url = String.Format("https://{0}.batchbook.com/service/todos.xml", Util.Subdomain);
            string location = Util.Post(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("todo");
                    wrtr.WriteElementString("title", title);
                    wrtr.WriteElementString("description", description);
                    wrtr.WriteElementString("due_date", dueDate.ToBatchBookFormat());
                    wrtr.WriteElementString("flagged", flagged.ToString());
                    wrtr.WriteElementString("complete", complete.ToString());
                    wrtr.WriteElementString("assigned_to", assignedTo);
                    wrtr.WriteEndElement();
                });

            Match match = Regex.Match(location, @"\.batchbook\.com/service/todos/(\d+)\.xml");
            if (match.Success)
                return Convert.ToInt32(match.Groups[1].Captures[0].Value);
            else
                throw new Exception("Unable to parse location string.");
        }

        public static int Create(
            string title,
            string description,
            DateTime dueDate,
            bool flagged,
            bool complete,
            string assignedTo)
        {
            return Create(Util.DefaultApiKey, title, description, dueDate, flagged, complete, assignedTo);
        }

        public static void Update(
            string apiKey,
            int todoId,
            string title,
            string description,
            DateTime dueDate,
            bool flagged,
            bool complete,
            string assignedTo)
        {
            string url = String.Format("https://{0}.batchbook.com/service/todos/{1}.xml", Util.Subdomain, todoId);
            Util.Put(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("todo");
                    wrtr.WriteElementString("title", title);
                    wrtr.WriteElementString("description", description);
                    wrtr.WriteElementString("due_date", dueDate.ToBatchBookFormat());
                    wrtr.WriteElementString("flagged", flagged.ToString());
                    wrtr.WriteElementString("complete", complete.ToString());
                    wrtr.WriteElementString("assigned_to", assignedTo);
                    wrtr.WriteEndElement();
                });
        }

        public static void Update(
            int todoId,
            string title,
            string description,
            DateTime dueDate,
            bool flagged,
            bool complete,
            string assignedTo)
        {
            Update(Util.DefaultApiKey,
                todoId,
                title,
                description,
                dueDate,
                flagged,
                complete,
                assignedTo);
        }

        public static void Destroy(string apiKey, int todoId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/todos/{1}.xml", Util.Subdomain, todoId);
            Util.Delete(url, apiKey);
        }

        public static void Destroy(int todoId)
        {
            Destroy(Util.DefaultApiKey, todoId);
        }

        internal Todo(XmlReader rdr)
        {
            rdr.ReadStartElement("todo");
            this.Id = rdr.ReadElementContentAsInt("id", "");
            this.Title = rdr.ReadElementString("title");
            this.Description = rdr.ReadElementString("description");
            this.DueDate = Convert.ToDateTime(rdr.ReadElementString("due_date"));
            this.Flagged = rdr.ReadElementContentAsBoolean("flagged", "");
            this.Complete = rdr.ReadElementContentAsBoolean("complete", "");
            this.AssignedBy = rdr.ReadElementString("assigned_by");
            this.AssignedTo = rdr.ReadElementString("assigned_to");
            this.Participants = Participant.BuildList(rdr);
            this.Tags = Tag.BuildList(rdr);
            this.Comments = MegaComment.BuildList(rdr);
            this.CreatedAt = DateTime.Parse(rdr.ReadElementString("created_at"));
            this.UpdatedAt = DateTime.Parse(rdr.ReadElementString("updated_at"));
            rdr.ReadEndElement();
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

        public DateTime DueDate
        {
            get;
            protected set;
        }

        public bool Flagged
        {
            get;
            protected set;
        }

        public bool Complete
        {
            get;
            protected set;
        }

        public string AssignedBy
        {
            get;
            protected set;
        }

        public string AssignedTo
        {
            get;
            protected set;
        }

        public Participant[] Participants
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
        
        public class Participant
        {
            internal static Participant[] BuildList(XmlReader rdr)
            {
                List<Participant> participants = new List<Participant>();
                rdr.ReadStartElement("participants");
                while (rdr.ReadToFollowing("participant_id"))
                {
                    participants.Add(new Participant(rdr));
                }
                rdr.ReadEndElement();

                return participants.ToArray();
            }

            internal Participant(XmlReader rdr)
            {
                this.Id = rdr.ReadElementContentAsInt("participant_id", "");
            }

            public int Id
            {
                get;
                protected set;
            }

            public Person GetPerson(string apiKey)
            {
                return Person.Get(apiKey, this.Id);
            }
        }
    }
}
