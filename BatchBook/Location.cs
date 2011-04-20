using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace BatchBook
{
    public class Location
    {
        internal static Location[] BuildList(XmlReader rdr)
        {
            if (rdr.IsStartElement("locations") && !rdr.IsEmptyElement)
            {
                rdr.ReadStartElement("locations");
                List<Location> locations = new List<Location>();
                while (rdr.Name == "location")
                {
                    locations.Add(new Location(rdr));
                }
                rdr.ReadEndElement();
                return locations.ToArray();
            }
            else
            {
                rdr.Skip();
                return new Location[0];
            }
        }

        public static Location GetByCompany(string apiKey, int companyId, string locationName)
        {
            string url = String.Format("https://{0}.batchbook.com/service/companies/{1}/locations/{2}.xml", Util.Subdomain, companyId, HttpUtility.UrlEncode(locationName));
            return Util.Get(
                url,
                apiKey,
                rdr => new Location(rdr));
        }

        public static Location GetByCompany(int companyId, string locationName)
        {
            return GetByCompany(Util.DefaultApiKey, companyId, locationName);
        }

        public static Location GetByPerson(string apiKey, int personId, string locationName)
        {
            string url = String.Format("https://{0}.batchbook.com/service/people/{1}/locations/{2}.xml", Util.Subdomain, personId, HttpUtility.UrlEncode(locationName));
            return Util.Get(
                url,
                apiKey,
                rdr => new Location(rdr));
        }

        public static Location GetByPerson(int personId, string locationName)
        {
            return GetByPerson(Util.DefaultApiKey, personId, locationName);
        }

        public static Location[] ListByCompany(string apiKey, int companyId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/companies/{1}/locations.xml", Util.Subdomain, companyId);
            return Util.Get(
                url,
                apiKey,
                BuildList);
        }

        public static Location[] ListByCompany(int companyId)
        {
            return ListByCompany(Util.DefaultApiKey, companyId);
        }

        public static Location[] ListByPerson(string apiKey, int personId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/people/{1}/locations.xml", Util.Subdomain, personId);
            return Util.Get(
                url,
                apiKey,
                BuildList);
        }

        public static Location[] ListByPerson(int personId)
        {
            return ListByPerson(Util.DefaultApiKey, personId);
        }

        public static void CreateOnCompany(
            string apiKey,
            int companyId,
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
            string country
            )
        {
            string url = String.Format("https://{0}.batchbook.com/service/companies/{1}/locations.xml", Util.Subdomain, companyId);
            string location = Util.Post(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("location");
                    wrtr.WriteElementString("label", label);
                    wrtr.WriteElementString("primary", isPrimary.ToString());
                    wrtr.WriteElementString("email", email);
                    wrtr.WriteElementString("website", website);
                    wrtr.WriteElementString("phone", phone);
                    wrtr.WriteElementString("cell", cell);
                    wrtr.WriteElementString("fax", fax);
                    wrtr.WriteElementString("street_1", street1);
                    wrtr.WriteElementString("street_2", street2);
                    wrtr.WriteElementString("city", city);
                    wrtr.WriteElementString("state", state);
                    wrtr.WriteElementString("postal_code", postalCode);
                    wrtr.WriteElementString("country", country);
                    wrtr.WriteEndElement();
                });
        }

        public static void CreateOnCompany(
            int companyId,
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
            string country
            )
        {
            CreateOnCompany(
                Util.DefaultApiKey,
                companyId,
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

        public static void CreateOnPerson(
            string apiKey,
            int personId,
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
            string url = String.Format("https://{0}.batchbook.com/service/people/{1}/locations.xml", Util.Subdomain, personId);
            string location = Util.Post(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("location");
                    wrtr.WriteElementString("label", label);
                    wrtr.WriteElementString("primary", isPrimary.ToString());
                    wrtr.WriteElementString("email", email);
                    wrtr.WriteElementString("website", website);
                    wrtr.WriteElementString("phone", phone);
                    wrtr.WriteElementString("cell", cell);
                    wrtr.WriteElementString("fax", fax);
                    wrtr.WriteElementString("street_1", street1);
                    wrtr.WriteElementString("street_2", street2);
                    wrtr.WriteElementString("city", city);
                    wrtr.WriteElementString("state", state);
                    wrtr.WriteElementString("postal_code", postalCode);
                    wrtr.WriteElementString("country", country);
                    wrtr.WriteEndElement();
                });
        }

        public static void CreateOnPerson(
            int personId,
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
            CreateOnPerson(
                Util.DefaultApiKey,
                personId,
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

        public static void UpdateOnCompany(
            string apiKey,
            int companyId,
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
            string url = String.Format("https://{0}.batchbook.com/service/companies/{1}/locations/{2}.xml", Util.Subdomain, companyId, HttpUtility.UrlEncode(label));
            Util.Put(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("location");
                    wrtr.WriteElementString("label", label);
                    wrtr.WriteElementString("primary", isPrimary.ToString());
                    wrtr.WriteElementString("email", email);
                    wrtr.WriteElementString("website", website);
                    wrtr.WriteElementString("phone", phone);
                    wrtr.WriteElementString("cell", cell);
                    wrtr.WriteElementString("fax", fax);
                    wrtr.WriteElementString("street_1", street1);
                    wrtr.WriteElementString("street_2", street2);
                    wrtr.WriteElementString("city", city);
                    wrtr.WriteElementString("state", state);
                    wrtr.WriteElementString("postal_code", postalCode);
                    wrtr.WriteElementString("country", country);
                    wrtr.WriteEndElement();
                });
        }

        public static void UpdateOnCompany(
            int companyId,
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
            UpdateOnCompany(
                Util.DefaultApiKey,
                companyId,
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


        public static void UpdateOnPerson(
            string apiKey,
            int personId,
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
             string url = String.Format("https://{0}.batchbook.com/service/people/{1}/locations/{2}.xml", Util.Subdomain, personId, HttpUtility.UrlEncode(label));
            Util.Put(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("location");
                    wrtr.WriteElementString("label", label);
                    wrtr.WriteElementString("primary", isPrimary.ToString());
                    wrtr.WriteElementString("email", email);
                    wrtr.WriteElementString("website", website);
                    wrtr.WriteElementString("phone", phone);
                    wrtr.WriteElementString("cell", cell);
                    wrtr.WriteElementString("fax", fax);
                    wrtr.WriteElementString("street_1", street1);
                    wrtr.WriteElementString("street_2", street2);
                    wrtr.WriteElementString("city", city);
                    wrtr.WriteElementString("state", state);
                    wrtr.WriteElementString("postal_code", postalCode);
                    wrtr.WriteElementString("country", country);
                    wrtr.WriteEndElement();
                });
        }

        public static void UpdateOnPerson(
            int personId,
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
            UpdateOnPerson(
                Util.DefaultApiKey,
                personId,
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

        public static void DestroyOnCompany(string apiKey, int companyId, string label)
        {
            string url = String.Format("https://{0}.batchbook.com/service/companies/{1}/locations/{2}.xml", Util.Subdomain, companyId, HttpUtility.UrlEncode(label));
            Util.Delete(url, apiKey);
        }

        public static void DestroyOnCompany(int companyId, string label)
        {
            DestroyOnCompany(Util.DefaultApiKey, companyId, label);
        }

        public static void DestroyOnPerson(string apiKey, int personId, string label)
        {
            string url = String.Format("https://{0}.batchbook.com/service/people/{1}/locations/{2}.xml", Util.Subdomain, personId, HttpUtility.UrlEncode(label));
            Util.Delete(url, apiKey);
        }
        
        internal Location(XmlReader rdr)
        {
            rdr.ReadStartElement("location");
            this.Id = rdr.ReadElementContentAsInt("id", "");
            this.Label = rdr.ReadElementString("label");
            this.IsPrimary = rdr.ReadElementContentAsBoolean("primary", "");
            this.Email = rdr.ReadElementString("email");
            this.Website = rdr.ReadElementString("website");
            this.Phone = rdr.ReadElementString("phone");
            this.Cell = rdr.ReadElementString("cell");
            this.Fax = rdr.ReadElementString("fax");
            this.Street1 = rdr.ReadElementString("street_1");
            this.Street2 = rdr.ReadElementString("street_2");
            this.City = rdr.ReadElementString("city");
            this.State = rdr.ReadElementString("state");
            this.PostalCode = rdr.ReadElementString("postal_code");
            this.Country = rdr.ReadElementString("country");
            rdr.ReadEndElement();
        }
        
        public int Id
        {
            get;
            protected set;
        }

        public string Label
        {
            get;
            protected set;
        }

        public bool IsPrimary
        {
            get;
            protected set;
        }

        public string Email
        {
            get;
            protected set;
        }

        public string Website
        {
            get;
            protected set;
        }

        public string Phone
        {
            get;
            protected set;
        }

        public string Cell
        {
            get;
            protected set;
        }

        public string Fax
        {
            get;
            protected set;
        }

        public string Street1
        {
            get;
            protected set;
        }

        public string Street2
        {
            get;
            protected set;
        }

        public string City
        {
            get;
            protected set;
        }

        public string State
        {
            get;
            protected set;
        }

        public string PostalCode
        {
            get;
            protected set;
        }

        public string Country
        {
            get;
            protected set;
        }
    }
}
