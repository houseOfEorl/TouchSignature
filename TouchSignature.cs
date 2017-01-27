using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Text;
using System.Collections;
using System.IO;
using System.Reflection;
using TouchSignature;

namespace TouchSignature
{

    public class TouchSignature : System.Windows.Forms.Form
    {

        static void Main()
        {
            Application.Run(new TouchSignature());
        }

        #region Properties
        private string SignatureFileName = "Signature.txt";
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Panel areaSignature;
        private System.Windows.Forms.Button btnSave;
        private Button btnLoad;
        SignatureControl MySignature;

        #endregion

        #region Constructor
        public TouchSignature()
        {

            InitializeComponent();

            try
            {
                MySignature = new SignatureControl(areaSignature.Width, areaSignature.Height);
                MySignature.Size = areaSignature.Size;

                areaSignature.Controls.Add(MySignature);
            }
            catch (Exception err) { MessageBox.Show(err.Message); }
        }
        #endregion


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.areaSignature = new System.Windows.Forms.Panel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnSave.Location = new System.Drawing.Point(243, 134);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(54, 20);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnNew.Location = new System.Drawing.Point(183, 134);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(54, 20);
            this.btnNew.TabIndex = 3;
            this.btnNew.Text = "New";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // areaSignature
            // 
            this.areaSignature.BackColor = System.Drawing.Color.Gainsboro;
            this.areaSignature.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.areaSignature.Location = new System.Drawing.Point(8, 8);
            this.areaSignature.MaximumSize = new System.Drawing.Size(532, 120);
            this.areaSignature.MinimumSize = new System.Drawing.Size(532, 120);
            this.areaSignature.Name = "areaSignature";
            this.areaSignature.Size = new System.Drawing.Size(532, 120);
            this.areaSignature.TabIndex = 1;
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnLoad.Location = new System.Drawing.Point(303, 134);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(54, 20);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // TouchSignature
            // 
            this.ClientSize = new System.Drawing.Size(551, 174);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.areaSignature);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNew);
            this.MinimizeBox = false;
            this.Name = "TouchSignature";
            this.Text = "Signature";
            this.Load += new System.EventHandler(this.HandSignature_Load);
            this.ResumeLayout(false);

        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void HandSignature_Load(object sender, System.EventArgs e)
        {
            MySignature.Clear(true);
        }

        private void btnNew_Click(object sender, System.EventArgs e)
        {
            MySignature.Clear(true);
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var filePath = Path.Combine(@"C:\", SignatureFileName);

                MySignature.Save(filePath);
                MySignature.Clear(false);
                MessageBox.Show("Picture successfully saved: " + filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                var filePath = Path.Combine(@"C:\", SignatureFileName);

                MySignature.Clear(false);
                MySignature.Load(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

}
