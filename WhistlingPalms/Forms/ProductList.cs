using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Data.Odbc;
using System.Configuration;

namespace WhistlingPalms
{
    public partial class ProductList : Form
    {
        public ProductList()
        {
            InitializeComponent();
        }

        private void ProductList_Load(object sender, EventArgs e)
        {
            this.tblProductsTableAdapter.FillActiveProducts(this.inventoryStoreDataSet.tblProducts);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tpInventory")
            {
                if (!(this.inventoryStoreDataSet.InventoryDetails.Rows.Count > 0))
                {
                    this.inventoryDetailsTableAdapter.Fill(this.inventoryStoreDataSet.InventoryDetails);
                }
            }            
        }

        private void tblProductsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && tblProductsDataGridView.Columns[e.ColumnIndex].Name == "EditColumn")
            {
                DataRowView drv = tblProductsDataGridView.Rows[e.RowIndex].DataBoundItem as DataRowView;
                AddNewProduct frm = new AddNewProduct(Convert.ToInt32(drv.Row["ProductID"]));
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.tblProductsTableAdapter.FillActiveProducts(this.inventoryStoreDataSet.tblProducts);
                    //if (this.inventoryStoreDataSet.tblProducts.Rows.Count > 0)
                    //{
                    //    tblProductsDataGridView.CurrentCell = tblProductsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    //}
                }
            }
        }

        private void btnImportStock_Click(object sender, EventArgs e)
        {
            string connString = "Dsn=SageLine50v10;uid=manager;Pwd=eplslrc;";
            OdbcConnection conn = new OdbcConnection(connString);
            OdbcCommand comm = new OdbcCommand();
            comm.Connection = conn;
            comm.CommandTimeout = 300;
            comm.CommandText = "SELECT STOCK.STOCK_CODE, STOCK.DESCRIPTION, STOCK.SALES_PRICE FROM STOCK STOCK";
            conn.Open();

            OdbcDataAdapter da = new OdbcDataAdapter(comm);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds;
        }
    }
}
