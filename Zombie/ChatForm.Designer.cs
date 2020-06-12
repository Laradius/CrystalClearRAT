namespace Zombie
{
    partial class ChatForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chatOutputTextBox = new System.Windows.Forms.TextBox();
            this.chatInputTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // chatOutputTextBox
            // 
            this.chatOutputTextBox.BackColor = System.Drawing.Color.LightGray;
            this.chatOutputTextBox.Location = new System.Drawing.Point(12, 12);
            this.chatOutputTextBox.Multiline = true;
            this.chatOutputTextBox.Name = "chatOutputTextBox";
            this.chatOutputTextBox.ReadOnly = true;
            this.chatOutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.chatOutputTextBox.Size = new System.Drawing.Size(776, 365);
            this.chatOutputTextBox.TabIndex = 0;
            // 
            // chatInputTextBox
            // 
            this.chatInputTextBox.Location = new System.Drawing.Point(12, 383);
            this.chatInputTextBox.Name = "chatInputTextBox";
            this.chatInputTextBox.Size = new System.Drawing.Size(776, 22);
            this.chatInputTextBox.TabIndex = 1;
            this.chatInputTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chatInputTextBox_KeyDown);
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 416);
            this.Controls.Add(this.chatInputTextBox);
            this.Controls.Add(this.chatOutputTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ChatForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ChatForm";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox chatOutputTextBox;
        private System.Windows.Forms.TextBox chatInputTextBox;
    }
}