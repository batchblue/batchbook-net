using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BatchBook;

namespace BatchBookSample
{
    public partial class ViewPerson : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Person = BatchBook.Person.Get(int.Parse(Request["personId"]));
            this.DataBind();
        }

        public Person Person
        {
            get;
            protected set;
        }
    }
}