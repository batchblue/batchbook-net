using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BatchBook;

namespace BatchBookSample
{
    public partial class ViewCommunication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Communication = BatchBook.Communication.Get(int.Parse(Request["communicationId"]));
            this.DataBind();
        }

        public Communication Communication
        {
            get;
            protected set;
        }
    }
}