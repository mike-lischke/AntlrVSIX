
import * as vscode from 'vscode';
import * as vscodelc from 'vscode-languageclient';


/**
 * Method to get workspace configuration option
 * @param option name of the option (e.g. for antlr.path should be path)
 * @param defaultValue default value to return if option is not set
 */
function getConfig<T>(option: string, defaultValue?: any): T {
    const config = vscode.workspace.getConfiguration('antlr');
    return config.get<T>(option, defaultValue);
}

namespace SwitchSourceHeaderRequest {
export const type =
    new vscodelc.RequestType<vscodelc.TextDocumentIdentifier, string|undefined,
                             void, void>('textDocument/switchSourceHeader');
}

class FileStatus {
    private statuses = new Map<string, any>();
    private readonly statusBarItem =
        vscode.window.createStatusBarItem(vscode.StatusBarAlignment.Left, 10);

    onFileUpdated(fileStatus: any) {
        const filePath = vscode.Uri.parse(fileStatus.uri);
        this.statuses.set(filePath.fsPath, fileStatus);
        this.updateStatus();
    }

    updateStatus() {
        const path = vscode.window.activeTextEditor.document.fileName;
        const status = this.statuses.get(path);
        if (!status) {
          this.statusBarItem.hide();
          return;
        }
        this.statusBarItem.text = `antlr: ` + status.state;
        this.statusBarItem.show();
    }

    clear() {
        this.statuses.clear();
        this.statusBarItem.hide();
    }

    dispose() {
        this.statusBarItem.dispose();
    }
}

/**
 *  this method is called when your extension is activate
 *  your extension is activated the very first time the command is executed
 */
export function activate(context: vscode.ExtensionContext) {
  
    const server: vscodelc.Executable = {
        command: `C:/Users/kenne/Documents/AntlrVSIX2/Server/bin/Debug/netcoreapp3.1/Server.exe`,
        args: [],
        options: {shell: false, detached: true, windowsHide: false }
    };

    const serverOptions: vscodelc.ServerOptions = server;

    let clientOptions: vscodelc.LanguageClientOptions = {
        // Register the server for plain text documents
        documentSelector: [
            {scheme: 'file', language: 'antlr2'},
            {scheme: 'file', language: 'antlr3'},
            {scheme: 'file', language: 'antlr4'},
            {scheme: 'file', language: 'bison'},
            {scheme: 'file', language: 'ebnf'}, 
        ]
      };

    const client = new vscodelc.LanguageClient('Antlr Language Server', serverOptions, clientOptions);
    console.log('Antlr Language Server is now active!');
    client.start();
}
