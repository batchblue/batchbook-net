using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BatchBookSample
{
    public partial class ListCompanyCommunications : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Company = BatchBook.Company.Get(int.Parse(Request["companyId"]));
            this.DataBind();
        }

        public BatchBook.Company Company
        {
            get;
            protected set;
        }
    }
}