﻿namespace Trash
{
    using Antlr4.Runtime;
    using LanguageServer;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Repl
    {
        List<string> History { get; set; } = new List<string>();
        const string PreviousHistoryFfn = ".trash.rc";

        public Repl()
        {
        }

        void ReadHistory()
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (!System.IO.File.Exists(home + Path.DirectorySeparatorChar + PreviousHistoryFfn)) return;
            var history = System.IO.File.ReadAllLines(home + Path.DirectorySeparatorChar + PreviousHistoryFfn);
            History = history.ToList();
        }

        void WriteHistory()
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            System.IO.File.WriteAllLines(home + Path.DirectorySeparatorChar + PreviousHistoryFfn, History);
        }

        public void Run()
        {
            string input;
            ReadHistory();
            do
            {
                Console.Write("> ");
                input = Console.ReadLine();
            } while (Execute(input));
            WriteHistory();
        }

        public bool Execute(string line)
        {
            try
            {
                var input = line + ";";
                var str = new AntlrInputStream(input);
                var lexer = new ReplLexer(str);
                var tokens = new CommonTokenStream(lexer);
                var parser = new ReplParser(tokens);
                var tree = parser.cmd();
                if (tree.read() != null)
                {
                    History.Add(line);
                    var r = tree.read();
                    var f = r.ffn().GetText();
                    f = f.Substring(1, f.Length - 2);
                    var s1 = new AntlrFileStream(f.Substring(1, f.Length - 2));
                    var l1 = new ANTLRv4Lexer(s1);
                    var t1 = new CommonTokenStream(l1);
                    var p1 = new ANTLRv4Parser(t1);
                    var tr = p1.grammarSpec();
                }
                else if (tree.import_() != null)
                {
                    History.Add(line);
                    var import = tree.import_();
                    var type = import.type()?.GetText();
                    var f = import.ffn().GetText();
                    f = f.Substring(1, f.Length - 2);
                    if (type == "antlr3")
                    {
                        var ii = System.IO.File.ReadAllText(f);
                        Dictionary<string, string> res = new Dictionary<string, string>();
                        LanguageServer.Antlr3Import.Try(f, ii, ref res);
                        System.Console.Write(res.First().Value);
                    }
                    else if (type == "antlr2")
                    {
                        var ii = System.IO.File.ReadAllText(f);
                        Dictionary<string, string> res = new Dictionary<string, string>();
                        LanguageServer.Antlr2Import.Try(f, ii, ref res);
                        System.Console.Write(res.First().Value);
                    }
                    else if (type == "bison")
                    {
                        var ii = System.IO.File.ReadAllText(f);
                        Dictionary<string, string> res = new Dictionary<string, string>();
                        LanguageServer.BisonImport.Try(f, ii, ref res);
                        System.Console.Write(res.First().Value);
                    }
                }
                else if (tree.history() != null)
                {
                    History.Add(line);
                    System.Console.WriteLine();
                    for (int i = 0; i < History.Count; ++i)
                    {
                        var h = History[i];
                        System.Console.WriteLine(i + " " + h);
                    }
                }
                else if (tree.quit() != null)
                {
                    History.Add(line);
                    return false;
                }
                else if (tree.empty() != null)
                {
                }
                else if (tree.bang() != null)
                {
                    var bang = tree.bang();
                    if (bang.@int() != null)
                    {
                        var snum = bang.@int().GetText();
                        var num = Int32.Parse(snum);
                        return Execute(History[num]);
                    }
                    else if (bang.BANG() != null)
                    {
                        return Execute(History.Last());
                    }
                }
            }
            catch
            {
                System.Console.WriteLine("Err");
            }
            return true;
        }
    }

}
