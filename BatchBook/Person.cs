using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web;

namespace BatchBook
{
    public class Person : Contact
    {
        private static Person[] BuildList(XmlReader rdr)
        {
            if (rdr.IsStartElement("people") && !rdr.IsEmptyElement)
            {
                List<Person> people = new List<Person>();
                rdr.ReadStartElement("people");
                while (rdr.Name == "person")
                {
                    people.Add(new Person(rdr));
                }
                rdr.ReadEndElement();
                return people.ToArray();
            }
            else
            {
                rdr.Skip();
                return new Person[0];
            }
        }
        
        public static Person Get(string apiKey, int personId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/people/{1}.xml", Util.Subdomain, personId);
            return Util.Get(
                url,
                apiKey,
                rdr => new Person(rdr));
        }

        public static Person Get(int personId)
        {
            return Get(Util.DefaultApiKey, personId);
        }

        public static Person[] ListByCompany(string apiKey, int companyId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/companies/{1}/people.xml", Util.Subdomain, companyId);
            return Util.Get(url, apiKey, BuildList);
        }

        public static Person[] ListByCompany(int companyId)
        {
            return ListByCompany(Util.DefaultApiKey, companyId);
        }
        
        public static Person[] List(string apiKey, int limit, int offset)
        {
            string url = String.Format("https://{0}.batchbook.com/service/people.xml?offset={1}&limit={2}", Util.Subdomain, offset, limit);
            return Util.Get(url, apiKey, BuildList);
        }

        public static Person[] List(int limit, int offset)
        {
            return List(Util.DefaultApiKey, limit, offset);
        }

        public static Person[] Search(string apiKey, string name, string email, DateTime? updatedSince, string tag, string state)
        {
            List<string> parameters = new List<string>();
            if (name != null)
                parameters.Add("name=" + HttpUtility.UrlEncode(name));
            if (email != null)
                parameters.Add("email=" + HttpUtility.UrlEncode(email));
            if (updatedSince != null)
                parameters.Add("email=" + HttpUtility.UrlEncode(updatedSince.Value.ToBatchBookFormat()));
            if (tag != null)
                parameters.Add("tag=" + HttpUtility.UrlEncode(tag));
            if (state != null)
                parameters.Add("state=" + HttpUtility.UrlEncode(state));

            if (parameters.Count == 0)
                throw new ArgumentException("At least one argument must not be null.");

            string url = String.Format("https://{0}.batchbook.com/service/people.xml?{1}", Util.Subdomain, String.Join("&", parameters.ToArray()));
            return Util.Get(url, apiKey, BuildList);
        }

        public static Person[] Search(string name, string email, DateTime? updatedSince, string tag, string state)
        {
            return Search(Util.DefaultApiKey, name, email, updatedSince, tag, state);
        }

        public static int Create(
            string apiKey,
            string firstName,
            string lastName,
            string title,
            string notes,
            int? companyId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/people.xml", Util.Subdomain);
            string location  = Util.Post(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("person");
                    wrtr.WriteElementString("first_name", firstName);
                    wrtr.WriteElementString("last_name", lastName);
                    wrtr.WriteElementString("title", title);
                    wrtr.WriteElementString("notes", notes);
                    if (companyId != null)
                        wrtr.WriteElementString("company_id", companyId.Value.ToString());
                    wrtr.WriteEndElement();
                });

            Match match = Regex.Match(location, @"\.batchbook\.com/service/people/(\d+)\.xml");
            if (match.Success)
                return Convert.ToInt32(match.Groups[1].Captures[0].Value);
            else
                throw new Exception("Unable to parse location string.");
        }

        public static int Create(
            string firstName,
            string lastName,
            string title,
            string notes,
            int? companyId)
        {
            return Create(Util.DefaultApiKey, firstName, lastName, title, notes, companyId);
        }

        public static void Update(
            string apiKey, 
            int personId, 
            string firstName, 
            string lastName, 
            string title,
            string notes,
            string company)
        {
            string url = String.Format("https://{0}.batchbook.com/service/people/{1}.xml", Util.Subdomain, personId);
            Util.Put(
                url, 
                apiKey, 
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("person");
                    wrtr.WriteElementString("first_name", firstName);
                    wrtr.WriteElementString("last_name", lastName);
                    wrtr.WriteElementString("title", title);
                    wrtr.WriteElementString("notes", notes);
                    if (company != null)
                        wrtr.WriteElementString("company", company);
                    wrtr.WriteEndElement();
                });
        }

        public static void Update(
            int personId,
            string firstName,
            string lastName,
            string title,
            string notes,
            string company)
        {
            Update(Util.DefaultApiKey, personId, firstName, lastName, title, notes, company);
        }

        public static void Destroy(string apiKey, int personId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/people/{1}.xml", Util.Subdomain, personId);
            Util.Delete(url, apiKey);
        }

        public static void Destroy(int personId)
        {
            Destroy(Util.DefaultApiKey, personId);
        }

        public static Person[] GetByDeal(string apiKey, int dealId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/deals/{1}/people.xml", Util.Subdomain, dealId);
            return Util.Get(
                url,
                apiKey,
                BuildList);
        }

        public static Person[] GetByDeal(int dealId)
        {
            return GetByDeal(Util.DefaultApiKey, dealId);
        }

        internal Person(XmlReader rdr)
        {
            rdr.ReadStartElement("person");
            this.Id = int.Parse(rdr.ReadElementString("id"));
            this.FirstName = rdr.ReadElementString("first_name");
            this.LastName = rdr.ReadElementString("last_name");
            //Skipping Image Fields
            rdr.ReadToFollowing("title");
            this.Title = rdr.ReadElementString("title");
            this.Company = rdr.ReadElementString("company");

            int companyId;
            if (int.TryParse(rdr.ReadElementString("company_id"), out companyId))
                this.CompanyId = companyId;
            
            this.Tags = Tag.BuildList(rdr);
            this.Locations = Location.BuildList(rdr);
            this.MegaComments = MegaComment.BuildList(rdr);
            this.Notes = rdr.ReadElementString("notes");
            this.CreatedAt = rdr.ReadElementString("created_at").FromBatchBookFormat();
            this.UpdatedAt = rdr.ReadElementString("updated_at").FromBatchBookFormat();
            rdr.ReadEndElement();
        }
        
        public int Id
        {
            get;
            protected set;
        }

        public string FirstName
        {
            get;
            protected set;
        }

        public string LastName
        {
            get;
            protected set;
        }

        public string Title
        {
            get;
            protected set;
        }

        public string Company
        {
            get;
            protected set;
        }

        public int? CompanyId 
        {
            get;
            protected set;
        }

        public Tag[] Tags
        {
            get;
            protected set;
        }

        public Location[] Locations
        {
            get;
            protected set;
        }

        public MegaComment[] MegaComments
        {
            get;
            protected set;
        }

        public string Notes
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

        public Communication[] Communications
        {
            get { return Communication.ListByPerson(this.Id); }
        }

        public Communication[] GetCommunications(string apiKey)
        {
            return Communication.ListByPerson(apiKey, this.Id);
        }

        public Todo[] Todos
        {
            get { return Todo.ListByPerson(this.Id); }
        }

        public Todo[] GetTodos(string apiKey)
        {
            return Todo.ListByPerson(apiKey, this.Id);
        }
    }
}
