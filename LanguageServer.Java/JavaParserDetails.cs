﻿namespace LanguageServer.Java
{
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System.Collections.Generic;

    class JavaParserDetails : ParserDetails
    {
        static Dictionary<string, IScope> _scopes = new Dictionary<string, IScope>();
        static Dictionary<string, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>> _attributes = new Dictionary<string, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>>();
        static IScope _global_scope = new SymbolTable().GLOBALS;

        public JavaParserDetails(Workspaces.Document item)
            : base(item)
        {
            // Passes executed in order for all files.
            Passes.Add(() =>
            {
                var dir = item.FullPath;
                dir = System.IO.Path.GetDirectoryName(dir);
                _scopes.TryGetValue(dir, out IScope value);
                if (value == null)
                {
                    value = new LocalScope(_global_scope);
                    _scopes[dir] = value;
                }
                this.RootScope = value;
                _attributes.TryGetValue(dir, out Dictionary<IParseTree, CombinedScopeSymbol> at);
                if (at == null)
                {
                    at = new Dictionary<IParseTree, CombinedScopeSymbol>();
                    _attributes[dir] = at;
                }
                this.Attributes = at;
            });
            Passes.Add(() =>
            {
                ParseTreeWalker.Default.Walk(new Pass1Listener(this), ParseTree);
            });
            Passes.Add(() =>
            {
                ParseTreeWalker.Default.Walk(new Pass2Listener(this), ParseTree);
            });
        }
    }
}
