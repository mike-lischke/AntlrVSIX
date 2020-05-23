﻿namespace LspAntlr
{
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.ComponentModel.Design;
    using System.Windows;

    public class AboutCommand
    {
        private readonly Microsoft.VisualStudio.Shell.Package _package;
        private readonly MenuCommand _menu_item1;

        private AboutCommand(Microsoft.VisualStudio.Shell.Package package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            OleMenuCommandService commandService = ServiceProvider.GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService == null)
            {
                throw new ArgumentNullException("OleMenuCommandService");
            }

            {
                // Set up hook for context menu.
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7010);
                _menu_item1 = new MenuCommand(MenuItemCallback, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item1);
            }
        }
        public static AboutCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider => _package;

        public static void Initialize(Microsoft.VisualStudio.Shell.Package package)
        {
            Instance = new AboutCommand(package);
        }

#pragma warning disable VSTHRD100
        private async void MenuItemCallback(object sender, EventArgs e)
#pragma warning restore VSTHRD100
        {
            await Application.Current.Dispatcher.InvokeAsync(delegate
            {
                AboutBox inputDialog = new AboutBox();
                inputDialog.Show();
            });
        }
    }
}
