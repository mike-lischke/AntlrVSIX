﻿using Algorithms;
using Workspaces;

namespace Trash.Commands
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using LanguageServer;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;

    class CTree
    {
        public void Help()
        {
            System.Console.WriteLine(@"tree
Reads a tree from stdin and prints the tree as an indented node list.

Example:
    . | tree
");
        }

        private string Reconstruct(IParseTree tree)
        {
            Stack<IParseTree> stack = new Stack<IParseTree>();
            stack.Push(tree);
            StringBuilder sb = new StringBuilder();
            int last = -1;
            while (stack.Any())
            {
                var n = stack.Pop();
                if (n is TerminalNodeImpl term)
                {
                    var s = term.Symbol.InputStream;
                    var c = term.Payload.StartIndex;
                    var d = term.Payload.StopIndex;
                    if (last != -1 && c != -1 && c > last)
                    {
                        if (s != null)
                        {
                            var txt = s.GetText(new Antlr4.Runtime.Misc.Interval(last, c - 1));
                            sb.Append(txt);
                        }
                    }
                    if (c != -1 && d != -1)
                    {
                        if (s != null)
                        {
                            string txt = s.GetText(new Antlr4.Runtime.Misc.Interval(c, d));
                            sb.Append(txt);
                        }
                        else
                        {
                            string txt = term.Symbol.Text;
                            sb.Append(txt);
                        }
                    }
                    last = d + 1;
                }
                else
                    for (int i = n.ChildCount - 1; i >= 0; i--)
                    {
                        stack.Push(n.GetChild(i));
                    }
            }
            return sb.ToString();
        }

        public void Execute(Repl repl, ReplParser.CtreeContext tree, bool piped)
        {
            MyTuple<IParseTree[], Parser, Document, string> tuple = repl.tree_stack.Pop();
            var lines = tuple.Item4;
            var doc = repl.stack.Peek();
            var pr = ParsingResultsFactory.Create(doc);
            var lexer = pr.Lexer;
            var parser = pr.Parser;
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new AntlrJson.Impl2.ParseTreeConverter(null, null, lexer, parser));
            serializeOptions.WriteIndented = false;
            var obj1 = JsonSerializer.Deserialize<IParseTree>(lines, serializeOptions);
            if (obj1 == null) return;
            var nodes = new IParseTree[] { obj1 };
            StringBuilder sb = new StringBuilder();
            foreach (var node in nodes)
            {
                TerminalNodeImpl x = TreeEdits.LeftMostToken(node);
                var ts = x.Payload.TokenSource;
                sb.AppendLine();
                sb.AppendLine(
                    TreeOutput.OutputTree(
                        node,
                        ts as Lexer,
                        null).ToString());
            }
            tuple.Item4 = sb.ToString();
            repl.tree_stack.Push(tuple);
        }
    }
}