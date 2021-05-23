using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Zen.DBMS;
using Zen.DBMS.Schema;
using Zen.DBMS.SQLExecution;

namespace Zen.DBAToolbox
{
    /// <summary>
    /// Summary description for SchemaMgrDlg.
    /// </summary>
    public partial class SchemaMgrDlg : Form
    {
        private CaseSchema _umlSchema = null;

        public SchemaMgrDlg()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
        }

        private void OnCancel(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void OnGenerateSQL(object sender, System.EventArgs e)
        {
            if (_umlSchema == null)
            {
                _umlSchema = new CaseSchema();
                _umlSchema.LoadXml(Zen.DBAToolbox.Resources.Resource.ulmschema);
            }

            StringBuilder strCreateDDL = null;
            StringBuilder strDropDDL = null;
            BuildDDL(_umlSchema, Version.MINVERSION, Version.MAXVERSION,
                ref strCreateDDL, ref strDropDDL);

            sqlTextBox.Text = strCreateDDL.ToString();
        }

        private void BuildDDL(CaseSchema currentSchema, Version lowVersion, Version highVersion,
            ref StringBuilder strCreateDDL, ref StringBuilder strDropDDL)
        {
            if (currentSchema == null)
                return;

            SchemaBuilder builder = new SchemaBuilder(DBMSPlatformEnum.SqlServer, new SqlsvrDTDictionary());
            builder.SetSchemaVersion(lowVersion, highVersion);
            builder.BuildDDL(currentSchema, ref strCreateDDL, ref strDropDDL);
        }

    }
}
