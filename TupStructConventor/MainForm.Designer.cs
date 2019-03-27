namespace TarsTupHelper
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtBoxJS = new System.Windows.Forms.TextBox();
            this.txtBoxCSharp = new System.Windows.Forms.TextBox();
            this.txtBoxDiagnostics = new System.Windows.Forms.TextBox();
            this.btnConvent = new System.Windows.Forms.Button();
            this.lblJavaScript = new System.Windows.Forms.Label();
            this.lblCSharp = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtBoxJS
            // 
            this.txtBoxJS.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBoxJS.Location = new System.Drawing.Point(21, 37);
            this.txtBoxJS.Multiline = true;
            this.txtBoxJS.Name = "txtBoxJS";
            this.txtBoxJS.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBoxJS.Size = new System.Drawing.Size(479, 479);
            this.txtBoxJS.TabIndex = 0;
            this.txtBoxJS.WordWrap = false;
            // 
            // txtBoxCSharp
            // 
            this.txtBoxCSharp.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBoxCSharp.Location = new System.Drawing.Point(517, 37);
            this.txtBoxCSharp.Multiline = true;
            this.txtBoxCSharp.Name = "txtBoxCSharp";
            this.txtBoxCSharp.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBoxCSharp.Size = new System.Drawing.Size(461, 479);
            this.txtBoxCSharp.TabIndex = 1;
            this.txtBoxCSharp.WordWrap = false;
            // 
            // txtBoxDiagnostics
            // 
            this.txtBoxDiagnostics.Location = new System.Drawing.Point(21, 531);
            this.txtBoxDiagnostics.Multiline = true;
            this.txtBoxDiagnostics.Name = "txtBoxDiagnostics";
            this.txtBoxDiagnostics.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxDiagnostics.Size = new System.Drawing.Size(957, 106);
            this.txtBoxDiagnostics.TabIndex = 2;
            this.txtBoxDiagnostics.WordWrap = false;
            // 
            // btnConvent
            // 
            this.btnConvent.Location = new System.Drawing.Point(409, 646);
            this.btnConvent.Name = "btnConvent";
            this.btnConvent.Size = new System.Drawing.Size(195, 29);
            this.btnConvent.TabIndex = 3;
            this.btnConvent.Text = "JS --> C#";
            this.btnConvent.UseVisualStyleBackColor = true;
            this.btnConvent.Click += new System.EventHandler(this.btnConvent_Click);
            // 
            // lblJavaScript
            // 
            this.lblJavaScript.AutoSize = true;
            this.lblJavaScript.Location = new System.Drawing.Point(22, 11);
            this.lblJavaScript.Name = "lblJavaScript";
            this.lblJavaScript.Size = new System.Drawing.Size(96, 16);
            this.lblJavaScript.TabIndex = 4;
            this.lblJavaScript.Text = "JavaScript:";
            // 
            // lblCSharp
            // 
            this.lblCSharp.AutoSize = true;
            this.lblCSharp.Location = new System.Drawing.Point(520, 11);
            this.lblCSharp.Name = "lblCSharp";
            this.lblCSharp.Size = new System.Drawing.Size(32, 16);
            this.lblCSharp.TabIndex = 5;
            this.lblCSharp.Text = "C#:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 685);
            this.Controls.Add(this.lblCSharp);
            this.Controls.Add(this.lblJavaScript);
            this.Controls.Add(this.btnConvent);
            this.Controls.Add(this.txtBoxDiagnostics);
            this.Controls.Add(this.txtBoxCSharp);
            this.Controls.Add(this.txtBoxJS);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tup Struct Conventor：JS --> C#";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxJS;
        private System.Windows.Forms.TextBox txtBoxCSharp;
        private System.Windows.Forms.TextBox txtBoxDiagnostics;
        private System.Windows.Forms.Button btnConvent;
        private System.Windows.Forms.Label lblJavaScript;
        private System.Windows.Forms.Label lblCSharp;
    }
}

