¶
iC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Repositories\IBaseRepositoryNoLazyLoad.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Repositories )
{ 
public 

	interface %
IBaseRepositoryNoLazyLoad .
<. /
T/ 0
>0 1
where2 7
T8 9
:: ;
class< A
{ 
void 
Refresh 
( 
T 
obj 
) 
; 
void		 
Add		 
(		 
T		 
obj		 
)		 
;		 
void 
AddNotCommit 
( 
T 
obj 
)  
;  !
void 
AddAll 
( 
IEnumerable 
<  
T  !
>! "
obj# &
)& '
;' (
void 
AddOrUpdateAll 
( 
IEnumerable '
<' (
T( )
>) *
obj+ .
). /
;/ 0
void 
AddAllNotCommit 
( 
IEnumerable (
<( )
T) *
>* +
obj, /
)/ 0
;0 1
void #
AddOrUpdateAllNotCommit $
($ %
IEnumerable% 0
<0 1
T1 2
>2 3
obj4 7
)7 8
;8 9
T 	
GetById
 
( 
int 
id 
) 
; 
IEnumerable 
< 
T 
> 
GetAll 
( 
) 
;  
IEnumerable 
< 
T 
> 
GetAllAsNoTracking )
() *
)* +
;+ ,
void 
Update 
( 
T 
obj 
) 
; 
void 
	UpdateAll 
( 
IEnumerable "
<" #
T# $
>$ %
listObj& -
)- .
;. /
void 
UpdateNotCommit 
( 
T 
obj "
)" #
;# $
void!! 
Remove!! 
(!! 
T!! 
obj!! 
)!! 
;!! 
void## 
RemoveAndCommit## 
(## 
T## 
obj## "
)##" #
;### $
void%% 
Delete%% 
(%% 
int%% 
id%% 
)%% 
;%% 
void'' 
	RemoveAll'' 
('' 
IEnumerable'' "
<''" #
T''# $
>''$ %
obj''& )
)'') *
;''* +
T)) 	
First))
 
()) 
))) 
;)) 
void++ 
AddOrUpdate++ 
(++ 
T++ 
obj++ 
)++ 
;++  
void--  
AddOrUpdateNotCommit-- !
(--! "
T--" #
obj--$ '
)--' (
;--( )
void// 
Commit// 
(// 
)// 
;// 
void11 
Dispose11 
(11 
)11 
;11 
void33 
Dettach33 
(33 
T33 
obj33 
)33 
;33 
}44 
}55 Ï
_C:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Repositories\IBaseRepository.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Repositories )
{ 
public 

	interface 
IBaseRepository $
<$ %
T% &
>& '
where( -
T. /
:0 1
class2 7
{ 
void 
Refresh 
( 
T 
obj 
) 
; 
void		 
Add		 
(		 
T		 
obj		 
)		 
;		 
void 
AddNotCommit 
( 
T 
obj 
)  
;  !
void 
AddAll 
( 
IEnumerable 
<  
T  !
>! "
obj# &
)& '
;' (
void 
AddOrUpdateAll 
( 
IEnumerable '
<' (
T( )
>) *
obj+ .
). /
;/ 0
void 
AddAllNotCommit 
( 
IEnumerable (
<( )
T) *
>* +
obj, /
)/ 0
;0 1
void #
AddOrUpdateAllNotCommit $
($ %
IEnumerable% 0
<0 1
T1 2
>2 3
obj4 7
)7 8
;8 9
T 	
GetById
 
( 
int 
id 
) 
; 
IEnumerable 
< 
T 
> 
GetAll 
( 
) 
;  
IEnumerable 
< 
T 
> 
GetAllAsNoTracking )
() *
)* +
;+ ,
void 
Update 
( 
T 
obj 
) 
; 
void 
	UpdateAll 
( 
IEnumerable "
<" #
T# $
>$ %
listObj& -
)- .
;. /
void 
UpdateNotCommit 
( 
T 
obj "
)" #
;# $
void!! 
Remove!! 
(!! 
T!! 
obj!! 
)!! 
;!! 
void## 
RemoveAndCommit## 
(## 
T## 
obj## "
)##" #
;### $
void%% 
Delete%% 
(%% 
int%% 
id%% 
)%% 
;%% 
void'' 
	RemoveAll'' 
('' 
IEnumerable'' "
<''" #
T''# $
>''$ %
obj''& )
)'') *
;''* +
T)) 	
First))
 
()) 
))) 
;)) 
void++ 
AddOrUpdate++ 
(++ 
T++ 
obj++ 
)++ 
;++  
void--  
AddOrUpdateNotCommit-- !
(--! "
T--" #
obj--$ '
)--' (
;--( )
void// 
Commit// 
(// 
)// 
;// 
void11 
Dispose11 
(11 
)11 
;11 
void33 
Dettach33 
(33 
T33 
obj33 
)33 
;33 
int55 

ExecuteSql55 
(55 
string55 
v55 
)55  
;55  !
List77 
<77 
T77 
>77 
ExecuteSqlQuery77 
(77  
string77  &
v77' (
)77( )
;77) *
void99 
AddOrUpdate99 
(99 
T99 
obj99 
,99 
bool99  $
useTransaction99% 3
)993 4
;994 5
}:: 
};; à
hC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Repositories\IGetDataResultRepository.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Repositories )
{ 
public 

	interface $
IGetDataResultRepository -
<- .
T. /
>/ 0
where1 6
T7 8
:9 :
class; @
{ 
IEnumerable 
<  
ConsolidationLevel01 (
>( )*
GetLastEntryConsildatedLevel01* H
(H I
)I J
;J K
IEnumerable		 
<		  
ConsolidationLevel02		 (
>		( )*
GetLastEntryConsildatedLevel02		* H
(		H I
IEnumerable		I T
<		T U 
ConsolidationLevel01		U i
>		i j
cl1		k n
)		n o
;		o p
IEnumerable

 
<

 
CollectionLevel02

 %
>

% &)
GetLastEntryCollectionLevel02

' D
(

D E
IEnumerable

E P
<

P Q 
ConsolidationLevel02

Q e
>

e f
cl2

g j
)

j k
;

k l
IEnumerable 
< 
CollectionLevel03 %
>% &)
GetLastEntryCollectionLevel03' D
(D E
IEnumerableE P
<P Q
CollectionLevel02Q b
>b c
cll2d h
)h i
;i j
CollectionHtml 
GetHtmlLastEntry '
(' (
SyncDTO( /
	idUnidade0 9
)9 :
;: ;
IEnumerable 
<  
ConsolidationLevel01 (
>( )1
%GetLastEntryConsildatedLevel01ToMerge* O
(O P
)P Q
;Q R 
ConsolidationLevel01 ,
 GetExistentLevel01Consollidation =
(= > 
ConsolidationLevel01> R 
level01ConsolidationS g
)g h
;h i 
ConsolidationLevel02 ,
 GetExistentLevel02Consollidation =
(= > 
ConsolidationLevel02> R 
level02ConsolidationS g
,g h 
ConsolidationLevel01i }!
consolidationLevel01	~ í
)
í ì
;
ì î
void 
Remove 
( 
int 
id 
) 
; 
void 
SetDuplicated 
( 
IEnumerable &
<& '
CollectionLevel02' 8
>8 9+
collectionAVerificarDuplicidade: Y
,Y Z
int[ ^
	level01Id_ h
)h i
;i j
} 
} ó'
aC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Repositories\IParamsRepository.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Repositories )
{ 
public 

	interface 
IParamsRepository &
{ 
void 
SaveParLevel1 
( 
	ParLevel1 $
saveParamLevel1% 4
,4 5
List6 :
<: ;
ParHeaderField; I
>I J
listaParHEadFieldK \
,\ ]
List^ b
<b c
ParLevel1XClusterc t
>t u#
listaParLevel1XCluster	v å
,
å ç
List
é í
<
í ì
int
ì ñ
>
ñ ó
removerHeadField
ò ®
,
® ©
List
™ Æ
<
Æ Ø
ParCounterXLocal
Ø ø
>
ø ¿"
listaParCounterLocal
¡ ’
,
’ ÷
List
◊ €
<
€ ‹(
ParNotConformityRuleXLevel
‹ ˆ
>
ˆ ˜!
listNonCoformitRule
¯ ã
,
ã å
List
ç ë
<
ë í

ParRelapse
í ú
>
ú ù
listaReincidencia
û Ø
,
Ø ∞
List
± µ
<
µ ∂
ParGoal
∂ Ω
>
Ω æ
listParGoal
ø  
)
  À
;
À Ã
void		 
SaveParLevel2		 
(		 
	ParLevel2		 $
saveParamLevel2		% 4
,		4 5
List		6 :
<		: ;
ParLevel3Group		; I
>		I J
listaParLevel3Group		K ^
,		^ _
List		` d
<		d e
ParCounterXLocal		e u
>		u v!
listParCounterXLocal			w ã
,
		ã å
List
		ç ë
<
		ë í(
ParNotConformityRuleXLevel
		í ¨
>
		¨ ≠.
 saveParamNotConformityRuleXLevel
		Æ Œ
,
		Œ œ
List
		– ‘
<
		‘ ’
ParEvaluation
		’ ‚
>
		‚ „!
saveParamEvaluation
		‰ ˜
,
		˜ ¯
List
		˘ ˝
<
		˝ ˛
	ParSample
		˛ á
>
		á à
saveParamSample
		â ò
,
		ò ô
List
		ö û
<
		û ü

ParRelapse
		ü ©
>
		© ™
listParRelapse
		´ π
)
		π ∫
;
		∫ ª
void  
RemoveParLevel3Group !
(! "
ParLevel3Group" 0
paramLevel03group1 B
)B C
;C D
void 
SaveParLocal 
( 
ParLocal "

paramLocal# -
)- .
;. /
void 
SaveParCounter 
( 

ParCounter &
paramCounter' 3
)3 4
;4 5
void  
SaveParCounterXLocal !
(! "
ParCounterXLocal" 2
paramCounterLocal3 D
)D E
;E F
void 
SaveParRelapse 
( 

ParRelapse &
paramRelapse' 3
)3 4
;4 5
void $
SaveParNotConformityRule %
(% & 
ParNotConformityRule& :"
paramNotConformityRule; Q
)Q R
;R S
void *
SaveParNotConformityRuleXLevel +
(+ ,&
ParNotConformityRuleXLevel, F"
paramNotConformityRuleG ]
)] ^
;^ _
void 
SaveParCompany 
( 

ParCompany &
paramCompany' 3
)3 4
;4 5
void 
SaveParLevel3Level2  
(  !
ParLevel3Level2! 0
paramLevel3Level21 B
)B C
;C D
void 
SaveParLevel3 
( 
	ParLevel3 $
saveParamLevel3% 4
,4 5
List6 :
<: ;
ParLevel3Value; I
>I J$
listSaveParamLevel3ValueK c
,c d
Liste i
<i j

ParRelapsej t
>t u
listParRelapse	v Ñ
,
Ñ Ö
List
Ü ä
<
ä ã
ParLevel3Level2
ã ö
>
ö õ#
parLevel3Level2pontos
ú ±
,
± ≤
int
≥ ∂
level1Id
∑ ø
)
ø ¿
;
¿ ¡
void 

ExecuteSql 
( 
string 
sql "
)" #
;# $!
ParLevel2XHeaderField 
SaveParHeaderLevel2 1
(1 2!
ParLevel2XHeaderField2 G!
parLevel2XHeaderFieldH ]
)] ^
;^ _
void 
SaveVinculoL3L2L1 
( 
int "
idLevel1# +
,+ ,
int- 0
idLevel21 9
,9 :
int; >
idLevel3? G
,G H
intI L
?L M
userIdN T
,T U
intV Y
?Y Z
	companyId[ d
)d e
;e f
} 
} ¨
dC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Repositories\IParLevel3Repository.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Repositories )
{ 
public 

	interface  
IParLevel3Repository )
{ 
List 
< 
ParLevel3Level2 
> $
GetLevel3VinculadoLevel2 6
(6 7
int7 :
idLevel1; C
)C D
;D E
} 
}		 ¬
jC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Repositories\IRelatorioColetaRepository.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Repositories )
{ 
public 

	interface &
IRelatorioColetaRepository /
</ 0
T0 1
>1 2
where3 8
T9 :
:; <
class= B
{ 
IEnumerable 
< 
T 
> 
	GetByDate  
(  !!
DataCarrierFormulario! 6
form7 ;
); <
;< =
IEnumerable

 
<

  
ConsolidationLevel01

 (
>

( ),
 GetEntryConsildatedLevel01ByDate

* J
(

J K!
DataCarrierFormulario

K `
form

a e
)

e f
;

f g
IEnumerable 
<  
ConsolidationLevel01 (
>( )3
'GetEntryConsildatedLevel01ByDateAndUnit* Q
(Q R!
DataCarrierFormularioR g
formh l
)l m
;m n
IEnumerable 
< 
CollectionLevel02 %
>% &)
GetLastEntryCollectionLevel02' D
(D E
IEnumerableE P
<P Q 
ConsolidationLevel02Q e
>e f
cl2g j
,j k"
DataCarrierFormulario	l Å
form
Ç Ü
)
Ü á
;
á à
IEnumerable 
< 
CollectionLevel03 %
>% &)
GetLastEntryCollectionLevel03' D
(D E
IEnumerableE P
<P Q
CollectionLevel02Q b
>b c
cll2d h
,h i!
DataCarrierFormularioj 
form
Ä Ñ
)
Ñ Ö
;
Ö Ü
IEnumerable 
<  
ConsolidationLevel02 (
>( )*
GetLastEntryConsildatedLevel02* H
(H I
IEnumerableI T
<T U 
ConsolidationLevel01U i
>i j
cl1k n
)n o
;o p
IEnumerable 
<  
ConsolidationLevel02 (
>( ),
 GetEntryConsildatedLevel02ByDate* J
(J K!
DataCarrierFormularioK `
forma e
)e f
;f g
} 
} ƒ
cC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Repositories\ISaveCollectionRepo.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Repositories )
{ 
public 

	interface 
ISaveCollectionRepo (
{ 
void		 
SaveAllLevel		 
(		 
List		 
<		 
CollectionLevel02		 0
>		0 1$
_collectionLevel02ToSave		2 J
,		J K
List		L P
<		P Q
CollectionLevel03		Q b
>		b c$
_collectionLevel03ToSave		d |
,		| }
List			~ Ç
<
		Ç É
CorrectiveAction
		É ì
>
		ì î%
_correctiveActionToSave
		ï ¨
)
		¨ ≠
;
		≠ Æ
}

 
} €	
_C:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Repositories\IUserRepository.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Repositories )
{ 
public 

	interface 
IUserRepository $
{ 
UserSgq 
AuthenticationLogin #
(# $
UserSgq$ +
user, 0
)0 1
;1 2
void 
Salvar 
( 
UserSgq 
user  
)  !
;! "
UserSgq 
	GetByName 
( 
string  
username! )
)) *
;* +
List 
< 
UserSgq 
> 

GetAllUser  
(  !
)! "
;" #
bool  
UserNameIsCadastrado !
(! "
string" (
Name) -
,- .
int/ 2
id3 5
)5 6
;6 7
List 
< 
UserSgq 
> 
GetAllUserByUnit &
(& '
int' *
	unidadeId+ 4
)4 5
;5 6
} 
} ã
fC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Repositories\ICollectionLevel02Repo.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Repositories )
{ 
public 

	interface "
ICollectionLevel02Repo +
{ 
void #
UpdateCollectionLevel02 $
($ %
CollectionLevel02% 6
collectionLevel027 H
)H I
;I J
} 
} Ω
YC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Services\IDefectDomain.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Services %
{ 
public 

	interface 
IDefectDomain "
{		 
void

 
MergeDefect

 
(

 
List

 
<

 
	DefectDTO

 '
>

' (
	defectDTO

) 2
)

2 3
;

3 4
List 
< 
	DefectDTO 
> 

GetDefects "
(" #
int# &
ParCompany_Id' 4
)4 5
;5 6
} 
} ÷
ZC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Services\ICompanyDomain.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Services %
{ 
public 

	interface 
ICompanyDomain #
{ 
ParCompanyDTO		 
AddUpdateParCompany		 )
(		) *
ParCompanyDTO		* 7
parCompanyDTO		8 E
)		E F
;		F G
ParStructureDTO

 !
AddUpdateParStructure

 -
(

- .
ParStructureDTO

. =
parStructureDTO

> M
)

M N
;

N O 
ParStructureGroupDTO &
AddUpdateParStructureGroup 7
(7 8 
ParStructureGroupDTO8 L 
parStructureGroupDTOM a
)a b
;b c#
ParCompanyXStructureDTO ,
 AddUpdateParCompanyXStructureDTO  @
(@ A#
ParCompanyXStructureDTOA X#
parCompanyXStructureDTOY p
)p q
;q r
void 
SaveParCompany 
( 

ParCompany &

parCompany' 1
)1 2
;2 3
void $
SaveParCompanyXStructure %
(% &
List& *
<* + 
ParCompanyXStructure+ ?
>? @$
listParCompanyXStructureA Y
,Y Z

ParCompany[ e

parCompanyf p
)p q
;q r
void !
SaveParCompanyCluster "
(" #
List# '
<' (
ParCompanyCluster( 9
>9 :!
listParCompanyCluster; P
,P Q

ParCompanyR \

parCompany] g
)g h
;h i
} 
} Ò
ZC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Services\IExampleDomain.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Services %
{ 
public 

	interface 
IExampleDomain #
{ 
ContextExampleDTO 
AddUpdateExample *
(* +
ContextExampleDTO+ <
	paramsDto= F
)F G
;G H
}

 
} ¿
YC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Services\IParamsDomain.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Services %
{ 
public 

	interface 
IParamsDomain "
{ 
List		 
<		 
ParLevel1DTO		 
>		 
GetAllLevel1		 '
(		' (
)		( )
;		) *
	ParamsDdl "
CarregaDropDownsParams (
(( )
)) *
;* +
	ParamsDTO 
AddUpdateLevel1 !
(! "
	ParamsDTO" +
	paramsDto, 5
)5 6
;6 7
ParLevel1DTO 
	GetLevel1 
( 
int "
IdParLevel1# .
). /
;/ 0
	ParamsDTO 
AddUpdateLevel2 !
(! "
	ParamsDTO" +
	paramsDto, 5
)5 6
;6 7
	ParamsDTO 
	GetLevel2 
( 
int 
idParLevel2  +
,+ ,
int- 0
idParLevel31 <
,< =
int> A
idParLevel1B M
)M N
;N O
ParLevel3GroupDTO  
RemoveParLevel3Group .
(. /
int/ 2
Id3 5
)5 6
;6 7
	ParamsDTO 
AddUpdateLevel3 !
(! "
	ParamsDTO" +
	paramsDto, 5
)5 6
;6 7
	ParamsDTO 
	GetLevel3 
( 
int 
idParLevel3  +
,+ ,
int- 0
?0 1
idParLevel22 =
)= >
;> ?
List 
< $
ParLevel3Level2Level1DTO %
>% &
AddVinculoL1L2' 5
(5 6
int6 9
idLevel1: B
,B C
intD G
idLevel2H P
,P Q
intR U
idLevel3V ^
,^ _
int` c
?c d
userIde k
,k l
intm p
?p q
	companyIdr {
=| }
null	~ Ç
)
Ç É
;
É Ñ
ParLevel3Level2DTO 
AddVinculoL3L2 )
() *
int* -
idLevel2. 6
,6 7
int8 ;
idLevel3< D
,D E
decimalF M
pesoN R
,R S
intT W
?W X
groupLevel2Y d
)d e
;e f
bool 
RemVinculoL1L2 
( 
int 
idLevel1  (
,( )
int* -
idLevel2. 6
)6 7
;7 8
bool )
VerificaShowBtnRemVinculoL1L2 *
(* +
int+ .
idLevel1/ 7
,7 8
int9 <
idLevel2= E
)E F
;F G
bool &
SetRequiredCamposCabecalho '
(' (
int( +
id, .
,. /
int0 3
required4 <
)< =
;= >
ParMultipleValues %
SetDefaultMultiplaEscolha 3
(3 4
int4 7
idHeader8 @
,@ A
intB E

idMultipleF P
)P Q
;Q R!
ParLevel2XHeaderField $
AddRemoveParHeaderLevel2 6
(6 7!
ParLevel2XHeaderField7 L!
parLevel2XHeaderFieldM b
)b c
;c d
}"" 
}## ¬
bC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Services\IRelatorioColetaDomain.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Services %
{ 
public 

	interface "
IRelatorioColetaDomain +
{ 
GenericReturn		 
<		 $
ResultSetRelatorioColeta		 .
>		. / 
GetCollectionLevel02		0 D
(		D E!
DataCarrierFormulario		E Z
form		[ _
)		_ `
;		` a
GenericReturn

 
<

 $
ResultSetRelatorioColeta

 .
>

. / 
GetCollectionLevel03

0 D
(

D E!
DataCarrierFormulario

E Z
form

[ _
)

_ `
;

` a
GenericReturn 
< $
ResultSetRelatorioColeta .
>. /#
GetConsolidationLevel010 G
(G H!
DataCarrierFormularioH ]
form^ b
)b c
;c d
GenericReturn 
< $
ResultSetRelatorioColeta .
>. /#
GetConsolidationLevel020 G
(G H!
DataCarrierFormularioH ]
form^ b
)b c
;c d
GenericReturn 
< 

GetSyncDTO  
>  !
GetEntryByDate" 0
(0 1!
DataCarrierFormulario1 F
formG K
)K L
;L M
} 
} Ê
WC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Services\IBaseDomain.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Services %
{ 
public 

	interface 
IBaseDomain  
<  !
T! "
," #
Y$ %
>% &
where' ,
T- .
:/ 0
class1 6
where7 <
Y= >
:? @
classA F
{ 
Y 	
GetById
 
( 
int 
id 
) 
; 
Y 	
GetByIdNoLazyLoad
 
( 
int 
id  "
)" #
;# $
IEnumerable 
< 
Y 
> 
GetAll 
( 
) 
;  
IEnumerable 
< 
Y 
> 
GetAllNoLazyLoad '
(' (
)( )
;) *
Y 	
First
 
( 
) 
; 
Y 	
FirstNoLazyLoad
 
( 
) 
; 
Y 	
AddOrUpdate
 
( 
Y 
obj 
) 
; 
int 

ExecuteSql 
( 
string 
v 
)  
;  !
Y 	
AddOrUpdate
 
( 
Y 

userSgqDto "
," #
bool$ (
v) *
)* +
;+ ,
} 
}   ª

WC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Interfaces\Services\IUserDomain.cs
	namespace 	
Dominio
 
. 

Interfaces 
. 
Services %
{ 
public 

	interface 
IUserDomain  
{ 
GenericReturn		 
<		 
UserDTO		 
>		 
AuthenticationLogin		 2
(		2 3
UserDTO		3 :
user		; ?
)		? @
;		@ A
GenericReturn

 
<

 
UserDTO

 
>

 
	GetByName

 (
(

( )
string

) /
username

0 8
)

8 9
;

9 :
GenericReturn 
< 

UserSgqDTO  
>  !

GetByName2" ,
(, -
string- 3
username4 <
)< =
;= >
GenericReturn 
< 
List 
< 
UserDTO "
>" #
># $"
GetAllUserValidationAd% ;
(; <
UserDTO< C
userDtoD K
)K L
;L M
List 
< 
UserDTO 
> 
GetAllUserByUnit &
(& '
int' *
	unidadeId+ 4
)4 5
;5 6
} 
} Å
MC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\LogService\LoggerNLog.cs
	namespace 	
Dominio
 
. 

LogService 
{ 
public 

static 
class 

LoggerNLog "
{ 
public 
static 
Logger  
logger! '
=( )

LogManager* 4
.4 5!
GetCurrentClassLogger5 J
(J K
)K L
;L M
} 
}		 ˙
OC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str "
)" #
]# $
[		 
assembly		 	
:			 

AssemblyDescription		 
(		 
$str		 !
)		! "
]		" #
[

 
assembly

 	
:

	 
!
AssemblyConfiguration

  
(

  !
$str

! #
)

# $
]

$ %
[ 
assembly 	
:	 

AssemblyCompany 
( 
$str 
) 
] 
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str $
)$ %
]% &
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str 0
)0 1
]1 2
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
[ 
assembly 	
:	 

Guid 
( 
$str 6
)6 7
]7 8
[## 
assembly## 	
:##	 

AssemblyVersion## 
(## 
$str## $
)##$ %
]##% &
[$$ 
assembly$$ 	
:$$	 

AssemblyFileVersion$$ 
($$ 
$str$$ (
)$$( )
]$$) *ﬁ*
MC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Services\DefectDomain.cs
	namespace 	
Dominio
 
. 
Services 
{ 
public 

class 
DefectDomain 
: 
IDefectDomain  -
{ 
private 
IBaseRepository 
<  
Defect  &
>& '
_baseRepoDefect( 7
;7 8
public 
DefectDomain 
( 
IBaseRepository 
< 
Defect "
>" #
baseRepoDefect$ 2
) 
{ 	
_baseRepoDefect 
= 
baseRepoDefect ,
;, -
} 	
public%% 
void%% 
MergeDefect%% 
(%%  
List%%  $
<%%$ %
	DefectDTO%%% .
>%%. /
listDefectDto%%0 =
)%%= >
{&& 	
if'' 
('' 
listDefectDto'' 
!='' 
null''  $
)''$ %
{(( 
foreach)) 
()) 
	DefectDTO)) "
	defectDTO))# ,
in))- /
listDefectDto))0 =
)))= >
{** 
List++ 
<++ 
Defect++ 
>++  
defects++! (
=++) *
_baseRepoDefect+++ :
.++: ;
GetAll++; A
(++A B
)++B C
.++C D
Where++D I
(++I J
r++J K
=>++L N
r,, 
.,, 
CurrentEvaluation,, '
==,,( *
	defectDTO,,+ 4
.,,4 5
CurrentEvaluation,,5 F
&&,,G I
r-- 
.-- 
ParCompany_Id-- #
==--$ &
	defectDTO--' 0
.--0 1
ParCompany_Id--1 >
&&--? A
r.. 
... 
ParLevel1_Id.. "
==..# %
	defectDTO..& /
.../ 0
ParLevel1_Id..0 <
)..< =
...= >
ToList..> D
(..D E
)..E F
;..F G
if00 
(00 
defects00 
.00  
Count00  %
==00& (
$num00) *
)00* +
{11 
Defect22 
defect22 %
=22& '
Mapper22( .
.22. /
Map22/ 2
<222 3
Defect223 9
>229 :
(22: ;
	defectDTO22; D
)22D E
;22E F
defect33 
.33 
AddDate33 &
=33' (
DateTime33) 1
.331 2
Now332 5
;335 6
defect44 
.44 
Active44 %
=44& '
true44( ,
;44, -
_baseRepoDefect66 '
.66' (
Add66( +
(66+ ,
defect66, 2
)662 3
;663 4
}77 
else88 
{99 
var:: 
defect:: "
=::# $
defects::% ,
.::, -
FirstOrDefault::- ;
(::; <
)::< =
;::= >
defect<< 
.<< 
Evaluations<< *
+=<<+ -
(<<. /
	defectDTO<</ 8
.<<8 9
Evaluations<<9 D
-<<E F
defect<<G M
.<<M N
Evaluations<<N Y
)<<Y Z
;<<Z [
defect== 
.== 
Defects== &
+===' )
(==* +
	defectDTO==+ 4
.==4 5
Defects==5 <
-=== >
defect==? E
.==E F
Defects==F M
)==M N
;==N O
defect>> 
.>> 
	AlterDate>> (
=>>) *
DateTime>>+ 3
.>>3 4
Now>>4 7
;>>7 8
defect?? 
.?? 
Active?? %
=??& '
true??( ,
;??, -
_baseRepoDefectAA '
.AA' (
UpdateAA( .
(AA. /
defectAA/ 5
)AA5 6
;AA6 7
}BB 
}CC 
}DD 
}FF 	
publicHH 
ListHH 
<HH 
	DefectDTOHH 
>HH 

GetDefectsHH )
(HH) *
intHH* -
ParCompany_IdHH. ;
)HH; <
{II 	
varJJ 
todayJJ 
=JJ 
DateTimeJJ  
.JJ  !
NowJJ! $
.JJ$ %
DateJJ% )
;JJ) *
varKK 
tomorrowKK 
=KK 
DateTimeKK #
.KK# $
NowKK$ '
.KK' (
AddDaysKK( /
(KK/ 0
$numKK0 1
)KK1 2
.KK2 3
DateKK3 7
;KK7 8
varRR 
listRR 
=RR 
MapperRR 
.RR 
MapRR !
<RR! "
ListRR" &
<RR& '
	DefectDTORR' 0
>RR0 1
>RR1 2
(RR2 3
_baseRepoDefectRR3 B
.RRB C
GetAllRRC I
(RRI J
)RRJ K
.RRK L
WhereRRL Q
(RRQ R
rRRR S
=>RRT V
rSS 
.SS 
ParCompany_IdSS 
==SS  "
ParCompany_IdSS# 0
&&SS1 3
GuardTT 
.TT 
InsideFrequencyTT %
(TT% &
rTT& '
.TT' (
DateTT( ,
.TT, -
DateTT- 1
,TT1 2
rTT3 4
.TT4 5
	ParLevel1TT5 >
.TT> ?
ParFrequency_IdTT? N
)TTN O
)TTO P
)TTP Q
;TTQ R
returnVV 
listVV 
;VV 
}WW 	
}ZZ 
}[[ —Ö
NC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Services\CompanyDomain.cs
	namespace 	
Dominio
 
. 
Services 
{ 
public 

class 
CompanyDomain 
:  
ICompanyDomain! /
{ 
private 
IBaseRepository 
<  

ParCompany  *
>* +
_baseRepoParCompany, ?
;? @
private 
IBaseRepository 
<  
ParStructure  ,
>, -!
_baseRepoParStructure. C
;C D
private 
IBaseRepository 
<  
ParStructureGroup  1
>1 2&
_baseRepoParStructureGroup3 M
;M N
private 
IBaseRepository 
<   
ParCompanyXStructure  4
>4 5)
_baseRepoParCompanyXStructure6 S
;S T
private 
IBaseRepository 
<  
ParCompanyCluster  1
>1 2&
_baseRepoParCompanyCluster3 M
;M N
public 
CompanyDomain 
( 
IBaseRepository 
< 

ParCompany &
>& '
baseRepoParCompany( :
,: ;
IBaseRepository 
< 
ParStructure (
>( ) 
baseRepoParStructure* >
,> ?
IBaseRepository 
< 
ParStructureGroup -
>- .%
baseRepoParStructureGroup/ H
,H I
IBaseRepository 
<  
ParCompanyXStructure 0
>0 1(
baseRepoParCompanyXStructure2 N
,N O
IBaseRepository 
< 
ParCompanyCluster -
>- .%
baseRepoParCompanyCluster/ H
) 
{ 	
_baseRepoParCompany 
=  !
baseRepoParCompany" 4
;4 5!
_baseRepoParStructure   !
=  " # 
baseRepoParStructure  $ 8
;  8 9&
_baseRepoParStructureGroup!! &
=!!' (%
baseRepoParStructureGroup!!) B
;!!B C)
_baseRepoParCompanyXStructure"" )
=""* +(
baseRepoParCompanyXStructure"", H
;""H I&
_baseRepoParCompanyCluster## &
=##' (%
baseRepoParCompanyCluster##) B
;##B C
}$$ 	
public44 
ParCompanyDTO44 
AddUpdateParCompany44 0
(440 1
ParCompanyDTO441 >
parCompanyDTO44? L
)44L M
{55 	

ParCompany66 
parCompanySalvar66 '
=66( )
Mapper66* 0
.660 1
Map661 4
<664 5

ParCompany665 ?
>66? @
(66@ A
parCompanyDTO66A N
)66N O
;66O P
_baseRepoParCompany88 
.88   
AddOrUpdateNotCommit88  4
(884 5
parCompanySalvar885 E
)88E F
;88F G
_baseRepoParCompany99 
.99  
Commit99  &
(99& '
)99' (
;99( )
parCompanyDTO;; 
.;; 
Id;; 
=;; 
parCompanySalvar;; /
.;;/ 0
Id;;0 2
;;;2 3
if== 
(== 
parCompanyDTO== 
.== !
ListParCompanyCluster== 3
!===4 6
null==7 ;
)==; <
foreach>> 
(>> 
var>> 
parCompanyCluster>> .
in>>/ 1
parCompanyDTO>>2 ?
.>>? @!
ListParCompanyCluster>>@ U
)>>U V
{?? 
parCompanyCluster@@ %
.@@% &
ParCompany_Id@@& 3
=@@4 5
parCompanyDTO@@6 C
.@@C D
Id@@D F
;@@F G
ParCompanyClusterAA %#
parCompanyClusterSalvarAA& =
=AA> ?
MapperAA@ F
.AAF G
MapAAG J
<AAJ K
ParCompanyClusterAAK \
>AA\ ]
(AA] ^
parCompanyClusterAA^ o
)AAo p
;AAp q&
_baseRepoParCompanyClusterBB .
.BB. / 
AddOrUpdateNotCommitBB/ C
(BBC D#
parCompanyClusterSalvarBBD [
)BB[ \
;BB\ ]&
_baseRepoParCompanyClusterCC .
.CC. /
CommitCC/ 5
(CC5 6
)CC6 7
;CC7 8
}DD 
ifFF 
(FF 
parCompanyDTOFF 
.FF $
ListParCompanyXStructureFF 6
!=FF7 9
nullFF: >
)FF> ?
foreachGG 
(GG 
varGG  
parCompanyXStructureGG 1
inGG2 4
parCompanyDTOGG5 B
.GGB C$
ListParCompanyXStructureGGC [
)GG[ \
{HH  
parCompanyXStructureII (
.II( )
ParCompany_IdII) 6
=II7 8
parCompanyDTOII9 F
.IIF G
IdIIG I
;III J 
ParCompanyXStructureJJ (&
parCompanyXStructureSalvarJJ) C
=JJD E
MapperJJF L
.JJL M
MapJJM P
<JJP Q 
ParCompanyXStructureJJQ e
>JJe f
(JJf g 
parCompanyXStructureJJg {
)JJ{ |
;JJ| })
_baseRepoParCompanyXStructureKK 1
.KK1 2 
AddOrUpdateNotCommitKK2 F
(KKF G&
parCompanyXStructureSalvarKKG a
)KKa b
;KKb c)
_baseRepoParCompanyXStructureLL 1
.LL1 2
CommitLL2 8
(LL8 9
)LL9 :
;LL: ;
}MM 
returnOO 
parCompanyDTOOO  
;OO  !
}PP 	
publicRR 
voidRR 
SaveParCompanyRR "
(RR" #

ParCompanyRR# -

parCompanyRR. 8
)RR8 9
{SS 	
ifUU 
(UU 

parCompanyUU 
.UU 
IdUU 
==UU  
$numUU! "
)UU" #
{VV 
_baseRepoParCompanyWW #
.WW# $
AddWW$ '
(WW' (

parCompanyWW( 2
)WW2 3
;WW3 4
}XX 
elseYY 
{ZZ 
Guard[[ 
.[[ 

verifyDate[[  
([[  !

parCompany[[! +
,[[+ ,
$str[[- 8
)[[8 9
;[[9 :
_baseRepoParCompany\\ #
.\\# $
Update\\$ *
(\\* +

parCompany\\+ 5
)\\5 6
;\\6 7
}]] 
}^^ 	
public`` 
void`` !
SaveParCompanyCluster`` )
(``) *
List``* .
<``. /
ParCompanyCluster``/ @
>``@ A!
listParCompanyCluster``B W
,``W X

ParCompany``Y c

parCompany``d n
)``n o
{aa 	
Listbb 
<bb 
ParCompanyClusterbb "
>bb" #
dbListbb$ *
=bb+ ,&
_baseRepoParCompanyClusterbb- G
.bbG H
GetAllbbH N
(bbN O
)bbO P
.bbP Q
WherebbQ V
(bbV W
rbbW X
=>bbY [
rbb\ ]
.bb] ^
ParCompany_Idbb^ k
==bbl n

parCompanybbo y
.bby z
Idbbz |
&&bb} 
r
bbÄ Å
.
bbÅ Ç
Active
bbÇ à
==
bbâ ã
true
bbå ê
)
bbê ë
.
bbë í
ToList
bbí ò
(
bbò ô
)
bbô ö
;
bbö õ
foreachdd 
(dd 
ParCompanyClusterdd &
companyClusterdd' 5
indd6 8
dbListdd9 ?
)dd? @
{ee 
ParCompanyClusterff !
saveff" &
=ff' (!
listParCompanyClusterff) >
.ff> ?
Whereff? D
(ffD E
rffE F
=>ffG I
rffJ K
.ffK L
ParCluster_IdffL Y
==ffZ \
companyClusterff] k
.ffk l
ParCluster_Idffl y
&&ffz |
rgg, -
.gg- .
ParCompany_Idgg. ;
==gg< >
companyClustergg? M
.ggM N
ParCompany_IdggN [
&&gg\ ^
rhh, -
.hh- .
Activehh. 4
==hh5 7
truehh8 <
)hh< =
.hh= >
FirstOrDefaulthh> L
(hhL M
)hhM N
;hhN O
ifjj 
(jj 
savejj 
==jj 
nulljj  
)jj  !
{kk 
companyClusterll "
.ll" #
Activell# )
=ll* +
falsell, 1
;ll1 2
Guardmm 
.mm 

verifyDatemm $
(mm$ %
companyClustermm% 3
,mm3 4
$strmm5 @
)mm@ A
;mmA B&
_baseRepoParCompanyClusternn .
.nn. /
Updatenn/ 5
(nn5 6
companyClusternn6 D
)nnD E
;nnE F
}oo 
elsepp 
{qq 
saverr 
.rr 
ParCompany_Idrr &
=rr' (
companyClusterrr) 7
.rr7 8
ParCompany_Idrr8 E
;rrE F
savess 
.ss 
Idss 
=ss 
companyClusterss ,
.ss, -
Idss- /
;ss/ 0
Guardtt 
.tt 

verifyDatett $
(tt$ %
companyClustertt% 3
,tt3 4
$strtt5 @
)tt@ A
;ttA B&
_baseRepoParCompanyClusteruu .
.uu. /
Updateuu/ 5
(uu5 6
companyClusteruu6 D
)uuD E
;uuE F
}vv !
listParCompanyClusterww %
.ww% &
Removeww& ,
(ww, -
saveww- 1
)ww1 2
;ww2 3
}xx 
foreachzz 
(zz 
ParCompanyClusterzz &
companyClusterzz' 5
inzz6 8!
listParCompanyClusterzz9 N
)zzN O
{{{ 
companyCluster|| 
.|| 
Active|| %
=||& '
true||( ,
;||, -
companyCluster}} 
.}} 
ParCompany_Id}} ,
=}}- .

parCompany}}/ 9
.}}9 :
Id}}: <
;}}< =&
_baseRepoParCompanyCluster~~ *
.~~* +
Add~~+ .
(~~. /
companyCluster~~/ =
)~~= >
;~~> ?
} 
}
ÄÄ 	
public
ÇÇ 
void
ÇÇ &
SaveParCompanyXStructure
ÇÇ ,
(
ÇÇ, -
List
ÇÇ- 1
<
ÇÇ1 2"
ParCompanyXStructure
ÇÇ2 F
>
ÇÇF G&
listParCompanyXStructure
ÇÇH `
,
ÇÇ` a

ParCompany
ÇÇb l

parCompany
ÇÇm w
)
ÇÇw x
{
ÉÉ 	
List
ÑÑ 
<
ÑÑ "
ParCompanyXStructure
ÑÑ %
>
ÑÑ% &
dbList
ÑÑ' -
=
ÑÑ. /+
_baseRepoParCompanyXStructure
ÑÑ0 M
.
ÑÑM N
GetAll
ÑÑN T
(
ÑÑT U
)
ÑÑU V
.
ÑÑV W
Where
ÑÑW \
(
ÑÑ\ ]
r
ÑÑ] ^
=>
ÑÑ_ a
r
ÑÑb c
.
ÑÑc d
ParCompany_Id
ÑÑd q
==
ÑÑr t

parCompany
ÑÑu 
.ÑÑ Ä
IdÑÑÄ Ç
&&ÑÑÉ Ö
rÑÑÜ á
.ÑÑá à
ActiveÑÑà é
==ÑÑè ë
trueÑÑí ñ
)ÑÑñ ó
.ÑÑó ò
ToListÑÑò û
(ÑÑû ü
)ÑÑü †
;ÑÑ† °
foreach
ÜÜ 
(
ÜÜ "
ParCompanyXStructure
ÜÜ )
companyStructure
ÜÜ* :
in
ÜÜ; =
dbList
ÜÜ> D
)
ÜÜD E
{
áá "
ParCompanyXStructure
àà $
save
àà% )
=
àà* +&
listParCompanyXStructure
àà, D
.
ààD E
Where
ààE J
(
ààJ K
r
ààK L
=>
ààM O
r
ààP Q
.
ààQ R
ParStructure_Id
ààR a
==
ààb d
companyStructure
ààe u
.
ààu v
ParStructure_Idààv Ö
&&ààÜ à
r
ââ, -
.
ââ- .
ParCompany_Id
ââ. ;
==
ââ< >
companyStructure
ââ? O
.
ââO P
ParCompany_Id
ââP ]
&&
ââ^ `
r
ää, -
.
ää- .
Active
ää. 4
==
ää5 7
true
ää8 <
)
ää< =
.
ää= >
FirstOrDefault
ää> L
(
ääL M
)
ääM N
;
ääN O
if
åå 
(
åå 
save
åå 
==
åå 
null
åå  
)
åå  !
{
çç 
companyStructure
éé $
.
éé$ %
Active
éé% +
=
éé, -
false
éé. 3
;
éé3 4
Guard
èè 
.
èè 

verifyDate
èè $
(
èè$ %
companyStructure
èè% 5
,
èè5 6
$str
èè7 B
)
èèB C
;
èèC D+
_baseRepoParCompanyXStructure
êê 1
.
êê1 2
Update
êê2 8
(
êê8 9
companyStructure
êê9 I
)
êêI J
;
êêJ K
}
ëë 
else
íí 
{
ìì 
save
îî 
.
îî 
ParCompany_Id
îî &
=
îî' (
companyStructure
îî) 9
.
îî9 :
ParCompany_Id
îî: G
;
îîG H
save
ïï 
.
ïï 
Id
ïï 
=
ïï 
companyStructure
ïï .
.
ïï. /
Id
ïï/ 1
;
ïï1 2
Guard
ññ 
.
ññ 

verifyDate
ññ $
(
ññ$ %
companyStructure
ññ% 5
,
ññ5 6
$str
ññ7 B
)
ññB C
;
ññC D+
_baseRepoParCompanyXStructure
óó 1
.
óó1 2
Update
óó2 8
(
óó8 9
companyStructure
óó9 I
)
óóI J
;
óóJ K
}
òò &
listParCompanyXStructure
ôô (
.
ôô( )
Remove
ôô) /
(
ôô/ 0
save
ôô0 4
)
ôô4 5
;
ôô5 6
}
öö 
if
úú 
(
úú &
listParCompanyXStructure
úú '
!=
úú( *
null
úú+ /
)
úú/ 0
foreach
ùù 
(
ùù "
ParCompanyXStructure
ùù -
companyStructure
ùù. >
in
ùù? A&
listParCompanyXStructure
ùùB Z
)
ùùZ [
{
ûû 
companyStructure
üü $
.
üü$ %
Active
üü% +
=
üü, -
true
üü. 2
;
üü2 3
companyStructure
†† $
.
††$ %
ParCompany_Id
††% 2
=
††3 4

parCompany
††5 ?
.
††? @
Id
††@ B
;
††B C+
_baseRepoParCompanyXStructure
°° 1
.
°°1 2
Add
°°2 5
(
°°5 6
companyStructure
°°6 F
)
°°F G
;
°°G H
}
¢¢ 
}
•• 	
public
®® %
ParCompanyXStructureDTO
®® &.
 AddUpdateParCompanyXStructureDTO
®®' G
(
®®G H%
ParCompanyXStructureDTO
®®H _%
parCompanyXStructureDTO
®®` w
)
®®w x
{
©© 	
throw
™™ 
new
™™ %
NotImplementedException
™™ -
(
™™- .
)
™™. /
;
™™/ 0
}
´´ 	
public
≠≠ 
ParStructureDTO
≠≠ #
AddUpdateParStructure
≠≠ 4
(
≠≠4 5
ParStructureDTO
≠≠5 D
parStructureDTO
≠≠E T
)
≠≠T U
{
ÆÆ 	
ParStructure
ØØ  
parStructureSalvar
ØØ +
=
ØØ, -
Mapper
ØØ. 4
.
ØØ4 5
Map
ØØ5 8
<
ØØ8 9
ParStructure
ØØ9 E
>
ØØE F
(
ØØF G
parStructureDTO
ØØG V
)
ØØV W
;
ØØW X#
_baseRepoParStructure
±± !
.
±±! ""
AddOrUpdateNotCommit
±±" 6
(
±±6 7 
parStructureSalvar
±±7 I
)
±±I J
;
±±J K#
_baseRepoParStructure
≤≤ !
.
≤≤! "
Commit
≤≤" (
(
≤≤( )
)
≤≤) *
;
≤≤* +
parStructureDTO
¥¥ 
.
¥¥ 
Id
¥¥ 
=
¥¥   
parStructureSalvar
¥¥! 3
.
¥¥3 4
Id
¥¥4 6
;
¥¥6 7
return
∂∂ 
parStructureDTO
∂∂ "
;
∂∂" #
}
∑∑ 	
public
ππ "
ParStructureGroupDTO
ππ #(
AddUpdateParStructureGroup
ππ$ >
(
ππ> ?"
ParStructureGroupDTO
ππ? S"
parStructureGroupDTO
ππT h
)
ππh i
{
∫∫ 	
ParStructureGroup
ªª %
parStructureGroupSalvar
ªª 5
=
ªª6 7
Mapper
ªª8 >
.
ªª> ?
Map
ªª? B
<
ªªB C
ParStructureGroup
ªªC T
>
ªªT U
(
ªªU V"
parStructureGroupDTO
ªªV j
)
ªªj k
;
ªªk l(
_baseRepoParStructureGroup
ΩΩ &
.
ΩΩ& '"
AddOrUpdateNotCommit
ΩΩ' ;
(
ΩΩ; <%
parStructureGroupSalvar
ΩΩ< S
)
ΩΩS T
;
ΩΩT U(
_baseRepoParStructureGroup
ææ &
.
ææ& '
Commit
ææ' -
(
ææ- .
)
ææ. /
;
ææ/ 0"
parStructureGroupDTO
¿¿  
.
¿¿  !
Id
¿¿! #
=
¿¿$ %%
parStructureGroupSalvar
¿¿& =
.
¿¿= >
Id
¿¿> @
;
¿¿@ A
return
¬¬ "
parStructureGroupDTO
¬¬ '
;
¬¬' (
}
√√ 	
}
∆∆ 
}«« ’
NC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Services\ExampleDomain.cs
	namespace 	
Dominio
 
. 
Services 
{ 
public		 

class		 
ExampleDomain		 
:		  
IExampleDomain		! /
{

 
private 
IBaseRepository 
<  
Example  '
>' (
_baseRepoExample) 9
;9 :
public 
ExampleDomain 
( 
IBaseRepository 
< 
Example #
># $
baseRepoExample% 4
) 
{ 	
_baseRepoExample 
= 
baseRepoExample .
;. /
} 	
public$$ 
ContextExampleDTO$$  
AddUpdateExample$$! 1
($$1 2
ContextExampleDTO$$2 C
	paramsDto$$D M
)$$M N
{%% 	
	paramsDto'' 
.'' 
example'' 
.'' 
IsValid'' %
(''% &
)''& '
;''' (
Example(( 
exampleSalvar(( !
=((" #
Mapper(($ *
.((* +
Map((+ .
<((. /
Example((/ 6
>((6 7
(((7 8
	paramsDto((8 A
.((A B
example((B I
)((I J
;((J K
_baseRepoExample** 
.**  
AddOrUpdateNotCommit** 1
(**1 2
exampleSalvar**2 ?
)**? @
;**@ A
_baseRepoExample++ 
.++ 
Commit++ #
(++# $
)++$ %
;++% &
	paramsDto-- 
.-- 
example-- 
.-- 
Id--  
=--! "
exampleSalvar--# 0
.--0 1
Id--1 3
;--3 4
return// 
	paramsDto// 
;// 
}00 	
}33 
}44 ä›
MC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Services\ParamsDomain.cs
	namespace 	
Dominio
 
. 
Services 
{ 
public 

class 
ParamsDomain 
: 
IParamsDomain  -
{ 
private 
SgqDbDevEntities  
db! #
=$ %
new& )
SgqDbDevEntities* :
(: ;
); <
;< =
private 
IBaseRepository 
<  
	ParLevel1  )
>) *
_baseRepoParLevel1+ =
;= >
private 
IBaseRepository 
<  
	ParLevel2  )
>) *
_baseRepoParLevel2+ =
;= >
private 
IBaseRepository 
<  
	ParLevel3  )
>) *
_baseRepoParLevel3+ =
;= >
private %
IBaseRepositoryNoLazyLoad )
<) *
	ParLevel1* 3
>3 4!
_baseRepoParLevel1NLL5 J
;J K
private %
IBaseRepositoryNoLazyLoad )
<) *
	ParLevel2* 3
>3 4!
_baseRepoParLevel2NLL5 J
;J K
private %
IBaseRepositoryNoLazyLoad )
<) *
	ParLevel3* 3
>3 4!
_baseRepoParLevel3NLL5 J
;J K
private %
IBaseRepositoryNoLazyLoad )
<) *
ParLevel2Level1* 9
>9 :$
_baseRepoParLevel2Level1; S
;S T
private 
IBaseRepository 
<  
ParLevel1XCluster  1
>1 2&
_baseRepoParLevel1XCluster3 M
;M N
private   
IBaseRepository   
<    
ParFrequency    ,
>  , -
_baseParFrequency  . ?
;  ? @
private!! 
IBaseRepository!! 
<!!   
ParConsolidationType!!  4
>!!4 5%
_baseParConsolidationType!!6 O
;!!O P
private"" 
IBaseRepository"" 
<""  

ParCluster""  *
>""* +
_baseParCluster"", ;
;""; <
private## 
IBaseRepository## 
<##  
ParLevelDefiniton##  1
>##1 2"
_baseParLevelDefiniton##3 I
;##I J
private$$ 
IBaseRepository$$ 
<$$  
ParFieldType$$  ,
>$$, -
_baseParFieldType$$. ?
;$$? @
private%% 
IBaseRepository%% 
<%%  
ParDepartment%%  -
>%%- .
_baseParDepartment%%/ A
;%%A B
private&& 
IBaseRepository&& 
<&&  
ParLevel3Group&&  .
>&&. /
_baseParLevel3Group&&0 C
;&&C D
private'' 
IBaseRepository'' 
<''  
ParLocal''  (
>''( )
_baseParLocal''* 7
;''7 8
private(( 
IBaseRepository(( 
<((  

ParCounter((  *
>((* +
_baseParCounter((, ;
;((; <
private)) 
IBaseRepository)) 
<))  
ParCounterXLocal))  0
>))0 1!
_baseParCounterXLocal))2 G
;))G H
private** 
IBaseRepository** 
<**  

ParRelapse**  *
>*** +
_baseParRelapse**, ;
;**; <
private++ 
IBaseRepository++ 
<++   
ParNotConformityRule++  4
>++4 5%
_baseParNotConformityRule++6 O
;++O P
private,, 
IBaseRepository,, 
<,,  &
ParNotConformityRuleXLevel,,  :
>,,: ;+
_baseParNotConformityRuleXLevel,,< [
;,,[ \
private-- 
IBaseRepository-- 
<--  

ParCompany--  *
>--* +
_baseParCompany--, ;
;--; <
private.. 
IBaseRepository.. 
<..  
ParHeaderField..  .
>... /#
_baseRepoParHeaderField..0 G
;..G H
private// 
IBaseRepository// 
<//  !
ParLevel1XHeaderField//  5
>//5 6*
_baseRepoParLevel1XHeaderField//7 U
;//U V
private00 
IBaseRepository00 
<00  !
ParLevel2XHeaderField00  5
>005 6*
_baseRepoParLevel2XHeaderField007 U
;00U V
private11 
IBaseRepository11 
<11  
ParMultipleValues11  1
>111 2&
_baseRepoParMultipleValues113 M
;11M N
private22 
IBaseRepository22 
<22  
ParEvaluation22  -
>22- .
_baseParEvaluation22/ A
;22A B
private33 
IBaseRepository33 
<33  
	ParSample33  )
>33) *
_baseParSample33+ 9
;339 :
private44 
IBaseRepository44 
<44  
ParLevel3Value44  .
>44. /
_baseParLevel3Value440 C
;44C D
private55 
IBaseRepository55 
<55  
ParLevel3InputType55  2
>552 3#
_baseParLevel3InputType554 K
;55K L
private66 
IBaseRepository66 
<66  
ParLevel3BoolFalse66  2
>662 3#
_baseParLevel3BoolFalse664 K
;66K L
private77 
IBaseRepository77 
<77  
ParLevel3BoolTrue77  1
>771 2"
_baseParLevel3BoolTrue773 I
;77I J
private88 
IBaseRepository88 
<88  
ParCounterXLocal88  0
>880 1%
_baseRepoParCounterXLocal882 K
;88K L
private99 
IBaseRepository99 
<99  
ParMeasurementUnit99  2
>992 3#
_baseParMeasurementUnit994 K
;99K L
private:: 
IBaseRepository:: 
<::  
ParLevel3Level2::  /
>::/ 0 
_baseParLevel3Level2::1 E
;::E F
private;; 
IBaseRepository;; 
<;;  
ParLevel3Level2;;  /
>;;/ 0$
_baseRepoParLevel3Level2;;1 I
;;;I J
private<< %
IBaseRepositoryNoLazyLoad<< )
<<<) *!
ParLevel3Level2Level1<<* ?
><<? @-
!_baseRepoParLevel3Level2Level1NNL<<A b
;<<b c
private== 
IBaseRepository== 
<==  !
ParLevel3Level2Level1==  5
>==5 6*
_baseRepoParLevel3Level2Level1==7 U
;==U V
private>> 
IBaseRepository>> 
<>>  
ParCriticalLevel>>  0
>>>0 1%
_baseRepoParCriticalLevel>>2 K
;>>K L
private?? 
IBaseRepository?? 
<??  

ParCompany??  *
>??* +
_baseRepoParCompany??, ?
;??? @
private@@ 
IBaseRepository@@ 
<@@  
Equipamentos@@  ,
>@@, -!
_baseRepoEquipamentos@@. C
;@@C D
privateAA 
IBaseRepositoryAA 
<AA  
ParScoreTypeAA  ,
>AA, -
_baseRepoParScoreAA. ?
;AA? @
privateBB  
IParLevel3RepositoryBB $
_repoParLevel3BB% 3
;BB3 4
privateDD 
IParamsRepositoryDD !
_paramsRepoDD" -
;DD- .
publicFF 
ParamsDomainFF 
(FF 
IBaseRepositoryFF +
<FF+ ,
	ParLevel1FF, 5
>FF5 6
baseRepoParLevel1FF7 H
,FFH I
IBaseRepositoryGG +
<GG+ ,
	ParLevel2GG, 5
>GG5 6
baseRepoParLevel2GG7 H
,GGH I
IBaseRepositoryHH +
<HH+ ,
	ParLevel3HH, 5
>HH5 6
baseRepoParLevel3HH7 H
,HHH I%
IBaseRepositoryNoLazyLoadII 5
<II5 6
	ParLevel1II6 ?
>II? @'
baseRepoParLevel1NoLazyLoadIIA \
,II\ ]%
IBaseRepositoryNoLazyLoadJJ 5
<JJ5 6
	ParLevel2JJ6 ?
>JJ? @'
baseRepoParLevel2NoLazyLoadJJA \
,JJ\ ]%
IBaseRepositoryNoLazyLoadKK 5
<KK5 6
	ParLevel3KK6 ?
>KK? @'
baseRepoParLevel3NoLazyLoadKKA \
,KK\ ]
IBaseRepositoryLL +
<LL+ ,
ParLevel1XClusterLL, =
>LL= >!
baseParLevel1XClusterLL? T
,LLT U
IBaseRepositoryMM +
<MM+ ,
ParFrequencyMM, 8
>MM8 9
baseParFrequencyMM: J
,MMJ K
IBaseRepositoryNN +
<NN+ , 
ParConsolidationTypeNN, @
>NN@ A$
baseParConsolidationTypeNNB Z
,NNZ [
IBaseRepositoryOO +
<OO+ ,

ParClusterOO, 6
>OO6 7
baseParClusterOO8 F
,OOF G
IBaseRepositoryPP +
<PP+ ,
ParLevelDefinitonPP, =
>PP= >!
baseParLevelDefinitonPP? T
,PPT U
IBaseRepositoryQQ +
<QQ+ ,
ParFieldTypeQQ, 8
>QQ8 9
baseParFieldTypeQQ: J
,QQJ K
IParamsRepositoryRR -

paramsRepoRR. 8
,RR8 9
IBaseRepositorySS +
<SS+ ,
ParDepartmentSS, 9
>SS9 :
baseParDepartmentSS; L
,SSL M
IBaseRepositoryTT +
<TT+ ,
ParLevel3GroupTT, :
>TT: ;
baseParLevel3GroupTT< N
,TTN O
IBaseRepositoryUU +
<UU+ ,
ParLocalUU, 4
>UU4 5
baseParLocalUU6 B
,UUB C
IBaseRepositoryVV +
<VV+ ,

ParCounterVV, 6
>VV6 7
baseParCounterVV8 F
,VVF G
IBaseRepositoryWW +
<WW+ ,
ParCounterXLocalWW, <
>WW< = 
baseParCounterXLocalWW> R
,WWR S
IBaseRepositoryXX +
<XX+ ,

ParRelapseXX, 6
>XX6 7
baseParRelapseXX8 F
,XXF G
IBaseRepositoryYY +
<YY+ , 
ParNotConformityRuleYY, @
>YY@ A$
baseParNotConformityRuleYYB Z
,YYZ [
IBaseRepositoryZZ +
<ZZ+ ,&
ParNotConformityRuleXLevelZZ, F
>ZZF G*
baseParNotConformityRuleXLevelZZH f
,ZZf g
IBaseRepository[[ +
<[[+ ,

ParCompany[[, 6
>[[6 7
baseParCompany[[8 F
,[[F G
IBaseRepository\\ +
<\\+ ,!
ParLevel1XHeaderField\\, A
>\\A B)
baseRepoParLevel1XHeaderField\\C `
,\\` a
IBaseRepository]] +
<]]+ ,!
ParLevel2XHeaderField]], A
>]]A B)
baseRepoParLevel2XHeaderField]]C `
,]]` a
IBaseRepository^^ +
<^^+ ,
ParMultipleValues^^, =
>^^= >%
baseRepoParMultipleValues^^? X
,^^X Y
IBaseRepository__ +
<__+ ,
ParHeaderField__, :
>__: ;"
baseRepoParHeaderField__< R
,__R S
IBaseRepository`` +
<``+ ,
ParEvaluation``, 9
>``9 :
baseParEvaluation``; L
,``L M
IBaseRepositoryaa +
<aa+ ,
	ParSampleaa, 5
>aa5 6
baseParSampleaa7 D
,aaD E
IBaseRepositorybb +
<bb+ ,
ParLevel3Valuebb, :
>bb: ;
baseParLevel3Valuebb< N
,bbN O
IBaseRepositorycc +
<cc+ ,
ParLevel3InputTypecc, >
>cc> ?"
baseParLevel3InputTypecc@ V
,ccV W
IBaseRepositorydd +
<dd+ ,
ParCounterXLocaldd, <
>dd< =$
baseRepoParCounterXLocaldd> V
,ddV W
IBaseRepositoryee +
<ee+ ,
ParMeasurementUnitee, >
>ee> ?"
baseParMeasurementUnitee@ V
,eeV W
IBaseRepositoryff +
<ff+ ,
ParLevel3BoolFalseff, >
>ff> ?"
baseParLevel3BoolFalseff@ V
,ffV W
IBaseRepositorygg +
<gg+ ,
ParLevel3BoolTruegg, =
>gg= >!
baseParLevel3BoolTruegg? T
,ggT U
IBaseRepositoryhh +
<hh+ ,
ParLevel3Level2hh, ;
>hh; <
baseParLevel3Level2hh= P
,hhP Q
IBaseRepositoryii +
<ii+ ,
ParLevel3Level2ii, ;
>ii; <#
baseRepoParLevel3Level2ii= T
,iiT U
IBaseRepositoryjj +
<jj+ ,!
ParLevel3Level2Level1jj, A
>jjA B)
baseRepoParLevel3Level2Level1jjC `
,jj` a 
IParLevel3Repositorykk 0
repoParLevel3kk1 >
,kk> ?
IBaseRepositoryll +
<ll+ ,
ParCriticalLevelll, <
>ll< =$
baseRepoParCriticalLevelll> V
,llV W
IBaseRepositorymm +
<mm+ ,

ParCompanymm, 6
>mm6 7
baseRepoParCompanymm8 J
,mmJ K
IBaseRepositorynn +
<nn+ ,
Equipamentosnn, 8
>nn8 9 
baseRepoEquipamentosnn: N
,nnN O%
IBaseRepositoryNoLazyLoadoo 5
<oo5 6
ParLevel2Level1oo6 E
>ooE F#
baseRepoParLevel2Level1ooG ^
,oo^ _
IBaseRepositorypp +
<pp+ ,
ParScoreTypepp, 8
>pp8 9
baseRepoParScorepp: J
,ppJ K%
IBaseRepositoryNoLazyLoadqq 5
<qq5 6!
ParLevel3Level2Level1qq6 K
>qqK L,
 baseRepoParLevel3Level2Level1NNLqqM m
)rr 
{ss 	-
!_baseRepoParLevel3Level2Level1NNLtt -
=tt. /,
 baseRepoParLevel3Level2Level1NNLtt0 P
;ttP Q
_baseRepoParScoreuu 
=uu 
baseRepoParScoreuu  0
;uu0 1$
_baseRepoParLevel2Level1vv $
=vv% &#
baseRepoParLevel2Level1vv' >
;vv> ?
_baseRepoParCompanyww 
=ww  !
baseRepoParCompanyww" 4
;ww4 5!
_baseRepoEquipamentosxx !
=xx" # 
baseRepoEquipamentosxx$ 8
;xx8 9%
_baseRepoParCriticalLevelyy %
=yy& '$
baseRepoParCriticalLevelyy( @
;yy@ A
_paramsRepozz 
=zz 

paramsRepozz $
;zz$ %%
_baseRepoParCounterXLocal{{ %
={{& '$
baseRepoParCounterXLocal{{( @
;{{@ A*
_baseRepoParLevel1XHeaderField|| *
=||+ ,)
baseRepoParLevel1XHeaderField||- J
;||J K*
_baseRepoParLevel2XHeaderField}} *
=}}+ ,)
baseRepoParLevel2XHeaderField}}- J
;}}J K&
_baseRepoParMultipleValues~~ &
=~~' (%
baseRepoParMultipleValues~~) B
;~~B C#
_baseRepoParHeaderField #
=$ %"
baseRepoParHeaderField& <
;< = 
_baseRepoParLevel1
ÄÄ 
=
ÄÄ  
baseRepoParLevel1
ÄÄ! 2
;
ÄÄ2 3 
_baseRepoParLevel2
ÅÅ 
=
ÅÅ  
baseRepoParLevel2
ÅÅ! 2
;
ÅÅ2 3 
_baseRepoParLevel3
ÇÇ 
=
ÇÇ  
baseRepoParLevel3
ÇÇ! 2
;
ÇÇ2 3#
_baseRepoParLevel1NLL
ÉÉ !
=
ÉÉ" #)
baseRepoParLevel1NoLazyLoad
ÉÉ$ ?
;
ÉÉ? @#
_baseRepoParLevel2NLL
ÑÑ !
=
ÑÑ" #)
baseRepoParLevel2NoLazyLoad
ÑÑ$ ?
;
ÑÑ? @#
_baseRepoParLevel3NLL
ÖÖ !
=
ÖÖ" #)
baseRepoParLevel3NoLazyLoad
ÖÖ$ ?
;
ÖÖ? @(
_baseRepoParLevel1XCluster
ÜÜ &
=
ÜÜ' (#
baseParLevel1XCluster
ÜÜ) >
;
ÜÜ> ?
_baseParFrequency
áá 
=
áá 
baseParFrequency
áá  0
;
áá0 1'
_baseParConsolidationType
àà %
=
àà& '&
baseParConsolidationType
àà( @
;
àà@ A
_baseParCluster
ââ 
=
ââ 
baseParCluster
ââ ,
;
ââ, -
_baseParFieldType
ää 
=
ää 
baseParFieldType
ää  0
;
ää0 1$
_baseParLevelDefiniton
ãã "
=
ãã# $#
baseParLevelDefiniton
ãã% :
;
ãã: ; 
_baseParDepartment
åå 
=
åå  
baseParDepartment
åå! 2
;
åå2 3!
_baseParLevel3Group
çç 
=
çç  ! 
baseParLevel3Group
çç" 4
;
çç4 5
_baseParLocal
éé 
=
éé 
baseParLocal
éé (
;
éé( )
_baseParCounter
èè 
=
èè 
baseParCounter
èè ,
;
èè, -#
_baseParCounterXLocal
êê !
=
êê" #"
baseParCounterXLocal
êê$ 8
;
êê8 9
_baseParRelapse
ëë 
=
ëë 
baseParRelapse
ëë ,
;
ëë, -'
_baseParNotConformityRule
íí %
=
íí& '&
baseParNotConformityRule
íí( @
;
íí@ A-
_baseParNotConformityRuleXLevel
ìì +
=
ìì, -,
baseParNotConformityRuleXLevel
ìì. L
;
ììL M
_baseParCompany
îî 
=
îî 
baseParCompany
îî ,
;
îî, - 
_baseParEvaluation
ïï 
=
ïï  
baseParEvaluation
ïï! 2
;
ïï2 3
_baseParSample
ññ 
=
ññ 
baseParSample
ññ *
;
ññ* +!
_baseParLevel3Value
óó 
=
óó  ! 
baseParLevel3Value
óó" 4
;
óó4 5%
_baseParLevel3InputType
òò #
=
òò$ %$
baseParLevel3InputType
òò& <
;
òò< =%
_baseParMeasurementUnit
ôô #
=
ôô$ %$
baseParMeasurementUnit
ôô& <
;
ôô< =%
_baseParLevel3BoolFalse
öö #
=
öö$ %$
baseParLevel3BoolFalse
öö& <
;
öö< =$
_baseParLevel3BoolTrue
õõ "
=
õõ# $#
baseParLevel3BoolTrue
õõ% :
;
õõ: ;"
_baseParLevel3Level2
úú  
=
úú! "!
baseParLevel3Level2
úú# 6
;
úú6 7&
_baseRepoParLevel3Level2
ùù $
=
ùù% &%
baseRepoParLevel3Level2
ùù' >
;
ùù> ?,
_baseRepoParLevel3Level2Level1
ûû *
=
ûû+ ,+
baseRepoParLevel3Level2Level1
ûû- J
;
ûûJ K
_repoParLevel3
üü 
=
üü 
repoParLevel3
üü *
;
üü* +
db
°° 
.
°° 
Configuration
°° 
.
°°  
LazyLoadingEnabled
°° /
=
°°0 1
false
°°2 7
;
°°7 8
}
¢¢ 	
public
≠≠ 
	ParamsDTO
≠≠ 
AddUpdateLevel1
≠≠ (
(
≠≠( )
	ParamsDTO
≠≠) 2
	paramsDto
≠≠3 <
)
≠≠< =
{
ÆÆ 	
	ParLevel1
±± 
saveParamLevel1
±± %
=
±±& '
Mapper
±±( .
.
±±. /
Map
±±/ 2
<
±±2 3
	ParLevel1
±±3 <
>
±±< =
(
±±= >
	paramsDto
±±> G
.
±±G H
parLevel1Dto
±±H T
)
±±T U
;
±±U V
List
≤≤ 
<
≤≤ 
ParGoal
≤≤ 
>
≤≤ 
listParGoal
≤≤ %
=
≤≤& '
Mapper
≤≤( .
.
≤≤. /
Map
≤≤/ 2
<
≤≤2 3
List
≤≤3 7
<
≤≤7 8
ParGoal
≤≤8 ?
>
≤≤? @
>
≤≤@ A
(
≤≤A B
	paramsDto
≤≤B K
.
≤≤K L
parLevel1Dto
≤≤L X
.
≤≤X Y
listParGoalLevel1
≤≤Y j
)
≤≤j k
;
≤≤k l
List
≥≥ 
<
≥≥ 

ParRelapse
≥≥ 
>
≥≥ 
listaReincidencia
≥≥ .
=
≥≥/ 0
Mapper
≥≥1 7
.
≥≥7 8
Map
≥≥8 ;
<
≥≥; <
List
≥≥< @
<
≥≥@ A

ParRelapse
≥≥A K
>
≥≥K L
>
≥≥L M
(
≥≥M N
	paramsDto
≥≥N W
.
≥≥W X
parLevel1Dto
≥≥X d
.
≥≥d e
listParRelapseDto
≥≥e v
)
≥≥v w
;
≥≥w x
List
¥¥ 
<
¥¥ 
ParHeaderField
¥¥ 
>
¥¥  
listaParHEadField
¥¥! 2
=
¥¥3 4
Mapper
¥¥5 ;
.
¥¥; <
Map
¥¥< ?
<
¥¥? @
List
¥¥@ D
<
¥¥D E
ParHeaderField
¥¥E S
>
¥¥S T
>
¥¥T U
(
¥¥U V
	paramsDto
¥¥V _
.
¥¥_ `#
listParHeaderFieldDto
¥¥` u
)
¥¥u v
;
¥¥v w
List
µµ 
<
µµ 
ParCounterXLocal
µµ !
>
µµ! ""
ListaParCounterLocal
µµ# 7
=
µµ8 9
Mapper
µµ: @
.
µµ@ A
Map
µµA D
<
µµD E
List
µµE I
<
µµI J
ParCounterXLocal
µµJ Z
>
µµZ [
>
µµ[ \
(
µµ\ ]
	paramsDto
µµ] f
.
µµf g
parLevel1Dto
µµg s
.
µµs t#
listParCounterXLocalµµt à
)µµà â
;µµâ ä
List
∂∂ 
<
∂∂ 
ParLevel1XCluster
∂∂ "
>
∂∂" #$
ListaParLevel1XCluster
∂∂$ :
=
∂∂; <
Mapper
∂∂= C
.
∂∂C D
Map
∂∂D G
<
∂∂G H
List
∂∂H L
<
∂∂L M
ParLevel1XCluster
∂∂M ^
>
∂∂^ _
>
∂∂_ `
(
∂∂` a
	paramsDto
∂∂a j
.
∂∂j k
parLevel1Dto
∂∂k w
.
∂∂w x$
listLevel1XClusterDto∂∂x ç
)∂∂ç é
;∂∂é è
List
∑∑ 
<
∑∑ (
ParNotConformityRuleXLevel
∑∑ +
>
∑∑+ ,!
listNonCoformitRule
∑∑- @
=
∑∑A B
Mapper
∑∑C I
.
∑∑I J
Map
∑∑J M
<
∑∑M N
List
∑∑N R
<
∑∑R S(
ParNotConformityRuleXLevel
∑∑S m
>
∑∑m n
>
∑∑n o
(
∑∑o p
	paramsDto
∑∑p y
.
∑∑y z
parLevel1Dto∑∑z Ü
.∑∑Ü á1
!listParNotConformityRuleXLevelDto∑∑á ®
)∑∑® ©
;∑∑© ™
if
ππ 
(
ππ 
saveParamLevel1
ππ 
.
ππ  
ParScoreType_Id
ππ  /
<=
ππ0 2
$num
ππ3 4
)
ππ4 5
saveParamLevel1
∫∫ 
.
∫∫  
ParScoreType_Id
∫∫  /
=
∫∫0 1
null
∫∫2 6
;
∫∫6 7
if
ºº 
(
ºº 
	paramsDto
ºº 
.
ºº #
listParHeaderFieldDto
ºº /
!=
ºº0 2
null
ºº3 7
)
ºº7 8
foreach
ΩΩ 
(
ΩΩ 
var
ΩΩ 
i
ΩΩ 
in
ΩΩ !
	paramsDto
ΩΩ" +
.
ΩΩ+ ,#
listParHeaderFieldDto
ΩΩ, A
.
ΩΩA B
Where
ΩΩB G
(
ΩΩG H
r
ΩΩH I
=>
ΩΩJ L
!
ΩΩM N
string
ΩΩN T
.
ΩΩT U
IsNullOrEmpty
ΩΩU b
(
ΩΩb c
r
ΩΩc d
.
ΩΩd e
DefaultOption
ΩΩe r
)
ΩΩr s
)
ΩΩs t
)
ΩΩt u
	paramsDto
ææ 
.
ææ #
listParHeaderFieldDto
ææ 3
.
ææ3 4
ForEach
ææ4 ;
(
ææ; <
r
ææ< =
=>
ææ> @
r
ææA B
.
ææB C"
parMultipleValuesDto
ææC W
.
ææW X
FirstOrDefault
ææX f
(
ææf g
c
ææg h
=>
ææi k
c
ææl m
.
ææm n
Name
ææn r
.
æær s
Equals
ææs y
(
ææy z
i
ææz {
.
ææ{ |
DefaultOptionææ| â
)ææâ ä
)ææä ã
.ææã å
IsDefaultOptionææå õ
=ææú ù
trueææû ¢
)ææ¢ £
;ææ£ §
List
¡¡ 
<
¡¡ 
int
¡¡ 
>
¡¡ 
removerHeadField
¡¡ &
=
¡¡' (
	paramsDto
¡¡) 2
.
¡¡2 3
parLevel1Dto
¡¡3 ?
.
¡¡? @#
removerParHeaderField
¡¡@ U
;
¡¡U V
try
√√ 
{
ƒƒ 
_paramsRepo
∆∆ 
.
∆∆ 
SaveParLevel1
∆∆ )
(
∆∆) *
saveParamLevel1
∆∆* 9
,
∆∆9 :
listaParHEadField
∆∆; L
,
∆∆L M$
ListaParLevel1XCluster
∆∆N d
,
∆∆d e
removerHeadField
∆∆f v
,
««, -"
ListaParCounterLocal
««. B
,
««B C!
listNonCoformitRule
««D W
,
««W X
listaReincidencia
««Y j
,
««j k
listParGoal
««l w
)
««w x
;
««x y
if
…… 
(
…… 
DTO
…… 
.
…… 
GlobalConfig
…… $
.
……$ %
Brasil
……% +
)
……+ ,
{
   
if
ÀÀ 
(
ÀÀ 
	paramsDto
ÀÀ !
.
ÀÀ! "
parLevel1Dto
ÀÀ" .
.
ÀÀ. /

IsSpecific
ÀÀ/ 9
)
ÀÀ9 :
{
ÃÃ 
var
ÕÕ 
query
ÕÕ !
=
ÕÕ" #
$str
ÕÕ$ \
;
ÕÕ\ ]
var
ŒŒ 
queryExcute
ŒŒ '
=
ŒŒ( )
string
ŒŒ* 0
.
ŒŒ0 1
Empty
ŒŒ1 6
;
ŒŒ6 7
queryExcute
œœ #
=
œœ$ %
string
œœ& ,
.
œœ, -
Format
œœ- 3
(
œœ3 4
query
œœ4 9
,
œœ9 :
$str
œœ; K
,
œœK L
	paramsDto
œœM V
.
œœV W
parLevel1Dto
œœW c
.
œœc d
AllowAddLevel3
œœd r
?
œœs t
$num
œœu v
:
œœw x
$num
œœy z
,
œœz {
	paramsDtoœœ| Ö
.œœÖ Ü
parLevel1DtoœœÜ í
.œœí ì
Idœœì ï
)œœï ñ
;œœñ ó
db
–– 
.
–– 
Database
–– #
.
––# $
ExecuteSqlCommand
––$ 5
(
––5 6
queryExcute
––6 A
)
––A B
;
––B C
queryExcute
—— #
=
——$ %
string
——& ,
.
——, -
Format
——- 3
(
——3 4
query
——4 9
,
——9 :
$str
——; W
,
——W X
	paramsDto
——Y b
.
——b c
parLevel1Dto
——c o
.
——o p)
AllowEditPatternLevel3Task——p ä
?——ã å
$num——ç é
:——è ê
$num——ë í
,——í ì
	paramsDto——î ù
.——ù û
parLevel1Dto——û ™
.——™ ´
Id——´ ≠
)——≠ Æ
;——Æ Ø
db
““ 
.
““ 
Database
““ #
.
““# $
ExecuteSqlCommand
““$ 5
(
““5 6
queryExcute
““6 A
)
““A B
;
““B C
queryExcute
”” #
=
””$ %
string
””& ,
.
””, -
Format
””- 3
(
””3 4
query
””4 9
,
””9 :
$str
””; T
,
””T U
	paramsDto
””V _
.
””_ `
parLevel1Dto
””` l
.
””l m&
AllowEditWeightOnLevel3””m Ñ
?””Ö Ü
$num””á à
:””â ä
$num””ã å
,””å ç
	paramsDto””é ó
.””ó ò
parLevel1Dto””ò §
.””§ •
Id””• ß
)””ß ®
;””® ©
db
‘‘ 
.
‘‘ 
Database
‘‘ #
.
‘‘# $
ExecuteSqlCommand
‘‘$ 5
(
‘‘5 6
queryExcute
‘‘6 A
)
‘‘A B
;
‘‘B C
}
’’ 
if
÷÷ 
(
÷÷ 
	paramsDto
÷÷ !
.
÷÷! "
parLevel1Dto
÷÷" .
.
÷÷. /
IsRecravacao
÷÷/ ;
)
÷÷; <
{
◊◊ 
var
ÿÿ 
query
ÿÿ !
=
ÿÿ" #
$str
ÿÿ$ \
;
ÿÿ\ ]
var
ŸŸ 
queryExcute
ŸŸ '
=
ŸŸ( )
string
ŸŸ* 0
.
ŸŸ0 1
Empty
ŸŸ1 6
;
ŸŸ6 7
queryExcute
⁄⁄ #
=
⁄⁄$ %
string
⁄⁄& ,
.
⁄⁄, -
Format
⁄⁄- 3
(
⁄⁄3 4
query
⁄⁄4 9
,
⁄⁄9 :
$str
⁄⁄; I
,
⁄⁄I J
	paramsDto
⁄⁄K T
.
⁄⁄T U
parLevel1Dto
⁄⁄U a
.
⁄⁄a b
IsRecravacao
⁄⁄b n
?
⁄⁄o p
$num
⁄⁄q r
:
⁄⁄s t
$num
⁄⁄u v
,
⁄⁄v w
	paramsDto⁄⁄x Å
.⁄⁄Å Ç
parLevel1Dto⁄⁄Ç é
.⁄⁄é è
Id⁄⁄è ë
)⁄⁄ë í
;⁄⁄í ì
db
€€ 
.
€€ 
Database
€€ #
.
€€# $
ExecuteSqlCommand
€€$ 5
(
€€5 6
queryExcute
€€6 A
)
€€A B
;
€€B C
}
‹‹ 
}
›› 
}
ﬁﬁ 
catch
ﬂﬂ 
(
ﬂﬂ 
DbUpdateException
ﬂﬂ $
e
ﬂﬂ% &
)
ﬂﬂ& '
{
‡‡ 
VerifyUniqueName
··  
(
··  !
saveParamLevel1
··! 0
,
··0 1
e
··2 3
)
··3 4
;
··4 5
}
‚‚ 
	paramsDto
‰‰ 
.
‰‰ 
parLevel1Dto
‰‰ "
.
‰‰" #
Id
‰‰# %
=
‰‰& '
saveParamLevel1
‰‰( 7
.
‰‰7 8
Id
‰‰8 :
;
‰‰: ;
return
ÂÂ 
	paramsDto
ÂÂ 
;
ÂÂ 
}
ÊÊ 	
public
ÌÌ 
ParLevel1DTO
ÌÌ 
	GetLevel1
ÌÌ %
(
ÌÌ% &
int
ÌÌ& )
idParLevel1
ÌÌ* 5
)
ÌÌ5 6
{
ÓÓ 	
ParLevel1DTO
ÔÔ 
parlevel1Dto
ÔÔ %
;
ÔÔ% &
db
ÚÚ 
.
ÚÚ 
Configuration
ÚÚ 
.
ÚÚ  
LazyLoadingEnabled
ÚÚ /
=
ÚÚ0 1
false
ÚÚ2 7
;
ÚÚ7 8
var
ˆˆ 
	parlevel1
ˆˆ 
=
ˆˆ  
_baseRepoParLevel1
ˆˆ .
.
ˆˆ. /
GetById
ˆˆ/ 6
(
ˆˆ6 7
idParLevel1
ˆˆ7 B
)
ˆˆB C
;
ˆˆC D
var
˜˜ 
counter
˜˜ 
=
˜˜ 
	parlevel1
˜˜ #
.
˜˜# $
ParCounterXLocal
˜˜$ 4
.
˜˜4 5
Where
˜˜5 :
(
˜˜: ;
r
˜˜; <
=>
˜˜= ?
r
˜˜@ A
.
˜˜A B
IsActive
˜˜B J
==
˜˜K M
true
˜˜N R
)
˜˜R S
.
˜˜S T
OrderByDescending
˜˜T e
(
˜˜e f
r
˜˜f g
=>
˜˜h j
r
˜˜k l
.
˜˜l m
IsActive
˜˜m u
)
˜˜u v
.
˜˜v w
ToList
˜˜w }
(
˜˜} ~
)
˜˜~ 
;˜˜ Ä
var
¯¯ 
goal
¯¯ 
=
¯¯ 
	parlevel1
¯¯  
.
¯¯  !
ParGoal
¯¯! (
.
¯¯( )
Where
¯¯) .
(
¯¯. /
r
¯¯/ 0
=>
¯¯1 3
r
¯¯4 5
.
¯¯5 6
IsActive
¯¯6 >
==
¯¯? A
true
¯¯B F
)
¯¯F G
.
¯¯G H
OrderByDescending
¯¯H Y
(
¯¯Y Z
r
¯¯Z [
=>
¯¯\ ^
r
¯¯_ `
.
¯¯` a
IsActive
¯¯a i
)
¯¯i j
.
¯¯j k
ToList
¯¯k q
(
¯¯q r
)
¯¯r s
;
¯¯s t
var
˘˘ 
cluster
˘˘ 
=
˘˘ 
	parlevel1
˘˘ #
.
˘˘# $
ParLevel1XCluster
˘˘$ 5
.
˘˘5 6
Where
˘˘6 ;
(
˘˘; <
r
˘˘< =
=>
˘˘> @
r
˘˘A B
.
˘˘B C
IsActive
˘˘C K
==
˘˘L N
true
˘˘O S
)
˘˘S T
.
˘˘T U
OrderByDescending
˘˘U f
(
˘˘f g
r
˘˘g h
=>
˘˘i k
r
˘˘l m
.
˘˘m n
IsActive
˘˘n v
)
˘˘v w
.
˘˘w x
ToList
˘˘x ~
(
˘˘~ 
)˘˘ Ä
;˘˘Ä Å
var
˙˙ 

listL3L2L1
˙˙ 
=
˙˙ 
db
˙˙ 
.
˙˙  #
ParLevel3Level2Level1
˙˙  5
.
˙˙5 6
Include
˙˙6 =
(
˙˙= >
$str
˙˙> O
)
˙˙O P
.
˙˙P Q
AsNoTracking
˙˙Q ]
(
˙˙] ^
)
˙˙^ _
.
˙˙_ `
Where
˙˙` e
(
˙˙e f
r
˙˙f g
=>
˙˙h j
r
˙˙k l
.
˙˙l m
Active
˙˙m s
==
˙˙t v
true
˙˙w {
&&
˙˙| ~
r˙˙ Ä
.˙˙Ä Å
ParLevel1_Id˙˙Å ç
==˙˙é ê
idParLevel1˙˙ë ú
)˙˙ú ù
.˙˙ù û
ToList˙˙û §
(˙˙§ •
)˙˙• ¶
;˙˙¶ ß
var
˚˚ 
relapse
˚˚ 
=
˚˚ 
	parlevel1
˚˚ #
.
˚˚# $

ParRelapse
˚˚$ .
.
˚˚. /
Where
˚˚/ 4
(
˚˚4 5
r
˚˚5 6
=>
˚˚7 9
r
˚˚: ;
.
˚˚; <
IsActive
˚˚< D
==
˚˚E G
true
˚˚H L
)
˚˚L M
.
˚˚M N
OrderByDescending
˚˚N _
(
˚˚_ `
r
˚˚` a
=>
˚˚b d
r
˚˚e f
.
˚˚f g
IsActive
˚˚g o
)
˚˚o p
.
˚˚p q
ToList
˚˚q w
(
˚˚w x
)
˚˚x y
;
˚˚y z
var
¸¸ 
notConformityrule
¸¸ !
=
¸¸" #
	parlevel1
¸¸$ -
.
¸¸- .(
ParNotConformityRuleXLevel
¸¸. H
.
¸¸H I
Where
¸¸I N
(
¸¸N O
r
¸¸O P
=>
¸¸Q S
r
¸¸T U
.
¸¸U V
IsActive
¸¸V ^
==
¸¸_ a
true
¸¸b f
)
¸¸f g
.
¸¸g h
OrderByDescending
¸¸h y
(
¸¸y z
r
¸¸z {
=>
¸¸| ~
r¸¸ Ä
.¸¸Ä Å
IsActive¸¸Å â
)¸¸â ä
.¸¸ä ã
ToList¸¸ã ë
(¸¸ë í
)¸¸í ì
;¸¸ì î
var
˝˝ 

cabecalhos
˝˝ 
=
˝˝ 
	parlevel1
˝˝ &
.
˝˝& '#
ParLevel1XHeaderField
˝˝' <
.
˝˝< =
Where
˝˝= B
(
˝˝B C
r
˝˝C D
=>
˝˝E G
r
˝˝H I
.
˝˝I J
IsActive
˝˝J R
==
˝˝S U
true
˝˝V Z
)
˝˝Z [
.
˝˝[ \
OrderBy
˝˝\ c
(
˝˝c d
r
˝˝d e
=>
˝˝f h
r
˝˝i j
.
˝˝j k
IsActive
˝˝k s
)
˝˝s t
.
˝˝t u
ToList
˝˝u {
(
˝˝{ |
)
˝˝| }
;
˝˝} ~
var
˛˛ 

level2List
˛˛ 
=
˛˛ #
_baseRepoParLevel2NLL
˛˛ 2
.
˛˛2 3
GetAll
˛˛3 9
(
˛˛9 :
)
˛˛: ;
.
˛˛; <
Where
˛˛< A
(
˛˛A B
r
˛˛B C
=>
˛˛D F
r
˛˛G H
.
˛˛H I
IsActive
˛˛I Q
==
˛˛R T
true
˛˛U Y
)
˛˛Y Z
;
˛˛Z [
parlevel1Dto
ÑÑ 
=
ÑÑ 
Mapper
ÑÑ !
.
ÑÑ! "
Map
ÑÑ" %
<
ÑÑ% &
ParLevel1DTO
ÑÑ& 2
>
ÑÑ2 3
(
ÑÑ3 4
	parlevel1
ÑÑ4 =
)
ÑÑ= >
;
ÑÑ> ?
parlevel1Dto
ÖÖ 
.
ÖÖ "
listParCounterXLocal
ÖÖ -
=
ÖÖ. /
Mapper
ÖÖ0 6
.
ÖÖ6 7
Map
ÖÖ7 :
<
ÖÖ: ;
List
ÖÖ; ?
<
ÖÖ? @!
ParCounterXLocalDTO
ÖÖ@ S
>
ÖÖS T
>
ÖÖT U
(
ÖÖU V
counter
ÖÖV ]
)
ÖÖ] ^
;
ÖÖ^ _
parlevel1Dto
ÜÜ 
.
ÜÜ 
listParGoalLevel1
ÜÜ *
=
ÜÜ+ ,
Mapper
ÜÜ- 3
.
ÜÜ3 4
Map
ÜÜ4 7
<
ÜÜ7 8
List
ÜÜ8 <
<
ÜÜ< =

ParGoalDTO
ÜÜ= G
>
ÜÜG H
>
ÜÜH I
(
ÜÜI J
goal
ÜÜJ N
)
ÜÜN O
;
ÜÜO P
parlevel1Dto
áá 
.
áá #
listLevel1XClusterDto
áá .
=
áá/ 0
Mapper
áá1 7
.
áá7 8
Map
áá8 ;
<
áá; <
List
áá< @
<
áá@ A"
ParLevel1XClusterDTO
ááA U
>
ááU V
>
ááV W
(
ááW X
cluster
ááX _
)
áá_ `
;
áá` a
parlevel1Dto
àà 
.
àà *
listParLevel3Level2Level1Dto
àà 5
=
àà6 7
Mapper
àà8 >
.
àà> ?
Map
àà? B
<
ààB C
List
ààC G
<
ààG H&
ParLevel3Level2Level1DTO
ààH `
>
àà` a
>
ààa b
(
ààb c

listL3L2L1
ààc m
)
ààm n
;
ààn o
parlevel1Dto
ââ 
.
ââ 
listParRelapseDto
ââ *
=
ââ+ ,
Mapper
ââ- 3
.
ââ3 4
Map
ââ4 7
<
ââ7 8
List
ââ8 <
<
ââ< =
ParRelapseDTO
ââ= J
>
ââJ K
>
ââK L
(
ââL M
relapse
ââM T
)
ââT U
;
ââU V
parlevel1Dto
ää 
.
ää /
!listParNotConformityRuleXLevelDto
ää :
=
ää; <
Mapper
ää= C
.
ääC D
Map
ääD G
<
ääG H
List
ääH L
<
ääL M+
ParNotConformityRuleXLevelDTO
ääM j
>
ääj k
>
ääk l
(
ääl m
notConformityrule
ääm ~
)
ää~ 
;ää Ä
parlevel1Dto
ãã 
.
ãã  
cabecalhosInclusos
ãã +
=
ãã, -
Mapper
ãã. 4
.
ãã4 5
Map
ãã5 8
<
ãã8 9
List
ãã9 =
<
ãã= >&
ParLevel1XHeaderFieldDTO
ãã> V
>
ããV W
>
ããW X
(
ããX Y

cabecalhos
ããY c
)
ããc d
;
ããd e
parlevel1Dto
åå 
.
åå +
parNotConformityRuleXLevelDto
åå 6
=
åå7 8
new
åå9 <+
ParNotConformityRuleXLevelDTO
åå= Z
(
ååZ [
)
åå[ \
;
åå\ ]
parlevel1Dto
éé 
.
éé 6
(CreateSelectListParamsViewModelListLevel
éé A
(
ééA B
Mapper
ééB H
.
ééH I
Map
ééI L
<
ééL M
List
ééM Q
<
ééQ R
ParLevel2DTO
ééR ^
>
éé^ _
>
éé_ `
(
éé` a

level2List
ééa k
)
éék l
,
éél m
parlevel1Dto
één z
.
ééz {+
listParLevel3Level2Level1Dtoéé{ ó
)ééó ò
;ééò ô
if
êê 
(
êê 
DTO
êê 
.
êê 
GlobalConfig
êê  
.
êê  !
Brasil
êê! '
)
êê' (
{
ëë 
var
íí 
query
íí 
=
íí 
$str
íí G
;
ííG H
var
ìì 
queryExcute
ìì 
=
ìì  !
string
ìì" (
.
ìì( )
Empty
ìì) .
;
ìì. /
queryExcute
îî 
=
îî 
string
îî $
.
îî$ %
Format
îî% +
(
îî+ ,
query
îî, 1
,
îî1 2
$str
îî3 C
,
îîC D
parlevel1Dto
îîE Q
.
îîQ R
Id
îîR T
)
îîT U
;
îîU V
parlevel1Dto
ïï 
.
ïï 
AllowAddLevel3
ïï +
=
ïï, -
db
ïï. 0
.
ïï0 1
Database
ïï1 9
.
ïï9 :
SqlQuery
ïï: B
<
ïïB C
bool
ïïC G
>
ïïG H
(
ïïH I
queryExcute
ïïI T
)
ïïT U
.
ïïU V
FirstOrDefault
ïïV d
(
ïïd e
)
ïïe f
;
ïïf g
queryExcute
ññ 
=
ññ 
string
ññ $
.
ññ$ %
Format
ññ% +
(
ññ+ ,
query
ññ, 1
,
ññ1 2
$str
ññ3 O
,
ññO P
parlevel1Dto
ññQ ]
.
ññ] ^
Id
ññ^ `
)
ññ` a
;
ñña b
parlevel1Dto
óó 
.
óó (
AllowEditPatternLevel3Task
óó 7
=
óó8 9
db
óó: <
.
óó< =
Database
óó= E
.
óóE F
SqlQuery
óóF N
<
óóN O
bool
óóO S
>
óóS T
(
óóT U
queryExcute
óóU `
)
óó` a
.
óóa b
FirstOrDefault
óób p
(
óóp q
)
óóq r
;
óór s
queryExcute
òò 
=
òò 
string
òò $
.
òò$ %
Format
òò% +
(
òò+ ,
query
òò, 1
,
òò1 2
$str
òò3 L
,
òòL M
parlevel1Dto
òòN Z
.
òòZ [
Id
òò[ ]
)
òò] ^
;
òò^ _
parlevel1Dto
ôô 
.
ôô %
AllowEditWeightOnLevel3
ôô 4
=
ôô5 6
db
ôô7 9
.
ôô9 :
Database
ôô: B
.
ôôB C
SqlQuery
ôôC K
<
ôôK L
bool
ôôL P
>
ôôP Q
(
ôôQ R
queryExcute
ôôR ]
)
ôô] ^
.
ôô^ _
FirstOrDefault
ôô_ m
(
ôôm n
)
ôôn o
;
ôôo p
queryExcute
úú 
=
úú 
string
úú $
.
úú$ %
Format
úú% +
(
úú+ ,
query
úú, 1
,
úú1 2
$str
úú3 A
,
úúA B
parlevel1Dto
úúC O
.
úúO P
Id
úúP R
)
úúR S
;
úúS T
parlevel1Dto
ùù 
.
ùù 
IsRecravacao
ùù )
=
ùù* +
db
ùù, .
.
ùù. /
Database
ùù/ 7
.
ùù7 8
SqlQuery
ùù8 @
<
ùù@ A
bool
ùùA E
>
ùùE F
(
ùùF G
queryExcute
ùùG R
)
ùùR S
.
ùùS T
FirstOrDefault
ùùT b
(
ùùb c
)
ùùc d
;
ùùd e
}
ûû 
foreach
≠≠ 
(
≠≠ 
var
≠≠ 
i
≠≠ 
in
≠≠ 
parlevel1Dto
≠≠ *
.
≠≠* +
listParRelapseDto
≠≠+ <
)
≠≠< =
{
ÆÆ 
i
ØØ 
.
ØØ 
	parLevel1
ØØ 
=
ØØ 
null
ØØ "
;
ØØ" #
i
∞∞ 
.
∞∞ 
	parLevel2
∞∞ 
=
∞∞ 
null
∞∞ "
;
∞∞" #
i
±± 
.
±± 
	parLevel3
±± 
=
±± 
null
±± "
;
±±" #
}
≤≤ 
return
∑∑ 
parlevel1Dto
∑∑ 
;
∑∑  
}
∏∏ 	
public
ææ 
	ParamsDTO
ææ 
AddUpdateLevel2
ææ (
(
ææ( )
	ParamsDTO
ææ) 2
	paramsDto
ææ3 <
)
ææ< =
{
øø 	
	ParLevel2
¡¡ 
saveParamLevel2
¡¡ %
=
¡¡& '
Mapper
¡¡( .
.
¡¡. /
Map
¡¡/ 2
<
¡¡2 3
	ParLevel2
¡¡3 <
>
¡¡< =
(
¡¡= >
	paramsDto
¡¡> G
.
¡¡G H
parLevel2Dto
¡¡H T
)
¡¡T U
;
¡¡U V
	paramsDto
¬¬ 
.
¬¬ 
parLevel2Dto
¬¬ "
.
¬¬" #'
CriaListaSampleEvaluation
¬¬# <
(
¬¬< =
)
¬¬= >
;
¬¬> ?
List
√√ 
<
√√ 
	ParSample
√√ 
>
√√ 
saveParamSample
√√ +
=
√√, -
Mapper
√√. 4
.
√√4 5
Map
√√5 8
<
√√8 9
List
√√9 =
<
√√= >
	ParSample
√√> G
>
√√G H
>
√√H I
(
√√I J
	paramsDto
√√J S
.
√√S T
parLevel2Dto
√√T `
.
√√` a

listSample
√√a k
)
√√k l
;
√√l m
List
ƒƒ 
<
ƒƒ 
ParEvaluation
ƒƒ 
>
ƒƒ !
saveParamEvaluation
ƒƒ  3
=
ƒƒ4 5
Mapper
ƒƒ6 <
.
ƒƒ< =
Map
ƒƒ= @
<
ƒƒ@ A
List
ƒƒA E
<
ƒƒE F
ParEvaluation
ƒƒF S
>
ƒƒS T
>
ƒƒT U
(
ƒƒU V
	paramsDto
ƒƒV _
.
ƒƒ_ `
parLevel2Dto
ƒƒ` l
.
ƒƒl m
listEvaluation
ƒƒm {
)
ƒƒ{ |
;
ƒƒ| }
List
≈≈ 
<
≈≈ 

ParRelapse
≈≈ 
>
≈≈ 
listParRelapse
≈≈ +
=
≈≈, -
Mapper
≈≈. 4
.
≈≈4 5
Map
≈≈5 8
<
≈≈8 9
List
≈≈9 =
<
≈≈= >

ParRelapse
≈≈> H
>
≈≈H I
>
≈≈I J
(
≈≈J K
	paramsDto
≈≈K T
.
≈≈T U
parLevel2Dto
≈≈U a
.
≈≈a b
listParRelapseDto
≈≈b s
)
≈≈s t
;
≈≈t u
List
∆∆ 
<
∆∆ 
ParLevel3Group
∆∆ 
>
∆∆  !
listaParLevel3Group
∆∆! 4
=
∆∆5 6
Mapper
∆∆7 =
.
∆∆= >
Map
∆∆> A
<
∆∆A B
List
∆∆B F
<
∆∆F G
ParLevel3Group
∆∆G U
>
∆∆U V
>
∆∆V W
(
∆∆W X
	paramsDto
∆∆X a
.
∆∆a b
parLevel2Dto
∆∆b n
.
∆∆n o$
listParLevel3GroupDto∆∆o Ñ
)∆∆Ñ Ö
;∆∆Ö Ü
List
«« 
<
«« 
ParCounterXLocal
«« !
>
««! ""
listParCounterXLocal
««# 7
=
««8 9
Mapper
««: @
.
««@ A
Map
««A D
<
««D E
List
««E I
<
««I J
ParCounterXLocal
««J Z
>
««Z [
>
««[ \
(
««\ ]
	paramsDto
««] f
.
««f g
parLevel2Dto
««g s
.
««s t#
listParCounterXLocal««t à
)««à â
;««â ä
List
»» 
<
»» (
ParNotConformityRuleXLevel
»» +
>
»»+ ,!
listNonCoformitRule
»»- @
=
»»A B
Mapper
»»C I
.
»»I J
Map
»»J M
<
»»M N
List
»»N R
<
»»R S(
ParNotConformityRuleXLevel
»»S m
>
»»m n
>
»»n o
(
»»o p
	paramsDto
»»p y
.
»»y z
parLevel2Dto»»z Ü
.»»Ü á1
!listParNotConformityRuleXLevelDto»»á ®
)»»® ©
;»»© ™
try
   
{
ÀÀ 
_paramsRepo
ÃÃ 
.
ÃÃ 
SaveParLevel2
ÃÃ )
(
ÃÃ) *
saveParamLevel2
ÃÃ* 9
,
ÃÃ9 :!
listaParLevel3Group
ÃÃ; N
,
ÃÃN O"
listParCounterXLocal
ÃÃP d
,
ÃÃd e!
listNonCoformitRule
ÃÃf y
,
ÃÃy z"
saveParamEvaluationÃÃ{ é
,ÃÃé è
saveParamSampleÃÃê ü
,ÃÃü †
listParRelapseÃÃ° Ø
)ÃÃØ ∞
;ÃÃ∞ ±
}
ÕÕ 
catch
ŒŒ 
(
ŒŒ 
DbUpdateException
ŒŒ $
e
ŒŒ% &
)
ŒŒ& '
{
œœ 
VerifyUniqueName
––  
(
––  !
saveParamLevel2
––! 0
,
––0 1
e
––2 3
)
––3 4
;
––4 5
}
—— 
	paramsDto
”” 
.
”” 
parLevel2Dto
”” "
.
””" #
Id
””# %
=
””& '
saveParamLevel2
””( 7
.
””7 8
Id
””8 :
;
””: ;
return
‘‘ 
	paramsDto
‘‘ 
;
‘‘ 
}
’’ 	
public
‹‹ 
	ParamsDTO
‹‹ 
	GetLevel2
‹‹ "
(
‹‹" #
int
‹‹# &
idParLevel2
‹‹' 2
,
‹‹2 3
int
‹‹4 7
level3Id
‹‹8 @
,
‹‹@ A
int
‹‹B E
level1Id
‹‹F N
)
‹‹N O
{
›› 	
var
·· 
	paramsDto
·· 
=
·· 
new
·· 
	ParamsDTO
··  )
(
··) *
)
··* +
;
··+ ,
var
‚‚ 
	parLevel2
‚‚ 
=
‚‚  
_baseRepoParLevel2
‚‚ .
.
‚‚. /
GetById
‚‚/ 6
(
‚‚6 7
idParLevel2
‚‚7 B
)
‚‚B C
;
‚‚C D
var
„„ 
level2
„„ 
=
„„ 
Mapper
„„ 
.
„„  
Map
„„  #
<
„„# $
ParLevel2DTO
„„$ 0
>
„„0 1
(
„„1 2
	parLevel2
„„2 ;
)
„„; <
;
„„< =
var
‰‰ 
headerFieldLevel1
‰‰ !
=
‰‰" #
db
‰‰$ &
.
‰‰& '#
ParLevel1XHeaderField
‰‰' <
.
‰‰< =
Include
‰‰= D
(
‰‰D E
$str
‰‰E U
)
‰‰U V
.
‰‰V W
ToList
‰‰W ]
(
‰‰] ^
)
‰‰^ _
;
‰‰_ `
var
ÂÂ 
headerFieldLevel2
ÂÂ !
=
ÂÂ" #
db
ÂÂ$ &
.
ÂÂ& '#
ParLevel2XHeaderField
ÂÂ' <
.
ÂÂ< =
Where
ÂÂ= B
(
ÂÂB C
r
ÂÂC D
=>
ÂÂE G
r
ÂÂH I
.
ÂÂI J
IsActive
ÂÂJ R
==
ÂÂS U
true
ÂÂV Z
)
ÂÂZ [
.
ÂÂ[ \
ToList
ÂÂ\ b
(
ÂÂb c
)
ÂÂc d
;
ÂÂd e
var
ÊÊ 

evaluation
ÊÊ 
=
ÊÊ 
	parLevel2
ÊÊ &
.
ÊÊ& '
ParEvaluation
ÊÊ' 4
.
ÊÊ4 5
Where
ÊÊ5 :
(
ÊÊ: ;
r
ÊÊ; <
=>
ÊÊ= ?
r
ÊÊ@ A
.
ÊÊA B
IsActive
ÊÊB J
==
ÊÊK M
true
ÊÊN R
)
ÊÊR S
;
ÊÊS T
var
ÁÁ 
relapse
ÁÁ 
=
ÁÁ 
	parLevel2
ÁÁ #
.
ÁÁ# $

ParRelapse
ÁÁ$ .
.
ÁÁ. /
Where
ÁÁ/ 4
(
ÁÁ4 5
r
ÁÁ5 6
=>
ÁÁ7 9
r
ÁÁ: ;
.
ÁÁ; <
IsActive
ÁÁ< D
==
ÁÁE G
true
ÁÁH L
)
ÁÁL M
.
ÁÁM N
OrderByDescending
ÁÁN _
(
ÁÁ_ `
r
ÁÁ` a
=>
ÁÁb d
r
ÁÁe f
.
ÁÁf g
IsActive
ÁÁg o
)
ÁÁo p
;
ÁÁp q
var
ËË 
counter
ËË 
=
ËË 
	parLevel2
ËË #
.
ËË# $
ParCounterXLocal
ËË$ 4
.
ËË4 5
Where
ËË5 :
(
ËË: ;
r
ËË; <
=>
ËË= ?
r
ËË@ A
.
ËËA B
IsActive
ËËB J
==
ËËK M
true
ËËN R
)
ËËR S
.
ËËS T
OrderByDescending
ËËT e
(
ËËe f
r
ËËf g
=>
ËËh j
r
ËËk l
.
ËËl m
IsActive
ËËm u
)
ËËu v
;
ËËv w
var
ÈÈ 
nonConformityrule
ÈÈ !
=
ÈÈ" #
	parLevel2
ÈÈ$ -
.
ÈÈ- .(
ParNotConformityRuleXLevel
ÈÈ. H
.
ÈÈH I
Where
ÈÈI N
(
ÈÈN O
r
ÈÈO P
=>
ÈÈQ S
r
ÈÈT U
.
ÈÈU V
IsActive
ÈÈV ^
==
ÈÈ_ a
true
ÈÈb f
)
ÈÈf g
.
ÈÈg h
OrderByDescending
ÈÈh y
(
ÈÈy z
r
ÈÈz {
=>
ÈÈ| ~
rÈÈ Ä
.ÈÈÄ Å
IsActiveÈÈÅ â
)ÈÈâ ä
;ÈÈä ã
var
ÍÍ 
	headerAdd
ÍÍ 
=
ÍÍ 
headerFieldLevel1
ÍÍ -
.
ÍÍ- .
Where
ÍÍ. 3
(
ÍÍ3 4
r
ÍÍ4 5
=>
ÍÍ6 8
r
ÍÍ9 :
.
ÍÍ: ;
IsActive
ÍÍ; C
==
ÍÍD F
true
ÍÍG K
&&
ÍÍL N
r
ÍÍO P
.
ÍÍP Q
ParLevel1_Id
ÍÍQ ]
==
ÍÍ^ `
level1Id
ÍÍa i
)
ÍÍi j
;
ÍÍj k
var
ÎÎ 
headerRemove
ÎÎ 
=
ÎÎ 
headerFieldLevel2
ÎÎ 0
.
ÎÎ0 1
Where
ÎÎ1 6
(
ÎÎ6 7
r
ÎÎ7 8
=>
ÎÎ9 ;
r
ÎÎ< =
.
ÎÎ= >
IsActive
ÎÎ> F
==
ÎÎG I
true
ÎÎJ N
&&
ÎÎO Q
r
ÎÎR S
.
ÎÎS T
ParLevel1_Id
ÎÎT `
==
ÎÎa c
level1Id
ÎÎd l
&&
ÎÎm o
r
ÎÎp q
.
ÎÎq r
ParLevel2_Id
ÎÎr ~
==ÎÎ Å
idParLevel2ÎÎÇ ç
)ÎÎç é
;ÎÎé è
var
ÏÏ 
parLevel3Group
ÏÏ 
=
ÏÏ  
	parLevel2
ÏÏ! *
.
ÏÏ* +
ParLevel3Group
ÏÏ+ 9
.
ÏÏ9 :
Where
ÏÏ: ?
(
ÏÏ? @
r
ÏÏ@ A
=>
ÏÏB D
r
ÏÏE F
.
ÏÏF G
IsActive
ÏÏG O
==
ÏÏP R
true
ÏÏS W
)
ÏÏW X
.
ÏÏX Y
OrderByDescending
ÏÏY j
(
ÏÏj k
r
ÏÏk l
=>
ÏÏm o
r
ÏÏp q
.
ÏÏq r
IsActive
ÏÏr z
)
ÏÏz {
;
ÏÏ{ |
level2
ÓÓ 
.
ÓÓ 
listEvaluation
ÓÓ !
=
ÓÓ" #
Mapper
ÓÓ$ *
.
ÓÓ* +
Map
ÓÓ+ .
<
ÓÓ. /
List
ÓÓ/ 3
<
ÓÓ3 4
ParEvaluationDTO
ÓÓ4 D
>
ÓÓD E
>
ÓÓE F
(
ÓÓF G

evaluation
ÓÓG Q
)
ÓÓQ R
;
ÓÓR S
if
ÔÔ 
(
ÔÔ 
	parLevel2
ÔÔ 
.
ÔÔ 
	ParSample
ÔÔ #
.
ÔÔ# $
Count
ÔÔ$ )
(
ÔÔ) *
)
ÔÔ* +
>
ÔÔ, -
$num
ÔÔ. /
)
ÔÔ/ 0
level2
 
.
 

listSample
 !
=
" #
Mapper
$ *
.
* +
Map
+ .
<
. /
List
/ 3
<
3 4
ParSampleDTO
4 @
>
@ A
>
A B
(
B C
	parLevel2
C L
.
L M
	ParSample
M V
.
V W
Where
W \
(
\ ]
r
] ^
=>
_ a
r
b c
.
c d
IsActive
d l
==
m o
true
p t
)
t u
)
u v
;
v w
level2
ˆˆ 
.
ˆˆ +
RecuperaListaSampleEvaluation
ˆˆ 0
(
ˆˆ0 1
)
ˆˆ1 2
;
ˆˆ2 3
level2
˜˜ 
.
˜˜ 
listParRelapseDto
˜˜ $
=
˜˜% &
Mapper
˜˜' -
.
˜˜- .
Map
˜˜. 1
<
˜˜1 2
List
˜˜2 6
<
˜˜6 7
ParRelapseDTO
˜˜7 D
>
˜˜D E
>
˜˜E F
(
˜˜F G
relapse
˜˜G N
)
˜˜N O
;
˜˜O P
level2
¯¯ 
.
¯¯ "
listParCounterXLocal
¯¯ '
=
¯¯( )
Mapper
¯¯* 0
.
¯¯0 1
Map
¯¯1 4
<
¯¯4 5
List
¯¯5 9
<
¯¯9 :!
ParCounterXLocalDTO
¯¯: M
>
¯¯M N
>
¯¯N O
(
¯¯O P
counter
¯¯P W
)
¯¯W X
;
¯¯X Y
level2
˘˘ 
.
˘˘ /
!listParNotConformityRuleXLevelDto
˘˘ 4
=
˘˘5 6
Mapper
˘˘7 =
.
˘˘= >
Map
˘˘> A
<
˘˘A B
List
˘˘B F
<
˘˘F G+
ParNotConformityRuleXLevelDTO
˘˘G d
>
˘˘d e
>
˘˘e f
(
˘˘f g
nonConformityrule
˘˘g x
)
˘˘x y
;
˘˘y z
level2
˙˙ 
.
˙˙  
cabecalhosInclusos
˙˙ %
=
˙˙& '
Mapper
˙˙( .
.
˙˙. /
Map
˙˙/ 2
<
˙˙2 3
List
˙˙3 7
<
˙˙7 8&
ParLevel1XHeaderFieldDTO
˙˙8 P
>
˙˙P Q
>
˙˙Q R
(
˙˙R S
	headerAdd
˙˙S \
)
˙˙\ ]
;
˙˙] ^
level2
˚˚ 
.
˚˚  
cabecalhosExclusos
˚˚ %
=
˚˚& '
Mapper
˚˚( .
.
˚˚. /
Map
˚˚/ 2
<
˚˚2 3
List
˚˚3 7
<
˚˚7 8&
ParLevel2XHeaderFieldDTO
˚˚8 P
>
˚˚P Q
>
˚˚Q R
(
˚˚R S
headerRemove
˚˚S _
)
˚˚_ `
;
˚˚` a
var
ÄÄ  
vinculosComOLevel2
ÄÄ "
=
ÄÄ# $
	parLevel2
ÄÄ% .
.
ÄÄ. /
ParLevel3Level2
ÄÄ/ >
.
ÄÄ> ?
Where
ÄÄ? D
(
ÄÄD E
r
ÄÄE F
=>
ÄÄG I
r
ÄÄJ K
.
ÄÄK L
IsActive
ÄÄL T
==
ÄÄU W
true
ÄÄX \
)
ÄÄ\ ]
;
ÄÄ] ^
if
ÉÉ 
(
ÉÉ 
level1Id
ÉÉ 
>
ÉÉ 
$num
ÉÉ 
)
ÉÉ 
{
ÑÑ  
vinculosComOLevel2
ÜÜ "
=
ÜÜ# $
db
ÜÜ% '
.
ÜÜ' (
ParLevel3Level2
ÜÜ( 7
.
ÜÜ7 8
Where
ÜÜ8 =
(
ÜÜ= >
r
ÜÜ> ?
=>
ÜÜ@ B
r
ÜÜC D
.
ÜÜD E
ParLevel2_Id
ÜÜE Q
==
ÜÜR T
idParLevel2
ÜÜU `
&&
ÜÜa c
r
ÜÜd e
.
ÜÜe f
IsActive
ÜÜf n
==
ÜÜo q
true
ÜÜr v
&&
ÜÜw y
r
ÜÜz {
.
ÜÜ{ |$
ParLevel3Level2Level1ÜÜ| ë
.ÜÜë í
AnyÜÜí ï
(ÜÜï ñ
cÜÜñ ó
=>ÜÜò ö
cÜÜõ ú
.ÜÜú ù
ParLevel1_IdÜÜù ©
==ÜÜ™ ¨
level1IdÜÜ≠ µ
)ÜÜµ ∂
)ÜÜ∂ ∑
.ÜÜ∑ ∏
ToListÜÜ∏ æ
(ÜÜæ ø
)ÜÜø ¿
;ÜÜ¿ ¡
}
áá 
level2
éé 
.
éé $
listParLevel3Level2Dto
éé )
=
éé* +
Mapper
éé, 2
.
éé2 3
Map
éé3 6
<
éé6 7
List
éé7 ;
<
éé; < 
ParLevel3Level2DTO
éé< N
>
ééN O
>
ééO P
(
ééP Q 
vinculosComOLevel2
ééQ c
)
ééc d
;
ééd e
level2
èè 
.
èè 6
(CreateSelectListParamsViewModelListLevel
èè ;
(
èè; <
Mapper
èè< B
.
èèB C
Map
èèC F
<
èèF G
List
èèG K
<
èèK L
ParLevel3DTO
èèL X
>
èèX Y
>
èèY Z
(
èèZ [#
_baseRepoParLevel3NLL
èè[ p
.
èèp q
GetAll
èèq w
(
èèw x
)
èèx y
.
èèy z
Where
èèz 
(èè Ä
rèèÄ Å
=>èèÇ Ñ
rèèÖ Ü
.èèÜ á
IsActiveèèá è
==èèê í
trueèèì ó
)èèó ò
)èèò ô
,èèô ö
level2èèõ °
.èè° ¢&
listParLevel3Level2Dtoèè¢ ∏
)èè∏ π
;èèπ ∫
	paramsDto
ìì 
.
ìì +
parNotConformityRuleXLevelDto
ìì 3
=
ìì4 5
new
ìì6 9+
ParNotConformityRuleXLevelDTO
ìì: W
(
ììW X
)
ììX Y
;
ììY Z
	paramsDto
îî 
.
îî #
listParLevel3GroupDto
îî +
=
îî, -
new
îî. 1
List
îî2 6
<
îî6 7
ParLevel3GroupDTO
îî7 H
>
îîH I
(
îîI J
)
îîJ K
;
îîK L
level2
ïï 
.
ïï #
listParLevel3GroupDto
ïï (
=
ïï) *
Mapper
ïï+ 1
.
ïï1 2
Map
ïï2 5
<
ïï5 6
List
ïï6 :
<
ïï: ;
ParLevel3GroupDTO
ïï; L
>
ïïL M
>
ïïM N
(
ïïN O
parLevel3Group
ïïO ]
)
ïï] ^
;
ïï^ _
if
óó 
(
óó 
	parLevel2
óó 
.
óó 
ParLevel3Level2
óó )
.
óó) *
FirstOrDefault
óó* 8
(
óó8 9
r
óó9 :
=>
óó; =
r
óó> ?
.
óó? @
ParLevel3_Id
óó@ L
==
óóM O
level3Id
óóP X
&&
óóY [
r
óó\ ]
.
óó] ^
IsActive
óó^ f
==
óóg i
true
óój n
)
óón o
!=
óóp r
null
óós w
)
óów x
level2
òò 
.
òò &
pesoDoVinculoSelecionado
òò /
=
òò0 1
	parLevel2
òò2 ;
.
òò; <
ParLevel3Level2
òò< K
.
òòK L
FirstOrDefault
òòL Z
(
òòZ [
r
òò[ \
=>
òò] _
r
òò` a
.
òòa b
ParLevel3_Id
òòb n
==
òòo q
level3Id
òòr z
&&
òò{ }
r
òò~ 
.òò Ä
IsActiveòòÄ à
==òòâ ã
trueòòå ê
)òòê ë
.òòë í
Weightòòí ò
;òòò ô
else
ôô 
level2
öö 
.
öö &
pesoDoVinculoSelecionado
öö /
=
öö0 1
$num
öö2 3
;
öö3 4
	paramsDto
úú 
.
úú 
parLevel2Dto
úú "
=
úú# $
level2
úú% +
;
úú+ ,
	paramsDto
ùù 
.
ùù 
parLevel2Dto
ùù "
.
ùù" #$
listParLevel3Level2Dto
ùù# 9
=
ùù: ;
null
ùù< @
;
ùù@ A
if
üü 
(
üü 
level1Id
üü 
>
üü 
$num
üü 
)
üü 
{
†† 
var
°° $
possuiVinculoComLevel1
°° *
=
°°+ ,
db
°°- /
.
°°/ 0
ParLevel2Level1
°°0 ?
.
°°? @
Where
°°@ E
(
°°E F
r
°°F G
=>
°°H J
r
°°K L
.
°°L M
ParLevel1_Id
°°M Y
==
°°Z \
level1Id
°°] e
&&
°°f h
r
°°i j
.
°°j k
ParLevel2_Id
°°k w
==
°°x z
level2°°{ Å
.°°Å Ç
Id°°Ç Ñ
)°°Ñ Ö
;°°Ö Ü
if
¢¢ 
(
¢¢ $
possuiVinculoComLevel1
¢¢ *
!=
¢¢+ -
null
¢¢. 2
&&
¢¢3 5$
possuiVinculoComLevel1
¢¢6 L
.
¢¢L M
Count
¢¢M R
(
¢¢R S
)
¢¢S T
>
¢¢U V
$num
¢¢W X
)
¢¢X Y
	paramsDto
££ 
.
££ 
parLevel2Dto
££ *
.
££* +
isVinculado
££+ 6
=
££7 8
true
££9 =
;
££= >
}
§§ 
if
•• 
(
•• 
level1Id
•• 
>
•• 
$num
•• 
)
•• 
	paramsDto
¶¶ 
.
¶¶ 
parLevel2Dto
¶¶ &
.
¶¶& ' 
RegrasParamsLevel1
¶¶' 9
(
¶¶9 :
Mapper
¶¶: @
.
¶¶@ A
Map
¶¶A D
<
¶¶D E
ParLevel1DTO
¶¶E Q
>
¶¶Q R
(
¶¶R S
db
¶¶S U
.
¶¶U V
	ParLevel1
¶¶V _
.
¶¶_ `
FirstOrDefault
¶¶` n
(
¶¶n o
r
¶¶o p
=>
¶¶q s
r
¶¶t u
.
¶¶u v
Id
¶¶v x
==
¶¶y {
level1Id¶¶| Ñ
)¶¶Ñ Ö
)¶¶Ö Ü
)¶¶Ü á
;¶¶á à
return
´´ 
	paramsDto
´´ 
;
´´ 
}
¨¨ 	
public
ÆÆ 
	ParamsDTO
ÆÆ  
RemoveLevel03Group
ÆÆ +
(
ÆÆ+ ,
int
ÆÆ, /
Id
ÆÆ0 2
)
ÆÆ2 3
{
ØØ 	
var
±± 
parLevel3Group
±± 
=
±±  
Mapper
±±! '
.
±±' (
Map
±±( +
<
±±+ ,
ParLevel3Group
±±, :
>
±±: ;
(
±±; <!
_baseParLevel3Group
±±< O
.
±±O P
GetAll
±±P V
(
±±V W
)
±±W X
.
±±X Y
Where
±±Y ^
(
±±^ _
r
±±_ `
=>
±±a c
r
±±d e
.
±±e f
Id
±±f h
==
±±i k
Id
±±l n
)
±±n o
.
±±o p
FirstOrDefault
±±p ~
(
±±~ 
)±± Ä
)±±Ä Å
;±±Å Ç
if
≤≤ 
(
≤≤ 
parLevel3Group
≤≤ 
!=
≤≤ !
null
≤≤" &
)
≤≤& '
{
≥≥ 
_paramsRepo
¥¥ 
.
¥¥ "
RemoveParLevel3Group
¥¥ 0
(
¥¥0 1
parLevel3Group
¥¥1 ?
)
¥¥? @
;
¥¥@ A
}
µµ 
return
∂∂ 
null
∂∂ 
;
∂∂ 
}
∑∑ 	
public
ΩΩ 
	ParamsDTO
ΩΩ 
AddUpdateLevel3
ΩΩ (
(
ΩΩ( )
	ParamsDTO
ΩΩ) 2
	paramsDto
ΩΩ3 <
)
ΩΩ< =
{
ææ 	
	ParLevel3
¿¿ 
saveParamLevel3
¿¿ %
=
¿¿& '
Mapper
¿¿( .
.
¿¿. /
Map
¿¿/ 2
<
¿¿2 3
	ParLevel3
¿¿3 <
>
¿¿< =
(
¿¿= >
	paramsDto
¿¿> G
.
¿¿G H
parLevel3Dto
¿¿H T
)
¿¿T U
;
¿¿U V
if
ƒƒ 
(
ƒƒ 
	paramsDto
ƒƒ 
.
ƒƒ 
parLevel3Dto
ƒƒ &
.
ƒƒ& '
listLevel3Value
ƒƒ' 6
!=
ƒƒ7 9
null
ƒƒ: >
)
ƒƒ> ?
if
≈≈ 
(
≈≈ 
	paramsDto
≈≈ 
.
≈≈ 
parLevel3Dto
≈≈ *
.
≈≈* +
listLevel3Value
≈≈+ :
.
≈≈: ;
Count
≈≈; @
(
≈≈@ A
)
≈≈A B
>
≈≈C D
$num
≈≈E F
)
≈≈F G
	paramsDto
∆∆ 
.
∆∆ 
parLevel3Dto
∆∆ *
.
∆∆* +
listLevel3Value
∆∆+ :
.
∆∆: ;
ForEach
∆∆; B
(
∆∆B C
r
∆∆C D
=>
∆∆E G
r
∆∆H I
.
∆∆I J&
preparaParaInsertEmBanco
∆∆J b
(
∆∆b c
)
∆∆c d
)
∆∆d e
;
∆∆e f
List
»» 
<
»» 
ParLevel3Value
»» 
>
»»  &
listSaveParamLevel3Value
»»! 9
=
»»: ;
Mapper
»»< B
.
»»B C
Map
»»C F
<
»»F G
List
»»G K
<
»»K L
ParLevel3Value
»»L Z
>
»»Z [
>
»»[ \
(
»»\ ]
	paramsDto
»»] f
.
»»f g
parLevel3Dto
»»g s
.
»»s t
listLevel3Value»»t É
)»»É Ñ
;»»Ñ Ö
List
ŒŒ 
<
ŒŒ 

ParRelapse
ŒŒ 
>
ŒŒ 
listParRelapse
ŒŒ +
=
ŒŒ, -
Mapper
ŒŒ. 4
.
ŒŒ4 5
Map
ŒŒ5 8
<
ŒŒ8 9
List
ŒŒ9 =
<
ŒŒ= >

ParRelapse
ŒŒ> H
>
ŒŒH I
>
ŒŒI J
(
ŒŒJ K
	paramsDto
ŒŒK T
.
ŒŒT U
parLevel3Dto
ŒŒU a
.
ŒŒa b
listParRelapseDto
ŒŒb s
)
ŒŒs t
;
ŒŒt u
if
‘‘ 
(
‘‘ 
	paramsDto
‘‘ 
.
‘‘ 
parLevel3Dto
‘‘ &
.
‘‘& '
listLevel3Level2
‘‘' 7
!=
‘‘8 :
null
‘‘; ?
)
‘‘? @
if
’’ 
(
’’ 
	paramsDto
’’ 
.
’’ 
parLevel3Dto
’’ *
.
’’* +
listLevel3Level2
’’+ ;
.
’’; <
Count
’’< A
(
’’A B
)
’’B C
>
’’D E
$num
’’F G
)
’’G H
{
÷÷ 
	paramsDto
◊◊ 
.
◊◊ 
parLevel3Dto
◊◊ *
.
◊◊* +
listLevel3Level2
◊◊+ ;
.
◊◊; <
ForEach
◊◊< C
(
◊◊C D
r
◊◊D E
=>
◊◊F H
r
◊◊I J
.
◊◊J K&
preparaParaInsertEmBanco
◊◊K c
(
◊◊c d
)
◊◊d e
)
◊◊e f
;
◊◊f g
}
ÿÿ 
List
⁄⁄ 
<
⁄⁄ 
ParLevel3Level2
⁄⁄  
>
⁄⁄  !!
parLevel3Level2peso
⁄⁄" 5
=
⁄⁄6 7
Mapper
⁄⁄8 >
.
⁄⁄> ?
Map
⁄⁄? B
<
⁄⁄B C
List
⁄⁄C G
<
⁄⁄G H
ParLevel3Level2
⁄⁄H W
>
⁄⁄W X
>
⁄⁄X Y
(
⁄⁄Y Z
	paramsDto
⁄⁄Z c
.
⁄⁄c d
parLevel3Dto
⁄⁄d p
.
⁄⁄p q
listLevel3Level2⁄⁄q Å
)⁄⁄Å Ç
;⁄⁄Ç É
try
‡‡ 
{
·· 
_paramsRepo
„„ 
.
„„ 
SaveParLevel3
„„ )
(
„„) *
saveParamLevel3
„„* 9
,
„„9 :&
listSaveParamLevel3Value
„„; S
,
„„S T
listParRelapse
„„U c
,
„„c d!
parLevel3Level2peso
„„e x
?
„„x y
.
„„y z
ToList„„z Ä
(„„Ä Å
)„„Å Ç
,„„Ç É
	paramsDto„„Ñ ç
.„„ç é
level1Selected„„é ú
)„„ú ù
;„„ù û
db
ÂÂ 
.
ÂÂ 
Database
ÂÂ 
.
ÂÂ 
ExecuteSqlCommand
ÂÂ -
(
ÂÂ- .
string
ÂÂ. 4
.
ÂÂ4 5
Format
ÂÂ5 ;
(
ÂÂ; <
$str
ÂÂ< s
,
ÂÂs t
	paramsDto
ÂÂu ~
.
ÂÂ~ 
parLevel3DtoÂÂ ã
.ÂÂã å
IsPointLessÂÂå ó
?ÂÂò ô
$strÂÂö ù
:ÂÂû ü
$strÂÂ† £
,ÂÂ£ §
saveParamLevel3ÂÂ• ¥
.ÂÂ¥ µ
IdÂÂµ ∑
)ÂÂ∑ ∏
)ÂÂ∏ π
;ÂÂπ ∫
db
ÊÊ 
.
ÊÊ 
Database
ÊÊ 
.
ÊÊ 
ExecuteSqlCommand
ÊÊ -
(
ÊÊ- .
string
ÊÊ. 4
.
ÊÊ4 5
Format
ÊÊ5 ;
(
ÊÊ; <
$str
ÊÊ< o
,
ÊÊo p
	paramsDto
ÊÊq z
.
ÊÊz {
parLevel3DtoÊÊ{ á
.ÊÊá à
AllowNAÊÊà è
?ÊÊê ë
$strÊÊí ï
:ÊÊñ ó
$strÊÊò õ
,ÊÊõ ú
saveParamLevel3ÊÊù ¨
.ÊÊ¨ ≠
IdÊÊ≠ Ø
)ÊÊØ ∞
)ÊÊ∞ ±
;ÊÊ± ≤
if
ËË 
(
ËË 
	paramsDto
ËË 
.
ËË 
parLevel3Dto
ËË *
.
ËË* +&
ParLevel3Value_OuterList
ËË+ C
!=
ËËD F
null
ËËG K
)
ËËK L
foreach
ÈÈ 
(
ÈÈ 
var
ÈÈ  
i
ÈÈ! "
in
ÈÈ# %
	paramsDto
ÈÈ& /
.
ÈÈ/ 0
parLevel3Dto
ÈÈ0 <
.
ÈÈ< =&
ParLevel3Value_OuterList
ÈÈ= U
)
ÈÈU V
{
ÍÍ 
if
ÎÎ 
(
ÎÎ 
i
ÎÎ 
.
ÎÎ 
Id
ÎÎ  
<=
ÎÎ! #
$num
ÎÎ$ %
)
ÎÎ% &
{
ÏÏ 
i
ÌÌ 
.
ÌÌ 
ParLevel3_Id
ÌÌ *
=
ÌÌ+ ,
saveParamLevel3
ÌÌ- <
.
ÌÌ< =
Id
ÌÌ= ?
;
ÌÌ? @
i
ÓÓ 
.
ÓÓ 
ParLevel3_Name
ÓÓ ,
=
ÓÓ- .
saveParamLevel3
ÓÓ/ >
.
ÓÓ> ?
Name
ÓÓ? C
;
ÓÓC D
i
ÔÔ 
.
ÔÔ 
IsActive
ÔÔ &
=
ÔÔ' (
true
ÔÔ) -
;
ÔÔ- .
db
ÒÒ 
.
ÒÒ "
ParLevel3Value_Outer
ÒÒ 3
.
ÒÒ3 4
Add
ÒÒ4 7
(
ÒÒ7 8
Mapper
ÒÒ8 >
.
ÒÒ> ?
Map
ÒÒ? B
<
ÒÒB C"
ParLevel3Value_Outer
ÒÒC W
>
ÒÒW X
(
ÒÒX Y
i
ÒÒY Z
)
ÒÒZ [
)
ÒÒ[ \
;
ÒÒ\ ]
}
ÚÚ 
else
ÛÛ 
{
ÙÙ 
var
ıı 1
#queryUpdateParLevel3Value_OuterList
ıı  C
=
ııD E
string
ııF L
.
ııL M
Format
ııM S
(
ııS T
$str
ı¯T /
,
¯¯/ 0
$str
¯¯1 <
,
¯¯< =
i
¯¯> ?
.
¯¯? @
IsActive
¯¯@ H
?
¯¯I J
$str
¯¯K N
:
¯¯O P
$str
¯¯Q T
,
¯¯T U
i
¯¯V W
.
¯¯W X
Id
¯¯X Z
)
¯¯Z [
;
¯¯[ \
db
˙˙ 
.
˙˙ 
Database
˙˙ '
.
˙˙' (
ExecuteSqlCommand
˙˙( 9
(
˙˙9 :1
#queryUpdateParLevel3Value_OuterList
˙˙: ]
)
˙˙] ^
;
˙˙^ _
}
˚˚ 
}
˝˝ 
db
ˇˇ 
.
ˇˇ 
SaveChanges
ˇˇ 
(
ˇˇ 
)
ˇˇ  
;
ˇˇ  !
if
ÄÄ 
(
ÄÄ !
parLevel3Level2peso
ÄÄ '
!=
ÄÄ( *
null
ÄÄ+ /
)
ÄÄ/ 0
foreach
ÅÅ 
(
ÅÅ 
var
ÅÅ  
i
ÅÅ! "
in
ÅÅ# %!
parLevel3Level2peso
ÅÅ& 9
?
ÅÅ9 :
.
ÅÅ: ;
Where
ÅÅ; @
(
ÅÅ@ A
r
ÅÅA B
=>
ÅÅC E
r
ÅÅF G
.
ÅÅG H
IsActive
ÅÅH P
)
ÅÅP Q
)
ÅÅQ R
AddVinculoL1L2
ÇÇ &
(
ÇÇ& '
	paramsDto
ÇÇ' 0
.
ÇÇ0 1
level1Selected
ÇÇ1 ?
,
ÇÇ? @
	paramsDto
ÇÇA J
.
ÇÇJ K
level2Selected
ÇÇK Y
,
ÇÇY Z
saveParamLevel3
ÇÇ[ j
.
ÇÇj k
Id
ÇÇk m
,
ÇÇm n
$num
ÇÇo p
,
ÇÇp q
i
ÇÇr s
.
ÇÇs t
ParCompany_IdÇÇt Å
)ÇÇÅ Ç
;ÇÇÇ É
}
ÑÑ 
catch
ÖÖ 
(
ÖÖ 
DbUpdateException
ÖÖ $
e
ÖÖ% &
)
ÖÖ& '
{
ÜÜ 
VerifyUniqueName
áá  
(
áá  !
saveParamLevel3
áá! 0
,
áá0 1
e
áá2 3
)
áá3 4
;
áá4 5
}
àà 
	paramsDto
åå 
.
åå 
parLevel3Dto
åå "
.
åå" #
Id
åå# %
=
åå& '
saveParamLevel3
åå( 7
.
åå7 8
Id
åå8 :
;
åå: ;
return
çç 
	paramsDto
çç 
;
çç 
}
éé 	
public
êê 
	ParamsDTO
êê #
AddUpdateLevel3Level2
êê .
(
êê. /
	ParamsDTO
êê/ 8
	paramsDto
êê9 B
)
êêB C
{
ëë 	
ParLevel3Level2
íí "
saveParamLevel3Leve2
íí 0
=
íí1 2
Mapper
íí3 9
.
íí9 :
Map
íí: =
<
íí= >
ParLevel3Level2
íí> M
>
ííM N
(
ííN O
	paramsDto
ííO X
.
ííX Y
parLevel3Level2
ííY h
)
ííh i
;
ííi j
_paramsRepo
ìì 
.
ìì !
SaveParLevel3Level2
ìì +
(
ìì+ ,"
saveParamLevel3Leve2
ìì, @
)
ìì@ A
;
ììA B
	paramsDto
îî 
.
îî 
parLevel3Dto
îî "
.
îî" #
Id
îî# %
=
îî& '"
saveParamLevel3Leve2
îî( <
.
îî< =
Id
îî= ?
;
îî? @
return
ïï 
	paramsDto
ïï 
;
ïï 
}
ññ 	
public
ùù 
	ParamsDTO
ùù 
	GetLevel3
ùù "
(
ùù" #
int
ùù# &
idParLevel3
ùù' 2
,
ùù2 3
int
ùù4 7
?
ùù7 8
idParLevel2
ùù9 D
=
ùùE F
$num
ùùG H
)
ùùH I
{
ûû 	
	ParamsDTO
¢¢ 
retorno
¢¢ 
=
¢¢ 
new
¢¢  #
	ParamsDTO
¢¢$ -
(
¢¢- .
)
¢¢. /
;
¢¢/ 0
var
££ 
	parlevel3
££ 
=
££  
_baseRepoParLevel3
££ .
.
££. /
GetById
££/ 6
(
££6 7
idParLevel3
££7 B
)
££B C
;
££C D
var
§§ 
level3
§§ 
=
§§ 
Mapper
§§ 
.
§§  
Map
§§  #
<
§§# $
ParLevel3DTO
§§$ 0
>
§§0 1
(
§§1 2
	parlevel3
§§2 ;
)
§§; <
;
§§< =
var
•• 
relapse
•• 
=
•• 
	parlevel3
•• #
.
••# $

ParRelapse
••$ .
.
••. /
Where
••/ 4
(
••4 5
r
••5 6
=>
••7 9
r
••: ;
.
••; <
IsActive
••< D
==
••E G
true
••H L
)
••L M
.
••M N
OrderByDescending
••N _
(
••_ `
r
••` a
=>
••b d
r
••e f
.
••f g
IsActive
••g o
)
••o p
;
••p q
var
¶¶ 
group
¶¶ 
=
¶¶ 
db
¶¶ 
.
¶¶ 
ParLevel3Group
¶¶ )
.
¶¶) *
Where
¶¶* /
(
¶¶/ 0
r
¶¶0 1
=>
¶¶2 4
r
¶¶5 6
.
¶¶6 7
ParLevel2_Id
¶¶7 C
==
¶¶D F
idParLevel2
¶¶G R
&&
¶¶S U
r
¶¶V W
.
¶¶W X
IsActive
¶¶X `
==
¶¶a c
true
¶¶d h
)
¶¶h i
.
¶¶i j
ToList
¶¶j p
(
¶¶p q
)
¶¶q r
;
¶¶r s
var
ßß 
level3Level2
ßß 
=
ßß 
	parlevel3
ßß (
.
ßß( )
ParLevel3Level2
ßß) 8
.
ßß8 9
Where
ßß9 >
(
ßß> ?
r
ßß? @
=>
ßßA C
r
ßßD E
.
ßßE F
ParLevel2_Id
ßßF R
==
ßßS U
idParLevel2
ßßV a
&&
ßßb d
r
ßße f
.
ßßf g
ParLevel3_Id
ßßg s
==
ßßt v
idParLevel3ßßw Ç
&&ßßÉ Ö
rßßÜ á
.ßßá à
IsActiveßßà ê
==ßßë ì
trueßßî ò
)ßßò ô
.ßßô ö!
OrderByDescendingßßö ´
(ßß´ ¨
rßß¨ ≠
=>ßßÆ ∞
rßß± ≤
.ßß≤ ≥
IsActiveßß≥ ª
)ßßª º
;ßßº Ω
var
®® 
level3Value
®® 
=
®® 
	parlevel3
®® '
.
®®' (
ParLevel3Value
®®( 6
.
®®6 7
Where
®®7 <
(
®®< =
r
®®= >
=>
®®? A
r
®®B C
.
®®C D
IsActive
®®D L
==
®®M O
true
®®P T
)
®®T U
.
®®U V
OrderByDescending
®®V g
(
®®g h
r
®®h i
=>
®®j l
r
®®m n
.
®®n o
IsActive
®®o w
)
®®w x
;
®®x y
var
©© #
parlevel3Reencravacao
©© %
=
©©& '
db
©©( *
.
©©* +"
ParLevel3Value_Outer
©©+ ?
.
©©? @
Where
©©@ E
(
©©E F
r
©©F G
=>
©©H J
r
©©K L
.
©©L M
IsActive
©©M U
&&
©©V X
r
©©Y Z
.
©©Z [
ParLevel3_Id
©©[ g
==
©©h j
	parlevel3
©©k t
.
©©t u
Id
©©u w
)
©©w x
.
©©x y
ToList
©©y 
(©© Ä
)©©Ä Å
;©©Å Ç
level3
ÆÆ 
.
ÆÆ 
listParRelapseDto
ÆÆ $
=
ÆÆ% &
Mapper
ÆÆ' -
.
ÆÆ- .
Map
ÆÆ. 1
<
ÆÆ1 2
List
ÆÆ2 6
<
ÆÆ6 7
ParRelapseDTO
ÆÆ7 D
>
ÆÆD E
>
ÆÆE F
(
ÆÆF G
relapse
ÆÆG N
)
ÆÆN O
;
ÆÆO P
level3
ØØ 
.
ØØ 
listGroupsLevel2
ØØ #
=
ØØ$ %
Mapper
ØØ& ,
.
ØØ, -
Map
ØØ- 0
<
ØØ0 1
List
ØØ1 5
<
ØØ5 6
ParLevel3GroupDTO
ØØ6 G
>
ØØG H
>
ØØH I
(
ØØI J
group
ØØJ O
)
ØØO P
;
ØØP Q
level3
∞∞ 
.
∞∞ 
listLevel3Level2
∞∞ #
=
∞∞$ %
Mapper
∞∞& ,
.
∞∞, -
Map
∞∞- 0
<
∞∞0 1
List
∞∞1 5
<
∞∞5 6 
ParLevel3Level2DTO
∞∞6 H
>
∞∞H I
>
∞∞I J
(
∞∞J K
level3Level2
∞∞K W
)
∞∞W X
;
∞∞X Y
level3
±± 
.
±± 
listLevel3Value
±± "
=
±±# $
Mapper
±±% +
.
±±+ ,
Map
±±, /
<
±±/ 0
List
±±0 4
<
±±4 5
ParLevel3ValueDTO
±±5 F
>
±±F G
>
±±G H
(
±±H I
level3Value
±±I T
)
±±T U
;
±±U V
retorno
≤≤ 
.
≤≤ 
parLevel3Value
≤≤ "
=
≤≤# $
new
≤≤% (
ParLevel3ValueDTO
≤≤) :
(
≤≤: ;
)
≤≤; <
;
≤≤< =
level3
≥≥ 
.
≥≥ &
ParLevel3Value_OuterList
≥≥ +
=
≥≥, -
Mapper
≥≥. 4
.
≥≥4 5
Map
≥≥5 8
<
≥≥8 9
List
≥≥9 =
<
≥≥= >)
ParLevel3Value_OuterListDTO
≥≥> Y
>
≥≥Y Z
>
≥≥Z [
(
≥≥[ \#
parlevel3Reencravacao
≥≥\ q
)
≥≥q r
;
≥≥r s
level3
¥¥ 
.
¥¥ -
ParLevel3Value_OuterListGrouped
¥¥ 2
=
¥¥3 4
level3
¥¥5 ;
.
¥¥; <&
ParLevel3Value_OuterList
¥¥< T
.
¥¥T U
GroupBy
¥¥U \
(
¥¥\ ]
r
¥¥] ^
=>
¥¥^ `
r
¥¥` a
.
¥¥a b
ParCompany_Id
¥¥b o
)
¥¥o p
;
¥¥p q
if
ππ 
(
ππ 
level3
ππ 
.
ππ 
listLevel3Level2
ππ '
.
ππ' (
Count
ππ( -
(
ππ- .
)
ππ. /
>
ππ0 1
$num
ππ2 3
)
ππ3 4
level3
∫∫ 
.
∫∫ 

hasVinculo
∫∫ !
=
∫∫" #
true
∫∫$ (
;
∫∫( )
foreach
ºº 
(
ºº 
var
ºº 
Level3Value
ºº $
in
ºº% '
level3
ºº( .
.
ºº. /
listLevel3Value
ºº/ >
)
ºº> ?
Level3Value
ΩΩ 
.
ΩΩ 

PreparaGet
ΩΩ &
(
ΩΩ& '
)
ΩΩ' (
;
ΩΩ( )
var
øø 
	pointLess
øø 
=
øø 
db
øø 
.
øø 
Database
øø '
.
øø' (
SqlQuery
øø( 0
<
øø0 1
bool
øø1 5
>
øø5 6
(
øø6 7
string
øø7 =
.
øø= >
Format
øø> D
(
øøD E
$str
øøE w
,
øøw x
level3
øøy 
.øø Ä
IdøøÄ Ç
)øøÇ É
)øøÉ Ñ
.øøÑ Ö
FirstOrDefaultøøÖ ì
(øøì î
)øøî ï
;øøï ñ
level3
¿¿ 
.
¿¿ 
IsPointLess
¿¿ 
=
¿¿  
	pointLess
¿¿! *
;
¿¿* +
var
¬¬ 
AllowNA
¬¬ 
=
¬¬ 
db
¬¬ 
.
¬¬ 
Database
¬¬ %
.
¬¬% &
SqlQuery
¬¬& .
<
¬¬. /
bool
¬¬/ 3
>
¬¬3 4
(
¬¬4 5
string
¬¬5 ;
.
¬¬; <
Format
¬¬< B
(
¬¬B C
$str
¬¬C q
,
¬¬q r
level3
¬¬s y
.
¬¬y z
Id
¬¬z |
)
¬¬| }
)
¬¬} ~
.
¬¬~ 
FirstOrDefault¬¬ ç
(¬¬ç é
)¬¬é è
;¬¬è ê
level3
√√ 
.
√√ 
AllowNA
√√ 
=
√√ 
AllowNA
√√ $
;
√√$ %
retorno
∆∆ 
.
∆∆ 
parLevel3Dto
∆∆  
=
∆∆! "
level3
∆∆# )
;
∆∆) *
return
»» 
retorno
»» 
;
»» 
}
…… 	
public
ˆˆ 
	ParamsDTO
ˆˆ 1
#AddUpdateParNotConformityRuleXLevel
ˆˆ <
(
ˆˆ< =
	ParamsDTO
ˆˆ= F
	paramsDto
ˆˆG P
)
ˆˆP Q
{
˜˜ 	(
ParNotConformityRuleXLevel
¯¯ &,
saveParNotConformityRuleXLevel
¯¯' E
=
¯¯F G
Mapper
¯¯H N
.
¯¯N O
Map
¯¯O R
<
¯¯R S(
ParNotConformityRuleXLevel
¯¯S m
>
¯¯m n
(
¯¯n o
	paramsDto
¯¯o x
.
¯¯x y,
parNotConformityRuleXLevelDto¯¯y ñ
)¯¯ñ ó
;¯¯ó ò
_paramsRepo
˘˘ 
.
˘˘ ,
SaveParNotConformityRuleXLevel
˘˘ 6
(
˘˘6 7,
saveParNotConformityRuleXLevel
˘˘7 U
)
˘˘U V
;
˘˘V W
	paramsDto
˙˙ 
.
˙˙ +
parNotConformityRuleXLevelDto
˙˙ 3
.
˙˙3 4
Id
˙˙4 6
=
˙˙7 8,
saveParNotConformityRuleXLevel
˙˙9 W
.
˙˙W X
Id
˙˙X Z
;
˙˙Z [
return
˚˚ 
	paramsDto
˚˚ 
;
˚˚ 
}
¸¸ 	
private
˛˛ 
static
˛˛ 
void
˛˛ 
VerifyUniqueName
˛˛ ,
<
˛˛, -
T
˛˛- .
>
˛˛. /
(
˛˛/ 0
T
˛˛0 1
obj
˛˛2 5
,
˛˛5 6
DbUpdateException
˛˛7 H
e
˛˛I J
)
˛˛J K
{
ˇˇ 	
if
ÄÄ 
(
ÄÄ 
e
ÄÄ 
.
ÄÄ 
InnerException
ÄÄ  
!=
ÄÄ! #
null
ÄÄ$ (
)
ÄÄ( )
if
ÅÅ 
(
ÅÅ 
e
ÅÅ 
.
ÅÅ 
InnerException
ÅÅ $
.
ÅÅ$ %
InnerException
ÅÅ% 3
!=
ÅÅ4 6
null
ÅÅ7 ;
)
ÅÅ; <
{
ÇÇ 
SqlException
ÉÉ  
innerException
ÉÉ! /
=
ÉÉ0 1
e
ÉÉ2 3
.
ÉÉ3 4
InnerException
ÉÉ4 B
.
ÉÉB C
InnerException
ÉÉC Q
as
ÉÉR T
SqlException
ÉÉU a
;
ÉÉa b
if
ÑÑ 
(
ÑÑ 
innerException
ÑÑ &
!=
ÑÑ' )
null
ÑÑ* .
&&
ÑÑ/ 1
(
ÑÑ2 3
innerException
ÑÑ3 A
.
ÑÑA B
Number
ÑÑB H
==
ÑÑI K
$num
ÑÑL P
||
ÑÑQ S
innerException
ÑÑT b
.
ÑÑb c
Number
ÑÑc i
==
ÑÑj l
$num
ÑÑm q
)
ÑÑq r
)
ÑÑr s
{
ÖÖ 
if
ÜÜ 
(
ÜÜ 
innerException
ÜÜ *
.
ÜÜ* +
Message
ÜÜ+ 2
.
ÜÜ2 3
IndexOf
ÜÜ3 :
(
ÜÜ: ;
$str
ÜÜ; A
)
ÜÜA B
>
ÜÜC D
$num
ÜÜE F
)
ÜÜF G
throw
áá !
new
áá" %
ExceptionHelper
áá& 5
(
áá5 6
$str
áá6 @
+
ááA B
obj
ááC F
.
ááF G
GetType
ááG N
(
ááN O
)
ááO P
.
ááP Q
GetProperty
ááQ \
(
áá\ ]
$str
áá] c
)
áác d
.
áád e
GetValue
ááe m
(
áám n
obj
áán q
)
ááq r
+
áás t
$strááu Ç
)ááÇ É
;ááÉ Ñ
}
àà 
else
ââ 
{
ää 
throw
ãã 
e
ãã 
;
ãã  
}
åå 
}
éé 
}
èè 	
public
ïï 
	ParamsDdl
ïï $
CarregaDropDownsParams
ïï /
(
ïï/ 0
)
ïï0 1
{
ññ 	
lock
óó 
(
óó 
this
óó 
)
óó 
{
òò 
var
ôô !
DdlParConsolidation
ôô '
=
ôô( )
Mapper
ôô* 0
.
ôô0 1
Map
ôô1 4
<
ôô4 5
List
ôô5 9
<
ôô9 :%
ParConsolidationTypeDTO
ôô: Q
>
ôôQ R
>
ôôR S
(
ôôS T'
_baseParConsolidationType
ôôT m
.
ôôm n!
GetAllAsNoTrackingôôn Ä
(ôôÄ Å
)ôôÅ Ç
)ôôÇ É
;ôôÉ Ñ
var
õõ 
DdlFrequency
õõ  
=
õõ! "
Mapper
õõ# )
.
õõ) *
Map
õõ* -
<
õõ- .
List
õõ. 2
<
õõ2 3
ParFrequencyDTO
õõ3 B
>
õõB C
>
õõC D
(
õõD E
_baseParFrequency
õõE V
.
õõV W 
GetAllAsNoTracking
õõW i
(
õõi j
)
õõj k
)
õõk l
;
õõl m
var
úú 
DdlparLevel1
úú  
=
úú! "
Mapper
úú# )
.
úú) *
Map
úú* -
<
úú- .
List
úú. 2
<
úú2 3
ParLevel1DTO
úú3 ?
>
úú? @
>
úú@ A
(
úúA B#
_baseRepoParLevel1NLL
úúB W
.
úúW X 
GetAllAsNoTracking
úúX j
(
úúj k
)
úúk l
)
úúl m
;
úúm n
var
ùù 
DdlparLevel2
ùù  
=
ùù! "
Mapper
ùù# )
.
ùù) *
Map
ùù* -
<
ùù- .
List
ùù. 2
<
ùù2 3
ParLevel2DTO
ùù3 ?
>
ùù? @
>
ùù@ A
(
ùùA B#
_baseRepoParLevel2NLL
ùùB W
.
ùùW X 
GetAllAsNoTracking
ùùX j
(
ùùj k
)
ùùk l
)
ùùl m
;
ùùm n
var
ûû 
DdlparLevel3
ûû  
=
ûû! "
Mapper
ûû# )
.
ûû) *
Map
ûû* -
<
ûû- .
List
ûû. 2
<
ûû2 3
ParLevel3DTO
ûû3 ?
>
ûû? @
>
ûû@ A
(
ûûA B#
_baseRepoParLevel3NLL
ûûB W
.
ûûW X 
GetAllAsNoTracking
ûûX j
(
ûûj k
)
ûûk l
)
ûûl m
;
ûûm n
var
†† 
DdlparCluster
†† !
=
††" #
Mapper
††$ *
.
††* +
Map
††+ .
<
††. /
List
††/ 3
<
††3 4
ParClusterDTO
††4 A
>
††A B
>
††B C
(
††C D
_baseParCluster
††D S
.
††S T 
GetAllAsNoTracking
††T f
(
††f g
)
††g h
)
††h i
;
††i j
var
°° #
DdlparLevelDefinition
°° )
=
°°* +
Mapper
°°, 2
.
°°2 3
Map
°°3 6
<
°°6 7
List
°°7 ;
<
°°; <"
ParLevelDefinitonDTO
°°< P
>
°°P Q
>
°°Q R
(
°°R S$
_baseParLevelDefiniton
°°S i
.
°°i j 
GetAllAsNoTracking
°°j |
(
°°| }
)
°°} ~
)
°°~ 
;°° Ä
var
¢¢ 
DdlParFieldType
¢¢ #
=
¢¢$ %
Mapper
¢¢& ,
.
¢¢, -
Map
¢¢- 0
<
¢¢0 1
List
¢¢1 5
<
¢¢5 6
ParFieldTypeDTO
¢¢6 E
>
¢¢E F
>
¢¢F G
(
¢¢G H
_baseParFieldType
¢¢H Y
.
¢¢Y Z 
GetAllAsNoTracking
¢¢Z l
(
¢¢l m
)
¢¢m n
)
¢¢n o
;
¢¢o p
var
££ 
DdlParDepartment
££ $
=
££% &
Mapper
££' -
.
££- .
Map
££. 1
<
££1 2
List
££2 6
<
££6 7
ParDepartmentDTO
££7 G
>
££G H
>
££H I
(
££I J 
_baseParDepartment
££J \
.
££\ ] 
GetAllAsNoTracking
££] o
(
££o p
)
££p q
)
££q r
;
££r s
var
§§ %
DdlParNotConformityRule
§§ +
=
§§, -
Mapper
§§. 4
.
§§4 5
Map
§§5 8
<
§§8 9
List
§§9 =
<
§§= >%
ParNotConformityRuleDTO
§§> U
>
§§U V
>
§§V W
(
§§W X'
_baseParNotConformityRule
§§X q
.
§§q r!
GetAllAsNoTracking§§r Ñ
(§§Ñ Ö
)§§Ö Ü
)§§Ü á
;§§á à
var
¶¶  
DdlParLocal_Level1
¶¶ &
=
¶¶' (
Mapper
¶¶) /
.
¶¶/ 0
Map
¶¶0 3
<
¶¶3 4
List
¶¶4 8
<
¶¶8 9
ParLocalDTO
¶¶9 D
>
¶¶D E
>
¶¶E F
(
¶¶F G
_baseParLocal
¶¶G T
.
¶¶T U 
GetAllAsNoTracking
¶¶U g
(
¶¶g h
)
¶¶h i
.
¶¶i j
Where
¶¶j o
(
¶¶o p
p
¶¶p q
=>
¶¶r t
p
¶¶u v
.
¶¶v w
Level
¶¶w |
==
¶¶} 
$num¶¶Ä Å
)¶¶Å Ç
)¶¶Ç É
;¶¶É Ñ
var
ßß  
DdlParLocal_Level2
ßß &
=
ßß' (
Mapper
ßß) /
.
ßß/ 0
Map
ßß0 3
<
ßß3 4
List
ßß4 8
<
ßß8 9
ParLocalDTO
ßß9 D
>
ßßD E
>
ßßE F
(
ßßF G
_baseParLocal
ßßG T
.
ßßT U 
GetAllAsNoTracking
ßßU g
(
ßßg h
)
ßßh i
.
ßßi j
Where
ßßj o
(
ßßo p
p
ßßp q
=>
ßßr t
p
ßßu v
.
ßßv w
Level
ßßw |
==
ßß} 
$numßßÄ Å
)ßßÅ Ç
)ßßÇ É
;ßßÉ Ñ
var
©© "
DdlParCounter_Level1
©© (
=
©©) *
Mapper
©©+ 1
.
©©1 2
Map
©©2 5
<
©©5 6
List
©©6 :
<
©©: ;
ParCounterDTO
©©; H
>
©©H I
>
©©I J
(
©©J K
_baseParCounter
©©K Z
.
©©Z [ 
GetAllAsNoTracking
©©[ m
(
©©m n
)
©©n o
.
©©o p
Where
©©p u
(
©©u v
p
©©v w
=>
©©x z
p
©©{ |
.
©©| }
Level©©} Ç
==©©É Ö
$num©©Ü á
)©©á à
.©©à â
Where©©â é
(©©é è
p©©è ê
=>©©ë ì
p©©î ï
.©©ï ñ
Hashkey©©ñ ù
!=©©û †
null©©° •
)©©• ¶
)©©¶ ß
;©©ß ®
var
™™ "
DdlParCounter_Level2
™™ (
=
™™) *
Mapper
™™+ 1
.
™™1 2
Map
™™2 5
<
™™5 6
List
™™6 :
<
™™: ;
ParCounterDTO
™™; H
>
™™H I
>
™™I J
(
™™J K
_baseParCounter
™™K Z
.
™™Z [ 
GetAllAsNoTracking
™™[ m
(
™™m n
)
™™n o
.
™™o p
Where
™™p u
(
™™u v
p
™™v w
=>
™™x z
p
™™{ |
.
™™| }
Level™™} Ç
==™™É Ö
$num™™Ü á
)™™á à
.™™à â
Where™™â é
(™™é è
p™™è ê
=>™™ë ì
p™™î ï
.™™ï ñ
Hashkey™™ñ ù
!=™™û †
null™™° •
)™™• ¶
)™™¶ ß
;™™ß ®
var
¨¨ #
DdlParLevel3InputType
¨¨ )
=
¨¨* +
Mapper
¨¨, 2
.
¨¨2 3
Map
¨¨3 6
<
¨¨6 7
List
¨¨7 ;
<
¨¨; <#
ParLevel3InputTypeDTO
¨¨< Q
>
¨¨Q R
>
¨¨R S
(
¨¨S T%
_baseParLevel3InputType
¨¨T k
.
¨¨k l 
GetAllAsNoTracking
¨¨l ~
(
¨¨~ 
)¨¨ Ä
)¨¨Ä Å
;¨¨Å Ç
var
≠≠ #
DdlParMeasurementUnit
≠≠ )
=
≠≠* +
Mapper
≠≠, 2
.
≠≠2 3
Map
≠≠3 6
<
≠≠6 7
List
≠≠7 ;
<
≠≠; <#
ParMeasurementUnitDTO
≠≠< Q
>
≠≠Q R
>
≠≠R S
(
≠≠S T%
_baseParMeasurementUnit
≠≠T k
.
≠≠k l 
GetAllAsNoTracking
≠≠l ~
(
≠≠~ 
)≠≠ Ä
)≠≠Ä Å
;≠≠Å Ç
var
ØØ #
DdlParLevel3BoolFalse
ØØ )
=
ØØ* +
Mapper
ØØ, 2
.
ØØ2 3
Map
ØØ3 6
<
ØØ6 7
List
ØØ7 ;
<
ØØ; <#
ParLevel3BoolFalseDTO
ØØ< Q
>
ØØQ R
>
ØØR S
(
ØØS T%
_baseParLevel3BoolFalse
ØØT k
.
ØØk l 
GetAllAsNoTracking
ØØl ~
(
ØØ~ 
)ØØ Ä
)ØØÄ Å
;ØØÅ Ç
var
∞∞ "
DdlParLevel3BoolTrue
∞∞ (
=
∞∞) *
Mapper
∞∞+ 1
.
∞∞1 2
Map
∞∞2 5
<
∞∞5 6
List
∞∞6 :
<
∞∞: ;"
ParLevel3BoolTrueDTO
∞∞; O
>
∞∞O P
>
∞∞P Q
(
∞∞Q R$
_baseParLevel3BoolTrue
∞∞R h
.
∞∞h i 
GetAllAsNoTracking
∞∞i {
(
∞∞{ |
)
∞∞| }
)
∞∞} ~
;
∞∞~ 
var
≤≤ 

DdlparCrit
≤≤ 
=
≤≤  
Mapper
≤≤! '
.
≤≤' (
Map
≤≤( +
<
≤≤+ ,
List
≤≤, 0
<
≤≤0 1!
ParCriticalLevelDTO
≤≤1 D
>
≤≤D E
>
≤≤E F
(
≤≤F G'
_baseRepoParCriticalLevel
≤≤G `
.
≤≤` a 
GetAllAsNoTracking
≤≤a s
(
≤≤s t
)
≤≤t u
)
≤≤u v
;
≤≤v w
var
¥¥ 
DdlparCompany
¥¥ !
=
¥¥" #
Mapper
¥¥$ *
.
¥¥* +
Map
¥¥+ .
<
¥¥. /
List
¥¥/ 3
<
¥¥3 4
ParCompanyDTO
¥¥4 A
>
¥¥A B
>
¥¥B C
(
¥¥C D!
_baseRepoParCompany
¥¥D W
.
¥¥W X 
GetAllAsNoTracking
¥¥X j
(
¥¥j k
)
¥¥k l
)
¥¥l m
;
¥¥m n
var
µµ 
DdlScoretype
µµ  
=
µµ! "
Mapper
µµ# )
.
µµ) *
Map
µµ* -
<
µµ- .
List
µµ. 2
<
µµ2 3
ParScoreTypeDTO
µµ3 B
>
µµB C
>
µµC D
(
µµD E
_baseRepoParScore
µµE V
.
µµV W 
GetAllAsNoTracking
µµW i
(
µµi j
)
µµj k
)
µµk l
;
µµl m
var
∑∑ 
retorno
∑∑ 
=
∑∑ 
new
∑∑ !
	ParamsDdl
∑∑" +
(
∑∑+ ,
)
∑∑, -
;
∑∑- .
retorno
ªª 
.
ªª 
SetDdlsNivel123
ªª '
(
ªª' (
DdlparLevel1
ªª( 4
,
ªª4 5
DdlparLevel2
ºº  ,
,
ºº, -
DdlparLevel3
ΩΩ  ,
)
ΩΩ, -
;
ΩΩ- .
retorno
øø 
.
øø 
SetDdls
øø 
(
øø  !
DdlParConsolidation
øø  3
,
øø3 4
DdlFrequency
øø5 A
,
øøA B
DdlparCluster
øøC P
,
øøP Q#
DdlparLevelDefinition
øøR g
,
øøg h
DdlParFieldType
øøi x
,
øøx y
DdlParDepartmentøøz ä
,øøä ã$
DdlParCounter_Level1øøå †
,øø† ° 
DdlParLocal_Level1
¿¿  2
,
¿¿2 3"
DdlParCounter_Level2
¿¿4 H
,
¿¿H I 
DdlParLocal_Level2
¿¿J \
,
¿¿\ ]%
DdlParNotConformityRule
¿¿^ u
,
¿¿u v$
DdlParLevel3InputType¿¿w å
,¿¿å ç%
DdlParMeasurementUnit¿¿é £
,¿¿£ §#
DdlParLevel3BoolFalse
¡¡  5
,
¡¡5 6"
DdlParLevel3BoolTrue
¡¡7 K
,
¡¡K L

DdlparCrit
¡¡M W
,
¡¡W X
DdlparCompany
¡¡Y f
,
¡¡f g
DdlScoretype
¡¡h t
)
¡¡t u
;
¡¡u v
return
¬¬ 
retorno
¬¬ 
;
¬¬ 
}
√√ 
}
ƒƒ 	
public
   
List
   
<
   &
ParLevel3Level2Level1DTO
   ,
>
  , -
AddVinculoL1L2
  . <
(
  < =
int
  = @
idLevel1
  A I
,
  I J
int
  K N
idLevel2
  O W
,
  W X
int
  Y \
idLevel3
  ] e
,
  e f
int
  g j
?
  j k
userId
  l r
=
  s t
$num
  u v
,
  v w
int
  x {
?
  { |
	companyId  } Ü
=  á à
null  â ç
)  ç é
{
ÀÀ 	
var
ÕÕ 
retorno
ÕÕ 
=
ÕÕ 
new
ÕÕ 
List
ÕÕ "
<
ÕÕ" #&
ParLevel3Level2Level1DTO
ÕÕ# ;
>
ÕÕ; <
(
ÕÕ< =
)
ÕÕ= >
;
ÕÕ> ?
_paramsRepo
ŒŒ 
.
ŒŒ 
SaveVinculoL3L2L1
ŒŒ )
(
ŒŒ) *
idLevel1
ŒŒ* 2
,
ŒŒ2 3
idLevel2
ŒŒ4 <
,
ŒŒ< =
idLevel3
ŒŒ> F
,
ŒŒF G
userId
ŒŒH N
,
ŒŒN O
	companyId
ŒŒP Y
)
ŒŒY Z
;
ŒŒZ [
return
œœ 
retorno
œœ 
;
œœ 
}
—— 	
public
”” 
bool
”” +
VerificaShowBtnRemVinculoL1L2
”” 1
(
””1 2
int
””2 5
idLevel1
””6 >
,
””> ?
int
””@ C
idLevel2
””D L
)
””L M
{
‘‘ 	
var
’’ 
response
’’ 
=
’’ 
false
’’  
;
’’  !
using
÷÷ 
(
÷÷ 
var
÷÷ 
db
÷÷ 
=
÷÷ 
new
÷÷ 
SgqDbDevEntities
÷÷  0
(
÷÷0 1
)
÷÷1 2
)
÷÷2 3
{
◊◊ 
var
ÿÿ 
sql1
ÿÿ 
=
ÿÿ 
$str
ÿÿ V
+
ÿÿW X
idLevel1
ÿÿY a
+
ÿÿb c
$strÿÿd ∂
+ÿÿ∑ ∏
idLevel2ÿÿπ ¡
+ÿÿ¬ √
$strÿÿƒ ÷
;ÿÿ÷ ◊
var
ŸŸ 
result1
ŸŸ 
=
ŸŸ 
db
ŸŸ  
.
ŸŸ  !
Database
ŸŸ! )
.
ŸŸ) *
SqlQuery
ŸŸ* 2
<
ŸŸ2 3#
ParLevel3Level2Level1
ŸŸ3 H
>
ŸŸH I
(
ŸŸI J
sql1
ŸŸJ N
)
ŸŸN O
.
ŸŸO P
ToList
ŸŸP V
(
ŸŸV W
)
ŸŸW X
;
ŸŸX Y
var
⁄⁄ 
result2
⁄⁄ 
=
⁄⁄ 
db
⁄⁄  
.
⁄⁄  !
ParLevel2Level1
⁄⁄! 0
.
⁄⁄0 1
Where
⁄⁄1 6
(
⁄⁄6 7
r
⁄⁄7 8
=>
⁄⁄9 ;
r
⁄⁄< =
.
⁄⁄= >
ParLevel1_Id
⁄⁄> J
==
⁄⁄K M
idLevel1
⁄⁄N V
&&
⁄⁄W Y
r
⁄⁄Z [
.
⁄⁄[ \
ParLevel2_Id
⁄⁄\ h
==
⁄⁄i k
idLevel2
⁄⁄l t
&&
⁄⁄u w
r
⁄⁄x y
.
⁄⁄y z
IsActive⁄⁄z Ç
==⁄⁄É Ö
true⁄⁄Ü ä
)⁄⁄ä ã
.⁄⁄ã å
ToList⁄⁄å í
(⁄⁄í ì
)⁄⁄ì î
;⁄⁄î ï
if
€€ 
(
€€ 
result1
€€ 
?
€€ 
.
€€ 
Count
€€ "
(
€€" #
)
€€# $
>
€€% &
$num
€€' (
||
€€) +
result2
€€, 3
?
€€3 4
.
€€4 5
Count
€€5 :
(
€€: ;
)
€€; <
>
€€= >
$num
€€? @
)
€€@ A
response
‹‹ 
=
‹‹ 
true
‹‹ #
;
‹‹# $
else
›› 
response
ﬁﬁ 
=
ﬁﬁ 
false
ﬁﬁ $
;
ﬁﬁ$ %
}
ﬂﬂ 
return
‡‡ 
response
‡‡ 
;
‡‡ 
}
·· 	
public
„„ 
bool
„„ 
RemVinculoL1L2
„„ "
(
„„" #
int
„„# &
idLevel1
„„' /
,
„„/ 0
int
„„1 4
idLevel2
„„5 =
)
„„= >
{
‰‰ 	
using
ÁÁ 
(
ÁÁ 
var
ÁÁ 
db
ÁÁ 
=
ÁÁ 
new
ÁÁ 
SgqDbDevEntities
ÁÁ  0
(
ÁÁ0 1
)
ÁÁ1 2
)
ÁÁ2 3
{
ËË 
var
ÈÈ 
sql1
ÈÈ 
=
ÈÈ 
$str
ÈÈ V
+
ÈÈW X
idLevel1
ÈÈY a
+
ÈÈb c
$strÈÈd ∂
+ÈÈ∑ ∏
idLevel2ÈÈπ ¡
+ÈÈ¬ √
$strÈÈƒ «
;ÈÈ« »
var
ÍÍ 
sql2
ÍÍ 
=
ÍÍ 
$str
ÍÍ P
+
ÍÍQ R
idLevel1
ÍÍS [
+
ÍÍ\ ]
$str
ÍÍ^ t
+
ÍÍu v
idLevel2
ÍÍw 
;ÍÍ Ä
var
ÎÎ 
result1
ÎÎ 
=
ÎÎ 
db
ÎÎ  
.
ÎÎ  !
Database
ÎÎ! )
.
ÎÎ) *
SqlQuery
ÎÎ* 2
<
ÎÎ2 3#
ParLevel3Level2Level1
ÎÎ3 H
>
ÎÎH I
(
ÎÎI J
sql1
ÎÎJ N
)
ÎÎN O
.
ÎÎO P
ToList
ÎÎP V
(
ÎÎV W
)
ÎÎW X
;
ÎÎX Y
var
ÏÏ 
result2
ÏÏ 
=
ÏÏ 
db
ÏÏ  
.
ÏÏ  !
ParLevel2Level1
ÏÏ! 0
.
ÏÏ0 1
Where
ÏÏ1 6
(
ÏÏ6 7
r
ÏÏ7 8
=>
ÏÏ9 ;
r
ÏÏ< =
.
ÏÏ= >
ParLevel1_Id
ÏÏ> J
==
ÏÏK M
idLevel1
ÏÏN V
&&
ÏÏW Y
r
ÏÏZ [
.
ÏÏ[ \
ParLevel2_Id
ÏÏ\ h
==
ÏÏi k
idLevel2
ÏÏl t
)
ÏÏt u
.
ÏÏu v
ToList
ÏÏv |
(
ÏÏ| }
)
ÏÏ} ~
;
ÏÏ~ 
if
ÌÌ 
(
ÌÌ 
result1
ÌÌ 
?
ÌÌ 
.
ÌÌ 
Count
ÌÌ "
(
ÌÌ" #
)
ÌÌ# $
>
ÌÌ% &
$num
ÌÌ' (
||
ÌÌ) +
result2
ÌÌ, 3
?
ÌÌ3 4
.
ÌÌ4 5
Count
ÌÌ5 :
(
ÌÌ: ;
)
ÌÌ; <
>
ÌÌ= >
$num
ÌÌ? @
)
ÌÌ@ A
{
ÓÓ 
if
ÔÔ 
(
ÔÔ 
result1
ÔÔ 
?
ÔÔ  
.
ÔÔ  !
Count
ÔÔ! &
(
ÔÔ& '
)
ÔÔ' (
>
ÔÔ) *
$num
ÔÔ+ ,
)
ÔÔ, -
sql1
 
=
 
$str
 S
+
T U
idLevel1
V ^
+
_ `
$stra ≥
+¥ µ
idLevel2∂ æ
+ø ¿
$str¡ ƒ
;ƒ ≈
if
ÒÒ 
(
ÒÒ 
result2
ÒÒ 
?
ÒÒ  
.
ÒÒ  !
Count
ÒÒ! &
(
ÒÒ& '
)
ÒÒ' (
>
ÒÒ) *
$num
ÒÒ+ ,
)
ÒÒ, -
sql2
ÚÚ 
=
ÚÚ 
$str
ÚÚ M
+
ÚÚN O
idLevel1
ÚÚP X
+
ÚÚY Z
$str
ÚÚ[ q
+
ÚÚr s
idLevel2
ÚÚt |
;
ÚÚ| }
}
ÛÛ 
else
ÙÙ 
{
ıı 
return
ˆˆ 
false
ˆˆ  
;
ˆˆ  !
}
˜˜ 
var
˘˘ 
r1
˘˘ 
=
˘˘ 
db
˘˘ 
.
˘˘ 
Database
˘˘ $
.
˘˘$ %
ExecuteSqlCommand
˘˘% 6
(
˘˘6 7
sql1
˘˘7 ;
)
˘˘; <
;
˘˘< =
var
˙˙ 
r2
˙˙ 
=
˙˙ 
db
˙˙ 
.
˙˙ 
Database
˙˙ $
.
˙˙$ %
ExecuteSqlCommand
˙˙% 6
(
˙˙6 7
sql2
˙˙7 ;
)
˙˙; <
;
˙˙< =
}
˚˚ 
return
˝˝ 
true
˝˝ 
;
˝˝ 
}
˛˛ 	
public
ÑÑ  
ParLevel3Level2DTO
ÑÑ !
AddVinculoL3L2
ÑÑ" 0
(
ÑÑ0 1
int
ÑÑ1 4
idLevel2
ÑÑ5 =
,
ÑÑ= >
int
ÑÑ? B
idLevel3
ÑÑC K
,
ÑÑK L
decimal
ÑÑM T
peso
ÑÑU Y
,
ÑÑY Z
int
ÑÑ[ ^
?
ÑÑ^ _
groupLevel2
ÑÑ` k
=
ÑÑl m
$num
ÑÑn o
)
ÑÑo p
{
ÖÖ 	
ParLevel3Level2
ÜÜ $
objLelvel2Level3ToSave
ÜÜ 2
;
ÜÜ2 3
var
áá 
level2
áá 
=
áá  
_baseRepoParLevel2
áá +
.
áá+ ,
GetById
áá, 3
(
áá3 4
idLevel2
áá4 <
)
áá< =
;
áá= >$
objLelvel2Level3ToSave
ââ "
=
ââ# $
new
ââ% (
ParLevel3Level2
ââ) 8
(
ââ8 9
)
ââ9 :
{
ää 
ParLevel2_Id
ãã 
=
ãã 
idLevel2
ãã '
,
ãã' (
ParLevel3_Id
åå 
=
åå 
idLevel3
åå '
,
åå' (
Weight
çç 
=
çç 
peso
çç 
,
çç 
ParLevel3Group_Id
éé !
=
éé" #
groupLevel2
éé$ /
==
éé0 2
$num
éé3 4
?
éé5 6
null
éé7 ;
:
éé< =
groupLevel2
éé> I
}
èè 
;
èè 
var
ëë 
	existente
ëë 
=
ëë &
_baseRepoParLevel3Level2
ëë 4
.
ëë4 5
GetAll
ëë5 ;
(
ëë; <
)
ëë< =
.
ëë= >
FirstOrDefault
ëë> L
(
ëëL M
r
ëëM N
=>
ëëO Q
r
ëëR S
.
ëëS T
ParLevel2_Id
ëëT `
==
ëëa c
idLevel2
ëëd l
&&
ëëm o
r
ëëp q
.
ëëq r
ParLevel3_Id
ëër ~
==ëë Å
idLevel3ëëÇ ä
)ëëä ã
;ëëã å
if
íí 
(
íí 
	existente
íí 
!=
íí 
null
íí !
)
íí! "
{
ìì $
objLelvel2Level3ToSave
îî &
=
îî' (
	existente
îî) 2
;
îî2 3$
objLelvel2Level3ToSave
ïï &
.
ïï& '
Weight
ïï' -
=
ïï. /
	existente
ïï0 9
.
ïï9 :
Weight
ïï: @
;
ïï@ A$
objLelvel2Level3ToSave
ññ &
.
ññ& '
ParLevel3Group_Id
ññ' 8
=
ññ9 :
groupLevel2
ññ; F
==
ññG I
$num
ññJ K
?
ññL M
null
ññN R
:
ññS T
groupLevel2
ññU `
;
ññ` a
}
óó 
else
òò 
{
ôô $
objLelvel2Level3ToSave
öö &
.
öö& '
Weight
öö' -
=
öö. /
$num
öö0 1
;
öö1 2
}
õõ $
objLelvel2Level3ToSave
úú "
.
úú" #
IsActive
úú# +
=
úú, -
true
úú. 2
;
úú2 3&
_baseRepoParLevel3Level2
ùù $
.
ùù$ %
AddOrUpdate
ùù% 0
(
ùù0 1$
objLelvel2Level3ToSave
ùù1 G
)
ùùG H
;
ùùH I 
ParLevel3Level2DTO
ûû 

objtReturn
ûû )
=
ûû* +
Mapper
ûû, 2
.
ûû2 3
Map
ûû3 6
<
ûû6 7 
ParLevel3Level2DTO
ûû7 I
>
ûûI J
(
ûûJ K$
objLelvel2Level3ToSave
ûûK a
)
ûûa b
;
ûûb c
return
¢¢ 

objtReturn
¢¢ 
;
¢¢ 
}
££ 	
public
•• 
ParLevel3GroupDTO
••  "
RemoveParLevel3Group
••! 5
(
••5 6
int
••6 9
Id
••: <
)
••< =
{
¶¶ 	
var
ßß 
parLevel3Group
ßß 
=
ßß  !
_baseParLevel3Group
ßß! 4
.
ßß4 5
GetAll
ßß5 ;
(
ßß; <
)
ßß< =
.
ßß= >
FirstOrDefault
ßß> L
(
ßßL M
r
ßßM N
=>
ßßO Q
r
ßßR S
.
ßßS T
Id
ßßT V
==
ßßW Y
Id
ßßZ \
)
ßß\ ]
;
ßß] ^
parLevel3Group
®® 
.
®® 
IsActive
®® #
=
®®$ %
false
®®& +
;
®®+ ,!
_baseParLevel3Group
©© 
.
©©  
AddOrUpdate
©©  +
(
©©+ ,
parLevel3Group
©©, :
)
©©: ;
;
©©; <
return
´´ 
Mapper
´´ 
.
´´ 
Map
´´ 
<
´´ 
ParLevel3GroupDTO
´´ /
>
´´/ 0
(
´´0 1
parLevel3Group
´´1 ?
)
´´? @
;
´´@ A
}
¨¨ 	
public
≤≤ 
List
≤≤ 
<
≤≤ 
ParLevel1DTO
≤≤  
>
≤≤  !
GetAllLevel1
≤≤" .
(
≤≤. /
)
≤≤/ 0
{
≥≥ 	
var
¥¥ 
retorno
¥¥ 
=
¥¥ 
new
¥¥ 
List
¥¥ "
<
¥¥" #
ParLevel1DTO
¥¥# /
>
¥¥/ 0
(
¥¥0 1
)
¥¥1 2
;
¥¥2 3
var
∂∂ 

listLevel1
∂∂ 
=
∂∂  
_baseRepoParLevel1
∂∂ /
.
∂∂/ 0
GetAll
∂∂0 6
(
∂∂6 7
)
∂∂7 8
.
∂∂8 9
Where
∂∂9 >
(
∂∂> ?
r
∂∂? @
=>
∂∂A C
r
∂∂D E
.
∂∂E F
IsActive
∂∂F N
==
∂∂O Q
true
∂∂R V
)
∂∂V W
;
∂∂W X
foreach
∏∏ 
(
∏∏ 
var
∏∏ 
level1
∏∏ 
in
∏∏  "

listLevel1
∏∏# -
)
∏∏- .
{
ππ 
retorno
∫∫ 
.
∫∫ 
Add
∫∫ 
(
∫∫ !
GetLevel1TelaColeta
∫∫ /
(
∫∫/ 0
level1
∫∫0 6
.
∫∫6 7
Id
∫∫7 9
)
∫∫9 :
)
∫∫: ;
;
∫∫; <
}
ªª 
return
ΩΩ 
retorno
ΩΩ 
;
ΩΩ 
}
ææ 	
public
≈≈ 
ParLevel1DTO
≈≈ !
GetLevel1TelaColeta
≈≈ /
(
≈≈/ 0
int
≈≈0 3
idParLevel1
≈≈4 ?
)
≈≈? @
{
∆∆ 	
var
»» 
	parlevel1
»» 
=
»»  
_baseRepoParLevel1
»» .
.
»». /
GetById
»»/ 6
(
»»6 7
idParLevel1
»»7 B
)
»»B C
;
»»C D
var
…… 
retorno
…… 
=
…… 
Mapper
……  
.
……  !
Map
……! $
<
……$ %
ParLevel1DTO
……% 1
>
……1 2
(
……2 3 
_baseRepoParLevel1
……3 E
.
……E F
GetById
……F M
(
……M N
idParLevel1
……N Y
)
……Y Z
)
……Z [
;
……[ \
retorno
ÃÃ 
.
ÃÃ #
listLevel1XClusterDto
ÃÃ )
=
ÃÃ* +
Mapper
ÃÃ, 2
.
ÃÃ2 3
Map
ÃÃ3 6
<
ÃÃ6 7
List
ÃÃ7 ;
<
ÃÃ; <"
ParLevel1XClusterDTO
ÃÃ< P
>
ÃÃP Q
>
ÃÃQ R
(
ÃÃR S
	parlevel1
ÃÃS \
.
ÃÃ\ ]
ParLevel1XCluster
ÃÃ] n
.
ÃÃn o
Where
ÃÃo t
(
ÃÃt u
r
ÃÃu v
=>
ÃÃw y
r
ÃÃz {
.
ÃÃ{ |
IsActiveÃÃ| Ñ
==ÃÃÖ á
trueÃÃà å
)ÃÃå ç
)ÃÃç é
;ÃÃé è
retorno
œœ 
.
œœ  
cabecalhosInclusos
œœ &
=
œœ' (
Mapper
œœ) /
.
œœ/ 0
Map
œœ0 3
<
œœ3 4
List
œœ4 8
<
œœ8 9&
ParLevel1XHeaderFieldDTO
œœ9 Q
>
œœQ R
>
œœR S
(
œœS T,
_baseRepoParLevel1XHeaderField
œœT r
.
œœr s
GetAll
œœs y
(
œœy z
)
œœz {
.
œœ{ |
Whereœœ| Å
(œœÅ Ç
rœœÇ É
=>œœÑ Ü
rœœá à
.œœà â
ParLevel1_Idœœâ ï
==œœñ ò
retornoœœô †
.œœ† °
Idœœ° £
&&œœ§ ¶
rœœß ®
.œœ® ©
IsActiveœœ© ±
==œœ≤ ¥
trueœœµ π
)œœπ ∫
)œœ∫ ª
;œœª º
retorno
““ 
.
““ !
contadoresIncluidos
““ '
=
““( )
Mapper
““* 0
.
““0 1
Map
““1 4
<
““4 5
List
““5 9
<
““9 :!
ParCounterXLocalDTO
““: M
>
““M N
>
““N O
(
““O P'
_baseRepoParCounterXLocal
““P i
.
““i j
GetAll
““j p
(
““p q
)
““q r
.
““r s
Where
““s x
(
““x y
r
““y z
=>
““{ }
r
““~ 
.““ Ä
ParLevel1_Id““Ä å
==““ç è
retorno““ê ó
.““ó ò
Id““ò ö
)““ö õ
)““õ ú
;““ú ù
retorno
’’ 
.
’’ *
listParLevel3Level2Level1Dto
’’ 0
=
’’1 2
Mapper
’’3 9
.
’’9 :
Map
’’: =
<
’’= >
List
’’> B
<
’’B C&
ParLevel3Level2Level1DTO
’’C [
>
’’[ \
>
’’\ ]
(
’’] ^,
_baseRepoParLevel3Level2Level1
’’^ |
.
’’| }
GetAll’’} É
(’’É Ñ
)’’Ñ Ö
.’’Ö Ü
Where’’Ü ã
(’’ã å
r’’å ç
=>’’é ê
r’’ë í
.’’í ì
ParLevel1_Id’’ì ü
==’’† ¢
retorno’’£ ™
.’’™ ´
Id’’´ ≠
)’’≠ Æ
.’’Æ Ø
ToList’’Ø µ
(’’µ ∂
)’’∂ ∑
)’’∑ ∏
;’’∏ π
retorno
÷÷ 
.
÷÷ 6
(CreateSelectListParamsViewModelListLevel
÷÷ <
(
÷÷< =
Mapper
÷÷= C
.
÷÷C D
Map
÷÷D G
<
÷÷G H
List
÷÷H L
<
÷÷L M
ParLevel2DTO
÷÷M Y
>
÷÷Y Z
>
÷÷Z [
(
÷÷[ \ 
_baseRepoParLevel2
÷÷\ n
.
÷÷n o
GetAll
÷÷o u
(
÷÷u v
)
÷÷v w
)
÷÷w x
,
÷÷x y
retorno÷÷z Å
.÷÷Å Ç,
listParLevel3Level2Level1Dto÷÷Ç û
)÷÷û ü
;÷÷ü †
retorno
ÿÿ 
.
ÿÿ "
listParLevel2Colleta
ÿÿ (
=
ÿÿ) *
new
ÿÿ+ .
List
ÿÿ/ 3
<
ÿÿ3 4
ParLevel2DTO
ÿÿ4 @
>
ÿÿ@ A
(
ÿÿA B
)
ÿÿB C
;
ÿÿC D
foreach
ŸŸ 
(
ŸŸ 
var
ŸŸ &
ParLevel3Level2Level1Dto
ŸŸ 1
in
ŸŸ2 4
retorno
ŸŸ5 <
.
ŸŸ< =*
listParLevel3Level2Level1Dto
ŸŸ= Y
.
ŸŸY Z
Distinct
ŸŸZ b
(
ŸŸb c
)
ŸŸc d
)
ŸŸd e
{
⁄⁄ 
if
€€ 
(
€€ 
!
€€ 
retorno
€€ 
.
€€ "
listParLevel2Colleta
€€ 1
.
€€1 2
Any
€€2 5
(
€€5 6
r
€€6 7
=>
€€8 :
r
€€; <
.
€€< =
Id
€€= ?
==
€€@ B&
ParLevel3Level2Level1Dto
€€C [
.
€€[ \
ParLevel3Level2
€€\ k
.
€€k l
	ParLevel2
€€l u
.
€€u v
Id
€€v x
)
€€x y
)
€€y z
{
‹‹ &
ParLevel3Level2Level1Dto
›› ,
.
››, -
ParLevel3Level2
››- <
.
››< =
	ParLevel2
››= F
.
››F G"
listParCounterXLocal
››G [
=
››\ ]
Mapper
››^ d
.
››d e
Map
››e h
<
››h i
List
››i m
<
››m n"
ParCounterXLocalDTO››n Å
>››Å Ç
>››Ç É
(››É Ñ%
_baseParCounterXLocal››Ñ ô
.››ô ö
GetAll››ö †
(››† °
)››° ¢
.››¢ £
Where››£ ®
(››® ©
r››© ™
=>››´ ≠
r››Æ Ø
.››Ø ∞
ParLevel2_Id››∞ º
==››Ω ø(
ParLevel3Level2Level1Dto››¿ ÿ
.››ÿ Ÿ
ParLevel3Level2››Ÿ Ë
.››Ë È
	ParLevel2››È Ú
.››Ú Û
Id››Û ı
&&››ˆ ¯
r››˘ ˙
.››˙ ˚
IsActive››˚ É
==››Ñ Ü
true››á ã
)››ã å
.››å ç
ToList››ç ì
(››ì î
)››î ï
)››ï ñ
;››ñ ó&
ParLevel3Level2Level1Dto
ﬁﬁ ,
.
ﬁﬁ, -
ParLevel3Level2
ﬁﬁ- <
.
ﬁﬁ< =
	ParLevel2
ﬁﬁ= F
.
ﬁﬁF G
ParamEvaluation
ﬁﬁG V
=
ﬁﬁW X
Mapper
ﬁﬁY _
.
ﬁﬁ_ `
Map
ﬁﬁ` c
<
ﬁﬁc d
ParEvaluationDTO
ﬁﬁd t
>
ﬁﬁt u
(
ﬁﬁu v!
_baseParEvaluationﬁﬁv à
.ﬁﬁà â
GetAllﬁﬁâ è
(ﬁﬁè ê
)ﬁﬁê ë
.ﬁﬁë í
Whereﬁﬁí ó
(ﬁﬁó ò
rﬁﬁò ô
=>ﬁﬁö ú
rﬁﬁù û
.ﬁﬁû ü
ParLevel2_Idﬁﬁü ´
==ﬁﬁ¨ Æ(
ParLevel3Level2Level1DtoﬁﬁØ «
.ﬁﬁ« »
ParLevel3Level2ﬁﬁ» ◊
.ﬁﬁ◊ ÿ
	ParLevel2ﬁﬁÿ ·
.ﬁﬁ· ‚
Idﬁﬁ‚ ‰
)ﬁﬁ‰ Â
.ﬁﬁÂ Ê
FirstOrDefaultﬁﬁÊ Ù
(ﬁﬁÙ ı
)ﬁﬁı ˆ
)ﬁﬁˆ ˜
;ﬁﬁ˜ ¯&
ParLevel3Level2Level1Dto
ﬂﬂ ,
.
ﬂﬂ, -
ParLevel3Level2
ﬂﬂ- <
.
ﬂﬂ< =
	ParLevel2
ﬂﬂ= F
.
ﬂﬂF G
ParamSample
ﬂﬂG R
=
ﬂﬂS T
Mapper
ﬂﬂU [
.
ﬂﬂ[ \
Map
ﬂﬂ\ _
<
ﬂﬂ_ `
ParSampleDTO
ﬂﬂ` l
>
ﬂﬂl m
(
ﬂﬂm n
_baseParSample
ﬂﬂn |
.
ﬂﬂ| }
GetAllﬂﬂ} É
(ﬂﬂÉ Ñ
)ﬂﬂÑ Ö
.ﬂﬂÖ Ü
WhereﬂﬂÜ ã
(ﬂﬂã å
rﬂﬂå ç
=>ﬂﬂé ê
rﬂﬂë í
.ﬂﬂí ì
ParLevel2_Idﬂﬂì ü
==ﬂﬂ† ¢(
ParLevel3Level2Level1Dtoﬂﬂ£ ª
.ﬂﬂª º
ParLevel3Level2ﬂﬂº À
.ﬂﬂÀ Ã
	ParLevel2ﬂﬂÃ ’
.ﬂﬂ’ ÷
Idﬂﬂ÷ ÿ
)ﬂﬂÿ Ÿ
.ﬂﬂŸ ⁄
FirstOrDefaultﬂﬂ⁄ Ë
(ﬂﬂË È
)ﬂﬂÈ Í
)ﬂﬂÍ Î
;ﬂﬂÎ Ï
retorno
‡‡ 
.
‡‡ "
listParLevel2Colleta
‡‡ 0
.
‡‡0 1
Add
‡‡1 4
(
‡‡4 5&
ParLevel3Level2Level1Dto
‡‡5 M
.
‡‡M N
ParLevel3Level2
‡‡N ]
.
‡‡] ^
	ParLevel2
‡‡^ g
)
‡‡g h
;
‡‡h i
}
·· 
var
‚‚ %
parLevel3Level2DoLevel2
‚‚ +
=
‚‚, -
_repoParLevel3
‚‚. <
.
‚‚< =&
GetLevel3VinculadoLevel2
‚‚= U
(
‚‚U V
retorno
‚‚V ]
.
‚‚] ^
Id
‚‚^ `
)
‚‚` a
;
‚‚a b
retorno
‰‰ 
.
‰‰ "
listParLevel2Colleta
‰‰ ,
.
‰‰, -
LastOrDefault
‰‰- :
(
‰‰: ;
)
‰‰; <
.
‰‰< =#
listaParLevel3Colleta
‰‰= R
=
‰‰S T
new
‰‰U X
List
‰‰Y ]
<
‰‰] ^
ParLevel3DTO
‰‰^ j
>
‰‰j k
(
‰‰k l
)
‰‰l m
;
‰‰m n
foreach
ÂÂ 
(
ÂÂ 
var
ÂÂ 
level3Level2
ÂÂ )
in
ÂÂ* ,%
parLevel3Level2DoLevel2
ÂÂ- D
.
ÂÂD E
Where
ÂÂE J
(
ÂÂJ K
r
ÂÂK L
=>
ÂÂM O
r
ÂÂP Q
.
ÂÂQ R
ParLevel2_Id
ÂÂR ^
==
ÂÂ_ a&
ParLevel3Level2Level1Dto
ÂÂb z
.
ÂÂz {
ParLevel3Level2ÂÂ{ ä
.ÂÂä ã
	ParLevel2ÂÂã î
.ÂÂî ï
IdÂÂï ó
)ÂÂó ò
)ÂÂò ô
{
ÊÊ 
if
ÁÁ 
(
ÁÁ 
!
ÁÁ 
retorno
ÁÁ  
.
ÁÁ  !"
listParLevel2Colleta
ÁÁ! 5
.
ÁÁ5 6
LastOrDefault
ÁÁ6 C
(
ÁÁC D
)
ÁÁD E
.
ÁÁE F#
listaParLevel3Colleta
ÁÁF [
.
ÁÁ[ \
Any
ÁÁ\ _
(
ÁÁ_ `
r
ÁÁ` a
=>
ÁÁb d
r
ÁÁe f
.
ÁÁf g
Id
ÁÁg i
==
ÁÁj l
level3Level2
ÁÁm y
.
ÁÁy z
ParLevel3_IdÁÁz Ü
)ÁÁÜ á
)ÁÁá à
retorno
ËË 
.
ËË  "
listParLevel2Colleta
ËË  4
.
ËË4 5
LastOrDefault
ËË5 B
(
ËËB C
)
ËËC D
.
ËËD E#
listaParLevel3Colleta
ËËE Z
.
ËËZ [
Add
ËË[ ^
(
ËË^ _
Mapper
ËË_ e
.
ËËe f
Map
ËËf i
<
ËËi j
ParLevel3DTO
ËËj v
>
ËËv w
(
ËËw x!
_baseRepoParLevel3ËËx ä
.ËËä ã
GetByIdËËã í
(ËËí ì
level3Level2ËËì ü
.ËËü †
ParLevel3_IdËË† ¨
)ËË¨ ≠
)ËË≠ Æ
)ËËÆ Ø
;ËËØ ∞
}
ÈÈ 
}
ÎÎ 
retorno
ÓÓ 
.
ÓÓ +
parNotConformityRuleXLevelDto
ÓÓ 1
=
ÓÓ2 3
Mapper
ÓÓ4 :
.
ÓÓ: ;
Map
ÓÓ; >
<
ÓÓ> ?+
ParNotConformityRuleXLevelDTO
ÓÓ? \
>
ÓÓ\ ]
(
ÓÓ] ^-
_baseParNotConformityRuleXLevel
ÓÓ^ }
.
ÓÓ} ~
GetAllÓÓ~ Ñ
(ÓÓÑ Ö
)ÓÓÖ Ü
.ÓÓÜ á
FirstOrDefaultÓÓá ï
(ÓÓï ñ
rÓÓñ ó
=>ÓÓò ö
rÓÓõ ú
.ÓÓú ù
ParLevel1_IdÓÓù ©
==ÓÓ™ ¨
retornoÓÓ≠ ¥
.ÓÓ¥ µ
IdÓÓµ ∑
)ÓÓ∑ ∏
)ÓÓ∏ π
??ÓÓ∫ º
newÓÓΩ ¿-
ParNotConformityRuleXLevelDTOÓÓ¡ ﬁ
(ÓÓﬁ ﬂ
)ÓÓﬂ ‡
;ÓÓ‡ ·
retorno
ÒÒ 
.
ÒÒ 
listParRelapseDto
ÒÒ %
=
ÒÒ& '
Mapper
ÒÒ( .
.
ÒÒ. /
Map
ÒÒ/ 2
<
ÒÒ2 3
List
ÒÒ3 7
<
ÒÒ7 8
ParRelapseDTO
ÒÒ8 E
>
ÒÒE F
>
ÒÒF G
(
ÒÒG H
_baseParRelapse
ÒÒH W
.
ÒÒW X
GetAll
ÒÒX ^
(
ÒÒ^ _
)
ÒÒ_ `
.
ÒÒ` a
Where
ÒÒa f
(
ÒÒf g
r
ÒÒg h
=>
ÒÒi k
r
ÒÒl m
.
ÒÒm n
ParLevel1_Id
ÒÒn z
==
ÒÒ{ }
retornoÒÒ~ Ö
.ÒÒÖ Ü
IdÒÒÜ à
)ÒÒà â
)ÒÒâ ä
;ÒÒä ã
return
ÛÛ 
retorno
ÛÛ 
;
ÛÛ 
}
ÙÙ 	
public
˘˘ 
bool
˘˘ (
SetRequiredCamposCabecalho
˘˘ .
(
˘˘. /
int
˘˘/ 2
id
˘˘3 5
,
˘˘5 6
int
˘˘7 :
required
˘˘; C
)
˘˘C D
{
˙˙ 	
var
˚˚ 
headerField
˚˚ 
=
˚˚ %
_baseRepoParHeaderField
˚˚ 5
.
˚˚5 6
GetById
˚˚6 =
(
˚˚= >
id
˚˚> @
)
˚˚@ A
;
˚˚A B
if
¸¸ 
(
¸¸ 
required
¸¸ 
==
¸¸ 
$num
¸¸ 
)
¸¸ 
headerField
˝˝ 
.
˝˝ 

IsRequired
˝˝ &
=
˝˝' (
true
˝˝) -
;
˝˝- .
else
˛˛ 
headerField
ˇˇ 
.
ˇˇ 

IsRequired
ˇˇ &
=
ˇˇ' (
false
ˇˇ) .
;
ˇˇ. /%
_baseRepoParHeaderField
ÅÅ #
.
ÅÅ# $
AddOrUpdate
ÅÅ$ /
(
ÅÅ/ 0
headerField
ÅÅ0 ;
)
ÅÅ; <
;
ÅÅ< =
return
ÉÉ 
headerField
ÉÉ 
.
ÉÉ 

IsRequired
ÉÉ )
.
ÉÉ) *
Value
ÉÉ* /
;
ÉÉ/ 0
}
ÖÖ 	
public
áá 
ParMultipleValues
áá  '
SetDefaultMultiplaEscolha
áá! :
(
áá: ;
int
áá; >
idHeader
áá? G
,
ááG H
int
ááI L

idMultiple
ááM W
)
ááW X
{
àà 	
var
ää 
headerFieldList
ää 
=
ää  !(
_baseRepoParMultipleValues
ää" <
.
ää< =
GetAll
ää= C
(
ääC D
)
ääD E
.
ääE F
Where
ääF K
(
ääK L
r
ääL M
=>
ääN P
r
ääQ R
.
ääR S
ParHeaderField_Id
ääS d
==
ääe g
idHeader
ääh p
)
ääp q
;
ääq r
foreach
åå 
(
åå 
ParMultipleValues
åå &
m
åå' (
in
åå) +
headerFieldList
åå, ;
)
åå; <
{
çç 
m
éé 
.
éé 
IsDefaultOption
éé !
=
éé" #
false
éé$ )
;
éé) *(
_baseRepoParMultipleValues
èè *
.
èè* +
AddOrUpdate
èè+ 6
(
èè6 7
m
èè7 8
)
èè8 9
;
èè9 :
}
êê 
var
íí 
multiple
íí 
=
íí 
new
íí 
ParMultipleValues
íí 0
(
íí0 1
)
íí1 2
;
íí2 3
if
îî 
(
îî 

idMultiple
îî 
>
îî 
$num
îî 
)
îî 
{
ïï 
multiple
ññ 
=
ññ (
_baseRepoParMultipleValues
ññ 5
.
ññ5 6
GetById
ññ6 =
(
ññ= >

idMultiple
ññ> H
)
ññH I
;
ññI J
if
óó 
(
óó 
multiple
óó 
.
óó 
IsDefaultOption
óó ,
==
óó- /
null
óó0 4
||
óó5 7
multiple
óó8 @
.
óó@ A
IsDefaultOption
óóA P
==
óóQ S
false
óóT Y
)
óóY Z
multiple
òò 
.
òò 
IsDefaultOption
òò ,
=
òò- .
true
òò/ 3
;
òò3 4
else
ôô 
multiple
öö 
.
öö 
IsDefaultOption
öö ,
=
öö- .
false
öö/ 4
;
öö4 5(
_baseRepoParMultipleValues
úú *
.
úú* +
AddOrUpdate
úú+ 6
(
úú6 7
multiple
úú7 ?
)
úú? @
;
úú@ A
}
ùù 
return
üü 
multiple
üü 
;
üü 
}
°° 	
public
££ #
ParLevel2XHeaderField
££ $&
AddRemoveParHeaderLevel2
££% =
(
££= >#
ParLevel2XHeaderField
££> S#
parLevel2XHeaderField
££T i
)
££i j
{
§§ 	#
parLevel2XHeaderField
•• !
.
••! "
AddDate
••" )
=
••* +
DateTime
••, 4
.
••4 5
Now
••5 8
;
••8 9#
parLevel2XHeaderField
¶¶ !
.
¶¶! "
IsActive
¶¶" *
=
¶¶+ ,
true
¶¶- 1
;
¶¶1 2#
parLevel2XHeaderField
®® !
=
®®" #
_paramsRepo
®®$ /
.
®®/ 0!
SaveParHeaderLevel2
®®0 C
(
®®C D#
parLevel2XHeaderField
®®D Y
)
®®Y Z
;
®®Z [
return
™™ #
parLevel2XHeaderField
™™ (
;
™™( )
}
´´ 	
}
≠≠ 
}ØØ —Ö
VC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Services\RelatorioColetaDomain.cs
	namespace 	
Dominio
 
. 
Services 
{ 
public 

class !
RelatorioColetaDomain &
:' ("
IRelatorioColetaDomain) ?
{ 
private 
IBaseRepository 
<  
Level02  '
>' (
_baseLevel02Repo) 9
;9 :
private 
IBaseRepository 
<  
Level03  '
>' (
_baseLevel03Repo) 9
;9 :
private 
IEnumerable 
< 
Level02 #
># $
_listLevel02% 1
;1 2
private 
IEnumerable 
< 
Level03 #
># $
_listLevel03% 1
;1 2
private &
IRelatorioColetaRepository *
<* +
CollectionLevel02+ <
>< ="
_repoCollectionLevel02> T
;T U
private &
IRelatorioColetaRepository *
<* +
CollectionLevel03+ <
>< ="
_repoCollectionLevel03> T
;T U
private &
IRelatorioColetaRepository *
<* + 
ConsolidationLevel01+ ?
>? @%
_repoConsolidationLevel01A Z
;Z [
private &
IRelatorioColetaRepository *
<* + 
ConsolidationLevel02+ ?
>? @%
_repoConsolidationLevel02A Z
;Z [
public !
RelatorioColetaDomain $
($ %&
IRelatorioColetaRepository &
<& '
CollectionLevel02' 8
>8 9!
repoCollectionLevel02: O
,O P&
IRelatorioColetaRepository &
<& '
CollectionLevel03' 8
>8 9!
repoCollectionLevel03: O
,O P&
IRelatorioColetaRepository &
<& ' 
ConsolidationLevel01' ;
>; <$
repoConsolidationLevel01= U
,U V&
IRelatorioColetaRepository &
<& ' 
ConsolidationLevel02' ;
>; <$
repoConsolidationLevel02= U
,U V
IBaseRepository   
<   
Level02   #
>  # $
baseLevel02Repo  % 4
,  4 5
IBaseRepository!! 
<!! 
Level03!! #
>!!# $
baseLevel03Repo!!% 4
)"" 
{## 	
_baseLevel02Repo$$ 
=$$ 
baseLevel02Repo$$ .
;$$. /
_baseLevel03Repo%% 
=%% 
baseLevel03Repo%% .
;%%. /"
_repoCollectionLevel02&& "
=&&# $!
repoCollectionLevel02&&% :
;&&: ;"
_repoCollectionLevel03'' "
=''# $!
repoCollectionLevel03''% :
;'': ;%
_repoConsolidationLevel01(( %
=((& '$
repoConsolidationLevel01((( @
;((@ A%
_repoConsolidationLevel02)) %
=))& '$
repoConsolidationLevel02))( @
;))@ A
_listLevel02++ 
=++ 
_baseLevel02Repo++ +
.+++ ,
GetAll++, 2
(++2 3
)++3 4
;++4 5
_listLevel03,, 
=,, 
_baseLevel03Repo,, +
.,,+ ,
GetAll,,, 2
(,,2 3
),,3 4
;,,4 5
}-- 	
public33 
GenericReturn33 
<33 $
ResultSetRelatorioColeta33 5
>335 6 
GetCollectionLevel02337 K
(33K L!
DataCarrierFormulario33L a
form33b f
)33f g
{44 	
try55 
{66 
var88 
result88 
=88 "
_repoCollectionLevel0288 3
.883 4
	GetByDate884 =
(88= >
form88> B
)88B C
.88C D
ToList88D J
(88J K
)88K L
;88L M
Guard:: 
.::  
CheckListNullOrEmpty:: *
(::* +
result::+ 1
,::1 2
$str::3 Z
)::Z [
;::[ \
var<< 
	resultSet<< 
=<< 
new<<  #$
ResultSetRelatorioColeta<<$ <
(<<< =
)<<= >
{== $
listCollectionLevel02DTO>> ,
=>>- .
Mapper>>/ 5
.>>5 6
Map>>6 9
<>>9 :
List>>: >
<>>> ? 
CollectionLevel02DTO>>? S
>>>S T
>>>T U
(>>U V
result>>V \
)>>\ ]
}?? 
;?? 
returnAA 
newAA 
GenericReturnAA (
<AA( )$
ResultSetRelatorioColetaAA) A
>AAA B
(AAB C
	resultSetAAC L
)AAL M
;AAM N
}CC 
catchDD 
(DD 
	ExceptionDD 
eDD 
)DD 
{EE 
returnFF 
newFF 
GenericReturnFF (
<FF( )$
ResultSetRelatorioColetaFF) A
>FFA B
(FFB C
eFFC D
,FFD E
$strFFF u
)FFu v
;FFv w
}GG 
}HH 	
publicJJ 
GenericReturnJJ 
<JJ $
ResultSetRelatorioColetaJJ 5
>JJ5 6 
GetCollectionLevel03JJ7 K
(JJK L!
DataCarrierFormularioJJL a
formJJb f
)JJf g
{KK 	
tryLL 
{MM 
varOO 
resultOO 
=OO "
_repoCollectionLevel03OO 3
.OO3 4
	GetByDateOO4 =
(OO= >
formOO> B
)OOB C
.OOC D
ToListOOD J
(OOJ K
)OOK L
;OOL M
GuardQQ 
.QQ  
CheckListNullOrEmptyQQ *
(QQ* +
resultQQ+ 1
,QQ1 2
$strQQ3 Z
)QQZ [
;QQ[ \
varSS 
	resultSetSS 
=SS 
newSS  #$
ResultSetRelatorioColetaSS$ <
(SS< =
)SS= >
{TT $
listCollectionLevel02DTOUU ,
=UU- .
MapperUU/ 5
.UU5 6
MapUU6 9
<UU9 :
ListUU: >
<UU> ? 
CollectionLevel02DTOUU? S
>UUS T
>UUT U
(UUU V
resultUUV \
)UU\ ]
}VV 
;VV 
returnXX 
newXX 
GenericReturnXX (
<XX( )$
ResultSetRelatorioColetaXX) A
>XXA B
(XXB C
	resultSetXXC L
)XXL M
;XXM N
}ZZ 
catch[[ 
([[ 
	Exception[[ 
e[[ 
)[[ 
{\\ 
return]] 
new]] 
GenericReturn]] (
<]]( )$
ResultSetRelatorioColeta]]) A
>]]A B
(]]B C
e]]C D
,]]D E
$str]]F u
)]]u v
;]]v w
}^^ 
}__ 	
publicaa 
GenericReturnaa 
<aa $
ResultSetRelatorioColetaaa 5
>aa5 6#
GetConsolidationLevel01aa7 N
(aaN O!
DataCarrierFormularioaaO d
formaae i
)aai j
{bb 	
trycc 
{dd 
varff 
resultff 
=ff "
_repoCollectionLevel03ff 3
.ff3 4
	GetByDateff4 =
(ff= >
formff> B
)ffB C
.ffC D
ToListffD J
(ffJ K
)ffK L
;ffL M
Guardhh 
.hh  
CheckListNullOrEmptyhh *
(hh* +
resulthh+ 1
,hh1 2
$strhh3 ]
)hh] ^
;hh^ _
varjj 
	resultSetjj 
=jj 
newjj  #$
ResultSetRelatorioColetajj$ <
(jj< =
)jj= >
{kk $
listCollectionLevel02DTOll ,
=ll- .
Mapperll/ 5
.ll5 6
Mapll6 9
<ll9 :
Listll: >
<ll> ? 
CollectionLevel02DTOll? S
>llS T
>llT U
(llU V
resultllV \
)ll\ ]
}mm 
;mm 
returnoo 
newoo 
GenericReturnoo (
<oo( )$
ResultSetRelatorioColetaoo) A
>ooA B
(ooB C
	resultSetooC L
)ooL M
;ooM N
}qq 
catchrr 
(rr 
	Exceptionrr 
err 
)rr 
{ss 
returntt 
newtt 
GenericReturntt (
<tt( )$
ResultSetRelatorioColetatt) A
>ttA B
(ttB C
ettC D
,ttD E
$strttF x
)ttx y
;tty z
}uu 
}vv 	
publicxx 
GenericReturnxx 
<xx $
ResultSetRelatorioColetaxx 5
>xx5 6#
GetConsolidationLevel02xx7 N
(xxN O!
DataCarrierFormularioxxO d
formxxe i
)xxi j
{yy 	
tryzz 
{{{ 
var}} 
result}} 
=}} "
_repoCollectionLevel03}} 3
.}}3 4
	GetByDate}}4 =
(}}= >
form}}> B
)}}B C
.}}C D
ToList}}D J
(}}J K
)}}K L
;}}L M
Guard 
.  
CheckListNullOrEmpty *
(* +
result+ 1
,1 2
$str3 ]
)] ^
;^ _
var
ÅÅ 
	resultSet
ÅÅ 
=
ÅÅ 
new
ÅÅ  #&
ResultSetRelatorioColeta
ÅÅ$ <
(
ÅÅ< =
)
ÅÅ= >
{
ÇÇ &
listCollectionLevel02DTO
ÉÉ ,
=
ÉÉ- .
Mapper
ÉÉ/ 5
.
ÉÉ5 6
Map
ÉÉ6 9
<
ÉÉ9 :
List
ÉÉ: >
<
ÉÉ> ?"
CollectionLevel02DTO
ÉÉ? S
>
ÉÉS T
>
ÉÉT U
(
ÉÉU V
result
ÉÉV \
)
ÉÉ\ ]
}
ÑÑ 
;
ÑÑ 
return
ÜÜ 
new
ÜÜ 
GenericReturn
ÜÜ (
<
ÜÜ( )&
ResultSetRelatorioColeta
ÜÜ) A
>
ÜÜA B
(
ÜÜB C
	resultSet
ÜÜC L
)
ÜÜL M
;
ÜÜM N
}
àà 
catch
ââ 
(
ââ 
	Exception
ââ 
e
ââ 
)
ââ 
{
ää 
return
ãã 
new
ãã 
GenericReturn
ãã (
<
ãã( )&
ResultSetRelatorioColeta
ãã) A
>
ããA B
(
ããB C
e
ããC D
,
ããD E
$str
ããF x
)
ããx y
;
ããy z
}
åå 
}
çç 	
public
èè 
GenericReturn
èè 
<
èè &
ResultSetRelatorioColeta
èè 5
>
èè5 6

GetAllData
èè7 A
(
èèA B#
DataCarrierFormulario
èèB W
form
èèX \
)
èè\ ]
{
êê 	
try
ëë 
{
íí 
var
îî %
resultCollectionLevel02
îî +
=
îî, -$
_repoCollectionLevel02
îî. D
.
îîD E
	GetByDate
îîE N
(
îîN O
form
îîO S
)
îîS T
.
îîT U
ToList
îîU [
(
îî[ \
)
îî\ ]
;
îî] ^
var
ïï %
resultCollectionLevel03
ïï +
=
ïï, -$
_repoCollectionLevel03
ïï. D
.
ïïD E
	GetByDate
ïïE N
(
ïïN O
form
ïïO S
)
ïïS T
.
ïïT U
ToList
ïïU [
(
ïï[ \
)
ïï\ ]
;
ïï] ^
var
ññ (
resultConsolidationLevel01
ññ .
=
ññ/ 0'
_repoConsolidationLevel01
ññ1 J
.
ññJ K
	GetByDate
ññK T
(
ññT U
form
ññU Y
)
ññY Z
.
ññZ [
ToList
ññ[ a
(
ñña b
)
ññb c
;
ññc d
var
óó (
resultConsolidationLevel02
óó .
=
óó/ 0'
_repoConsolidationLevel02
óó1 J
.
óóJ K
	GetByDate
óóK T
(
óóT U
form
óóU Y
)
óóY Z
.
óóZ [
ToList
óó[ a
(
óóa b
)
óób c
;
óóc d
if
ôô 
(
ôô %
resultCollectionLevel03
ôô +
.
ôô+ ,
IsNull
ôô, 2
(
ôô2 3
)
ôô3 4
&&
ôô5 7%
resultCollectionLevel02
ôô8 O
.
ôôO P
IsNull
ôôP V
(
ôôV W
)
ôôW X
&&
ôôY [(
resultConsolidationLevel01
ôô\ v
.
ôôv w
IsNull
ôôw }
(
ôô} ~
)
ôô~ 
&&ôôÄ Ç*
resultConsolidationLevel02ôôÉ ù
.ôôù û
IsNullôôû §
(ôô§ •
)ôô• ¶
)ôô¶ ß
throw
öö 
new
öö 
ExceptionHelper
öö -
(
öö- .
$str
öö. ?
)
öö? @
;
öö@ A
var
úú 
	resultSet
úú 
=
úú 
new
úú  #&
ResultSetRelatorioColeta
úú$ <
(
úú< =
)
úú= >
{
ùù &
listCollectionLevel02DTO
ûû ,
=
ûû- .
Mapper
ûû/ 5
.
ûû5 6
Map
ûû6 9
<
ûû9 :
List
ûû: >
<
ûû> ?"
CollectionLevel02DTO
ûû? S
>
ûûS T
>
ûûT U
(
ûûU V%
resultCollectionLevel02
ûûV m
)
ûûm n
,
ûûn o&
listCollectionLevel03DTO
üü ,
=
üü- .
Mapper
üü/ 5
.
üü5 6
Map
üü6 9
<
üü9 :
List
üü: >
<
üü> ?"
CollectionLevel03DTO
üü? S
>
üüS T
>
üüT U
(
üüU V%
resultCollectionLevel03
üüV m
)
üüm n
,
üün o&
listConsolidationLevel01
†† ,
=
††- .
Mapper
††/ 5
.
††5 6
Map
††6 9
<
††9 :
List
††: >
<
††> ?%
ConsolidationLevel01DTO
††? V
>
††V W
>
††W X
(
††X Y(
resultConsolidationLevel01
††Y s
)
††s t
,
††t u&
listConsolidationLevel02
°° ,
=
°°- .
Mapper
°°/ 5
.
°°5 6
Map
°°6 9
<
°°9 :
List
°°: >
<
°°> ?%
ConsolidationLevel02DTO
°°? V
>
°°V W
>
°°W X
(
°°X Y(
resultConsolidationLevel02
°°Y s
)
°°s t
,
°°t u
}
¢¢ 
;
¢¢ 
return
§§ 
new
§§ 
GenericReturn
§§ (
<
§§( )&
ResultSetRelatorioColeta
§§) A
>
§§A B
(
§§B C
	resultSet
§§C L
)
§§L M
;
§§M N
}
¶¶ 
catch
ßß 
(
ßß 
	Exception
ßß 
e
ßß 
)
ßß 
{
®® 
return
©© 
new
©© 
GenericReturn
©© (
<
©©( )&
ResultSetRelatorioColeta
©©) A
>
©©A B
(
©©B C
e
©©C D
,
©©D E
$str
©©F x
)
©©x y
;
©©y z
}
™™ 
}
´´ 	
public
ØØ 
GenericReturn
ØØ 
<
ØØ 

GetSyncDTO
ØØ '
>
ØØ' (
GetEntryByDate
ØØ) 7
(
ØØ7 8#
DataCarrierFormulario
ØØ8 M
form
ØØN R
)
ØØR S
{
∞∞ 	
try
±± 
{
≤≤ 
var
µµ $
consildatedLelve01List
µµ *
=
µµ+ ,'
_repoConsolidationLevel01
µµ- F
.
µµF G5
'GetEntryConsildatedLevel01ByDateAndUnit
µµG n
(
µµn o
form
µµo s
)
µµs t
.
µµt u
ToList
µµu {
(
µµ{ |
)
µµ| }
;
µµ} ~
var
∏∏ (
consildatedLelve01ListDTO1
∏∏ .
=
∏∏/ 0
Mapper
∏∏1 7
.
∏∏7 8
Map
∏∏8 ;
<
∏∏; <
List
∏∏< @
<
∏∏@ A%
ConsolidationLevel01DTO
∏∏A X
>
∏∏X Y
>
∏∏Y Z
(
∏∏Z [$
consildatedLelve01List
∏∏[ q
)
∏∏q r
;
∏∏r s
var
ªª 
processador
ªª 
=
ªª  !
new
ªª" %&
TableResultsForDataTable
ªª& >
(
ªª> ?
)
ªª? @
;
ªª@ A
var
ºº 4
&resultadosProcessadosParaFormatoTabela
ºº :
=
ºº; <
processador
ºº= H
.
ººH I3
%DataCollectionReportsProcessedResults
ººI n
(
ººn o)
consildatedLelve01ListDTO1ººo â
)ººâ ä
;ººä ã
return
øø 
new
øø 
GenericReturn
øø (
<
øø( )

GetSyncDTO
øø) 3
>
øø3 4
(
øø4 5
new
øø5 8

GetSyncDTO
øø9 C
(
øøC D
)
øøD E
{
¿¿ "
ConsolidationLevel01
¡¡ (
=
¡¡) *4
&resultadosProcessadosParaFormatoTabela
¡¡+ Q
}
¬¬ 
)
¬¬ 
;
¬¬ 
}
ƒƒ 
catch
≈≈ 
(
≈≈ 
	Exception
≈≈ 
e
≈≈ 
)
≈≈ 
{
∆∆ 
return
«« 
new
«« 
GenericReturn
«« (
<
««( )

GetSyncDTO
««) 3
>
««3 4
(
««4 5
e
««5 6
,
««6 7
$str
««8 J
)
««J K
;
««K L
}
»» 
}
…… 	
}
ÀÀ 
}ÃÃ ÉF
KC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Services\BaseDomain.cs
	namespace 	
Dominio
 
. 
Services 
{		 
public

 

class

 

BaseDomain

 
<

 
T

 
,

 
Y

  
>

  !
:

" #
IDisposable

$ /
,

/ 0
IBaseDomain

1 <
<

< =
T

= >
,

> ?
Y

@ A
>

A B
where

C H
T

I J
:

K L
class

M R
where

S X
Y

Y Z
:

[ \
class

] b
{ 
private 
readonly 
IBaseRepository (
<( )
T) *
>* +
_repositoryBase, ;
;; <
private 
readonly %
IBaseRepositoryNoLazyLoad 2
<2 3
T3 4
>4 5
_repositoryBaseNll6 H
;H I
private 
string 

inseridoOk !
{" #
get$ '
{( )
return* 0
$str1 K
;K L
}M N
}O P
private 
string 

AlteradoOk !
{" #
get$ '
{( )
return* 0
$str1 M
;M N
}O P
}Q R
private 
string 
NaoInserido "
{# $
get% (
{) *
return+ 1
$str2 W
;W X
}Y Z
}[ \
private 
string 
emptyObj 
{  !
get" %
{& '
return( .
$str/ f
;f g
}h i
}j k
public 

BaseDomain 
( 
IBaseRepository )
<) *
T* +
>+ ,
repositoryBase- ;
,; <%
IBaseRepositoryNoLazyLoad= V
<V W
TW X
>X Y
repositoryBaseNllZ k
)k l
{ 	
_repositoryBase 
= 
repositoryBase ,
;, -
_repositoryBaseNll 
=  
repositoryBaseNll! 2
;2 3
} 	
public 
Y 
GetById 
( 
int 
id 
)  
{ 	
var 
result 
= 
_repositoryBase '
.' (
GetById( /
(/ 0
id0 2
)2 3
;3 4
return 
Mapper 
. 
Map 
< 
Y 
> 
(  
result  &
)& '
;' (
} 	
public!! 
IEnumerable!! 
<!! 
Y!! 
>!! 
GetAll!! $
(!!$ %
)!!% &
{"" 	
var## 
result## 
=## 
Mapper## 
.##  
Map##  #
<### $
IEnumerable##$ /
<##/ 0
Y##0 1
>##1 2
>##2 3
(##3 4
_repositoryBase##4 C
.##C D
GetAll##D J
(##J K
)##K L
)##L M
;##M N
return$$ 
result$$ 
;$$ 
}%% 	
public'' 
void'' 
Dispose'' 
('' 
)'' 
{(( 	
_repositoryBase)) 
.)) 
Dispose)) #
())# $
)))$ %
;))% &
}** 	
public-- 
Y-- 
First-- 
(-- 
)-- 
{.. 	
var// 
result// 
=// 
_repositoryBase// '
.//' (
First//( -
(//- .
)//. /
;/// 0
return00 
Mapper00 
.00 
Map00 
<00 
Y00 
>00 
(00  
result00  &
)00& '
;00' (
}11 	
public33 
Y33 
GetByIdNoLazyLoad33 "
(33" #
int33# &
id33' )
)33) *
{44 	
var55 
result55 
=55 
_repositoryBaseNll55 +
.55+ ,
GetById55, 3
(553 4
id554 6
)556 7
;557 8
return66 
Mapper66 
.66 
Map66 
<66 
Y66 
>66  
(66  !
result66! '
)66' (
;66( )
}77 	
public99 
IEnumerable99 
<99 
Y99 
>99 
GetAllNoLazyLoad99 .
(99. /
)99/ 0
{:: 	
var;; 
result;; 
=;; 
Mapper;; 
.;;  
Map;;  #
<;;# $
IEnumerable;;$ /
<;;/ 0
Y;;0 1
>;;1 2
>;;2 3
(;;3 4
_repositoryBaseNll;;4 F
.;;F G
GetAll;;G M
(;;M N
);;N O
);;O P
;;;P Q
return<< 
result<< 
;<< 
}== 	
public?? 
Y?? 
FirstNoLazyLoad??  
(??  !
)??! "
{@@ 	
varAA 
resultAA 
=AA 
_repositoryBaseNllAA +
.AA+ ,
FirstAA, 1
(AA1 2
)AA2 3
;AA3 4
returnBB 
MapperBB 
.BB 
MapBB 
<BB 
YBB 
>BB  
(BB  !
resultBB! '
)BB' (
;BB( )
}CC 	
publicEE 
YEE 
AddOrUpdateEE 
(EE 
YEE 
objEE "
)EE" #
{FF 	
tryGG 
{HH 
varII 
saveObjII 
=II 
MapperII $
.II$ %
MapII% (
<II( )
TII) *
>II* +
(II+ ,
objII, /
)II/ 0
;II0 1
ifNN 
(NN 
saveObjNN 
.NN 
GetTypeNN #
(NN# $
)NN$ %
.NN% &
GetPropertyNN& 1
(NN1 2
$strNN2 6
)NN6 7
!=NN8 :
nullNN; ?
)NN? @
{OO 
_repositoryBasePP #
.PP# $
AddOrUpdatePP$ /
(PP/ 0
saveObjPP0 7
)PP7 8
;PP8 9
returnQQ 
MapperQQ !
.QQ! "
MapQQ" %
<QQ% &
YQQ& '
>QQ' (
(QQ( )
saveObjQQ) 0
)QQ0 1
;QQ1 2
}RR 
elseSS 
{TT 
throwUU 
newUU 
ExceptionHelperUU -
(UU- .
$strUU. O
)UUO P
;UUP Q
}VV 
}WW 
catchXX 
(XX 
ExceptionHelperXX "
exXX# %
)XX% &
{YY 
throwZZ 
newZZ 
ExceptionHelperZZ )
(ZZ) *
$strZZ* G
,ZZG H
exZZI K
)ZZK L
;ZZL M
}[[ 
}\\ 	
public^^ 
Y^^ 
AddOrUpdate^^ 
(^^ 
Y^^ 
obj^^ "
,^^" #
bool^^$ (
useTransaction^^) 7
)^^7 8
{__ 	
try`` 
{aa 
varbb 
saveObjbb 
=bb 
Mapperbb $
.bb$ %
Mapbb% (
<bb( )
Tbb) *
>bb* +
(bb+ ,
objbb, /
)bb/ 0
;bb0 1
ifcc 
(cc 
saveObjcc 
.cc 
GetTypecc #
(cc# $
)cc$ %
.cc% &
GetPropertycc& 1
(cc1 2
$strcc2 6
)cc6 7
!=cc8 :
nullcc; ?
)cc? @
{dd 
_repositoryBaseee #
.ee# $
AddOrUpdateee$ /
(ee/ 0
saveObjee0 7
,ee7 8
useTransactionee9 G
)eeG H
;eeH I
returnff 
Mapperff !
.ff! "
Mapff" %
<ff% &
Yff& '
>ff' (
(ff( )
saveObjff) 0
)ff0 1
;ff1 2
}gg 
elsehh 
{ii 
throwjj 
newjj 
ExceptionHelperjj -
(jj- .
$strjj. O
)jjO P
;jjP Q
}kk 
}ll 
catchmm 
(mm 
ExceptionHelpermm "
exmm# %
)mm% &
{nn 
throwoo 
newoo 
ExceptionHelperoo )
(oo) *
$stroo* G
,ooG H
exooI K
)ooK L
;ooL M
}pp 
}qq 	
publicss 
intss 

ExecuteSqlss 
(ss 
stringss $
vss% &
)ss& '
{tt 	
returnuu 
_repositoryBaseuu "
.uu" #

ExecuteSqluu# -
(uu- .
vuu. /
)uu/ 0
;uu0 1
}vv 	
}
ƒƒ 
}∆∆  »
KC:\Users\Note-pc\Source\Repos\ddd.bitbucket2\Dominio\Services\UserDomain.cs
	namespace 	
Dominio
 
. 
Services 
{ 
public 

class 

UserDomain 
: 
IUserDomain )
{ 
public 
static 
class 
	mensagens %
{ 	
public 
static 
string  
naoEncontrado! .
{ 
get 
{ 
if 
( 
GlobalConfig $
.$ %
LanguageBrasil% 3
)3 4
return 
$str j
;j k
else 
return 
$str d
;d e
} 
} 
public   
static   
string    

falhaGeral  ! +
{!! 
get"" 
{## 
if$$ 
($$ 
GlobalConfig$$ $
.$$$ %
LanguageBrasil$$% 3
)$$3 4
return%% 
$str%% P
;%%P Q
else&& 
return'' 
$str'' G
;''G H
}(( 
})) 
public++ 
static++ 
string++  
erroUnidade++! ,
{,, 
get-- 
{.. 
if// 
(// 
GlobalConfig// $
.//$ %
LanguageBrasil//% 3
)//3 4
return00 
$str00 ]
;00] ^
else11 
return22 
$str22 u
;22u v
}33 
}44 
}55 	
private77 
readonly77 
IUserRepository77 (
	_userRepo77) 2
;772 3
private88 
readonly88 
IBaseRepository88 (
<88( )
ParCompanyXUserSgq88) ;
>88; <#
_baseParCompanyXUserSgq88= T
;88T U
private99 
readonly99 
IBaseRepository99 (
<99( )

ParCompany99) 3
>993 4
_baseParCompany995 D
;99D E
private:: 
SgqDbDevEntities::  
db::! #
;::# $
private;; 
static;; 
string;; 
dominio;; %
=;;& '
$str;;( :
;;;: ;
publicDD 

UserDomainDD 
(DD 
IUserRepositoryDD )
userRepoDD* 2
,EE 
IBaseRepositoryEE 
<EE 
ParCompanyXUserSgqEE 0
>EE0 1"
baseParCompanyXUserSgqEE2 H
,FF 
IBaseRepositoryFF 
<FF 

ParCompanyFF (
>FF( )
baseParCompanyFF* 8
)FF8 9
{GG 	
_baseParCompanyHH 
=HH 
baseParCompanyHH ,
;HH, -#
_baseParCompanyXUserSgqII #
=II$ %"
baseParCompanyXUserSgqII& <
;II< =
	_userRepoJJ 
=JJ 
userRepoJJ  
;JJ  !
dbKK 
=KK 
newKK 
SgqDbDevEntitiesKK %
(KK% &
)KK& '
;KK' (
}LL 	
publicVV 
GenericReturnVV 
<VV 
UserDTOVV $
>VV$ %
AuthenticationLoginVV& 9
(VV9 :
UserDTOVV: A
userDtoVVB I
)VVI J
{WW 	
tryYY 
{ZZ 
UserSgq[[ 

userByName[[ "
;[[" #
UserSgq\\ 
isUser\\ 
=\\  
null\\! %
;\\% &
if]] 
(]] 
userDto]] 
.]] 
IsNull]] "
(]]" #
)]]# $
)]]$ %
throw^^ 
new^^ 
ExceptionHelper^^ -
(^^- .
$str^^. S
)^^S T
;^^T U
userDtoaa 
.aa 
ValidaObjetoUserDTOaa +
(aa+ ,
)aa, -
;aa- .

userByNamedd 
=dd 
	_userRepodd &
.dd& '
	GetByNamedd' 0
(dd0 1
userDtodd1 8
.dd8 9
Namedd9 =
)dd= >
;dd> ?
ifff 
(ff 
!ff 
userDtoff 
.ff 
IsWebff !
)ff! "
DescriptografaSenhagg '
(gg' (
userDtogg( /
)gg/ 0
;gg0 1
ifkk 
(kk 
GlobalConfigkk  
.kk  !
Brasilkk! '
)kk' (
isUserll 
=ll 
LoginBrasilll (
(ll( )
userDtoll) 0
,ll0 1

userByNamell2 <
)ll< =
;ll= >
ifoo 
(oo 
GlobalConfigoo  
.oo  !
Euaoo! $
)oo$ %
isUserpp 
=pp 
LoginEUApp %
(pp% &
userDtopp& -
,pp- .

userByNamepp/ 9
)pp9 :
;pp: ;
ifss 
(ss 
GlobalConfigss  
.ss  !
Ytoarass! '
)ss' (
isUsertt 
=tt 
LoginSgqtt %
(tt% &
userDtott& -
,tt- .

userByNamett/ 9
)tt9 :
;tt: ;
ifvv 
(vv 
isUservv 
.vv 
IsNullvv !
(vv! "
)vv" #
)vv# $
throwww 
newww 
ExceptionHelperww -
(ww- .
	mensagensww. 7
.ww7 8
naoEncontradoww8 E
)wwE F
;wwF G
ifzz 
(zz 
isUserzz 
.zz 
ParCompany_Idzz (
==zz) +
nullzz, 0
)zz0 1
throw{{ 
new{{ 
	Exception{{ '
({{' (
	mensagens{{( 1
.{{1 2
erroUnidade{{2 =
){{= >
;{{> ?
if|| 
(|| 
isUser|| 
.|| 
ParCompany_Id|| (
<=||) +
$num||, -
)||- .
throw}} 
new}} 
	Exception}} '
(}}' (
	mensagens}}( 1
.}}1 2
erroUnidade}}2 =
)}}= >
;}}> ?
var
ÄÄ 
defaultCompany
ÄÄ "
=
ÄÄ# $%
_baseParCompanyXUserSgq
ÄÄ% <
.
ÄÄ< =
GetAll
ÄÄ= C
(
ÄÄC D
)
ÄÄD E
.
ÄÄE F
FirstOrDefault
ÄÄF T
(
ÄÄT U
r
ÅÅ 
=>
ÅÅ 
r
ÅÅ 
.
ÅÅ 

UserSgq_Id
ÅÅ %
==
ÅÅ& (
isUser
ÅÅ) /
.
ÅÅ/ 0
Id
ÅÅ0 2
&&
ÅÅ3 5
r
ÅÅ6 7
.
ÅÅ7 8
ParCompany_Id
ÅÅ8 E
==
ÅÅF H
isUser
ÅÅI O
.
ÅÅO P
ParCompany_Id
ÅÅP ]
)
ÅÅ] ^
;
ÅÅ^ _
if
ÉÉ 
(
ÉÉ 
defaultCompany
ÉÉ "
==
ÉÉ# %
null
ÉÉ& *
)
ÉÉ* +
{
ÑÑ 
defaultCompany
ÖÖ "
=
ÖÖ# $%
_baseParCompanyXUserSgq
ÖÖ% <
.
ÖÖ< =
GetAll
ÖÖ= C
(
ÖÖC D
)
ÖÖD E
.
ÖÖE F
FirstOrDefault
ÖÖF T
(
ÖÖT U
r
ÜÜ 
=>
ÜÜ 
r
ÜÜ 
.
ÜÜ 

UserSgq_Id
ÜÜ %
==
ÜÜ& (
isUser
ÜÜ) /
.
ÜÜ/ 0
Id
ÜÜ0 2
)
ÜÜ2 3
;
ÜÜ3 4
using
ââ 
(
ââ 
var
ââ 
db
ââ !
=
ââ" #
new
ââ$ '
SgqDbDevEntities
ââ( 8
(
ââ8 9
)
ââ9 :
)
ââ: ;
{
ää 
var
ãã 
atualizarUsuario
ãã ,
=
ãã- .
db
ãã/ 1
.
ãã1 2
UserSgq
ãã2 9
.
ãã9 :
FirstOrDefault
ãã: H
(
ããH I
r
ããI J
=>
ããJ L
r
ããM N
.
ããN O
Id
ããO Q
==
ããR T
isUser
ããU [
.
ãã[ \
Id
ãã\ ^
)
ãã^ _
;
ãã_ `
atualizarUsuario
åå (
.
åå( )
ParCompany_Id
åå) 6
=
åå7 8
defaultCompany
åå9 G
.
ååG H
ParCompany_Id
ååH U
;
ååU V
db
çç 
.
çç 
UserSgq
çç "
.
çç" #
Attach
çç# )
(
çç) *
atualizarUsuario
çç* :
)
çç: ;
;
çç; <
db
éé 
.
éé 
Entry
éé  
(
éé  !
atualizarUsuario
éé! 1
)
éé1 2
.
éé2 3
State
éé3 8
=
éé9 :
System
éé; A
.
ééA B
Data
ééB F
.
ééF G
Entity
ééG M
.
ééM N
EntityState
ééN Y
.
ééY Z
Modified
ééZ b
;
ééb c
db
èè 
.
èè 
SaveChanges
èè &
(
èè& '
)
èè' (
;
èè( )
}
êê 
}
ìì 
return
ïï 
new
ïï 
GenericReturn
ïï (
<
ïï( )
UserDTO
ïï) 0
>
ïï0 1
(
ïï1 2
Mapper
ïï2 8
.
ïï8 9
Map
ïï9 <
<
ïï< =
UserSgq
ïï= D
,
ïïD E
UserDTO
ïïF M
>
ïïM N
(
ïïN O
isUser
ïïO U
)
ïïU V
)
ïïV W
;
ïïW X
}
ññ 
catch
óó 
(
óó 
	Exception
óó 
e
óó 
)
óó 
{
òò 
new
ôô 
	CreateLog
ôô 
(
ôô 
e
ôô 
)
ôô  
;
ôô  !
return
öö 
new
öö 
GenericReturn
öö (
<
öö( )
UserDTO
öö) 0
>
öö0 1
(
öö1 2
e
öö2 3
,
öö3 4
e
öö5 6
.
öö6 7
Message
öö7 >
)
öö> ?
;
öö? @
}
õõ 
}
ùù 	
private
££ 
void
££ !
DescriptografaSenha
££ (
(
££( )
UserDTO
££) 0
userDto
££1 8
)
££8 9
{
§§ 	
userDto
•• 
.
•• 
Password
•• 
=
•• 
Guard
••  %
.
••% &
DecryptStringAES
••& 6
(
••6 7
userDto
••7 >
.
••> ?
Password
••? G
)
••G H
;
••H I
}
¶¶ 	
private
ÆÆ 
UserSgq
ÆÆ &
CheckUserAndPassDataBase
ÆÆ 0
(
ÆÆ0 1
UserDTO
ÆÆ1 8
userDto
ÆÆ9 @
)
ÆÆ@ A
{
ØØ 	
var
±± 
user
±± 
=
±± 
Mapper
±± 
.
±± 
Map
±± !
<
±±! "
UserDTO
±±" )
,
±±) *
UserSgq
±±+ 2
>
±±2 3
(
±±3 4
userDto
±±4 ;
)
±±; <
;
±±< =
var
≤≤ 
isUser
≤≤ 
=
≤≤ 
	_userRepo
≤≤ "
.
≤≤" #!
AuthenticationLogin
≤≤# 6
(
≤≤6 7
user
≤≤7 ;
)
≤≤; <
;
≤≤< =
return
≥≥ 
isUser
≥≥ 
;
≥≥ 
}
¥¥ 	
public
∂∂ 
List
∂∂ 
<
∂∂ 
UserDTO
∂∂ 
>
∂∂ 
GetAllUserByUnit
∂∂ -
(
∂∂- .
int
∂∂. 1
	unidadeId
∂∂2 ;
)
∂∂; <
{
∑∑ 	
return
∏∏ 
Mapper
∏∏ 
.
∏∏ 
Map
∏∏ 
<
∏∏ 
List
∏∏ "
<
∏∏" #
UserDTO
∏∏# *
>
∏∏* +
>
∏∏+ ,
(
∏∏, -
	_userRepo
∏∏- 6
.
∏∏6 7
GetAllUserByUnit
∏∏7 G
(
∏∏G H
	unidadeId
∏∏H Q
)
∏∏Q R
)
∏∏R S
;
∏∏S T
}
ππ 	
public
¿¿ 
GenericReturn
¿¿ 
<
¿¿ 
UserDTO
¿¿ $
>
¿¿$ %
	GetByName
¿¿& /
(
¿¿/ 0
string
¿¿0 6
username
¿¿7 ?
)
¿¿? @
{
¡¡ 	
try
¬¬ 
{
√√ 
var
ƒƒ 
queryResult
ƒƒ 
=
ƒƒ  !
	_userRepo
ƒƒ" +
.
ƒƒ+ ,
	GetByName
ƒƒ, 5
(
ƒƒ5 6
username
ƒƒ6 >
)
ƒƒ> ?
;
ƒƒ? @
return
≈≈ 
new
≈≈ 
GenericReturn
≈≈ (
<
≈≈( )
UserDTO
≈≈) 0
>
≈≈0 1
(
≈≈1 2
Mapper
≈≈2 8
.
≈≈8 9
Map
≈≈9 <
<
≈≈< =
UserSgq
≈≈= D
,
≈≈D E
UserDTO
≈≈F M
>
≈≈M N
(
≈≈N O
queryResult
≈≈O Z
)
≈≈Z [
)
≈≈[ \
;
≈≈\ ]
}
∆∆ 
catch
«« 
(
«« 
	Exception
«« 
e
«« 
)
«« 
{
»» 
return
…… 
new
…… 
GenericReturn
…… (
<
……( )
UserDTO
……) 0
>
……0 1
(
……1 2
e
……2 3
,
……3 4
$str
……5 O
)
……O P
;
……P Q
}
   
}
ÀÀ 	
public
““ 
GenericReturn
““ 
<
““ 

UserSgqDTO
““ '
>
““' (

GetByName2
““) 3
(
““3 4
string
““4 :
username
““; C
)
““C D
{
”” 	
try
‘‘ 
{
’’ 
var
÷÷ 
queryResult
÷÷ 
=
÷÷  !
	_userRepo
÷÷" +
.
÷÷+ ,
	GetByName
÷÷, 5
(
÷÷5 6
username
÷÷6 >
)
÷÷> ?
;
÷÷? @
return
◊◊ 
new
◊◊ 
GenericReturn
◊◊ (
<
◊◊( )

UserSgqDTO
◊◊) 3
>
◊◊3 4
(
◊◊4 5
Mapper
◊◊5 ;
.
◊◊; <
Map
◊◊< ?
<
◊◊? @
UserSgq
◊◊@ G
,
◊◊G H

UserSgqDTO
◊◊I S
>
◊◊S T
(
◊◊T U
queryResult
◊◊U `
)
◊◊` a
)
◊◊a b
;
◊◊b c
}
ÿÿ 
catch
ŸŸ 
(
ŸŸ 
	Exception
ŸŸ 
e
ŸŸ 
)
ŸŸ 
{
⁄⁄ 
return
€€ 
new
€€ 
GenericReturn
€€ (
<
€€( )

UserSgqDTO
€€) 3
>
€€3 4
(
€€4 5
e
€€5 6
,
€€6 7
$str
€€8 R
)
€€R S
;
€€S T
}
‹‹ 
}
›› 	
public
·· 
UserSgq
·· 
LoginSgq
·· 
(
··  
UserDTO
··  '
userDto
··( /
,
··/ 0
UserSgq
··1 8

userByName
··9 C
)
··C D
{
‚‚ 	
return
ÓÓ &
CheckUserAndPassDataBase
ÓÓ +
(
ÓÓ+ ,
userDto
ÓÓ, 3
)
ÓÓ3 4
;
ÓÓ4 5
}
 	
private
ÄÄ 
UserSgq
ÄÄ 
LoginEUA
ÄÄ  
(
ÄÄ  !
UserDTO
ÄÄ! (
userDto
ÄÄ) 0
,
ÄÄ0 1
UserSgq
ÄÄ2 9

userByName
ÄÄ: D
)
ÄÄD E
{
ÅÅ 	
if
çç 
(
çç 
GlobalConfig
çç 
.
çç 
mockLoginEUA
çç )
)
çç) *
{
éé 
UserSgq
èè 
userDev
èè 
=
èè  !&
CheckUserAndPassDataBase
èè" :
(
èè: ;
userDto
èè; B
)
èèB C
;
èèC D
return
êê 
userDev
êê 
;
êê 
}
ëë 
if
ìì 
(
ìì 

userByName
ìì 
!=
ìì 
null
ìì "
)
ìì" #
{
îî 
var
ïï 
IsActive
ïï 
=
ïï 
db
ïï !
.
ïï! "
Database
ïï" *
.
ïï* +
SqlQuery
ïï+ 3
<
ïï3 4
bool
ïï4 8
>
ïï8 9
(
ïï9 :
$str
ïï: d
+
ïïe f

userByName
ïïg q
.
ïïq r
Id
ïïr t
)
ïït u
.
ïïu v
FirstOrDefaultïïv Ñ
(ïïÑ Ö
)ïïÖ Ü
;ïïÜ á
if
ññ 
(
ññ 
!
ññ 
IsActive
ññ 
)
ññ 
throw
óó 
new
óó 
	Exception
óó '
(
óó' (
$str
óó( 8
)
óó8 9
;
óó9 :
}
òò 
if
õõ 
(
õõ 
CheckUserInAD
õõ 
(
õõ 
dominio
õõ %
,
õõ% &
userDto
õõ' .
.
õõ. /
Name
õõ/ 3
,
õõ3 4
userDto
õõ5 <
.
õõ< =
Password
õõ= E
)
õõE F
)
õõF G
{
úú 
UserSgq
†† 
isUser
†† 
=
††  &
CheckUserAndPassDataBase
††! 9
(
††9 :
userDto
††: A
)
††A B
;
††B C
if
££ 
(
££ 

userByName
££ 
.
££ 
	IsNotNull
££ (
(
££( )
)
££) *
&&
££+ -
isUser
££. 4
.
££4 5
IsNull
££5 ;
(
££; <
)
££< =
)
££= >
{
§§ 
isUser
•• 
=
•• %
AlteraSenhaAlteradaNoAd
•• 4
(
••4 5
userDto
••5 <
,
••< =

userByName
••> H
)
••H I
;
••I J
if
¶¶ 
(
¶¶ 
isUser
¶¶ 
.
¶¶ 
IsNull
¶¶ %
(
¶¶% &
)
¶¶& '
)
¶¶' (
throw
ßß 
new
ßß !
	Exception
ßß" +
(
ßß+ ,
$str
ßß, R
)
ßßR S
;
ßßS T
}
®® 
return
ÆÆ 
isUser
ÆÆ 
;
ÆÆ 
}
∞∞ 
return
≤≤ 
null
≤≤ 
;
≤≤ 
}
≥≥ 	
private
µµ 
UserSgq
µµ %
AlteraSenhaAlteradaNoAd
µµ /
(
µµ/ 0
UserDTO
µµ0 7
userDto
µµ8 ?
,
µµ? @
UserSgq
µµA H

userByName
µµI S
)
µµS T
{
∂∂ 	
UserSgq
∑∑ 
isUser
∑∑ 
;
∑∑ 

userByName
∏∏ 
.
∏∏ 
Password
∏∏ 
=
∏∏  !
Guard
∏∏" '
.
∏∏' (
EncryptStringAES
∏∏( 8
(
∏∏8 9
userDto
∏∏9 @
.
∏∏@ A
Password
∏∏A I
)
∏∏I J
;
∏∏J K
	_userRepo
ππ 
.
ππ 
Salvar
ππ 
(
ππ 

userByName
ππ '
)
ππ' (
;
ππ( )
isUser
ªª 
=
ªª &
CheckUserAndPassDataBase
ªª -
(
ªª- .
userDto
ªª. 5
)
ªª5 6
;
ªª6 7
return
ºº 
isUser
ºº 
;
ºº 
}
ΩΩ 	
private
øø 
UserSgq
øø 
CreateUserFromAd
øø (
(
øø( )
UserDTO
øø) 0
userDto
øø1 8
)
øø8 9
{
¿¿ 	
userDto
¡¡ 
.
¡¡ 
ParCompany_Id
¡¡ !
=
¡¡" #
_baseParCompany
¡¡$ 3
.
¡¡3 4
First
¡¡4 9
(
¡¡9 :
)
¡¡: ;
.
¡¡; <
Id
¡¡< >
;
¡¡> ?
userDto
¬¬ 
.
¬¬ 
FullName
¬¬ 
=
¬¬ 
userDto
¬¬ &
.
¬¬& '
Name
¬¬' +
;
¬¬+ ,
userDto
√√ 
.
√√ 
PasswordDate
√√  
=
√√! "
DateTime
√√# +
.
√√+ ,
Now
√√, /
;
√√/ 0
userDto
ƒƒ 
.
ƒƒ !
ValidaObjetoUserDTO
ƒƒ '
(
ƒƒ' (
)
ƒƒ( )
;
ƒƒ) *
var
≈≈ 
newUser
≈≈ 
=
≈≈ 
Mapper
≈≈  
.
≈≈  !
Map
≈≈! $
<
≈≈$ %
UserSgq
≈≈% ,
>
≈≈, -
(
≈≈- .
userDto
≈≈. 5
)
≈≈5 6
;
≈≈6 7
	_userRepo
∆∆ 
.
∆∆ 
Salvar
∆∆ 
(
∆∆ 
newUser
∆∆ $
)
∆∆$ %
;
∆∆% &
return
«« 
newUser
«« 
;
«« 
}
»» 	
public
œœ 
GenericReturn
œœ 
<
œœ 
List
œœ !
<
œœ! "
UserDTO
œœ" )
>
œœ) *
>
œœ* +$
GetAllUserValidationAd
œœ, B
(
œœB C
UserDTO
œœC J
userDto
œœK R
)
œœR S
{
–– 	
try
—— 
{
““ !
AuthenticationLogin
‘‘ #
(
‘‘# $
userDto
‘‘$ +
)
‘‘+ ,
;
‘‘, -
var
÷÷ 
retorno
÷÷ 
=
÷÷ 
Mapper
÷÷ $
.
÷÷$ %
Map
÷÷% (
<
÷÷( )
List
÷÷) -
<
÷÷- .
UserSgq
÷÷. 5
>
÷÷5 6
,
÷÷6 7
List
÷÷8 <
<
÷÷< =
UserDTO
÷÷= D
>
÷÷D E
>
÷÷E F
(
÷÷F G
	_userRepo
÷÷G P
.
÷÷P Q

GetAllUser
÷÷Q [
(
÷÷[ \
)
÷÷\ ]
)
÷÷] ^
;
÷÷^ _
foreach
ÿÿ 
(
ÿÿ 
var
ÿÿ 
i
ÿÿ 
in
ÿÿ !
retorno
ÿÿ" )
)
ÿÿ) *
{
ŸŸ 
if
⁄⁄ 
(
⁄⁄ 
!
⁄⁄ 
string
⁄⁄ 
.
⁄⁄  
IsNullOrEmpty
⁄⁄  -
(
⁄⁄- .
i
⁄⁄. /
.
⁄⁄/ 0
Password
⁄⁄0 8
)
⁄⁄8 9
)
⁄⁄9 :
{
€€ 
var
‹‹ 
decript
‹‹ #
=
‹‹$ %
Guard
‹‹& +
.
‹‹+ ,
DecryptStringAES
‹‹, <
(
‹‹< =
i
‹‹= >
.
‹‹> ?
Password
‹‹? G
)
‹‹G H
;
‹‹H I
if
›› 
(
›› 
i
›› 
.
›› 
Password
›› &
.
››& '
Equals
››' -
(
››- .
decript
››. 5
)
››5 6
)
››6 7
Guard
ﬁﬁ !
.
ﬁﬁ! "
EncryptStringAES
ﬁﬁ" 2
(
ﬁﬁ2 3
i
ﬁﬁ3 4
.
ﬁﬁ4 5
Password
ﬁﬁ5 =
)
ﬁﬁ= >
;
ﬁﬁ> ?
}
‡‡ 
}
·· 
return
„„ 
new
„„ 
GenericReturn
„„ (
<
„„( )
List
„„) -
<
„„- .
UserDTO
„„. 5
>
„„5 6
>
„„6 7
(
„„7 8
retorno
„„8 ?
)
„„? @
;
„„@ A
}
‰‰ 
catch
ÂÂ 
(
ÂÂ 
	Exception
ÂÂ 
e
ÂÂ 
)
ÂÂ 
{
ÊÊ 
return
ÁÁ 
new
ÁÁ 
GenericReturn
ÁÁ (
<
ÁÁ( )
List
ÁÁ) -
<
ÁÁ- .
UserDTO
ÁÁ. 5
>
ÁÁ5 6
>
ÁÁ6 7
(
ÁÁ7 8
e
ÁÁ8 9
,
ÁÁ9 :
	mensagens
ÁÁ; D
.
ÁÁD E

falhaGeral
ÁÁE O
)
ÁÁO P
;
ÁÁP Q
}
ËË 
}
ÈÈ 	
public
ÎÎ 
static
ÎÎ 
bool
ÎÎ 
CheckUserInAD
ÎÎ (
(
ÎÎ( )
string
ÎÎ) /
domain
ÎÎ0 6
,
ÎÎ6 7
string
ÎÎ8 >
username
ÎÎ? G
,
ÎÎG H
string
ÎÎI O
password
ÎÎP X
,
ÎÎX Y
string
ÎÎZ `
userVerific
ÎÎa l
)
ÎÎl m
{
ÏÏ 	
try
ÌÌ 
{
ÓÓ 
using
ÔÔ 
(
ÔÔ 
var
ÔÔ 
domainContext
ÔÔ (
=
ÔÔ) *
new
ÔÔ+ .
PrincipalContext
ÔÔ/ ?
(
ÔÔ? @
ContextType
ÔÔ@ K
.
ÔÔK L
Domain
ÔÔL R
,
ÔÔR S
domain
ÔÔT Z
,
ÔÔZ [
username
ÔÔ\ d
,
ÔÔd e
password
ÔÔf n
)
ÔÔn o
)
ÔÔo p
{
 
using
ÒÒ 
(
ÒÒ 
var
ÒÒ 
user
ÒÒ #
=
ÒÒ$ %
new
ÒÒ& )
UserPrincipal
ÒÒ* 7
(
ÒÒ7 8
domainContext
ÒÒ8 E
)
ÒÒE F
)
ÒÒF G
{
ÚÚ 
user
ÛÛ 
.
ÛÛ 
SamAccountName
ÛÛ +
=
ÛÛ, -
userVerific
ÛÛ. 9
;
ÛÛ9 :
using
ıı 
(
ıı 
var
ıı "
pS
ıı# %
=
ıı& '
new
ıı( +
PrincipalSearcher
ıı, =
(
ıı= >
)
ıı> ?
)
ıı? @
{
ˆˆ 
pS
˜˜ 
.
˜˜ 
QueryFilter
˜˜ *
=
˜˜+ ,
user
˜˜- 1
;
˜˜1 2
using
˘˘ !
(
˘˘" ##
PrincipalSearchResult
˘˘# 8
<
˘˘8 9
	Principal
˘˘9 B
>
˘˘B C
results
˘˘D K
=
˘˘L M
pS
˘˘N P
.
˘˘P Q
FindAll
˘˘Q X
(
˘˘X Y
)
˘˘Y Z
)
˘˘Z [
{
˙˙ 
if
˚˚  "
(
˚˚# $
results
˚˚$ +
!=
˚˚, .
null
˚˚/ 3
&&
˚˚4 6
results
˚˚7 >
.
˚˚> ?
Count
˚˚? D
(
˚˚D E
)
˚˚E F
>
˚˚G H
$num
˚˚I J
)
˚˚J K
{
¸¸  !
return
˝˝$ *
true
˝˝+ /
;
˝˝/ 0
}
˛˛  !
}
ˇˇ 
}
ÄÄ 
}
ÅÅ 
}
ÇÇ 
return
ÉÉ 
false
ÉÉ 
;
ÉÉ 
}
ÑÑ 
catch
ÖÖ 
(
ÖÖ 
	Exception
ÖÖ 
)
ÖÖ 
{
ÜÜ 
return
áá 
false
áá 
;
áá 
}
àà 
}
ââ 	
public
ãã 
static
ãã 
bool
ãã 
CheckUserInAD
ãã (
(
ãã( )
string
ãã) /
domain
ãã0 6
,
ãã6 7
string
ãã8 >
username
ãã? G
,
ããG H
string
ããI O
password
ããP X
)
ããX Y
{
åå 	
using
çç 
(
çç 
PrincipalContext
çç #
pc
çç$ &
=
çç' (
new
çç) ,
PrincipalContext
çç- =
(
çç= >
ContextType
çç> I
.
ççI J
Domain
ççJ P
,
ççP Q
domain
ççR X
)
ççX Y
)
ççY Z
{
éé 
var
èè 
	userValid
èè 
=
èè 
pc
èè  "
.
èè" #!
ValidateCredentials
èè# 6
(
èè6 7
username
èè7 ?
,
èè? @
password
èèA I
)
èèI J
;
èèJ K
return
êê 
	userValid
êê  
;
êê  !
}
ëë 
}
íí 	
private
¢¢ 
UserSgq
¢¢ 
LoginBrasil
¢¢ #
(
¢¢# $
UserDTO
¢¢$ +
userDto
¢¢, 3
,
¢¢3 4
UserSgq
¢¢5 <

userByName
¢¢= G
)
¢¢G H
{
££ 	
var
¶¶ 
isCreate
¶¶ 
=
¶¶ 
false
¶¶  
;
¶¶  !
try
ßß 
{
®® 
if
©© 
(
©© 

userByName
©© 
==
©© !
null
©©" &
)
©©& '
{
™™ &
CriaUSerSgqPeloUserSgqBR
´´ ,
(
´´, -
userDto
´´- 4
)
´´4 5
;
´´5 6
isCreate
¨¨ 
=
¨¨ 
true
¨¨ #
;
¨¨# $
}
≠≠ 
}
ØØ 
catch
∞∞ 
(
∞∞ 
	Exception
∞∞ 
e
∞∞ 
)
∞∞ 
{
±± 
throw
≤≤ 
new
≤≤ 
	Exception
≤≤ #
(
≤≤# $
$str
≤≤$ c
,
≤≤c d
e
≤≤e f
)
≤≤f g
;
≤≤g h
}
≥≥ 
UserSgq
∫∫ 
isUser
∫∫ 
=
∫∫ &
CheckUserAndPassDataBase
∫∫ 5
(
∫∫5 6
userDto
∫∫6 =
)
∫∫= >
;
∫∫> ?
try
ææ 
{
øø 
userDto
¿¿ 
.
¿¿ 
Id
¿¿ 
=
¿¿ 
isUser
¿¿ #
.
¿¿# $
Id
¿¿$ &
;
¿¿& '/
!AtualizaRolesSgqBrPelosDadosDoErp
¡¡ 1
(
¡¡1 2
userDto
¡¡2 9
)
¡¡9 :
;
¡¡: ;
if
√√ 
(
√√ 
isCreate
√√ 
&&
√√ 
isUser
√√  &
.
√√& '
ParCompany_Id
√√' 4
==
√√5 7
null
√√8 <
||
√√= ?
!
√√@ A
(
√√A B
isUser
√√B H
.
√√H I
ParCompany_Id
√√I V
>
√√W X
$num
√√Y Z
)
√√Z [
)
√√[ \
{
ƒƒ 
var
≈≈ 
firstCompany
≈≈ $
=
≈≈% &%
_baseParCompanyXUserSgq
≈≈' >
.
≈≈> ?
GetAll
≈≈? E
(
≈≈E F
)
≈≈F G
.
≈≈G H
FirstOrDefault
≈≈H V
(
≈≈V W
r
≈≈W X
=>
≈≈Y [
r
≈≈\ ]
.
≈≈] ^

UserSgq_Id
≈≈^ h
==
≈≈i k
isUser
≈≈l r
.
≈≈r s
Id
≈≈s u
)
≈≈u v
;
≈≈v w
isUser
∆∆ 
.
∆∆ 
ParCompany_Id
∆∆ (
=
∆∆) *
firstCompany
∆∆+ 7
.
∆∆7 8
ParCompany_Id
∆∆8 E
;
∆∆E F
	_userRepo
«« 
.
«« 
Salvar
«« $
(
««$ %
isUser
««% +
)
««+ ,
;
««, -
}
»» 
}
   
catch
ÀÀ 
(
ÀÀ 
	Exception
ÀÀ 
e
ÀÀ 
)
ÀÀ 
{
ÃÃ 
throw
ÕÕ 
new
ÕÕ 
	Exception
ÕÕ #
(
ÕÕ# $
$str
ÕÕ$ c
,
ÕÕc d
e
ÕÕe f
)
ÕÕf g
;
ÕÕg h
}
ŒŒ 
isUser
—— 
.
——  
ParCompanyXUserSgq
—— %
=
——& '%
_baseParCompanyXUserSgq
——( ?
.
——? @
GetAll
——@ F
(
——F G
)
——G H
.
——H I
Where
——I N
(
——N O
r
——O P
=>
——Q S
r
——T U
.
——U V

UserSgq_Id
——V `
==
——a c
isUser
——d j
.
——j k
Id
——k m
)
——m n
.
——n o
ToList
——o u
(
——u v
)
——v w
;
——w x
return
““ 
isUser
““ 
;
““ 
}
”” 	
private
ŸŸ 
void
ŸŸ /
!AtualizaRolesSgqBrPelosDadosDoErp
ŸŸ 6
(
ŸŸ6 7
UserDTO
ŸŸ7 >
userDto
ŸŸ? F
)
ŸŸF G
{
⁄⁄ 	
using
€€ 
(
€€ 
var
€€ 
db
€€ 
=
€€ 
new
€€  
SGQ_GlobalEntities
€€  2
(
€€2 3
)
€€3 4
)
€€4 5
{
‹‹ 
Usuario
›› 
usuarioSgqBr
›› $
;
››$ %
try
ﬂﬂ 
{
‡‡ 
usuarioSgqBr
··  
=
··! "
db
··# %
.
··% &
Usuario
··& -
.
··- .
AsNoTracking
··. :
(
··: ;
)
··; <
.
··< =
FirstOrDefault
··= K
(
··K L
r
··L M
=>
··N P
r
··Q R
.
··R S
cSigla
··S Y
.
··Y Z
ToLower
··Z a
(
··a b
)
··b c
==
··d f
userDto
··g n
.
··n o
Name
··o s
.
··s t
ToLower
··t {
(
··{ |
)
··| }
)
··} ~
;
··~ 
}
‚‚ 
catch
„„ 
(
„„ 
	Exception
„„  
e
„„! "
)
„„" #
{
‰‰ 
throw
ÂÂ 
new
ÂÂ 
	Exception
ÂÂ '
(
ÂÂ' (
$str
ÂÂ( T
,
ÂÂT U
e
ÂÂV W
)
ÂÂW X
;
ÂÂX Y
}
ÊÊ 
if
ËË 
(
ËË 
usuarioSgqBr
ËË  
!=
ËË! #
null
ËË$ (
)
ËË( )
{
ÈÈ 
IEnumerable
ÍÍ 
<
ÍÍ  "
UsuarioPerfilEmpresa
ÍÍ  4
>
ÍÍ4 5'
usuarioPerfilEmpresaSgqBr
ÍÍ6 O
;
ÍÍO P
IEnumerable
ÎÎ 
<
ÎÎ   
ParCompanyXUserSgq
ÎÎ  2
>
ÎÎ2 3
rolesSgqGlobal
ÎÎ4 B
;
ÎÎB C
IEnumerable
ÏÏ 
<
ÏÏ  

ParCompany
ÏÏ  *
>
ÏÏ* +!
allCompanySgqGlobal
ÏÏ, ?
;
ÏÏ? @
try
ÓÓ 
{
ÔÔ '
usuarioPerfilEmpresaSgqBr
 1
=
2 3
db
4 6
.
6 7"
UsuarioPerfilEmpresa
7 K
.
K L
Where
L Q
(
Q R
r
R S
=>
T V
r
W X
.
X Y

nCdUsuario
Y c
==
d f
usuarioSgqBr
g s
.
s t

nCdUsuario
t ~
)
~ 
; Ä
rolesSgqGlobal
ÒÒ &
=
ÒÒ' (%
_baseParCompanyXUserSgq
ÒÒ) @
.
ÒÒ@ A
GetAll
ÒÒA G
(
ÒÒG H
)
ÒÒH I
.
ÒÒI J
Where
ÒÒJ O
(
ÒÒO P
r
ÒÒP Q
=>
ÒÒR T
r
ÒÒU V
.
ÒÒV W

UserSgq_Id
ÒÒW a
==
ÒÒb d
userDto
ÒÒe l
.
ÒÒl m
Id
ÒÒm o
)
ÒÒo p
;
ÒÒp q!
allCompanySgqGlobal
ÚÚ +
=
ÚÚ, -
_baseParCompany
ÚÚ. =
.
ÚÚ= >
GetAll
ÚÚ> D
(
ÚÚD E
)
ÚÚE F
;
ÚÚF G
}
ÛÛ 
catch
ÙÙ 
(
ÙÙ 
	Exception
ÙÙ $
e
ÙÙ% &
)
ÙÙ& '
{
ıı 
throw
ˆˆ 
new
ˆˆ !
	Exception
ˆˆ" +
(
ˆˆ+ ,
$str
ˆˆ, Y
,
ˆˆY Z
e
ˆˆ[ \
)
ˆˆ\ ]
;
ˆˆ] ^
}
˜˜ 
foreach
˘˘ 
(
˘˘ 
var
˘˘  
upe
˘˘! $
in
˘˘% ''
usuarioPerfilEmpresaSgqBr
˘˘( A
)
˘˘A B
{
˙˙ 
var
¸¸ 
perfilSgqBr
¸¸ '
=
¸¸( )
db
¸¸* ,
.
¸¸, -
Perfil
¸¸- 3
.
¸¸3 4
FirstOrDefault
¸¸4 B
(
¸¸B C
r
¸¸C D
=>
¸¸E G
r
¸¸H I
.
¸¸I J
	nCdPerfil
¸¸J S
==
¸¸T V
upe
¸¸W Z
.
¸¸Z [
	nCdPerfil
¸¸[ d
)
¸¸d e
.
¸¸e f
	nCdPerfil
¸¸f o
.
¸¸o p
ToString
¸¸p x
(
¸¸x y
)
¸¸y z
;
¸¸z {
var
˝˝ !
parCompanySgqGlobal
˝˝ /
=
˝˝0 1!
allCompanySgqGlobal
˝˝2 E
.
˝˝E F
FirstOrDefault
˝˝F T
(
˝˝T U
r
˝˝U V
=>
˝˝W Y
r
˝˝Z [
.
˝˝[ \
IntegrationId
˝˝\ i
==
˝˝j l
upe
˝˝m p
.
˝˝p q

nCdEmpresa
˝˝q {
)
˝˝{ |
;
˝˝| }
if
˛˛ 
(
˛˛ !
parCompanySgqGlobal
˛˛ /
!=
˛˛0 2
null
˛˛3 7
)
˛˛7 8
{
ˇˇ 
if
ÄÄ 
(
ÄÄ  
rolesSgqGlobal
ÄÄ  .
.
ÄÄ. /
Any
ÄÄ/ 2
(
ÄÄ2 3
r
ÄÄ3 4
=>
ÄÄ5 7
r
ÄÄ8 9
.
ÄÄ9 :
ParCompany_Id
ÄÄ: G
==
ÄÄH J!
parCompanySgqGlobal
ÄÄK ^
.
ÄÄ^ _
Id
ÄÄ_ a
&&
ÄÄb d
r
ÄÄe f
.
ÄÄf g

UserSgq_Id
ÄÄg q
==
ÄÄr t
userDto
ÄÄu |
.
ÄÄ| }
Id
ÄÄ} 
&&ÄÄÄ Ç
rÄÄÉ Ñ
.ÄÄÑ Ö
RoleÄÄÖ â
==ÄÄä å
perfilSgqBrÄÄç ò
)ÄÄò ô
)ÄÄô ö
{
ÅÅ 
}
ÉÉ 
else
ÑÑ  
if
ÑÑ! #
(
ÑÑ$ %
!
ÑÑ% &
rolesSgqGlobal
ÑÑ& 4
.
ÑÑ4 5
Any
ÑÑ5 8
(
ÑÑ8 9
r
ÑÑ9 :
=>
ÑÑ; =
r
ÑÑ> ?
.
ÑÑ? @
ParCompany_Id
ÑÑ@ M
==
ÑÑN P!
parCompanySgqGlobal
ÑÑQ d
.
ÑÑd e
Id
ÑÑe g
&&
ÑÑh j
r
ÑÑk l
.
ÑÑl m

UserSgq_Id
ÑÑm w
==
ÑÑx z
userDtoÑÑ{ Ç
.ÑÑÇ É
IdÑÑÉ Ö
)ÑÑÖ Ü
)ÑÑÜ á
{
ÖÖ 
var
áá  # 
adicionaRoleGlobal
áá$ 6
=
áá7 8
new
áá9 < 
ParCompanyXUserSgq
áá= O
(
ááO P
)
ááP Q
{
àà  !
ParCompany_Id
ââ$ 1
=
ââ2 3!
parCompanySgqGlobal
ââ4 G
.
ââG H
Id
ââH J
,
ââJ K

UserSgq_Id
ää$ .
=
ää/ 0
userDto
ää1 8
.
ää8 9
Id
ää9 ;
,
ää; <
Role
ãã$ (
=
ãã) *
perfilSgqBr
ãã+ 6
}
åå  !
;
åå! "%
_baseParCompanyXUserSgq
çç  7
.
çç7 8
AddOrUpdate
çç8 C
(
ççC D 
adicionaRoleGlobal
ççD V
)
ççV W
;
ççW X
}
èè 
}
êê 
}
íí 
try
îî 
{
ïï 
var
óó (
existentesSomenteSgqGlobal
óó 6
=
óó7 8%
_baseParCompanyXUserSgq
óó9 P
.
óóP Q
GetAll
óóQ W
(
óóW X
)
óóX Y
.
óóY Z
Where
óóZ _
(
óó_ `
r
óó` a
=>
óób d
r
óóe f
.
óóf g

UserSgq_Id
óóg q
==
óór t
userDto
óóu |
.
óó| }
Id
óó} 
)óó Ä
;óóÄ Å
var
òò *
todosOsPerfisSgqBrAssociados
òò 8
=
òò9 :
db
òò; =
.
òò= >
Perfil
òò> D
.
òòD E
Where
òòE J
(
òòJ K
r
òòK L
=>
òòM O'
usuarioPerfilEmpresaSgqBr
òòP i
.
òòi j
Any
òòj m
(
òòm n
upe
òòn q
=>
òòr t
upe
òòu x
.
òòx y
	nCdPerfilòòy Ç
==òòÉ Ö
ròòÜ á
.òòá à
	nCdPerfilòòà ë
)òòë í
)òòí ì
;òòì î
if
ôô 
(
ôô *
todosOsPerfisSgqBrAssociados
ôô 8
!=
ôô9 ;
null
ôô< @
)
ôô@ A
{
öö (
existentesSomenteSgqGlobal
õõ 6
=
õõ7 8(
existentesSomenteSgqGlobal
õõ9 S
.
õõS T
Where
õõT Y
(
õõY Z
r
õõZ [
=>
õõ\ ^*
todosOsPerfisSgqBrAssociados
õõ_ {
.
õõ{ |
Any
õõ| 
(õõ Ä
tõõÄ Å
=>õõÇ Ñ
!õõÖ Ü
(õõÜ á
tõõá à
.õõà â
	nCdPerfilõõâ í
.õõí ì
ToStringõõì õ
(õõõ ú
)õõú ù
==õõû †
rõõ° ¢
.õõ¢ £
Roleõõ£ ß
)õõß ®
)õõ® ©
)õõ© ™
;õõ™ ´
foreach
ùù #
(
ùù$ %
var
ùù% ($
removerPerfilSgqGlobal
ùù) ?
in
ùù@ B(
existentesSomenteSgqGlobal
ùùC ]
)
ùù] ^%
_baseParCompanyXUserSgq
ûû  7
.
ûû7 8
Remove
ûû8 >
(
ûû> ?$
removerPerfilSgqGlobal
ûû? U
)
ûûU V
;
ûûV W
}
üü 
}
†† 
catch
°° 
(
°° 
	Exception
°° $
e
°°% &
)
°°& '
{
¢¢ 
throw
§§ 
new
§§ !
	Exception
§§" +
(
§§+ ,
$str
§§, u
,
§§u v
e
§§w x
)
§§x y
;
§§y z
}
•• 
}
ßß 
}
©© 
}
´´ 	
private
±± 
void
±± &
CriaUSerSgqPeloUserSgqBR
±± -
(
±±- .
UserDTO
±±. 5
userDto
±±6 =
)
±±= >
{
≤≤ 	
using
≥≥ 
(
≥≥ 
var
≥≥ 
db
≥≥ 
=
≥≥ 
new
≥≥  
SGQ_GlobalEntities
≥≥  2
(
≥≥2 3
)
≥≥3 4
)
≥≥4 5
{
¥¥ 
try
µµ 
{
∂∂ 
var
∑∑ !
existenteNoDbAntigo
∑∑ +
=
∑∑, -
db
∑∑. 0
.
∑∑0 1
Usuario
∑∑1 8
.
∑∑8 9
FirstOrDefault
∑∑9 G
(
∑∑G H
r
∑∑H I
=>
∑∑J L
r
∑∑M N
.
∑∑N O
cSigla
∑∑O U
.
∑∑U V
ToLower
∑∑V ]
(
∑∑] ^
)
∑∑^ _
==
∑∑` b
userDto
∑∑c j
.
∑∑j k
Name
∑∑k o
.
∑∑o p
ToLower
∑∑p w
(
∑∑w x
)
∑∑x y
)
∑∑y z
;
∑∑z {
if
∏∏ 
(
∏∏ !
existenteNoDbAntigo
∏∏ +
!=
∏∏, .
null
∏∏/ 3
)
∏∏3 4
{
ππ 
UserSgq
∫∫ 

newUserSgq
∫∫  *
;
∫∫* +
try
ºº 
{
ΩΩ 

newUserSgq
ææ &
=
ææ' (
new
ææ) ,
UserSgq
ææ- 4
(
ææ4 5
)
ææ5 6
{
øø 
Name
¿¿  $
=
¿¿% &!
existenteNoDbAntigo
¿¿' :
.
¿¿: ;
cSigla
¿¿; A
.
¿¿A B
ToLower
¿¿B I
(
¿¿I J
)
¿¿J K
,
¿¿K L
FullName
¡¡  (
=
¡¡) *!
existenteNoDbAntigo
¡¡+ >
.
¡¡> ?

cNmUsuario
¡¡? I
,
¡¡I J
Password
√√  (
=
√√) *
Guard
√√+ 0
.
√√0 1
EncryptStringAES
√√1 A
(
√√A B
userDto
√√B I
.
√√I J
Password
√√J R
)
√√R S
}
ƒƒ 
;
ƒƒ 
}
≈≈ 
catch
∆∆ 
(
∆∆ 
	Exception
∆∆ (
e
∆∆) *
)
∆∆* +
{
«« 
throw
»» !
new
»»" %
	Exception
»»& /
(
»»/ 0
$str
»»0 R
,
»»R S
e
»»T U
)
»»U V
;
»»V W
}
…… 
try
ÀÀ 
{
ÃÃ 
	_userRepo
ÕÕ %
.
ÕÕ% &
Salvar
ÕÕ& ,
(
ÕÕ, -

newUserSgq
ÕÕ- 7
)
ÕÕ7 8
;
ÕÕ8 9
userDto
ŒŒ #
.
ŒŒ# $
Id
ŒŒ$ &
=
ŒŒ' (

newUserSgq
ŒŒ) 3
.
ŒŒ3 4
Id
ŒŒ4 6
;
ŒŒ6 7
}
œœ 
catch
–– 
(
–– 
	Exception
–– (
e
––) *
)
––* +
{
—— 
throw
““ !
new
““" %
	Exception
““& /
(
““/ 0
$str
““0 r
,
““r s
e
““t u
)
““u v
;
““v w
}
”” 
}
’’ 
else
÷÷ 
{
◊◊ 
throw
ÿÿ 
new
ÿÿ !
	Exception
ÿÿ" +
(
ÿÿ+ ,
$str
ÿÿ, b
)
ÿÿb c
;
ÿÿc d
}
ŸŸ 
}
⁄⁄ 
catch
€€ 
(
€€ 
	Exception
€€  
e
€€! "
)
€€" #
{
‹‹ 
new
›› 
	CreateLog
›› !
(
››! "
new
››" %
	Exception
››& /
(
››/ 0
$str
››0 a
,
››a b
e
››c d
)
››d e
)
››e f
;
››f g
throw
ﬁﬁ 
e
ﬁﬁ 
;
ﬁﬁ 
}
ﬂﬂ 
}
‡‡ 
}
·· 	
}
ÂÂ 
}ÁÁ 