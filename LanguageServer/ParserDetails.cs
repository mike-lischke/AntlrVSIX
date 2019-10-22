﻿namespace LanguageServer
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ParserDetails : ICloneable
    {
        public virtual Workspaces.Document Item { get; set; }
        public virtual string FullFileName { get { return this.Item?.FullPath; } }
        public virtual string Code { get { return this.Item?.Code; } }
        public virtual bool Changed { get { return Item == null ? true : Item.Changed; } }
        public virtual void Cleanup() { }
        public virtual IGrammarDescription Gd { get; set; }

        public virtual Dictionary<TerminalNodeImpl, int> Refs { get; set; } = new Dictionary<TerminalNodeImpl, int>();
        public virtual HashSet<string> PropagateChangesTo { get; set; } = new HashSet<string>();
        public virtual Dictionary<TerminalNodeImpl, int> Defs { get; set; } = new Dictionary<TerminalNodeImpl, int>();
        public virtual Dictionary<TerminalNodeImpl, int> Tags { get; set; } = new Dictionary<TerminalNodeImpl, int>();

        public virtual HashSet<IParseTree> Errors { get; set; } = new HashSet<IParseTree>();

        public virtual Dictionary<IToken, int> Comments { get; set; } = new Dictionary<IToken, int>();

        public virtual Dictionary<IParseTree, Symtab.CombinedScopeSymbol> Attributes { get; set; } = new Dictionary<IParseTree, Symtab.CombinedScopeSymbol>();

        public virtual Symtab.IScope RootScope { get; set; }

        public virtual IParseTree ParseTree { get; set; } = null;

        public virtual IEnumerable<IParseTree> AllNodes { get; set; } = null;

        public ParserDetails(Workspaces.Document item)
        {
            Item = item;
            Item.Changed = true;
        }


        public virtual void Parse()
        {
            var item = Item;
            var code = item.Code;
            var ffn = item.FullPath;
            bool has_changed = item.Changed;
            item.Changed = false;
            if (!has_changed) return;

            //if (item.GetProperty("BuildAction") == "prjBuildActionNone")
            //    return null;

            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
            if (gd == null) throw new Exception();
            gd.Parse(this);

            this.AllNodes = DFSVisitor.DFS(this.ParseTree as ParserRuleContext);
            this.Comments = gd.ExtractComments(code);
            this.Defs = new Dictionary<TerminalNodeImpl, int>();
            this.Refs = new Dictionary<TerminalNodeImpl, int>();
            this.Tags = new Dictionary<TerminalNodeImpl, int>();
            this.Errors = new HashSet<IParseTree>();
            this.Cleanup();
        }

        public virtual List<Action> Passes { get; } = new List<Action>();

        public void Pass(int pass_number)
        {
            Passes[pass_number]();
        }

        public virtual void GatherDefs()
        {
            var item = Item;
            var ffn = item.FullPath;
            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
            if (gd == null) throw new Exception();
            for (int classification = 0; classification < gd.IdentifyDefinition.Count; ++classification)
            {
                var fun = gd.IdentifyDefinition[classification];
                if (fun == null) continue;
                var it = this.AllNodes.Where(t => fun(gd, this.Attributes, t));
                foreach (var t in it)
                {
                    var x = (t as TerminalNodeImpl);
                    if (x == null) continue;
                    if (x.Symbol == null) continue;
                    try
                    {
                        this.Defs.Add(x, classification);
                        this.Tags.Add(x, classification);
                    }
                    catch (ArgumentException)
                    {
                        // Duplicate
                    }
                }
            }
        }

        public virtual void GatherRefs()
        {
            var item = Item;
            var ffn = item.FullPath;
            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
            if (gd == null) throw new Exception();
            for (int classification = 0; classification < gd.Identify.Count; ++classification)
            {
                var fun = gd.Identify[classification];
                if (fun == null) continue;
                var it = this.AllNodes.Where(t => fun(gd, this.Attributes, t));
                foreach (var t in it)
                {
                    var x = (t as TerminalNodeImpl);
                    if (x == null) continue;
                    if (x.Symbol == null) continue;
                    try
                    {
                        this.Attributes.TryGetValue(x, out Symtab.CombinedScopeSymbol attr);
                        this.Tags.Add(x, classification);
                        if (attr == null) continue;
                        var sym = attr as Symtab.ISymbol;
                        if (sym == null) continue;
                        var def = sym.resolve();
                        if (def != null && def.file != null && def.file != ""
                            && def.file != ffn)
                        {
                            var def_item = Workspaces.Workspace.Instance.FindDocument(def.file);
                            var def_pd = ParserDetailsFactory.Create(def_item);
                            def_pd.PropagateChangesTo.Add(ffn);
                        }
                        this.Refs.Add(x, classification);
                    }
                    catch (ArgumentException)
                    {
                        // Duplicate
                    }
                }
            }
        }

        public virtual void GatherErrors()
        {
            var item = Item;
            var ffn = item.FullPath;
            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
            if (gd == null) throw new Exception();
            {
                var it = this.AllNodes.Where(t => t as Antlr4.Runtime.Tree.ErrorNodeImpl != null);
                foreach (var t in it)
                {
                    this.Errors.Add(t);
                }
            }
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
