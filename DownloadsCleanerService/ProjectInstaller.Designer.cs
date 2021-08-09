
namespace DownloadsCleanerService
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.downloadsCleanerServiceProcessInstaler = new System.ServiceProcess.ServiceProcessInstaller();
            this.downloadsCleanerServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // downloadsCleanerServiceProcessInstaler
            // 
            this.downloadsCleanerServiceProcessInstaler.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.downloadsCleanerServiceProcessInstaler.Password = null;
            this.downloadsCleanerServiceProcessInstaler.Username = null;
            // 
            // downloadsCleanerServiceInstaller
            // 
            this.downloadsCleanerServiceInstaller.Description = "Service for downloads cleaner.";
            this.downloadsCleanerServiceInstaller.DisplayName = "Downloads Cleaner Service";
            this.downloadsCleanerServiceInstaller.ServiceName = "DownloadsCleanerService";
            this.downloadsCleanerServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.downloadsCleanerServiceProcessInstaler,
            this.downloadsCleanerServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller downloadsCleanerServiceProcessInstaler;
        private System.ServiceProcess.ServiceInstaller downloadsCleanerServiceInstaller;
    }
}