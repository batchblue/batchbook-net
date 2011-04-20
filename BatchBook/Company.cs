using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace BatchBook
{
    public class Company : Contact
    {
        internal static Company[] BuildList(XmlReader rdr)
        {
            if (rdr.IsStartElement("companies") && !rdr.IsEmptyElement)
            {
                rdr.ReadStartElement("companies");
                List<Company> companies = new List<Company>();
                while (rdr.Name == "company")
                {
                    companies.Add(new Company(rdr));
                }
                rdr.ReadEndElement();
                return companies.ToArray();
            }
            else
            {
                rdr.Skip();
                return new Company[0];
            }
        }
        
        public static Company Get(string apiKey, int companyId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/companies/{1}.xml", Util.Subdomain, companyId);
            return Util.Get(url, apiKey, rdr => new Company(rdr));
        }

        public static Company Get(int companyId)
        {
            return Get(Util.DefaultApiKey, companyId);
        }

        public static Company[] List(string apiKey, int limit, int offset)
        {
            string url = String.Format("https://{0}.batchbook.com/service/companies.xml?offset={1}&limit={2}", Util.Subdomain, offset, limit);
            return Util.Get(url, apiKey, BuildList);
        }

        public static Company[] List(int limit, int offset)
        {
            return List(Util.DefaultApiKey, limit, offset);
        }

        public static Company[] Search(
            string apiKey,
            string name,
            string email,
            DateTime? updatedSince,
            string tag,
            string state)
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

            string url = String.Format("https://{0}.batchbook.com/service/companies.xml?{1}", Util.Subdomain, String.Join("&", parameters.ToArray()));
            return Util.Get(url, apiKey, BuildList);
        }

        public static Company[] Search(
            string name,
            string email,
            DateTime? updatedSince,
            string tag,
            string state)
        {
            return Search(Util.DefaultApiKey, name, email, updatedSince, tag, state);
        }
        
        public static int Create(string apiKey, string name, string notes)
        {
            string url = String.Format("https://{0}.batchbook.com/service/companies.xml", Util.Subdomain);
            string location = Util.Post(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("company");
                    wrtr.WriteElementString("name", name);
                    wrtr.WriteElementString("notes", notes);
                    wrtr.WriteEndElement();
                });

            Match match = Regex.Match(location, @"\.batchbook\.com/service/companies/(\d+)\.xml");
            if (match.Success)
                return Convert.ToInt32(match.Groups[1].Captures[0].Value);
            else
                throw new Exception("Unable to parse location string." );
        }

        public static int Create(string name, string notes)
        {
            return Create(Util.DefaultApiKey, name, notes);
        }

        public static void Update(string apiKey, int companyId, string name, string notes)
        {
            string url = String.Format("https://{0}.batchbook.com/service/companies/{1}.xml", Util.Subdomain, companyId);
            Util.Put(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("company");
                    wrtr.WriteElementString("name", name);
                    wrtr.WriteElementString("notes", notes);
                    wrtr.WriteEndElement();
                });
        }

        public static void Update(int companyId, string name, string notes)
        {
            Update(Util.DefaultApiKey, companyId, name, notes);
        }

        public static void Destroy(string apiKey, int companyId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/companies/{1}.xml", Util.Subdomain, companyId);
            Util.Delete(url, apiKey);
        }

        public static void Destroy(int companyId)
        {
            Destroy(Util.DefaultApiKey, companyId);
        }

        internal Company(XmlReader rdr)
        {
            rdr.ReadStartElement("company");
            this.Id = rdr.ReadElementContentAsInt("id", "");
            this.Name = rdr.ReadElementString("name");
            this.Notes = rdr.ReadElementString("notes");
            //Skipping Images
            rdr.ReadToFollowing("tags");
            this.Tags = Tag.BuildList(rdr);
            this.Locations = Location.BuildList(rdr);
            this.Comments = MegaComment.BuildList(rdr);
            this.CreatedAt = rdr.ReadElementString("created_at").FromBatchBookFormat();
            this.UpdatedAt = rdr.ReadElementString("updated_at").FromBatchBookFormat();
            rdr.ReadEndElement();
        }
        
        public int Id
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public string Notes
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

        public void AddLocation(
            string label,
            bool isPrimary,
            string email,
            string website,
            string phone,
            string cell,
            string fax,
            string street1,
            string street2,
            string city,
            string state,
            string postalCode,
            string country)
        {
            Location.CreateOnCompany(
                this.Id,
                label,
                isPrimary,
                email,
                website,
                phone,
                cell,
                fax,
                street1,
                street2,
                city,
                state,
                postalCode,
                country);
        }

        public Person[] People
        {
            get { return Person.ListByCompany(this.Id); }
        }
        
        public Person[] GetPeople(string apiKey)
        {
            return Person.ListByCompany(apiKey, this.Id);
        }

        public Communication[] Communications
        {
            get { return Communication.ListByCompany(this.Id); }
        }

        public Communication[] GetCommunications(string apiKey)
        {
            return Communication.ListByCompany(apiKey, this.Id);
        }

        public Todo[] Todos
        {
            get { return Todo.ListByCompany(this.Id); }
        }

        public Todo[] GetTodos(string apiKey)
        {
            return Todo.ListByCompany(apiKey, this.Id);
        }
    }
}
