using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Ex04Cart
{
    public partial class Order : System.Web.UI.Page
    {
        private Product selectedProduct;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlProducts.DataBind();
            }

            selectedProduct = this.GetSelectedProduct();
            lblName.Text = selectedProduct.Name;
            lblShortDescription.Text = selectedProduct.ShortDescription;
            lblLongDescription.Text = selectedProduct.LongDescription;
            lblUnitPrice.Text = selectedProduct.UnitPrice.ToString("c") + " each";
            imgProduct.ImageUrl = "Images/Products/" + selectedProduct.ImageFile;

        }

        private Product GetSelectedProduct()
        {
            DataView productsTable = (DataView)
              SqlDataSource1.Select(DataSourceSelectArguments.Empty);
            productsTable.RowFilter =
                string.Format("ProductID = '{0}'",
                ddlProducts.SelectedValue);
            DataRowView row = productsTable[0];

            Product product = new Product();
            product.ProductID = row["ProductID"].ToString();
            product.Name = row["Name"].ToString();
            product.ShortDescription = row["ShortDescription"].ToString();
            product.LongDescription = row["LongDescription"].ToString();
            product.UnitPrice = (decimal)row["UnitPrice"];
            product.ImageFile = row["ImageFile"].ToString();
            return product;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {

                CartItemList cart = CartItemList.GetCart();
                CartItem cartItem = cart[selectedProduct.ProductID];

                //if item isn't in cart, add it; 
                //otherwise, increase its quantity
                if (cartItem == null)
                {
                    cart.AddItem(selectedProduct, Convert.ToInt32(txtQuantity.Text));
                }
                else
                {
                    cartItem.AddQuantity(Convert.ToInt32(txtQuantity.Text));
                }
                Response.Redirect("~/Cart.aspx");
            }
        }
    }
}