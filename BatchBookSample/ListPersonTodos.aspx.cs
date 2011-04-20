using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BatchBookSample
{
    public partial class ListPersonTodos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Person = BatchBook.Person.Get(int.Parse(Request["personId"]));
            this.DataBind();
        }

        public BatchBook.Person Person
        {
            get;
            protected set;
        }
    }
}