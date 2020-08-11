# Trash -- a shell for performing transformations of Antlr grammars

"Trash" is a simple command-line shell for parsing, converting, analyzing, and transforming
Antlr grammars. You can use the tool to automate changes to a grammar,
or use it to determine whether performance
problems that you may experience in VS are due to
VS or the Antlr language server.

## Commands

### Alias

Aliases allow a string to be substituted for a word when it is used as the
first word of a simple command. The shell maintains a list of aliases that
may be set and unset with the alias and unalias builtin commands.

The first word of each simple command is checked to see if it has an alias.
If so, that word is replaced by the text of the alias.
Only [a-zA-Z_] appear in an alias name. The replacement text may contain
any valid shell input, including shell metacharacters. The first word of
 the replacement text is tested for aliases, but a word that is identical
 to an alias being expanded is not expanded a second time. This means that
 one may alias ls to "ls -F", for instance, and Bash does not try to recursively
 expand the replacement text. If the last character of the alias value is a
 blank, then the next command word following the alias is also checked
 for alias expansion.

Aliases are created and listed with the alias command, and removed with the
 unalias command.

`alias id=string`

* Set up an alias that assigns _string_ to _id_. The command _string_ 
is executed with _id_.

### Analyze

_Trash_ can perform an analysis of a grammar. The analysis includes a count of symbol
type, cycles, and unused symbols.

`analyze`


### History expansion

_Trash_ keeps a persistent history of prior commands in ~/.trash.rc that is read
when the program starts. 
History expansions introduce words from the history list into the input stream,
making it easy to repeat commands. Currently there is no editing
capability.

`!!`

* Execute the previous command.

`!n`

* Execute the command line _n_.

`!string`

* Execute the command that begins with _string_.

### Convert

`convert`

* Convert the parsed grammar file at the top of stack into Antlr4 syntax. The
resulting Antlr4 grammar replaces the top of stack.

### "."

`.`

* Print out the parse tree for the file at the top of stack.

### Find

`find xpath-string`

* Find all sub-trees in the parsed file at the top of stack using the given XPath expression.

### Fold

### History

`history`

* Print out the shell command history.

### Parse

`parse grammar-type`

* Parse the flie at the top of stack with the given parser type (_antlr2_, _antlr3, _antlr4_, or _bison_).

### Pop

`pop`

* Pop the top document from the stack. If the stack is empty, nothing is further popped.
There is no check as to whether the document has been written to disk. If you want to write
the file, use `write`.

### Print

`print`

* Print out text file at the top of stack.

### Quit

`quit` or `exit`

* Exit the shell program.

### Read

`read file-name`

* Read the text file _file-name_ and place it on the top of the stack.

### Rename

### Rotate

`rotate`

* Rotate the stack once.

### Remove Useless Parentheses

`rup xpath-expression`

* Find all blocks as specified by the xpath expression in the parsed file at the top of stack.
Rewrite the node with the parentheses removed, if the block satifies three constraints:
(1) the expression must be a `block` type in the Antlr4 grammar;
(2) the `block` node must have an `altList` that does not contain more than one child;
(3) the `ebnf` parent of `block` must not contain a `blockSuffix`.

### Stack

`stack`

* Print the stack of files.

### ULLiteral

`ulliteral xpath-expr`

* The ulliteral command applies the "upper- and lower-case string literal"
transform to a collection of terminal nodes in the parse tree,
which is identified with the supplied xpath expression. Prior to using this command,
the document must have been parsed.
The ulliteral operation substitutes a sequence of 
sets containing an upper and lower case characters
for a `STRING_LITERAL`.
The expression must point to the right-hand side `STRING_LITERAL` of
a parser or lexer rule.
The resulting code is parsed and placed
on the top of stack.

### Unalias

### Unfold

`unfold xpath-expr`

* The unfold command applies the unfold transform to a collection of terminal nodes in the parse tree,
which is identified with the supplied xpath expression. Prior to using this command, you must have the file parsed.
An unfold operation substitutes the right-hand side of a parser or lexer rule
into a reference of the rule name that occurs at the specified node.
The resulting code is parsed and placed
on the top of stack.

### Write

`write`

* Pop the stack, and write out the file specified.



