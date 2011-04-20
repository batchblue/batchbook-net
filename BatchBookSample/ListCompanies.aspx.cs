using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BatchBook;

namespace BatchBookSample
{
    public partial class ListCompanies : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Companies = Company.List(100, 0);
            this.DataBind();
        }

        public Company[] Companies
        {
            get;
            protected set;
        }
    }
}