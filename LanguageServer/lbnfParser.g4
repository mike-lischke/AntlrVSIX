// -*- Java -*- This ANTLRv4 file was machine-generated by BNFC
parser grammar lbnfParser;

options
{
    tokenVocab = lbnfLexer;
    contextSuperClass=AttributedParseTreeNode;
}

start_Grammar
  : grammar_ EOF
  ;
start_ListDef
  : listDef EOF
  ;
start_ListItem
  : listItem EOF
  ;
start_Def
  : def EOF
  ;
start_Item
  : item EOF
  ;
start_Cat
  : cat EOF
  ;
start_Label
  : label EOF
  ;
start_LabelId
  : labelId EOF
  ;
start_ProfItem
  : profItem EOF
  ;
start_IntList
  : intList EOF
  ;
start_ListInteger
  : listInteger EOF
  ;
start_ListIntList
  : listIntList EOF
  ;
start_ListProfItem
  : listProfItem EOF
  ;
start_ListString
  : listString EOF
  ;
start_ListRHS
  : listRHS EOF
  ;
start_RHS
  : rHS EOF
  ;
start_MinimumSize
  : minimumSize EOF
  ;
start_Reg2
  : reg2 EOF
  ;
start_Reg1
  : reg1 EOF
  ;
start_Reg3
  : reg3 EOF
  ;
start_Reg
  : reg EOF
  ;
start_ListIdent
  : listIdent EOF
  ;

grammar_
  : listDef
  ;
listDef
  :
  | listDef def Surrogate_id_SYMB_0
  ;
listItem
  :
  | listItem item
  ;
def
  : label Surrogate_id_SYMB_1 cat Surrogate_id_SYMB_2 listItem
  | Surrogate_id_SYMB_19 STRING
  | Surrogate_id_SYMB_19 STRING STRING
  | Surrogate_id_SYMB_23 label Surrogate_id_SYMB_1 cat Surrogate_id_SYMB_2 listItem
  | Surrogate_id_SYMB_33 IDENT reg
  | Surrogate_id_SYMB_28 Surrogate_id_SYMB_33 IDENT reg
  | Surrogate_id_SYMB_21 listIdent
  | Surrogate_id_SYMB_30 minimumSize cat STRING
  | Surrogate_id_SYMB_32 minimumSize cat STRING
  | Surrogate_id_SYMB_18 IDENT INTEGER
  | Surrogate_id_SYMB_29 IDENT Surrogate_id_SYMB_2 listRHS
  | Surrogate_id_SYMB_24 listString
  | Surrogate_id_SYMB_24 Surrogate_id_SYMB_31 listString
  | Surrogate_id_SYMB_24 Surrogate_id_SYMB_34
  ;
item
  : STRING
  | cat
  ;
cat
  : Surrogate_id_SYMB_3 cat Surrogate_id_SYMB_4
  | IDENT
  ;
label
  : labelId
  | labelId listProfItem
  | labelId labelId listProfItem
  | labelId labelId
  ;
labelId
  : IDENT
  | Surrogate_id_SYMB_5
  | Surrogate_id_SYMB_3 Surrogate_id_SYMB_4
  | Surrogate_id_SYMB_6 Surrogate_id_SYMB_7 Surrogate_id_SYMB_8
  | Surrogate_id_SYMB_6 Surrogate_id_SYMB_7 Surrogate_id_SYMB_3 Surrogate_id_SYMB_4 Surrogate_id_SYMB_8
  ;
profItem
  : Surrogate_id_SYMB_6 Surrogate_id_SYMB_3 listIntList Surrogate_id_SYMB_4 Surrogate_id_SYMB_9 Surrogate_id_SYMB_3 listInteger Surrogate_id_SYMB_4 Surrogate_id_SYMB_8
  ;
intList
  : Surrogate_id_SYMB_3 listInteger Surrogate_id_SYMB_4
  ;
listInteger
  :
  | INTEGER
  | INTEGER Surrogate_id_SYMB_9 listInteger
  ;
listIntList
  :
  | intList
  | intList Surrogate_id_SYMB_9 listIntList
  ;
listProfItem
  : profItem
  | profItem listProfItem
  ;
listString
  : STRING
  | STRING Surrogate_id_SYMB_9 listString
  ;
listRHS
  : rHS
  | rHS Surrogate_id_SYMB_10 listRHS
  ;
rHS
  : listItem
  ;
minimumSize
  : Surrogate_id_SYMB_27
  |
  ;
reg2
  : reg2 reg3
  | reg3
  ;
reg1
  : reg1 Surrogate_id_SYMB_10 reg2
  | reg2 Surrogate_id_SYMB_11 reg2
  | reg2
  ;
reg3
  : reg3 Surrogate_id_SYMB_12
  | reg3 Surrogate_id_SYMB_13
  | reg3 Surrogate_id_SYMB_14
  | Surrogate_id_SYMB_22
  | CHAR
  | Surrogate_id_SYMB_3 STRING Surrogate_id_SYMB_4
  | Surrogate_id_SYMB_15 STRING Surrogate_id_SYMB_16
  | Surrogate_id_SYMB_20
  | Surrogate_id_SYMB_25
  | Surrogate_id_SYMB_35
  | Surrogate_id_SYMB_26
  | Surrogate_id_SYMB_17
  | Surrogate_id_SYMB_6 reg Surrogate_id_SYMB_8
  ;
reg
  : reg1
  ;
listIdent
  : IDENT
  | IDENT Surrogate_id_SYMB_9 listIdent
  ;
