using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TupHelper
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 默认的 Demo JS 代码
            txtBoxJS.Text =
@"HUYA.BeginLiveNotice = function() {
    this.lPresenterUid = 0;
    this.iGameId = 0;
    this.sGameName = """";
    this.iRandomRange = 0;
    this.iStreamType = 0;
    this.vStreamInfo = new Taf.Vector(new HUYA.StreamInfo);
    this.vCdnList = new Taf.Vector(new Taf.STRING);
    this.lLiveId = 0;
    this.iPCDefaultBitRate = 0;
    this.iWebDefaultBitRate = 0;
    this.iMobileDefaultBitRate = 0;
    this.lMultiStreamFlag = 0;
    this.sNick = """";
    this.lYYId = 0;
    this.lAttendeeCount = 0;
    this.iCodecType = 0;
    this.iScreenType = 0;
    this.vMultiStreamInfo = new Taf.Vector(new HUYA.MultiStreamInfo);
    this.sLiveDesc = """";
    this.lLiveCompatibleFlag = 0;
    this.sAvatarUrl = """";
    this.iSourceType = 0;
    this.sSubchannelName = """";
    this.sVideoCaptureUrl = """";
    this.iStartTime = 0;
    this.lChannelId = 0;
    this.lSubChannelId = 0;
    this.sLocation = """";
    this.iCdnPolicyLevel = 0;
    this.iGameType = 0;
    this.mMiscInfo = new Taf.Map(new Taf.STRING, new Taf.STRING);
    this.iShortChannel = 0;
    this.iRoomId = 0;
    this.bIsRoomSecret = 0;
    this.iHashPolicy = 0
};";
            txtBoxJS.Focus();
            txtBoxJS.SelectionStart = 0;
            txtBoxJS.SelectionLength = 0;

            btnConvent.Focus();
        }

        private void btnConvent_Click(object sender, EventArgs e)
        {
            txtBoxCSharp.Text = "";
            txtBoxDiagnostics.Text = "";
            btnConvent.Enabled = false;

            TupStructConventor conventor = new TupStructConventor();
            conventor.SetJsCode(txtBoxJS.Text);

            string csharpCode = conventor.ConventToCSharp();
            txtBoxCSharp.Text = csharpCode;
            txtBoxDiagnostics.Text = conventor.Diagnostics();
            btnConvent.Enabled = true;
        }
    }
}
