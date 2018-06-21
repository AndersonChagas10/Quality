�
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
}55 �
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
};; �
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
consolidationLevel01	~ �
)
� �
;
� �
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
} �'
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
listaParLevel1XCluster	v �
,
� �
List
� �
<
� �
int
� �
>
� �
removerHeadField
� �
,
� �
List
� �
<
� �
ParCounterXLocal
� �
>
� �"
listaParCounterLocal
� �
,
� �
List
� �
<
� �(
ParNotConformityRuleXLevel
� �
>
� �!
listNonCoformitRule
� �
,
� �
List
� �
<
� �

ParRelapse
� �
>
� �
listaReincidencia
� �
,
� �
List
� �
<
� �
ParGoal
� �
>
� �
listParGoal
� �
)
� �
;
� �
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
listParCounterXLocal			w �
,
		� �
List
		� �
<
		� �(
ParNotConformityRuleXLevel
		� �
>
		� �.
 saveParamNotConformityRuleXLevel
		� �
,
		� �
List
		� �
<
		� �
ParEvaluation
		� �
>
		� �!
saveParamEvaluation
		� �
,
		� �
List
		� �
<
		� �
	ParSample
		� �
>
		� �
saveParamSample
		� �
,
		� �
List
		� �
<
		� �

ParRelapse
		� �
>
		� �
listParRelapse
		� �
)
		� �
;
		� �
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
listParRelapse	v �
,
� �
List
� �
<
� �
ParLevel3Level2
� �
>
� �#
parLevel3Level2pontos
� �
,
� �
int
� �
level1Id
� �
)
� �
;
� �
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
} �
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
}		 �
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
DataCarrierFormulario	l �
form
� �
)
� �
;
� �
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
� �
)
� �
;
� �
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
} �
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
List			~ �
<
		� �
CorrectiveAction
		� �
>
		� �%
_correctiveActionToSave
		� �
)
		� �
;
		� �
}

 
} �	
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
} �
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
} �
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
} �
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
} �
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
} �
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
null	~ �
)
� �
;
� �
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
}## �
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
} �
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
}   �

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
} �
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
}		 �
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
]$$) *�*
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
}[[ х
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
bb� �
.
bb� �
Active
bb� �
==
bb� �
true
bb� �
)
bb� �
.
bb� �
ToList
bb� �
(
bb� �
)
bb� �
;
bb� �
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
�� 	
public
�� 
void
�� &
SaveParCompanyXStructure
�� ,
(
��, -
List
��- 1
<
��1 2"
ParCompanyXStructure
��2 F
>
��F G&
listParCompanyXStructure
��H `
,
��` a

ParCompany
��b l

parCompany
��m w
)
��w x
{
�� 	
List
�� 
<
�� "
ParCompanyXStructure
�� %
>
��% &
dbList
��' -
=
��. /+
_baseRepoParCompanyXStructure
��0 M
.
��M N
GetAll
��N T
(
��T U
)
��U V
.
��V W
Where
��W \
(
��\ ]
r
��] ^
=>
��_ a
r
��b c
.
��c d
ParCompany_Id
��d q
==
��r t

parCompany
��u 
.�� �
Id��� �
&&��� �
r��� �
.��� �
Active��� �
==��� �
true��� �
)��� �
.��� �
ToList��� �
(��� �
)��� �
;��� �
foreach
�� 
(
�� "
ParCompanyXStructure
�� )
companyStructure
��* :
in
��; =
dbList
��> D
)
��D E
{
�� "
ParCompanyXStructure
�� $
save
��% )
=
��* +&
listParCompanyXStructure
��, D
.
��D E
Where
��E J
(
��J K
r
��K L
=>
��M O
r
��P Q
.
��Q R
ParStructure_Id
��R a
==
��b d
companyStructure
��e u
.
��u v
ParStructure_Id��v �
&&��� �
r
��, -
.
��- .
ParCompany_Id
��. ;
==
��< >
companyStructure
��? O
.
��O P
ParCompany_Id
��P ]
&&
��^ `
r
��, -
.
��- .
Active
��. 4
==
��5 7
true
��8 <
)
��< =
.
��= >
FirstOrDefault
��> L
(
��L M
)
��M N
;
��N O
if
�� 
(
�� 
save
�� 
==
�� 
null
��  
)
��  !
{
�� 
companyStructure
�� $
.
��$ %
Active
��% +
=
��, -
false
��. 3
;
��3 4
Guard
�� 
.
�� 

verifyDate
�� $
(
��$ %
companyStructure
��% 5
,
��5 6
$str
��7 B
)
��B C
;
��C D+
_baseRepoParCompanyXStructure
�� 1
.
��1 2
Update
��2 8
(
��8 9
companyStructure
��9 I
)
��I J
;
��J K
}
�� 
else
�� 
{
�� 
save
�� 
.
�� 
ParCompany_Id
�� &
=
��' (
companyStructure
��) 9
.
��9 :
ParCompany_Id
��: G
;
��G H
save
�� 
.
�� 
Id
�� 
=
�� 
companyStructure
�� .
.
��. /
Id
��/ 1
;
��1 2
Guard
�� 
.
�� 

verifyDate
�� $
(
��$ %
companyStructure
��% 5
,
��5 6
$str
��7 B
)
��B C
;
��C D+
_baseRepoParCompanyXStructure
�� 1
.
��1 2
Update
��2 8
(
��8 9
companyStructure
��9 I
)
��I J
;
��J K
}
�� &
listParCompanyXStructure
�� (
.
��( )
Remove
��) /
(
��/ 0
save
��0 4
)
��4 5
;
��5 6
}
�� 
if
�� 
(
�� &
listParCompanyXStructure
�� '
!=
��( *
null
��+ /
)
��/ 0
foreach
�� 
(
�� "
ParCompanyXStructure
�� -
companyStructure
��. >
in
��? A&
listParCompanyXStructure
��B Z
)
��Z [
{
�� 
companyStructure
�� $
.
��$ %
Active
��% +
=
��, -
true
��. 2
;
��2 3
companyStructure
�� $
.
��$ %
ParCompany_Id
��% 2
=
��3 4

parCompany
��5 ?
.
��? @
Id
��@ B
;
��B C+
_baseRepoParCompanyXStructure
�� 1
.
��1 2
Add
��2 5
(
��5 6
companyStructure
��6 F
)
��F G
;
��G H
}
�� 
}
�� 	
public
�� %
ParCompanyXStructureDTO
�� &.
 AddUpdateParCompanyXStructureDTO
��' G
(
��G H%
ParCompanyXStructureDTO
��H _%
parCompanyXStructureDTO
��` w
)
��w x
{
�� 	
throw
�� 
new
�� %
NotImplementedException
�� -
(
��- .
)
��. /
;
��/ 0
}
�� 	
public
�� 
ParStructureDTO
�� #
AddUpdateParStructure
�� 4
(
��4 5
ParStructureDTO
��5 D
parStructureDTO
��E T
)
��T U
{
�� 	
ParStructure
��  
parStructureSalvar
�� +
=
��, -
Mapper
��. 4
.
��4 5
Map
��5 8
<
��8 9
ParStructure
��9 E
>
��E F
(
��F G
parStructureDTO
��G V
)
��V W
;
��W X#
_baseRepoParStructure
�� !
.
��! ""
AddOrUpdateNotCommit
��" 6
(
��6 7 
parStructureSalvar
��7 I
)
��I J
;
��J K#
_baseRepoParStructure
�� !
.
��! "
Commit
��" (
(
��( )
)
��) *
;
��* +
parStructureDTO
�� 
.
�� 
Id
�� 
=
��   
parStructureSalvar
��! 3
.
��3 4
Id
��4 6
;
��6 7
return
�� 
parStructureDTO
�� "
;
��" #
}
�� 	
public
�� "
ParStructureGroupDTO
�� #(
AddUpdateParStructureGroup
��$ >
(
��> ?"
ParStructureGroupDTO
��? S"
parStructureGroupDTO
��T h
)
��h i
{
�� 	
ParStructureGroup
�� %
parStructureGroupSalvar
�� 5
=
��6 7
Mapper
��8 >
.
��> ?
Map
��? B
<
��B C
ParStructureGroup
��C T
>
��T U
(
��U V"
parStructureGroupDTO
��V j
)
��j k
;
��k l(
_baseRepoParStructureGroup
�� &
.
��& '"
AddOrUpdateNotCommit
��' ;
(
��; <%
parStructureGroupSalvar
��< S
)
��S T
;
��T U(
_baseRepoParStructureGroup
�� &
.
��& '
Commit
��' -
(
��- .
)
��. /
;
��/ 0"
parStructureGroupDTO
��  
.
��  !
Id
��! #
=
��$ %%
parStructureGroupSalvar
��& =
.
��= >
Id
��> @
;
��@ A
return
�� "
parStructureGroupDTO
�� '
;
��' (
}
�� 	
}
�� 
}�� �
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
}44 ��
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
�� 
=
��  
baseRepoParLevel1
��! 2
;
��2 3 
_baseRepoParLevel2
�� 
=
��  
baseRepoParLevel2
��! 2
;
��2 3 
_baseRepoParLevel3
�� 
=
��  
baseRepoParLevel3
��! 2
;
��2 3#
_baseRepoParLevel1NLL
�� !
=
��" #)
baseRepoParLevel1NoLazyLoad
��$ ?
;
��? @#
_baseRepoParLevel2NLL
�� !
=
��" #)
baseRepoParLevel2NoLazyLoad
��$ ?
;
��? @#
_baseRepoParLevel3NLL
�� !
=
��" #)
baseRepoParLevel3NoLazyLoad
��$ ?
;
��? @(
_baseRepoParLevel1XCluster
�� &
=
��' (#
baseParLevel1XCluster
��) >
;
��> ?
_baseParFrequency
�� 
=
�� 
baseParFrequency
��  0
;
��0 1'
_baseParConsolidationType
�� %
=
��& '&
baseParConsolidationType
��( @
;
��@ A
_baseParCluster
�� 
=
�� 
baseParCluster
�� ,
;
��, -
_baseParFieldType
�� 
=
�� 
baseParFieldType
��  0
;
��0 1$
_baseParLevelDefiniton
�� "
=
��# $#
baseParLevelDefiniton
��% :
;
��: ; 
_baseParDepartment
�� 
=
��  
baseParDepartment
��! 2
;
��2 3!
_baseParLevel3Group
�� 
=
��  ! 
baseParLevel3Group
��" 4
;
��4 5
_baseParLocal
�� 
=
�� 
baseParLocal
�� (
;
��( )
_baseParCounter
�� 
=
�� 
baseParCounter
�� ,
;
��, -#
_baseParCounterXLocal
�� !
=
��" #"
baseParCounterXLocal
��$ 8
;
��8 9
_baseParRelapse
�� 
=
�� 
baseParRelapse
�� ,
;
��, -'
_baseParNotConformityRule
�� %
=
��& '&
baseParNotConformityRule
��( @
;
��@ A-
_baseParNotConformityRuleXLevel
�� +
=
��, -,
baseParNotConformityRuleXLevel
��. L
;
��L M
_baseParCompany
�� 
=
�� 
baseParCompany
�� ,
;
��, - 
_baseParEvaluation
�� 
=
��  
baseParEvaluation
��! 2
;
��2 3
_baseParSample
�� 
=
�� 
baseParSample
�� *
;
��* +!
_baseParLevel3Value
�� 
=
��  ! 
baseParLevel3Value
��" 4
;
��4 5%
_baseParLevel3InputType
�� #
=
��$ %$
baseParLevel3InputType
��& <
;
��< =%
_baseParMeasurementUnit
�� #
=
��$ %$
baseParMeasurementUnit
��& <
;
��< =%
_baseParLevel3BoolFalse
�� #
=
��$ %$
baseParLevel3BoolFalse
��& <
;
��< =$
_baseParLevel3BoolTrue
�� "
=
��# $#
baseParLevel3BoolTrue
��% :
;
��: ;"
_baseParLevel3Level2
��  
=
��! "!
baseParLevel3Level2
��# 6
;
��6 7&
_baseRepoParLevel3Level2
�� $
=
��% &%
baseRepoParLevel3Level2
��' >
;
��> ?,
_baseRepoParLevel3Level2Level1
�� *
=
��+ ,+
baseRepoParLevel3Level2Level1
��- J
;
��J K
_repoParLevel3
�� 
=
�� 
repoParLevel3
�� *
;
��* +
db
�� 
.
�� 
Configuration
�� 
.
��  
LazyLoadingEnabled
�� /
=
��0 1
false
��2 7
;
��7 8
}
�� 	
public
�� 
	ParamsDTO
�� 
AddUpdateLevel1
�� (
(
��( )
	ParamsDTO
��) 2
	paramsDto
��3 <
)
��< =
{
�� 	
	ParLevel1
�� 
saveParamLevel1
�� %
=
��& '
Mapper
��( .
.
��. /
Map
��/ 2
<
��2 3
	ParLevel1
��3 <
>
��< =
(
��= >
	paramsDto
��> G
.
��G H
parLevel1Dto
��H T
)
��T U
;
��U V
List
�� 
<
�� 
ParGoal
�� 
>
�� 
listParGoal
�� %
=
��& '
Mapper
��( .
.
��. /
Map
��/ 2
<
��2 3
List
��3 7
<
��7 8
ParGoal
��8 ?
>
��? @
>
��@ A
(
��A B
	paramsDto
��B K
.
��K L
parLevel1Dto
��L X
.
��X Y
listParGoalLevel1
��Y j
)
��j k
;
��k l
List
�� 
<
�� 

ParRelapse
�� 
>
�� 
listaReincidencia
�� .
=
��/ 0
Mapper
��1 7
.
��7 8
Map
��8 ;
<
��; <
List
��< @
<
��@ A

ParRelapse
��A K
>
��K L
>
��L M
(
��M N
	paramsDto
��N W
.
��W X
parLevel1Dto
��X d
.
��d e
listParRelapseDto
��e v
)
��v w
;
��w x
List
�� 
<
�� 
ParHeaderField
�� 
>
��  
listaParHEadField
��! 2
=
��3 4
Mapper
��5 ;
.
��; <
Map
��< ?
<
��? @
List
��@ D
<
��D E
ParHeaderField
��E S
>
��S T
>
��T U
(
��U V
	paramsDto
��V _
.
��_ `#
listParHeaderFieldDto
��` u
)
��u v
;
��v w
List
�� 
<
�� 
ParCounterXLocal
�� !
>
��! ""
ListaParCounterLocal
��# 7
=
��8 9
Mapper
��: @
.
��@ A
Map
��A D
<
��D E
List
��E I
<
��I J
ParCounterXLocal
��J Z
>
��Z [
>
��[ \
(
��\ ]
	paramsDto
��] f
.
��f g
parLevel1Dto
��g s
.
��s t#
listParCounterXLocal��t �
)��� �
;��� �
List
�� 
<
�� 
ParLevel1XCluster
�� "
>
��" #$
ListaParLevel1XCluster
��$ :
=
��; <
Mapper
��= C
.
��C D
Map
��D G
<
��G H
List
��H L
<
��L M
ParLevel1XCluster
��M ^
>
��^ _
>
��_ `
(
��` a
	paramsDto
��a j
.
��j k
parLevel1Dto
��k w
.
��w x$
listLevel1XClusterDto��x �
)��� �
;��� �
List
�� 
<
�� (
ParNotConformityRuleXLevel
�� +
>
��+ ,!
listNonCoformitRule
��- @
=
��A B
Mapper
��C I
.
��I J
Map
��J M
<
��M N
List
��N R
<
��R S(
ParNotConformityRuleXLevel
��S m
>
��m n
>
��n o
(
��o p
	paramsDto
��p y
.
��y z
parLevel1Dto��z �
.��� �1
!listParNotConformityRuleXLevelDto��� �
)��� �
;��� �
if
�� 
(
�� 
saveParamLevel1
�� 
.
��  
ParScoreType_Id
��  /
<=
��0 2
$num
��3 4
)
��4 5
saveParamLevel1
�� 
.
��  
ParScoreType_Id
��  /
=
��0 1
null
��2 6
;
��6 7
if
�� 
(
�� 
	paramsDto
�� 
.
�� #
listParHeaderFieldDto
�� /
!=
��0 2
null
��3 7
)
��7 8
foreach
�� 
(
�� 
var
�� 
i
�� 
in
�� !
	paramsDto
��" +
.
��+ ,#
listParHeaderFieldDto
��, A
.
��A B
Where
��B G
(
��G H
r
��H I
=>
��J L
!
��M N
string
��N T
.
��T U
IsNullOrEmpty
��U b
(
��b c
r
��c d
.
��d e
DefaultOption
��e r
)
��r s
)
��s t
)
��t u
	paramsDto
�� 
.
�� #
listParHeaderFieldDto
�� 3
.
��3 4
ForEach
��4 ;
(
��; <
r
��< =
=>
��> @
r
��A B
.
��B C"
parMultipleValuesDto
��C W
.
��W X
FirstOrDefault
��X f
(
��f g
c
��g h
=>
��i k
c
��l m
.
��m n
Name
��n r
.
��r s
Equals
��s y
(
��y z
i
��z {
.
��{ |
DefaultOption��| �
)��� �
)��� �
.��� �
IsDefaultOption��� �
=��� �
true��� �
)��� �
;��� �
List
�� 
<
�� 
int
�� 
>
�� 
removerHeadField
�� &
=
��' (
	paramsDto
��) 2
.
��2 3
parLevel1Dto
��3 ?
.
��? @#
removerParHeaderField
��@ U
;
��U V
try
�� 
{
�� 
_paramsRepo
�� 
.
�� 
SaveParLevel1
�� )
(
��) *
saveParamLevel1
��* 9
,
��9 :
listaParHEadField
��; L
,
��L M$
ListaParLevel1XCluster
��N d
,
��d e
removerHeadField
��f v
,
��, -"
ListaParCounterLocal
��. B
,
��B C!
listNonCoformitRule
��D W
,
��W X
listaReincidencia
��Y j
,
��j k
listParGoal
��l w
)
��w x
;
��x y
if
�� 
(
�� 
DTO
�� 
.
�� 
GlobalConfig
�� $
.
��$ %
Brasil
��% +
)
��+ ,
{
�� 
if
�� 
(
�� 
	paramsDto
�� !
.
��! "
parLevel1Dto
��" .
.
��. /

IsSpecific
��/ 9
)
��9 :
{
�� 
var
�� 
query
�� !
=
��" #
$str
��$ \
;
��\ ]
var
�� 
queryExcute
�� '
=
��( )
string
��* 0
.
��0 1
Empty
��1 6
;
��6 7
queryExcute
�� #
=
��$ %
string
��& ,
.
��, -
Format
��- 3
(
��3 4
query
��4 9
,
��9 :
$str
��; K
,
��K L
	paramsDto
��M V
.
��V W
parLevel1Dto
��W c
.
��c d
AllowAddLevel3
��d r
?
��s t
$num
��u v
:
��w x
$num
��y z
,
��z {
	paramsDto��| �
.��� �
parLevel1Dto��� �
.��� �
Id��� �
)��� �
;��� �
db
�� 
.
�� 
Database
�� #
.
��# $
ExecuteSqlCommand
��$ 5
(
��5 6
queryExcute
��6 A
)
��A B
;
��B C
queryExcute
�� #
=
��$ %
string
��& ,
.
��, -
Format
��- 3
(
��3 4
query
��4 9
,
��9 :
$str
��; W
,
��W X
	paramsDto
��Y b
.
��b c
parLevel1Dto
��c o
.
��o p)
AllowEditPatternLevel3Task��p �
?��� �
$num��� �
:��� �
$num��� �
,��� �
	paramsDto��� �
.��� �
parLevel1Dto��� �
.��� �
Id��� �
)��� �
;��� �
db
�� 
.
�� 
Database
�� #
.
��# $
ExecuteSqlCommand
��$ 5
(
��5 6
queryExcute
��6 A
)
��A B
;
��B C
queryExcute
�� #
=
��$ %
string
��& ,
.
��, -
Format
��- 3
(
��3 4
query
��4 9
,
��9 :
$str
��; T
,
��T U
	paramsDto
��V _
.
��_ `
parLevel1Dto
��` l
.
��l m&
AllowEditWeightOnLevel3��m �
?��� �
$num��� �
:��� �
$num��� �
,��� �
	paramsDto��� �
.��� �
parLevel1Dto��� �
.��� �
Id��� �
)��� �
;��� �
db
�� 
.
�� 
Database
�� #
.
��# $
ExecuteSqlCommand
��$ 5
(
��5 6
queryExcute
��6 A
)
��A B
;
��B C
}
�� 
if
�� 
(
�� 
	paramsDto
�� !
.
��! "
parLevel1Dto
��" .
.
��. /
IsRecravacao
��/ ;
)
��; <
{
�� 
var
�� 
query
�� !
=
��" #
$str
��$ \
;
��\ ]
var
�� 
queryExcute
�� '
=
��( )
string
��* 0
.
��0 1
Empty
��1 6
;
��6 7
queryExcute
�� #
=
��$ %
string
��& ,
.
��, -
Format
��- 3
(
��3 4
query
��4 9
,
��9 :
$str
��; I
,
��I J
	paramsDto
��K T
.
��T U
parLevel1Dto
��U a
.
��a b
IsRecravacao
��b n
?
��o p
$num
��q r
:
��s t
$num
��u v
,
��v w
	paramsDto��x �
.��� �
parLevel1Dto��� �
.��� �
Id��� �
)��� �
;��� �
db
�� 
.
�� 
Database
�� #
.
��# $
ExecuteSqlCommand
��$ 5
(
��5 6
queryExcute
��6 A
)
��A B
;
��B C
}
�� 
}
�� 
}
�� 
catch
�� 
(
�� 
DbUpdateException
�� $
e
��% &
)
��& '
{
�� 
VerifyUniqueName
��  
(
��  !
saveParamLevel1
��! 0
,
��0 1
e
��2 3
)
��3 4
;
��4 5
}
�� 
	paramsDto
�� 
.
�� 
parLevel1Dto
�� "
.
��" #
Id
��# %
=
��& '
saveParamLevel1
��( 7
.
��7 8
Id
��8 :
;
��: ;
return
�� 
	paramsDto
�� 
;
�� 
}
�� 	
public
�� 
ParLevel1DTO
�� 
	GetLevel1
�� %
(
��% &
int
��& )
idParLevel1
��* 5
)
��5 6
{
�� 	
ParLevel1DTO
�� 
parlevel1Dto
�� %
;
��% &
db
�� 
.
�� 
Configuration
�� 
.
��  
LazyLoadingEnabled
�� /
=
��0 1
false
��2 7
;
��7 8
var
�� 
	parlevel1
�� 
=
��  
_baseRepoParLevel1
�� .
.
��. /
GetById
��/ 6
(
��6 7
idParLevel1
��7 B
)
��B C
;
��C D
var
�� 
counter
�� 
=
�� 
	parlevel1
�� #
.
��# $
ParCounterXLocal
��$ 4
.
��4 5
Where
��5 :
(
��: ;
r
��; <
=>
��= ?
r
��@ A
.
��A B
IsActive
��B J
==
��K M
true
��N R
)
��R S
.
��S T
OrderByDescending
��T e
(
��e f
r
��f g
=>
��h j
r
��k l
.
��l m
IsActive
��m u
)
��u v
.
��v w
ToList
��w }
(
��} ~
)
��~ 
;�� �
var
�� 
goal
�� 
=
�� 
	parlevel1
��  
.
��  !
ParGoal
��! (
.
��( )
Where
��) .
(
��. /
r
��/ 0
=>
��1 3
r
��4 5
.
��5 6
IsActive
��6 >
==
��? A
true
��B F
)
��F G
.
��G H
OrderByDescending
��H Y
(
��Y Z
r
��Z [
=>
��\ ^
r
��_ `
.
��` a
IsActive
��a i
)
��i j
.
��j k
ToList
��k q
(
��q r
)
��r s
;
��s t
var
�� 
cluster
�� 
=
�� 
	parlevel1
�� #
.
��# $
ParLevel1XCluster
��$ 5
.
��5 6
Where
��6 ;
(
��; <
r
��< =
=>
��> @
r
��A B
.
��B C
IsActive
��C K
==
��L N
true
��O S
)
��S T
.
��T U
OrderByDescending
��U f
(
��f g
r
��g h
=>
��i k
r
��l m
.
��m n
IsActive
��n v
)
��v w
.
��w x
ToList
��x ~
(
��~ 
)�� �
;��� �
var
�� 

listL3L2L1
�� 
=
�� 
db
�� 
.
��  #
ParLevel3Level2Level1
��  5
.
��5 6
Include
��6 =
(
��= >
$str
��> O
)
��O P
.
��P Q
AsNoTracking
��Q ]
(
��] ^
)
��^ _
.
��_ `
Where
��` e
(
��e f
r
��f g
=>
��h j
r
��k l
.
��l m
Active
��m s
==
��t v
true
��w {
&&
��| ~
r�� �
.��� �
ParLevel1_Id��� �
==��� �
idParLevel1��� �
)��� �
.��� �
ToList��� �
(��� �
)��� �
;��� �
var
�� 
relapse
�� 
=
�� 
	parlevel1
�� #
.
��# $

ParRelapse
��$ .
.
��. /
Where
��/ 4
(
��4 5
r
��5 6
=>
��7 9
r
��: ;
.
��; <
IsActive
��< D
==
��E G
true
��H L
)
��L M
.
��M N
OrderByDescending
��N _
(
��_ `
r
��` a
=>
��b d
r
��e f
.
��f g
IsActive
��g o
)
��o p
.
��p q
ToList
��q w
(
��w x
)
��x y
;
��y z
var
�� 
notConformityrule
�� !
=
��" #
	parlevel1
��$ -
.
��- .(
ParNotConformityRuleXLevel
��. H
.
��H I
Where
��I N
(
��N O
r
��O P
=>
��Q S
r
��T U
.
��U V
IsActive
��V ^
==
��_ a
true
��b f
)
��f g
.
��g h
OrderByDescending
��h y
(
��y z
r
��z {
=>
��| ~
r�� �
.��� �
IsActive��� �
)��� �
.��� �
ToList��� �
(��� �
)��� �
;��� �
var
�� 

cabecalhos
�� 
=
�� 
	parlevel1
�� &
.
��& '#
ParLevel1XHeaderField
��' <
.
��< =
Where
��= B
(
��B C
r
��C D
=>
��E G
r
��H I
.
��I J
IsActive
��J R
==
��S U
true
��V Z
)
��Z [
.
��[ \
OrderBy
��\ c
(
��c d
r
��d e
=>
��f h
r
��i j
.
��j k
IsActive
��k s
)
��s t
.
��t u
ToList
��u {
(
��{ |
)
��| }
;
��} ~
var
�� 

level2List
�� 
=
�� #
_baseRepoParLevel2NLL
�� 2
.
��2 3
GetAll
��3 9
(
��9 :
)
��: ;
.
��; <
Where
��< A
(
��A B
r
��B C
=>
��D F
r
��G H
.
��H I
IsActive
��I Q
==
��R T
true
��U Y
)
��Y Z
;
��Z [
parlevel1Dto
�� 
=
�� 
Mapper
�� !
.
��! "
Map
��" %
<
��% &
ParLevel1DTO
��& 2
>
��2 3
(
��3 4
	parlevel1
��4 =
)
��= >
;
��> ?
parlevel1Dto
�� 
.
�� "
listParCounterXLocal
�� -
=
��. /
Mapper
��0 6
.
��6 7
Map
��7 :
<
��: ;
List
��; ?
<
��? @!
ParCounterXLocalDTO
��@ S
>
��S T
>
��T U
(
��U V
counter
��V ]
)
��] ^
;
��^ _
parlevel1Dto
�� 
.
�� 
listParGoalLevel1
�� *
=
��+ ,
Mapper
��- 3
.
��3 4
Map
��4 7
<
��7 8
List
��8 <
<
��< =

ParGoalDTO
��= G
>
��G H
>
��H I
(
��I J
goal
��J N
)
��N O
;
��O P
parlevel1Dto
�� 
.
�� #
listLevel1XClusterDto
�� .
=
��/ 0
Mapper
��1 7
.
��7 8
Map
��8 ;
<
��; <
List
��< @
<
��@ A"
ParLevel1XClusterDTO
��A U
>
��U V
>
��V W
(
��W X
cluster
��X _
)
��_ `
;
��` a
parlevel1Dto
�� 
.
�� *
listParLevel3Level2Level1Dto
�� 5
=
��6 7
Mapper
��8 >
.
��> ?
Map
��? B
<
��B C
List
��C G
<
��G H&
ParLevel3Level2Level1DTO
��H `
>
��` a
>
��a b
(
��b c

listL3L2L1
��c m
)
��m n
;
��n o
parlevel1Dto
�� 
.
�� 
listParRelapseDto
�� *
=
��+ ,
Mapper
��- 3
.
��3 4
Map
��4 7
<
��7 8
List
��8 <
<
��< =
ParRelapseDTO
��= J
>
��J K
>
��K L
(
��L M
relapse
��M T
)
��T U
;
��U V
parlevel1Dto
�� 
.
�� /
!listParNotConformityRuleXLevelDto
�� :
=
��; <
Mapper
��= C
.
��C D
Map
��D G
<
��G H
List
��H L
<
��L M+
ParNotConformityRuleXLevelDTO
��M j
>
��j k
>
��k l
(
��l m
notConformityrule
��m ~
)
��~ 
;�� �
parlevel1Dto
�� 
.
��  
cabecalhosInclusos
�� +
=
��, -
Mapper
��. 4
.
��4 5
Map
��5 8
<
��8 9
List
��9 =
<
��= >&
ParLevel1XHeaderFieldDTO
��> V
>
��V W
>
��W X
(
��X Y

cabecalhos
��Y c
)
��c d
;
��d e
parlevel1Dto
�� 
.
�� +
parNotConformityRuleXLevelDto
�� 6
=
��7 8
new
��9 <+
ParNotConformityRuleXLevelDTO
��= Z
(
��Z [
)
��[ \
;
��\ ]
parlevel1Dto
�� 
.
�� 6
(CreateSelectListParamsViewModelListLevel
�� A
(
��A B
Mapper
��B H
.
��H I
Map
��I L
<
��L M
List
��M Q
<
��Q R
ParLevel2DTO
��R ^
>
��^ _
>
��_ `
(
��` a

level2List
��a k
)
��k l
,
��l m
parlevel1Dto
��n z
.
��z {+
listParLevel3Level2Level1Dto��{ �
)��� �
;��� �
if
�� 
(
�� 
DTO
�� 
.
�� 
GlobalConfig
��  
.
��  !
Brasil
��! '
)
��' (
{
�� 
var
�� 
query
�� 
=
�� 
$str
�� G
;
��G H
var
�� 
queryExcute
�� 
=
��  !
string
��" (
.
��( )
Empty
��) .
;
��. /
queryExcute
�� 
=
�� 
string
�� $
.
��$ %
Format
��% +
(
��+ ,
query
��, 1
,
��1 2
$str
��3 C
,
��C D
parlevel1Dto
��E Q
.
��Q R
Id
��R T
)
��T U
;
��U V
parlevel1Dto
�� 
.
�� 
AllowAddLevel3
�� +
=
��, -
db
��. 0
.
��0 1
Database
��1 9
.
��9 :
SqlQuery
��: B
<
��B C
bool
��C G
>
��G H
(
��H I
queryExcute
��I T
)
��T U
.
��U V
FirstOrDefault
��V d
(
��d e
)
��e f
;
��f g
queryExcute
�� 
=
�� 
string
�� $
.
��$ %
Format
��% +
(
��+ ,
query
��, 1
,
��1 2
$str
��3 O
,
��O P
parlevel1Dto
��Q ]
.
��] ^
Id
��^ `
)
��` a
;
��a b
parlevel1Dto
�� 
.
�� (
AllowEditPatternLevel3Task
�� 7
=
��8 9
db
��: <
.
��< =
Database
��= E
.
��E F
SqlQuery
��F N
<
��N O
bool
��O S
>
��S T
(
��T U
queryExcute
��U `
)
��` a
.
��a b
FirstOrDefault
��b p
(
��p q
)
��q r
;
��r s
queryExcute
�� 
=
�� 
string
�� $
.
��$ %
Format
��% +
(
��+ ,
query
��, 1
,
��1 2
$str
��3 L
,
��L M
parlevel1Dto
��N Z
.
��Z [
Id
��[ ]
)
��] ^
;
��^ _
parlevel1Dto
�� 
.
�� %
AllowEditWeightOnLevel3
�� 4
=
��5 6
db
��7 9
.
��9 :
Database
��: B
.
��B C
SqlQuery
��C K
<
��K L
bool
��L P
>
��P Q
(
��Q R
queryExcute
��R ]
)
��] ^
.
��^ _
FirstOrDefault
��_ m
(
��m n
)
��n o
;
��o p
queryExcute
�� 
=
�� 
string
�� $
.
��$ %
Format
��% +
(
��+ ,
query
��, 1
,
��1 2
$str
��3 A
,
��A B
parlevel1Dto
��C O
.
��O P
Id
��P R
)
��R S
;
��S T
parlevel1Dto
�� 
.
�� 
IsRecravacao
�� )
=
��* +
db
��, .
.
��. /
Database
��/ 7
.
��7 8
SqlQuery
��8 @
<
��@ A
bool
��A E
>
��E F
(
��F G
queryExcute
��G R
)
��R S
.
��S T
FirstOrDefault
��T b
(
��b c
)
��c d
;
��d e
}
�� 
foreach
�� 
(
�� 
var
�� 
i
�� 
in
�� 
parlevel1Dto
�� *
.
��* +
listParRelapseDto
��+ <
)
��< =
{
�� 
i
�� 
.
�� 
	parLevel1
�� 
=
�� 
null
�� "
;
��" #
i
�� 
.
�� 
	parLevel2
�� 
=
�� 
null
�� "
;
��" #
i
�� 
.
�� 
	parLevel3
�� 
=
�� 
null
�� "
;
��" #
}
�� 
return
�� 
parlevel1Dto
�� 
;
��  
}
�� 	
public
�� 
	ParamsDTO
�� 
AddUpdateLevel2
�� (
(
��( )
	ParamsDTO
��) 2
	paramsDto
��3 <
)
��< =
{
�� 	
	ParLevel2
�� 
saveParamLevel2
�� %
=
��& '
Mapper
��( .
.
��. /
Map
��/ 2
<
��2 3
	ParLevel2
��3 <
>
��< =
(
��= >
	paramsDto
��> G
.
��G H
parLevel2Dto
��H T
)
��T U
;
��U V
	paramsDto
�� 
.
�� 
parLevel2Dto
�� "
.
��" #'
CriaListaSampleEvaluation
��# <
(
��< =
)
��= >
;
��> ?
List
�� 
<
�� 
	ParSample
�� 
>
�� 
saveParamSample
�� +
=
��, -
Mapper
��. 4
.
��4 5
Map
��5 8
<
��8 9
List
��9 =
<
��= >
	ParSample
��> G
>
��G H
>
��H I
(
��I J
	paramsDto
��J S
.
��S T
parLevel2Dto
��T `
.
��` a

listSample
��a k
)
��k l
;
��l m
List
�� 
<
�� 
ParEvaluation
�� 
>
�� !
saveParamEvaluation
��  3
=
��4 5
Mapper
��6 <
.
��< =
Map
��= @
<
��@ A
List
��A E
<
��E F
ParEvaluation
��F S
>
��S T
>
��T U
(
��U V
	paramsDto
��V _
.
��_ `
parLevel2Dto
��` l
.
��l m
listEvaluation
��m {
)
��{ |
;
��| }
List
�� 
<
�� 

ParRelapse
�� 
>
�� 
listParRelapse
�� +
=
��, -
Mapper
��. 4
.
��4 5
Map
��5 8
<
��8 9
List
��9 =
<
��= >

ParRelapse
��> H
>
��H I
>
��I J
(
��J K
	paramsDto
��K T
.
��T U
parLevel2Dto
��U a
.
��a b
listParRelapseDto
��b s
)
��s t
;
��t u
List
�� 
<
�� 
ParLevel3Group
�� 
>
��  !
listaParLevel3Group
��! 4
=
��5 6
Mapper
��7 =
.
��= >
Map
��> A
<
��A B
List
��B F
<
��F G
ParLevel3Group
��G U
>
��U V
>
��V W
(
��W X
	paramsDto
��X a
.
��a b
parLevel2Dto
��b n
.
��n o$
listParLevel3GroupDto��o �
)��� �
;��� �
List
�� 
<
�� 
ParCounterXLocal
�� !
>
��! ""
listParCounterXLocal
��# 7
=
��8 9
Mapper
��: @
.
��@ A
Map
��A D
<
��D E
List
��E I
<
��I J
ParCounterXLocal
��J Z
>
��Z [
>
��[ \
(
��\ ]
	paramsDto
��] f
.
��f g
parLevel2Dto
��g s
.
��s t#
listParCounterXLocal��t �
)��� �
;��� �
List
�� 
<
�� (
ParNotConformityRuleXLevel
�� +
>
��+ ,!
listNonCoformitRule
��- @
=
��A B
Mapper
��C I
.
��I J
Map
��J M
<
��M N
List
��N R
<
��R S(
ParNotConformityRuleXLevel
��S m
>
��m n
>
��n o
(
��o p
	paramsDto
��p y
.
��y z
parLevel2Dto��z �
.��� �1
!listParNotConformityRuleXLevelDto��� �
)��� �
;��� �
try
�� 
{
�� 
_paramsRepo
�� 
.
�� 
SaveParLevel2
�� )
(
��) *
saveParamLevel2
��* 9
,
��9 :!
listaParLevel3Group
��; N
,
��N O"
listParCounterXLocal
��P d
,
��d e!
listNonCoformitRule
��f y
,
��y z"
saveParamEvaluation��{ �
,��� �
saveParamSample��� �
,��� �
listParRelapse��� �
)��� �
;��� �
}
�� 
catch
�� 
(
�� 
DbUpdateException
�� $
e
��% &
)
��& '
{
�� 
VerifyUniqueName
��  
(
��  !
saveParamLevel2
��! 0
,
��0 1
e
��2 3
)
��3 4
;
��4 5
}
�� 
	paramsDto
�� 
.
�� 
parLevel2Dto
�� "
.
��" #
Id
��# %
=
��& '
saveParamLevel2
��( 7
.
��7 8
Id
��8 :
;
��: ;
return
�� 
	paramsDto
�� 
;
�� 
}
�� 	
public
�� 
	ParamsDTO
�� 
	GetLevel2
�� "
(
��" #
int
��# &
idParLevel2
��' 2
,
��2 3
int
��4 7
level3Id
��8 @
,
��@ A
int
��B E
level1Id
��F N
)
��N O
{
�� 	
var
�� 
	paramsDto
�� 
=
�� 
new
�� 
	ParamsDTO
��  )
(
��) *
)
��* +
;
��+ ,
var
�� 
	parLevel2
�� 
=
��  
_baseRepoParLevel2
�� .
.
��. /
GetById
��/ 6
(
��6 7
idParLevel2
��7 B
)
��B C
;
��C D
var
�� 
level2
�� 
=
�� 
Mapper
�� 
.
��  
Map
��  #
<
��# $
ParLevel2DTO
��$ 0
>
��0 1
(
��1 2
	parLevel2
��2 ;
)
��; <
;
��< =
var
�� 
headerFieldLevel1
�� !
=
��" #
db
��$ &
.
��& '#
ParLevel1XHeaderField
��' <
.
��< =
Include
��= D
(
��D E
$str
��E U
)
��U V
.
��V W
ToList
��W ]
(
��] ^
)
��^ _
;
��_ `
var
�� 
headerFieldLevel2
�� !
=
��" #
db
��$ &
.
��& '#
ParLevel2XHeaderField
��' <
.
��< =
Where
��= B
(
��B C
r
��C D
=>
��E G
r
��H I
.
��I J
IsActive
��J R
==
��S U
true
��V Z
)
��Z [
.
��[ \
ToList
��\ b
(
��b c
)
��c d
;
��d e
var
�� 

evaluation
�� 
=
�� 
	parLevel2
�� &
.
��& '
ParEvaluation
��' 4
.
��4 5
Where
��5 :
(
��: ;
r
��; <
=>
��= ?
r
��@ A
.
��A B
IsActive
��B J
==
��K M
true
��N R
)
��R S
;
��S T
var
�� 
relapse
�� 
=
�� 
	parLevel2
�� #
.
��# $

ParRelapse
��$ .
.
��. /
Where
��/ 4
(
��4 5
r
��5 6
=>
��7 9
r
��: ;
.
��; <
IsActive
��< D
==
��E G
true
��H L
)
��L M
.
��M N
OrderByDescending
��N _
(
��_ `
r
��` a
=>
��b d
r
��e f
.
��f g
IsActive
��g o
)
��o p
;
��p q
var
�� 
counter
�� 
=
�� 
	parLevel2
�� #
.
��# $
ParCounterXLocal
��$ 4
.
��4 5
Where
��5 :
(
��: ;
r
��; <
=>
��= ?
r
��@ A
.
��A B
IsActive
��B J
==
��K M
true
��N R
)
��R S
.
��S T
OrderByDescending
��T e
(
��e f
r
��f g
=>
��h j
r
��k l
.
��l m
IsActive
��m u
)
��u v
;
��v w
var
�� 
nonConformityrule
�� !
=
��" #
	parLevel2
��$ -
.
��- .(
ParNotConformityRuleXLevel
��. H
.
��H I
Where
��I N
(
��N O
r
��O P
=>
��Q S
r
��T U
.
��U V
IsActive
��V ^
==
��_ a
true
��b f
)
��f g
.
��g h
OrderByDescending
��h y
(
��y z
r
��z {
=>
��| ~
r�� �
.��� �
IsActive��� �
)��� �
;��� �
var
�� 
	headerAdd
�� 
=
�� 
headerFieldLevel1
�� -
.
��- .
Where
��. 3
(
��3 4
r
��4 5
=>
��6 8
r
��9 :
.
��: ;
IsActive
��; C
==
��D F
true
��G K
&&
��L N
r
��O P
.
��P Q
ParLevel1_Id
��Q ]
==
��^ `
level1Id
��a i
)
��i j
;
��j k
var
�� 
headerRemove
�� 
=
�� 
headerFieldLevel2
�� 0
.
��0 1
Where
��1 6
(
��6 7
r
��7 8
=>
��9 ;
r
��< =
.
��= >
IsActive
��> F
==
��G I
true
��J N
&&
��O Q
r
��R S
.
��S T
ParLevel1_Id
��T `
==
��a c
level1Id
��d l
&&
��m o
r
��p q
.
��q r
ParLevel2_Id
��r ~
==�� �
idParLevel2��� �
)��� �
;��� �
var
�� 
parLevel3Group
�� 
=
��  
	parLevel2
��! *
.
��* +
ParLevel3Group
��+ 9
.
��9 :
Where
��: ?
(
��? @
r
��@ A
=>
��B D
r
��E F
.
��F G
IsActive
��G O
==
��P R
true
��S W
)
��W X
.
��X Y
OrderByDescending
��Y j
(
��j k
r
��k l
=>
��m o
r
��p q
.
��q r
IsActive
��r z
)
��z {
;
��{ |
level2
�� 
.
�� 
listEvaluation
�� !
=
��" #
Mapper
��$ *
.
��* +
Map
��+ .
<
��. /
List
��/ 3
<
��3 4
ParEvaluationDTO
��4 D
>
��D E
>
��E F
(
��F G

evaluation
��G Q
)
��Q R
;
��R S
if
�� 
(
�� 
	parLevel2
�� 
.
�� 
	ParSample
�� #
.
��# $
Count
��$ )
(
��) *
)
��* +
>
��, -
$num
��. /
)
��/ 0
level2
�� 
.
�� 

listSample
�� !
=
��" #
Mapper
��$ *
.
��* +
Map
��+ .
<
��. /
List
��/ 3
<
��3 4
ParSampleDTO
��4 @
>
��@ A
>
��A B
(
��B C
	parLevel2
��C L
.
��L M
	ParSample
��M V
.
��V W
Where
��W \
(
��\ ]
r
��] ^
=>
��_ a
r
��b c
.
��c d
IsActive
��d l
==
��m o
true
��p t
)
��t u
)
��u v
;
��v w
level2
�� 
.
�� +
RecuperaListaSampleEvaluation
�� 0
(
��0 1
)
��1 2
;
��2 3
level2
�� 
.
�� 
listParRelapseDto
�� $
=
��% &
Mapper
��' -
.
��- .
Map
��. 1
<
��1 2
List
��2 6
<
��6 7
ParRelapseDTO
��7 D
>
��D E
>
��E F
(
��F G
relapse
��G N
)
��N O
;
��O P
level2
�� 
.
�� "
listParCounterXLocal
�� '
=
��( )
Mapper
��* 0
.
��0 1
Map
��1 4
<
��4 5
List
��5 9
<
��9 :!
ParCounterXLocalDTO
��: M
>
��M N
>
��N O
(
��O P
counter
��P W
)
��W X
;
��X Y
level2
�� 
.
�� /
!listParNotConformityRuleXLevelDto
�� 4
=
��5 6
Mapper
��7 =
.
��= >
Map
��> A
<
��A B
List
��B F
<
��F G+
ParNotConformityRuleXLevelDTO
��G d
>
��d e
>
��e f
(
��f g
nonConformityrule
��g x
)
��x y
;
��y z
level2
�� 
.
��  
cabecalhosInclusos
�� %
=
��& '
Mapper
��( .
.
��. /
Map
��/ 2
<
��2 3
List
��3 7
<
��7 8&
ParLevel1XHeaderFieldDTO
��8 P
>
��P Q
>
��Q R
(
��R S
	headerAdd
��S \
)
��\ ]
;
��] ^
level2
�� 
.
��  
cabecalhosExclusos
�� %
=
��& '
Mapper
��( .
.
��. /
Map
��/ 2
<
��2 3
List
��3 7
<
��7 8&
ParLevel2XHeaderFieldDTO
��8 P
>
��P Q
>
��Q R
(
��R S
headerRemove
��S _
)
��_ `
;
��` a
var
��  
vinculosComOLevel2
�� "
=
��# $
	parLevel2
��% .
.
��. /
ParLevel3Level2
��/ >
.
��> ?
Where
��? D
(
��D E
r
��E F
=>
��G I
r
��J K
.
��K L
IsActive
��L T
==
��U W
true
��X \
)
��\ ]
;
��] ^
if
�� 
(
�� 
level1Id
�� 
>
�� 
$num
�� 
)
�� 
{
��  
vinculosComOLevel2
�� "
=
��# $
db
��% '
.
��' (
ParLevel3Level2
��( 7
.
��7 8
Where
��8 =
(
��= >
r
��> ?
=>
��@ B
r
��C D
.
��D E
ParLevel2_Id
��E Q
==
��R T
idParLevel2
��U `
&&
��a c
r
��d e
.
��e f
IsActive
��f n
==
��o q
true
��r v
&&
��w y
r
��z {
.
��{ |$
ParLevel3Level2Level1��| �
.��� �
Any��� �
(��� �
c��� �
=>��� �
c��� �
.��� �
ParLevel1_Id��� �
==��� �
level1Id��� �
)��� �
)��� �
.��� �
ToList��� �
(��� �
)��� �
;��� �
}
�� 
level2
�� 
.
�� $
listParLevel3Level2Dto
�� )
=
��* +
Mapper
��, 2
.
��2 3
Map
��3 6
<
��6 7
List
��7 ;
<
��; < 
ParLevel3Level2DTO
��< N
>
��N O
>
��O P
(
��P Q 
vinculosComOLevel2
��Q c
)
��c d
;
��d e
level2
�� 
.
�� 6
(CreateSelectListParamsViewModelListLevel
�� ;
(
��; <
Mapper
��< B
.
��B C
Map
��C F
<
��F G
List
��G K
<
��K L
ParLevel3DTO
��L X
>
��X Y
>
��Y Z
(
��Z [#
_baseRepoParLevel3NLL
��[ p
.
��p q
GetAll
��q w
(
��w x
)
��x y
.
��y z
Where
��z 
(�� �
r��� �
=>��� �
r��� �
.��� �
IsActive��� �
==��� �
true��� �
)��� �
)��� �
,��� �
level2��� �
.��� �&
listParLevel3Level2Dto��� �
)��� �
;��� �
	paramsDto
�� 
.
�� +
parNotConformityRuleXLevelDto
�� 3
=
��4 5
new
��6 9+
ParNotConformityRuleXLevelDTO
��: W
(
��W X
)
��X Y
;
��Y Z
	paramsDto
�� 
.
�� #
listParLevel3GroupDto
�� +
=
��, -
new
��. 1
List
��2 6
<
��6 7
ParLevel3GroupDTO
��7 H
>
��H I
(
��I J
)
��J K
;
��K L
level2
�� 
.
�� #
listParLevel3GroupDto
�� (
=
��) *
Mapper
��+ 1
.
��1 2
Map
��2 5
<
��5 6
List
��6 :
<
��: ;
ParLevel3GroupDTO
��; L
>
��L M
>
��M N
(
��N O
parLevel3Group
��O ]
)
��] ^
;
��^ _
if
�� 
(
�� 
	parLevel2
�� 
.
�� 
ParLevel3Level2
�� )
.
��) *
FirstOrDefault
��* 8
(
��8 9
r
��9 :
=>
��; =
r
��> ?
.
��? @
ParLevel3_Id
��@ L
==
��M O
level3Id
��P X
&&
��Y [
r
��\ ]
.
��] ^
IsActive
��^ f
==
��g i
true
��j n
)
��n o
!=
��p r
null
��s w
)
��w x
level2
�� 
.
�� &
pesoDoVinculoSelecionado
�� /
=
��0 1
	parLevel2
��2 ;
.
��; <
ParLevel3Level2
��< K
.
��K L
FirstOrDefault
��L Z
(
��Z [
r
��[ \
=>
��] _
r
��` a
.
��a b
ParLevel3_Id
��b n
==
��o q
level3Id
��r z
&&
��{ }
r
��~ 
.�� �
IsActive��� �
==��� �
true��� �
)��� �
.��� �
Weight��� �
;��� �
else
�� 
level2
�� 
.
�� &
pesoDoVinculoSelecionado
�� /
=
��0 1
$num
��2 3
;
��3 4
	paramsDto
�� 
.
�� 
parLevel2Dto
�� "
=
��# $
level2
��% +
;
��+ ,
	paramsDto
�� 
.
�� 
parLevel2Dto
�� "
.
��" #$
listParLevel3Level2Dto
��# 9
=
��: ;
null
��< @
;
��@ A
if
�� 
(
�� 
level1Id
�� 
>
�� 
$num
�� 
)
�� 
{
�� 
var
�� $
possuiVinculoComLevel1
�� *
=
��+ ,
db
��- /
.
��/ 0
ParLevel2Level1
��0 ?
.
��? @
Where
��@ E
(
��E F
r
��F G
=>
��H J
r
��K L
.
��L M
ParLevel1_Id
��M Y
==
��Z \
level1Id
��] e
&&
��f h
r
��i j
.
��j k
ParLevel2_Id
��k w
==
��x z
level2��{ �
.��� �
Id��� �
)��� �
;��� �
if
�� 
(
�� $
possuiVinculoComLevel1
�� *
!=
��+ -
null
��. 2
&&
��3 5$
possuiVinculoComLevel1
��6 L
.
��L M
Count
��M R
(
��R S
)
��S T
>
��U V
$num
��W X
)
��X Y
	paramsDto
�� 
.
�� 
parLevel2Dto
�� *
.
��* +
isVinculado
��+ 6
=
��7 8
true
��9 =
;
��= >
}
�� 
if
�� 
(
�� 
level1Id
�� 
>
�� 
$num
�� 
)
�� 
	paramsDto
�� 
.
�� 
parLevel2Dto
�� &
.
��& ' 
RegrasParamsLevel1
��' 9
(
��9 :
Mapper
��: @
.
��@ A
Map
��A D
<
��D E
ParLevel1DTO
��E Q
>
��Q R
(
��R S
db
��S U
.
��U V
	ParLevel1
��V _
.
��_ `
FirstOrDefault
��` n
(
��n o
r
��o p
=>
��q s
r
��t u
.
��u v
Id
��v x
==
��y {
level1Id��| �
)��� �
)��� �
)��� �
;��� �
return
�� 
	paramsDto
�� 
;
�� 
}
�� 	
public
�� 
	ParamsDTO
��  
RemoveLevel03Group
�� +
(
��+ ,
int
��, /
Id
��0 2
)
��2 3
{
�� 	
var
�� 
parLevel3Group
�� 
=
��  
Mapper
��! '
.
��' (
Map
��( +
<
��+ ,
ParLevel3Group
��, :
>
��: ;
(
��; <!
_baseParLevel3Group
��< O
.
��O P
GetAll
��P V
(
��V W
)
��W X
.
��X Y
Where
��Y ^
(
��^ _
r
��_ `
=>
��a c
r
��d e
.
��e f
Id
��f h
==
��i k
Id
��l n
)
��n o
.
��o p
FirstOrDefault
��p ~
(
��~ 
)�� �
)��� �
;��� �
if
�� 
(
�� 
parLevel3Group
�� 
!=
�� !
null
��" &
)
��& '
{
�� 
_paramsRepo
�� 
.
�� "
RemoveParLevel3Group
�� 0
(
��0 1
parLevel3Group
��1 ?
)
��? @
;
��@ A
}
�� 
return
�� 
null
�� 
;
�� 
}
�� 	
public
�� 
	ParamsDTO
�� 
AddUpdateLevel3
�� (
(
��( )
	ParamsDTO
��) 2
	paramsDto
��3 <
)
��< =
{
�� 	
	ParLevel3
�� 
saveParamLevel3
�� %
=
��& '
Mapper
��( .
.
��. /
Map
��/ 2
<
��2 3
	ParLevel3
��3 <
>
��< =
(
��= >
	paramsDto
��> G
.
��G H
parLevel3Dto
��H T
)
��T U
;
��U V
if
�� 
(
�� 
	paramsDto
�� 
.
�� 
parLevel3Dto
�� &
.
��& '
listLevel3Value
��' 6
!=
��7 9
null
��: >
)
��> ?
if
�� 
(
�� 
	paramsDto
�� 
.
�� 
parLevel3Dto
�� *
.
��* +
listLevel3Value
��+ :
.
��: ;
Count
��; @
(
��@ A
)
��A B
>
��C D
$num
��E F
)
��F G
	paramsDto
�� 
.
�� 
parLevel3Dto
�� *
.
��* +
listLevel3Value
��+ :
.
��: ;
ForEach
��; B
(
��B C
r
��C D
=>
��E G
r
��H I
.
��I J&
preparaParaInsertEmBanco
��J b
(
��b c
)
��c d
)
��d e
;
��e f
List
�� 
<
�� 
ParLevel3Value
�� 
>
��  &
listSaveParamLevel3Value
��! 9
=
��: ;
Mapper
��< B
.
��B C
Map
��C F
<
��F G
List
��G K
<
��K L
ParLevel3Value
��L Z
>
��Z [
>
��[ \
(
��\ ]
	paramsDto
��] f
.
��f g
parLevel3Dto
��g s
.
��s t
listLevel3Value��t �
)��� �
;��� �
List
�� 
<
�� 

ParRelapse
�� 
>
�� 
listParRelapse
�� +
=
��, -
Mapper
��. 4
.
��4 5
Map
��5 8
<
��8 9
List
��9 =
<
��= >

ParRelapse
��> H
>
��H I
>
��I J
(
��J K
	paramsDto
��K T
.
��T U
parLevel3Dto
��U a
.
��a b
listParRelapseDto
��b s
)
��s t
;
��t u
if
�� 
(
�� 
	paramsDto
�� 
.
�� 
parLevel3Dto
�� &
.
��& '
listLevel3Level2
��' 7
!=
��8 :
null
��; ?
)
��? @
if
�� 
(
�� 
	paramsDto
�� 
.
�� 
parLevel3Dto
�� *
.
��* +
listLevel3Level2
��+ ;
.
��; <
Count
��< A
(
��A B
)
��B C
>
��D E
$num
��F G
)
��G H
{
�� 
	paramsDto
�� 
.
�� 
parLevel3Dto
�� *
.
��* +
listLevel3Level2
��+ ;
.
��; <
ForEach
��< C
(
��C D
r
��D E
=>
��F H
r
��I J
.
��J K&
preparaParaInsertEmBanco
��K c
(
��c d
)
��d e
)
��e f
;
��f g
}
�� 
List
�� 
<
�� 
ParLevel3Level2
��  
>
��  !!
parLevel3Level2peso
��" 5
=
��6 7
Mapper
��8 >
.
��> ?
Map
��? B
<
��B C
List
��C G
<
��G H
ParLevel3Level2
��H W
>
��W X
>
��X Y
(
��Y Z
	paramsDto
��Z c
.
��c d
parLevel3Dto
��d p
.
��p q
listLevel3Level2��q �
)��� �
;��� �
try
�� 
{
�� 
_paramsRepo
�� 
.
�� 
SaveParLevel3
�� )
(
��) *
saveParamLevel3
��* 9
,
��9 :&
listSaveParamLevel3Value
��; S
,
��S T
listParRelapse
��U c
,
��c d!
parLevel3Level2peso
��e x
?
��x y
.
��y z
ToList��z �
(��� �
)��� �
,��� �
	paramsDto��� �
.��� �
level1Selected��� �
)��� �
;��� �
db
�� 
.
�� 
Database
�� 
.
�� 
ExecuteSqlCommand
�� -
(
��- .
string
��. 4
.
��4 5
Format
��5 ;
(
��; <
$str
��< s
,
��s t
	paramsDto
��u ~
.
��~ 
parLevel3Dto�� �
.��� �
IsPointLess��� �
?��� �
$str��� �
:��� �
$str��� �
,��� �
saveParamLevel3��� �
.��� �
Id��� �
)��� �
)��� �
;��� �
db
�� 
.
�� 
Database
�� 
.
�� 
ExecuteSqlCommand
�� -
(
��- .
string
��. 4
.
��4 5
Format
��5 ;
(
��; <
$str
��< o
,
��o p
	paramsDto
��q z
.
��z {
parLevel3Dto��{ �
.��� �
AllowNA��� �
?��� �
$str��� �
:��� �
$str��� �
,��� �
saveParamLevel3��� �
.��� �
Id��� �
)��� �
)��� �
;��� �
if
�� 
(
�� 
	paramsDto
�� 
.
�� 
parLevel3Dto
�� *
.
��* +&
ParLevel3Value_OuterList
��+ C
!=
��D F
null
��G K
)
��K L
foreach
�� 
(
�� 
var
��  
i
��! "
in
��# %
	paramsDto
��& /
.
��/ 0
parLevel3Dto
��0 <
.
��< =&
ParLevel3Value_OuterList
��= U
)
��U V
{
�� 
if
�� 
(
�� 
i
�� 
.
�� 
Id
��  
<=
��! #
$num
��$ %
)
��% &
{
�� 
i
�� 
.
�� 
ParLevel3_Id
�� *
=
��+ ,
saveParamLevel3
��- <
.
��< =
Id
��= ?
;
��? @
i
�� 
.
�� 
ParLevel3_Name
�� ,
=
��- .
saveParamLevel3
��/ >
.
��> ?
Name
��? C
;
��C D
i
�� 
.
�� 
IsActive
�� &
=
��' (
true
��) -
;
��- .
db
�� 
.
�� "
ParLevel3Value_Outer
�� 3
.
��3 4
Add
��4 7
(
��7 8
Mapper
��8 >
.
��> ?
Map
��? B
<
��B C"
ParLevel3Value_Outer
��C W
>
��W X
(
��X Y
i
��Y Z
)
��Z [
)
��[ \
;
��\ ]
}
�� 
else
�� 
{
�� 
var
�� 1
#queryUpdateParLevel3Value_OuterList
��  C
=
��D E
string
��F L
.
��L M
Format
��M S
(
��S T
$str
��T /
,
��/ 0
$str
��1 <
,
��< =
i
��> ?
.
��? @
IsActive
��@ H
?
��I J
$str
��K N
:
��O P
$str
��Q T
,
��T U
i
��V W
.
��W X
Id
��X Z
)
��Z [
;
��[ \
db
�� 
.
�� 
Database
�� '
.
��' (
ExecuteSqlCommand
��( 9
(
��9 :1
#queryUpdateParLevel3Value_OuterList
��: ]
)
��] ^
;
��^ _
}
�� 
}
�� 
db
�� 
.
�� 
SaveChanges
�� 
(
�� 
)
��  
;
��  !
if
�� 
(
�� !
parLevel3Level2peso
�� '
!=
��( *
null
��+ /
)
��/ 0
foreach
�� 
(
�� 
var
��  
i
��! "
in
��# %!
parLevel3Level2peso
��& 9
?
��9 :
.
��: ;
Where
��; @
(
��@ A
r
��A B
=>
��C E
r
��F G
.
��G H
IsActive
��H P
)
��P Q
)
��Q R
AddVinculoL1L2
�� &
(
��& '
	paramsDto
��' 0
.
��0 1
level1Selected
��1 ?
,
��? @
	paramsDto
��A J
.
��J K
level2Selected
��K Y
,
��Y Z
saveParamLevel3
��[ j
.
��j k
Id
��k m
,
��m n
$num
��o p
,
��p q
i
��r s
.
��s t
ParCompany_Id��t �
)��� �
;��� �
}
�� 
catch
�� 
(
�� 
DbUpdateException
�� $
e
��% &
)
��& '
{
�� 
VerifyUniqueName
��  
(
��  !
saveParamLevel3
��! 0
,
��0 1
e
��2 3
)
��3 4
;
��4 5
}
�� 
	paramsDto
�� 
.
�� 
parLevel3Dto
�� "
.
��" #
Id
��# %
=
��& '
saveParamLevel3
��( 7
.
��7 8
Id
��8 :
;
��: ;
return
�� 
	paramsDto
�� 
;
�� 
}
�� 	
public
�� 
	ParamsDTO
�� #
AddUpdateLevel3Level2
�� .
(
��. /
	ParamsDTO
��/ 8
	paramsDto
��9 B
)
��B C
{
�� 	
ParLevel3Level2
�� "
saveParamLevel3Leve2
�� 0
=
��1 2
Mapper
��3 9
.
��9 :
Map
��: =
<
��= >
ParLevel3Level2
��> M
>
��M N
(
��N O
	paramsDto
��O X
.
��X Y
parLevel3Level2
��Y h
)
��h i
;
��i j
_paramsRepo
�� 
.
�� !
SaveParLevel3Level2
�� +
(
��+ ,"
saveParamLevel3Leve2
��, @
)
��@ A
;
��A B
	paramsDto
�� 
.
�� 
parLevel3Dto
�� "
.
��" #
Id
��# %
=
��& '"
saveParamLevel3Leve2
��( <
.
��< =
Id
��= ?
;
��? @
return
�� 
	paramsDto
�� 
;
�� 
}
�� 	
public
�� 
	ParamsDTO
�� 
	GetLevel3
�� "
(
��" #
int
��# &
idParLevel3
��' 2
,
��2 3
int
��4 7
?
��7 8
idParLevel2
��9 D
=
��E F
$num
��G H
)
��H I
{
�� 	
	ParamsDTO
�� 
retorno
�� 
=
�� 
new
��  #
	ParamsDTO
��$ -
(
��- .
)
��. /
;
��/ 0
var
�� 
	parlevel3
�� 
=
��  
_baseRepoParLevel3
�� .
.
��. /
GetById
��/ 6
(
��6 7
idParLevel3
��7 B
)
��B C
;
��C D
var
�� 
level3
�� 
=
�� 
Mapper
�� 
.
��  
Map
��  #
<
��# $
ParLevel3DTO
��$ 0
>
��0 1
(
��1 2
	parlevel3
��2 ;
)
��; <
;
��< =
var
�� 
relapse
�� 
=
�� 
	parlevel3
�� #
.
��# $

ParRelapse
��$ .
.
��. /
Where
��/ 4
(
��4 5
r
��5 6
=>
��7 9
r
��: ;
.
��; <
IsActive
��< D
==
��E G
true
��H L
)
��L M
.
��M N
OrderByDescending
��N _
(
��_ `
r
��` a
=>
��b d
r
��e f
.
��f g
IsActive
��g o
)
��o p
;
��p q
var
�� 
group
�� 
=
�� 
db
�� 
.
�� 
ParLevel3Group
�� )
.
��) *
Where
��* /
(
��/ 0
r
��0 1
=>
��2 4
r
��5 6
.
��6 7
ParLevel2_Id
��7 C
==
��D F
idParLevel2
��G R
&&
��S U
r
��V W
.
��W X
IsActive
��X `
==
��a c
true
��d h
)
��h i
.
��i j
ToList
��j p
(
��p q
)
��q r
;
��r s
var
�� 
level3Level2
�� 
=
�� 
	parlevel3
�� (
.
��( )
ParLevel3Level2
��) 8
.
��8 9
Where
��9 >
(
��> ?
r
��? @
=>
��A C
r
��D E
.
��E F
ParLevel2_Id
��F R
==
��S U
idParLevel2
��V a
&&
��b d
r
��e f
.
��f g
ParLevel3_Id
��g s
==
��t v
idParLevel3��w �
&&��� �
r��� �
.��� �
IsActive��� �
==��� �
true��� �
)��� �
.��� �!
OrderByDescending��� �
(��� �
r��� �
=>��� �
r��� �
.��� �
IsActive��� �
)��� �
;��� �
var
�� 
level3Value
�� 
=
�� 
	parlevel3
�� '
.
��' (
ParLevel3Value
��( 6
.
��6 7
Where
��7 <
(
��< =
r
��= >
=>
��? A
r
��B C
.
��C D
IsActive
��D L
==
��M O
true
��P T
)
��T U
.
��U V
OrderByDescending
��V g
(
��g h
r
��h i
=>
��j l
r
��m n
.
��n o
IsActive
��o w
)
��w x
;
��x y
var
�� #
parlevel3Reencravacao
�� %
=
��& '
db
��( *
.
��* +"
ParLevel3Value_Outer
��+ ?
.
��? @
Where
��@ E
(
��E F
r
��F G
=>
��H J
r
��K L
.
��L M
IsActive
��M U
&&
��V X
r
��Y Z
.
��Z [
ParLevel3_Id
��[ g
==
��h j
	parlevel3
��k t
.
��t u
Id
��u w
)
��w x
.
��x y
ToList
��y 
(�� �
)��� �
;��� �
level3
�� 
.
�� 
listParRelapseDto
�� $
=
��% &
Mapper
��' -
.
��- .
Map
��. 1
<
��1 2
List
��2 6
<
��6 7
ParRelapseDTO
��7 D
>
��D E
>
��E F
(
��F G
relapse
��G N
)
��N O
;
��O P
level3
�� 
.
�� 
listGroupsLevel2
�� #
=
��$ %
Mapper
��& ,
.
��, -
Map
��- 0
<
��0 1
List
��1 5
<
��5 6
ParLevel3GroupDTO
��6 G
>
��G H
>
��H I
(
��I J
group
��J O
)
��O P
;
��P Q
level3
�� 
.
�� 
listLevel3Level2
�� #
=
��$ %
Mapper
��& ,
.
��, -
Map
��- 0
<
��0 1
List
��1 5
<
��5 6 
ParLevel3Level2DTO
��6 H
>
��H I
>
��I J
(
��J K
level3Level2
��K W
)
��W X
;
��X Y
level3
�� 
.
�� 
listLevel3Value
�� "
=
��# $
Mapper
��% +
.
��+ ,
Map
��, /
<
��/ 0
List
��0 4
<
��4 5
ParLevel3ValueDTO
��5 F
>
��F G
>
��G H
(
��H I
level3Value
��I T
)
��T U
;
��U V
retorno
�� 
.
�� 
parLevel3Value
�� "
=
��# $
new
��% (
ParLevel3ValueDTO
��) :
(
��: ;
)
��; <
;
��< =
level3
�� 
.
�� &
ParLevel3Value_OuterList
�� +
=
��, -
Mapper
��. 4
.
��4 5
Map
��5 8
<
��8 9
List
��9 =
<
��= >)
ParLevel3Value_OuterListDTO
��> Y
>
��Y Z
>
��Z [
(
��[ \#
parlevel3Reencravacao
��\ q
)
��q r
;
��r s
level3
�� 
.
�� -
ParLevel3Value_OuterListGrouped
�� 2
=
��3 4
level3
��5 ;
.
��; <&
ParLevel3Value_OuterList
��< T
.
��T U
GroupBy
��U \
(
��\ ]
r
��] ^
=>
��^ `
r
��` a
.
��a b
ParCompany_Id
��b o
)
��o p
;
��p q
if
�� 
(
�� 
level3
�� 
.
�� 
listLevel3Level2
�� '
.
��' (
Count
��( -
(
��- .
)
��. /
>
��0 1
$num
��2 3
)
��3 4
level3
�� 
.
�� 

hasVinculo
�� !
=
��" #
true
��$ (
;
��( )
foreach
�� 
(
�� 
var
�� 
Level3Value
�� $
in
��% '
level3
��( .
.
��. /
listLevel3Value
��/ >
)
��> ?
Level3Value
�� 
.
�� 

PreparaGet
�� &
(
��& '
)
��' (
;
��( )
var
�� 
	pointLess
�� 
=
�� 
db
�� 
.
�� 
Database
�� '
.
��' (
SqlQuery
��( 0
<
��0 1
bool
��1 5
>
��5 6
(
��6 7
string
��7 =
.
��= >
Format
��> D
(
��D E
$str
��E w
,
��w x
level3
��y 
.�� �
Id��� �
)��� �
)��� �
.��� �
FirstOrDefault��� �
(��� �
)��� �
;��� �
level3
�� 
.
�� 
IsPointLess
�� 
=
��  
	pointLess
��! *
;
��* +
var
�� 
AllowNA
�� 
=
�� 
db
�� 
.
�� 
Database
�� %
.
��% &
SqlQuery
��& .
<
��. /
bool
��/ 3
>
��3 4
(
��4 5
string
��5 ;
.
��; <
Format
��< B
(
��B C
$str
��C q
,
��q r
level3
��s y
.
��y z
Id
��z |
)
��| }
)
��} ~
.
��~ 
FirstOrDefault�� �
(��� �
)��� �
;��� �
level3
�� 
.
�� 
AllowNA
�� 
=
�� 
AllowNA
�� $
;
��$ %
retorno
�� 
.
�� 
parLevel3Dto
��  
=
��! "
level3
��# )
;
��) *
return
�� 
retorno
�� 
;
�� 
}
�� 	
public
�� 
	ParamsDTO
�� 1
#AddUpdateParNotConformityRuleXLevel
�� <
(
��< =
	ParamsDTO
��= F
	paramsDto
��G P
)
��P Q
{
�� 	(
ParNotConformityRuleXLevel
�� &,
saveParNotConformityRuleXLevel
��' E
=
��F G
Mapper
��H N
.
��N O
Map
��O R
<
��R S(
ParNotConformityRuleXLevel
��S m
>
��m n
(
��n o
	paramsDto
��o x
.
��x y,
parNotConformityRuleXLevelDto��y �
)��� �
;��� �
_paramsRepo
�� 
.
�� ,
SaveParNotConformityRuleXLevel
�� 6
(
��6 7,
saveParNotConformityRuleXLevel
��7 U
)
��U V
;
��V W
	paramsDto
�� 
.
�� +
parNotConformityRuleXLevelDto
�� 3
.
��3 4
Id
��4 6
=
��7 8,
saveParNotConformityRuleXLevel
��9 W
.
��W X
Id
��X Z
;
��Z [
return
�� 
	paramsDto
�� 
;
�� 
}
�� 	
private
�� 
static
�� 
void
�� 
VerifyUniqueName
�� ,
<
��, -
T
��- .
>
��. /
(
��/ 0
T
��0 1
obj
��2 5
,
��5 6
DbUpdateException
��7 H
e
��I J
)
��J K
{
�� 	
if
�� 
(
�� 
e
�� 
.
�� 
InnerException
��  
!=
��! #
null
��$ (
)
��( )
if
�� 
(
�� 
e
�� 
.
�� 
InnerException
�� $
.
��$ %
InnerException
��% 3
!=
��4 6
null
��7 ;
)
��; <
{
�� 
SqlException
��  
innerException
��! /
=
��0 1
e
��2 3
.
��3 4
InnerException
��4 B
.
��B C
InnerException
��C Q
as
��R T
SqlException
��U a
;
��a b
if
�� 
(
�� 
innerException
�� &
!=
��' )
null
��* .
&&
��/ 1
(
��2 3
innerException
��3 A
.
��A B
Number
��B H
==
��I K
$num
��L P
||
��Q S
innerException
��T b
.
��b c
Number
��c i
==
��j l
$num
��m q
)
��q r
)
��r s
{
�� 
if
�� 
(
�� 
innerException
�� *
.
��* +
Message
��+ 2
.
��2 3
IndexOf
��3 :
(
��: ;
$str
��; A
)
��A B
>
��C D
$num
��E F
)
��F G
throw
�� !
new
��" %
ExceptionHelper
��& 5
(
��5 6
$str
��6 @
+
��A B
obj
��C F
.
��F G
GetType
��G N
(
��N O
)
��O P
.
��P Q
GetProperty
��Q \
(
��\ ]
$str
��] c
)
��c d
.
��d e
GetValue
��e m
(
��m n
obj
��n q
)
��q r
+
��s t
$str��u �
)��� �
;��� �
}
�� 
else
�� 
{
�� 
throw
�� 
e
�� 
;
��  
}
�� 
}
�� 
}
�� 	
public
�� 
	ParamsDdl
�� $
CarregaDropDownsParams
�� /
(
��/ 0
)
��0 1
{
�� 	
lock
�� 
(
�� 
this
�� 
)
�� 
{
�� 
var
�� !
DdlParConsolidation
�� '
=
��( )
Mapper
��* 0
.
��0 1
Map
��1 4
<
��4 5
List
��5 9
<
��9 :%
ParConsolidationTypeDTO
��: Q
>
��Q R
>
��R S
(
��S T'
_baseParConsolidationType
��T m
.
��m n!
GetAllAsNoTracking��n �
(��� �
)��� �
)��� �
;��� �
var
�� 
DdlFrequency
��  
=
��! "
Mapper
��# )
.
��) *
Map
��* -
<
��- .
List
��. 2
<
��2 3
ParFrequencyDTO
��3 B
>
��B C
>
��C D
(
��D E
_baseParFrequency
��E V
.
��V W 
GetAllAsNoTracking
��W i
(
��i j
)
��j k
)
��k l
;
��l m
var
�� 
DdlparLevel1
��  
=
��! "
Mapper
��# )
.
��) *
Map
��* -
<
��- .
List
��. 2
<
��2 3
ParLevel1DTO
��3 ?
>
��? @
>
��@ A
(
��A B#
_baseRepoParLevel1NLL
��B W
.
��W X 
GetAllAsNoTracking
��X j
(
��j k
)
��k l
)
��l m
;
��m n
var
�� 
DdlparLevel2
��  
=
��! "
Mapper
��# )
.
��) *
Map
��* -
<
��- .
List
��. 2
<
��2 3
ParLevel2DTO
��3 ?
>
��? @
>
��@ A
(
��A B#
_baseRepoParLevel2NLL
��B W
.
��W X 
GetAllAsNoTracking
��X j
(
��j k
)
��k l
)
��l m
;
��m n
var
�� 
DdlparLevel3
��  
=
��! "
Mapper
��# )
.
��) *
Map
��* -
<
��- .
List
��. 2
<
��2 3
ParLevel3DTO
��3 ?
>
��? @
>
��@ A
(
��A B#
_baseRepoParLevel3NLL
��B W
.
��W X 
GetAllAsNoTracking
��X j
(
��j k
)
��k l
)
��l m
;
��m n
var
�� 
DdlparCluster
�� !
=
��" #
Mapper
��$ *
.
��* +
Map
��+ .
<
��. /
List
��/ 3
<
��3 4
ParClusterDTO
��4 A
>
��A B
>
��B C
(
��C D
_baseParCluster
��D S
.
��S T 
GetAllAsNoTracking
��T f
(
��f g
)
��g h
)
��h i
;
��i j
var
�� #
DdlparLevelDefinition
�� )
=
��* +
Mapper
��, 2
.
��2 3
Map
��3 6
<
��6 7
List
��7 ;
<
��; <"
ParLevelDefinitonDTO
��< P
>
��P Q
>
��Q R
(
��R S$
_baseParLevelDefiniton
��S i
.
��i j 
GetAllAsNoTracking
��j |
(
��| }
)
��} ~
)
��~ 
;�� �
var
�� 
DdlParFieldType
�� #
=
��$ %
Mapper
��& ,
.
��, -
Map
��- 0
<
��0 1
List
��1 5
<
��5 6
ParFieldTypeDTO
��6 E
>
��E F
>
��F G
(
��G H
_baseParFieldType
��H Y
.
��Y Z 
GetAllAsNoTracking
��Z l
(
��l m
)
��m n
)
��n o
;
��o p
var
�� 
DdlParDepartment
�� $
=
��% &
Mapper
��' -
.
��- .
Map
��. 1
<
��1 2
List
��2 6
<
��6 7
ParDepartmentDTO
��7 G
>
��G H
>
��H I
(
��I J 
_baseParDepartment
��J \
.
��\ ] 
GetAllAsNoTracking
��] o
(
��o p
)
��p q
)
��q r
;
��r s
var
�� %
DdlParNotConformityRule
�� +
=
��, -
Mapper
��. 4
.
��4 5
Map
��5 8
<
��8 9
List
��9 =
<
��= >%
ParNotConformityRuleDTO
��> U
>
��U V
>
��V W
(
��W X'
_baseParNotConformityRule
��X q
.
��q r!
GetAllAsNoTracking��r �
(��� �
)��� �
)��� �
;��� �
var
��  
DdlParLocal_Level1
�� &
=
��' (
Mapper
��) /
.
��/ 0
Map
��0 3
<
��3 4
List
��4 8
<
��8 9
ParLocalDTO
��9 D
>
��D E
>
��E F
(
��F G
_baseParLocal
��G T
.
��T U 
GetAllAsNoTracking
��U g
(
��g h
)
��h i
.
��i j
Where
��j o
(
��o p
p
��p q
=>
��r t
p
��u v
.
��v w
Level
��w |
==
��} 
$num��� �
)��� �
)��� �
;��� �
var
��  
DdlParLocal_Level2
�� &
=
��' (
Mapper
��) /
.
��/ 0
Map
��0 3
<
��3 4
List
��4 8
<
��8 9
ParLocalDTO
��9 D
>
��D E
>
��E F
(
��F G
_baseParLocal
��G T
.
��T U 
GetAllAsNoTracking
��U g
(
��g h
)
��h i
.
��i j
Where
��j o
(
��o p
p
��p q
=>
��r t
p
��u v
.
��v w
Level
��w |
==
��} 
$num��� �
)��� �
)��� �
;��� �
var
�� "
DdlParCounter_Level1
�� (
=
��) *
Mapper
��+ 1
.
��1 2
Map
��2 5
<
��5 6
List
��6 :
<
��: ;
ParCounterDTO
��; H
>
��H I
>
��I J
(
��J K
_baseParCounter
��K Z
.
��Z [ 
GetAllAsNoTracking
��[ m
(
��m n
)
��n o
.
��o p
Where
��p u
(
��u v
p
��v w
=>
��x z
p
��{ |
.
��| }
Level��} �
==��� �
$num��� �
)��� �
.��� �
Where��� �
(��� �
p��� �
=>��� �
p��� �
.��� �
Hashkey��� �
!=��� �
null��� �
)��� �
)��� �
;��� �
var
�� "
DdlParCounter_Level2
�� (
=
��) *
Mapper
��+ 1
.
��1 2
Map
��2 5
<
��5 6
List
��6 :
<
��: ;
ParCounterDTO
��; H
>
��H I
>
��I J
(
��J K
_baseParCounter
��K Z
.
��Z [ 
GetAllAsNoTracking
��[ m
(
��m n
)
��n o
.
��o p
Where
��p u
(
��u v
p
��v w
=>
��x z
p
��{ |
.
��| }
Level��} �
==��� �
$num��� �
)��� �
.��� �
Where��� �
(��� �
p��� �
=>��� �
p��� �
.��� �
Hashkey��� �
!=��� �
null��� �
)��� �
)��� �
;��� �
var
�� #
DdlParLevel3InputType
�� )
=
��* +
Mapper
��, 2
.
��2 3
Map
��3 6
<
��6 7
List
��7 ;
<
��; <#
ParLevel3InputTypeDTO
��< Q
>
��Q R
>
��R S
(
��S T%
_baseParLevel3InputType
��T k
.
��k l 
GetAllAsNoTracking
��l ~
(
��~ 
)�� �
)��� �
;��� �
var
�� #
DdlParMeasurementUnit
�� )
=
��* +
Mapper
��, 2
.
��2 3
Map
��3 6
<
��6 7
List
��7 ;
<
��; <#
ParMeasurementUnitDTO
��< Q
>
��Q R
>
��R S
(
��S T%
_baseParMeasurementUnit
��T k
.
��k l 
GetAllAsNoTracking
��l ~
(
��~ 
)�� �
)��� �
;��� �
var
�� #
DdlParLevel3BoolFalse
�� )
=
��* +
Mapper
��, 2
.
��2 3
Map
��3 6
<
��6 7
List
��7 ;
<
��; <#
ParLevel3BoolFalseDTO
��< Q
>
��Q R
>
��R S
(
��S T%
_baseParLevel3BoolFalse
��T k
.
��k l 
GetAllAsNoTracking
��l ~
(
��~ 
)�� �
)��� �
;��� �
var
�� "
DdlParLevel3BoolTrue
�� (
=
��) *
Mapper
��+ 1
.
��1 2
Map
��2 5
<
��5 6
List
��6 :
<
��: ;"
ParLevel3BoolTrueDTO
��; O
>
��O P
>
��P Q
(
��Q R$
_baseParLevel3BoolTrue
��R h
.
��h i 
GetAllAsNoTracking
��i {
(
��{ |
)
��| }
)
��} ~
;
��~ 
var
�� 

DdlparCrit
�� 
=
��  
Mapper
��! '
.
��' (
Map
��( +
<
��+ ,
List
��, 0
<
��0 1!
ParCriticalLevelDTO
��1 D
>
��D E
>
��E F
(
��F G'
_baseRepoParCriticalLevel
��G `
.
��` a 
GetAllAsNoTracking
��a s
(
��s t
)
��t u
)
��u v
;
��v w
var
�� 
DdlparCompany
�� !
=
��" #
Mapper
��$ *
.
��* +
Map
��+ .
<
��. /
List
��/ 3
<
��3 4
ParCompanyDTO
��4 A
>
��A B
>
��B C
(
��C D!
_baseRepoParCompany
��D W
.
��W X 
GetAllAsNoTracking
��X j
(
��j k
)
��k l
)
��l m
;
��m n
var
�� 
DdlScoretype
��  
=
��! "
Mapper
��# )
.
��) *
Map
��* -
<
��- .
List
��. 2
<
��2 3
ParScoreTypeDTO
��3 B
>
��B C
>
��C D
(
��D E
_baseRepoParScore
��E V
.
��V W 
GetAllAsNoTracking
��W i
(
��i j
)
��j k
)
��k l
;
��l m
var
�� 
retorno
�� 
=
�� 
new
�� !
	ParamsDdl
��" +
(
��+ ,
)
��, -
;
��- .
retorno
�� 
.
�� 
SetDdlsNivel123
�� '
(
��' (
DdlparLevel1
��( 4
,
��4 5
DdlparLevel2
��  ,
,
��, -
DdlparLevel3
��  ,
)
��, -
;
��- .
retorno
�� 
.
�� 
SetDdls
�� 
(
��  !
DdlParConsolidation
��  3
,
��3 4
DdlFrequency
��5 A
,
��A B
DdlparCluster
��C P
,
��P Q#
DdlparLevelDefinition
��R g
,
��g h
DdlParFieldType
��i x
,
��x y
DdlParDepartment��z �
,��� �$
DdlParCounter_Level1��� �
,��� � 
DdlParLocal_Level1
��  2
,
��2 3"
DdlParCounter_Level2
��4 H
,
��H I 
DdlParLocal_Level2
��J \
,
��\ ]%
DdlParNotConformityRule
��^ u
,
��u v$
DdlParLevel3InputType��w �
,��� �%
DdlParMeasurementUnit��� �
,��� �#
DdlParLevel3BoolFalse
��  5
,
��5 6"
DdlParLevel3BoolTrue
��7 K
,
��K L

DdlparCrit
��M W
,
��W X
DdlparCompany
��Y f
,
��f g
DdlScoretype
��h t
)
��t u
;
��u v
return
�� 
retorno
�� 
;
�� 
}
�� 
}
�� 	
public
�� 
List
�� 
<
�� &
ParLevel3Level2Level1DTO
�� ,
>
��, -
AddVinculoL1L2
��. <
(
��< =
int
��= @
idLevel1
��A I
,
��I J
int
��K N
idLevel2
��O W
,
��W X
int
��Y \
idLevel3
��] e
,
��e f
int
��g j
?
��j k
userId
��l r
=
��s t
$num
��u v
,
��v w
int
��x {
?
��{ |
	companyId��} �
=��� �
null��� �
)��� �
{
�� 	
var
�� 
retorno
�� 
=
�� 
new
�� 
List
�� "
<
��" #&
ParLevel3Level2Level1DTO
��# ;
>
��; <
(
��< =
)
��= >
;
��> ?
_paramsRepo
�� 
.
�� 
SaveVinculoL3L2L1
�� )
(
��) *
idLevel1
��* 2
,
��2 3
idLevel2
��4 <
,
��< =
idLevel3
��> F
,
��F G
userId
��H N
,
��N O
	companyId
��P Y
)
��Y Z
;
��Z [
return
�� 
retorno
�� 
;
�� 
}
�� 	
public
�� 
bool
�� +
VerificaShowBtnRemVinculoL1L2
�� 1
(
��1 2
int
��2 5
idLevel1
��6 >
,
��> ?
int
��@ C
idLevel2
��D L
)
��L M
{
�� 	
var
�� 
response
�� 
=
�� 
false
��  
;
��  !
using
�� 
(
�� 
var
�� 
db
�� 
=
�� 
new
�� 
SgqDbDevEntities
��  0
(
��0 1
)
��1 2
)
��2 3
{
�� 
var
�� 
sql1
�� 
=
�� 
$str
�� V
+
��W X
idLevel1
��Y a
+
��b c
$str��d �
+��� �
idLevel2��� �
+��� �
$str��� �
;��� �
var
�� 
result1
�� 
=
�� 
db
��  
.
��  !
Database
��! )
.
��) *
SqlQuery
��* 2
<
��2 3#
ParLevel3Level2Level1
��3 H
>
��H I
(
��I J
sql1
��J N
)
��N O
.
��O P
ToList
��P V
(
��V W
)
��W X
;
��X Y
var
�� 
result2
�� 
=
�� 
db
��  
.
��  !
ParLevel2Level1
��! 0
.
��0 1
Where
��1 6
(
��6 7
r
��7 8
=>
��9 ;
r
��< =
.
��= >
ParLevel1_Id
��> J
==
��K M
idLevel1
��N V
&&
��W Y
r
��Z [
.
��[ \
ParLevel2_Id
��\ h
==
��i k
idLevel2
��l t
&&
��u w
r
��x y
.
��y z
IsActive��z �
==��� �
true��� �
)��� �
.��� �
ToList��� �
(��� �
)��� �
;��� �
if
�� 
(
�� 
result1
�� 
?
�� 
.
�� 
Count
�� "
(
��" #
)
��# $
>
��% &
$num
��' (
||
��) +
result2
��, 3
?
��3 4
.
��4 5
Count
��5 :
(
��: ;
)
��; <
>
��= >
$num
��? @
)
��@ A
response
�� 
=
�� 
true
�� #
;
��# $
else
�� 
response
�� 
=
�� 
false
�� $
;
��$ %
}
�� 
return
�� 
response
�� 
;
�� 
}
�� 	
public
�� 
bool
�� 
RemVinculoL1L2
�� "
(
��" #
int
��# &
idLevel1
��' /
,
��/ 0
int
��1 4
idLevel2
��5 =
)
��= >
{
�� 	
using
�� 
(
�� 
var
�� 
db
�� 
=
�� 
new
�� 
SgqDbDevEntities
��  0
(
��0 1
)
��1 2
)
��2 3
{
�� 
var
�� 
sql1
�� 
=
�� 
$str
�� V
+
��W X
idLevel1
��Y a
+
��b c
$str��d �
+��� �
idLevel2��� �
+��� �
$str��� �
;��� �
var
�� 
sql2
�� 
=
�� 
$str
�� P
+
��Q R
idLevel1
��S [
+
��\ ]
$str
��^ t
+
��u v
idLevel2
��w 
;�� �
var
�� 
result1
�� 
=
�� 
db
��  
.
��  !
Database
��! )
.
��) *
SqlQuery
��* 2
<
��2 3#
ParLevel3Level2Level1
��3 H
>
��H I
(
��I J
sql1
��J N
)
��N O
.
��O P
ToList
��P V
(
��V W
)
��W X
;
��X Y
var
�� 
result2
�� 
=
�� 
db
��  
.
��  !
ParLevel2Level1
��! 0
.
��0 1
Where
��1 6
(
��6 7
r
��7 8
=>
��9 ;
r
��< =
.
��= >
ParLevel1_Id
��> J
==
��K M
idLevel1
��N V
&&
��W Y
r
��Z [
.
��[ \
ParLevel2_Id
��\ h
==
��i k
idLevel2
��l t
)
��t u
.
��u v
ToList
��v |
(
��| }
)
��} ~
;
��~ 
if
�� 
(
�� 
result1
�� 
?
�� 
.
�� 
Count
�� "
(
��" #
)
��# $
>
��% &
$num
��' (
||
��) +
result2
��, 3
?
��3 4
.
��4 5
Count
��5 :
(
��: ;
)
��; <
>
��= >
$num
��? @
)
��@ A
{
�� 
if
�� 
(
�� 
result1
�� 
?
��  
.
��  !
Count
��! &
(
��& '
)
��' (
>
��) *
$num
��+ ,
)
��, -
sql1
�� 
=
�� 
$str
�� S
+
��T U
idLevel1
��V ^
+
��_ `
$str��a �
+��� �
idLevel2��� �
+��� �
$str��� �
;��� �
if
�� 
(
�� 
result2
�� 
?
��  
.
��  !
Count
��! &
(
��& '
)
��' (
>
��) *
$num
��+ ,
)
��, -
sql2
�� 
=
�� 
$str
�� M
+
��N O
idLevel1
��P X
+
��Y Z
$str
��[ q
+
��r s
idLevel2
��t |
;
��| }
}
�� 
else
�� 
{
�� 
return
�� 
false
��  
;
��  !
}
�� 
var
�� 
r1
�� 
=
�� 
db
�� 
.
�� 
Database
�� $
.
��$ %
ExecuteSqlCommand
��% 6
(
��6 7
sql1
��7 ;
)
��; <
;
��< =
var
�� 
r2
�� 
=
�� 
db
�� 
.
�� 
Database
�� $
.
��$ %
ExecuteSqlCommand
��% 6
(
��6 7
sql2
��7 ;
)
��; <
;
��< =
}
�� 
return
�� 
true
�� 
;
�� 
}
�� 	
public
��  
ParLevel3Level2DTO
�� !
AddVinculoL3L2
��" 0
(
��0 1
int
��1 4
idLevel2
��5 =
,
��= >
int
��? B
idLevel3
��C K
,
��K L
decimal
��M T
peso
��U Y
,
��Y Z
int
��[ ^
?
��^ _
groupLevel2
��` k
=
��l m
$num
��n o
)
��o p
{
�� 	
ParLevel3Level2
�� $
objLelvel2Level3ToSave
�� 2
;
��2 3
var
�� 
level2
�� 
=
��  
_baseRepoParLevel2
�� +
.
��+ ,
GetById
��, 3
(
��3 4
idLevel2
��4 <
)
��< =
;
��= >$
objLelvel2Level3ToSave
�� "
=
��# $
new
��% (
ParLevel3Level2
��) 8
(
��8 9
)
��9 :
{
�� 
ParLevel2_Id
�� 
=
�� 
idLevel2
�� '
,
��' (
ParLevel3_Id
�� 
=
�� 
idLevel3
�� '
,
��' (
Weight
�� 
=
�� 
peso
�� 
,
�� 
ParLevel3Group_Id
�� !
=
��" #
groupLevel2
��$ /
==
��0 2
$num
��3 4
?
��5 6
null
��7 ;
:
��< =
groupLevel2
��> I
}
�� 
;
�� 
var
�� 
	existente
�� 
=
�� &
_baseRepoParLevel3Level2
�� 4
.
��4 5
GetAll
��5 ;
(
��; <
)
��< =
.
��= >
FirstOrDefault
��> L
(
��L M
r
��M N
=>
��O Q
r
��R S
.
��S T
ParLevel2_Id
��T `
==
��a c
idLevel2
��d l
&&
��m o
r
��p q
.
��q r
ParLevel3_Id
��r ~
==�� �
idLevel3��� �
)��� �
;��� �
if
�� 
(
�� 
	existente
�� 
!=
�� 
null
�� !
)
��! "
{
�� $
objLelvel2Level3ToSave
�� &
=
��' (
	existente
��) 2
;
��2 3$
objLelvel2Level3ToSave
�� &
.
��& '
Weight
��' -
=
��. /
	existente
��0 9
.
��9 :
Weight
��: @
;
��@ A$
objLelvel2Level3ToSave
�� &
.
��& '
ParLevel3Group_Id
��' 8
=
��9 :
groupLevel2
��; F
==
��G I
$num
��J K
?
��L M
null
��N R
:
��S T
groupLevel2
��U `
;
��` a
}
�� 
else
�� 
{
�� $
objLelvel2Level3ToSave
�� &
.
��& '
Weight
��' -
=
��. /
$num
��0 1
;
��1 2
}
�� $
objLelvel2Level3ToSave
�� "
.
��" #
IsActive
��# +
=
��, -
true
��. 2
;
��2 3&
_baseRepoParLevel3Level2
�� $
.
��$ %
AddOrUpdate
��% 0
(
��0 1$
objLelvel2Level3ToSave
��1 G
)
��G H
;
��H I 
ParLevel3Level2DTO
�� 

objtReturn
�� )
=
��* +
Mapper
��, 2
.
��2 3
Map
��3 6
<
��6 7 
ParLevel3Level2DTO
��7 I
>
��I J
(
��J K$
objLelvel2Level3ToSave
��K a
)
��a b
;
��b c
return
�� 

objtReturn
�� 
;
�� 
}
�� 	
public
�� 
ParLevel3GroupDTO
��  "
RemoveParLevel3Group
��! 5
(
��5 6
int
��6 9
Id
��: <
)
��< =
{
�� 	
var
�� 
parLevel3Group
�� 
=
��  !
_baseParLevel3Group
��! 4
.
��4 5
GetAll
��5 ;
(
��; <
)
��< =
.
��= >
FirstOrDefault
��> L
(
��L M
r
��M N
=>
��O Q
r
��R S
.
��S T
Id
��T V
==
��W Y
Id
��Z \
)
��\ ]
;
��] ^
parLevel3Group
�� 
.
�� 
IsActive
�� #
=
��$ %
false
��& +
;
��+ ,!
_baseParLevel3Group
�� 
.
��  
AddOrUpdate
��  +
(
��+ ,
parLevel3Group
��, :
)
��: ;
;
��; <
return
�� 
Mapper
�� 
.
�� 
Map
�� 
<
�� 
ParLevel3GroupDTO
�� /
>
��/ 0
(
��0 1
parLevel3Group
��1 ?
)
��? @
;
��@ A
}
�� 	
public
�� 
List
�� 
<
�� 
ParLevel1DTO
��  
>
��  !
GetAllLevel1
��" .
(
��. /
)
��/ 0
{
�� 	
var
�� 
retorno
�� 
=
�� 
new
�� 
List
�� "
<
��" #
ParLevel1DTO
��# /
>
��/ 0
(
��0 1
)
��1 2
;
��2 3
var
�� 

listLevel1
�� 
=
��  
_baseRepoParLevel1
�� /
.
��/ 0
GetAll
��0 6
(
��6 7
)
��7 8
.
��8 9
Where
��9 >
(
��> ?
r
��? @
=>
��A C
r
��D E
.
��E F
IsActive
��F N
==
��O Q
true
��R V
)
��V W
;
��W X
foreach
�� 
(
�� 
var
�� 
level1
�� 
in
��  "

listLevel1
��# -
)
��- .
{
�� 
retorno
�� 
.
�� 
Add
�� 
(
�� !
GetLevel1TelaColeta
�� /
(
��/ 0
level1
��0 6
.
��6 7
Id
��7 9
)
��9 :
)
��: ;
;
��; <
}
�� 
return
�� 
retorno
�� 
;
�� 
}
�� 	
public
�� 
ParLevel1DTO
�� !
GetLevel1TelaColeta
�� /
(
��/ 0
int
��0 3
idParLevel1
��4 ?
)
��? @
{
�� 	
var
�� 
	parlevel1
�� 
=
��  
_baseRepoParLevel1
�� .
.
��. /
GetById
��/ 6
(
��6 7
idParLevel1
��7 B
)
��B C
;
��C D
var
�� 
retorno
�� 
=
�� 
Mapper
��  
.
��  !
Map
��! $
<
��$ %
ParLevel1DTO
��% 1
>
��1 2
(
��2 3 
_baseRepoParLevel1
��3 E
.
��E F
GetById
��F M
(
��M N
idParLevel1
��N Y
)
��Y Z
)
��Z [
;
��[ \
retorno
�� 
.
�� #
listLevel1XClusterDto
�� )
=
��* +
Mapper
��, 2
.
��2 3
Map
��3 6
<
��6 7
List
��7 ;
<
��; <"
ParLevel1XClusterDTO
��< P
>
��P Q
>
��Q R
(
��R S
	parlevel1
��S \
.
��\ ]
ParLevel1XCluster
��] n
.
��n o
Where
��o t
(
��t u
r
��u v
=>
��w y
r
��z {
.
��{ |
IsActive��| �
==��� �
true��� �
)��� �
)��� �
;��� �
retorno
�� 
.
��  
cabecalhosInclusos
�� &
=
��' (
Mapper
��) /
.
��/ 0
Map
��0 3
<
��3 4
List
��4 8
<
��8 9&
ParLevel1XHeaderFieldDTO
��9 Q
>
��Q R
>
��R S
(
��S T,
_baseRepoParLevel1XHeaderField
��T r
.
��r s
GetAll
��s y
(
��y z
)
��z {
.
��{ |
Where��| �
(��� �
r��� �
=>��� �
r��� �
.��� �
ParLevel1_Id��� �
==��� �
retorno��� �
.��� �
Id��� �
&&��� �
r��� �
.��� �
IsActive��� �
==��� �
true��� �
)��� �
)��� �
;��� �
retorno
�� 
.
�� !
contadoresIncluidos
�� '
=
��( )
Mapper
��* 0
.
��0 1
Map
��1 4
<
��4 5
List
��5 9
<
��9 :!
ParCounterXLocalDTO
��: M
>
��M N
>
��N O
(
��O P'
_baseRepoParCounterXLocal
��P i
.
��i j
GetAll
��j p
(
��p q
)
��q r
.
��r s
Where
��s x
(
��x y
r
��y z
=>
��{ }
r
��~ 
.�� �
ParLevel1_Id��� �
==��� �
retorno��� �
.��� �
Id��� �
)��� �
)��� �
;��� �
retorno
�� 
.
�� *
listParLevel3Level2Level1Dto
�� 0
=
��1 2
Mapper
��3 9
.
��9 :
Map
��: =
<
��= >
List
��> B
<
��B C&
ParLevel3Level2Level1DTO
��C [
>
��[ \
>
��\ ]
(
��] ^,
_baseRepoParLevel3Level2Level1
��^ |
.
��| }
GetAll��} �
(��� �
)��� �
.��� �
Where��� �
(��� �
r��� �
=>��� �
r��� �
.��� �
ParLevel1_Id��� �
==��� �
retorno��� �
.��� �
Id��� �
)��� �
.��� �
ToList��� �
(��� �
)��� �
)��� �
;��� �
retorno
�� 
.
�� 6
(CreateSelectListParamsViewModelListLevel
�� <
(
��< =
Mapper
��= C
.
��C D
Map
��D G
<
��G H
List
��H L
<
��L M
ParLevel2DTO
��M Y
>
��Y Z
>
��Z [
(
��[ \ 
_baseRepoParLevel2
��\ n
.
��n o
GetAll
��o u
(
��u v
)
��v w
)
��w x
,
��x y
retorno��z �
.��� �,
listParLevel3Level2Level1Dto��� �
)��� �
;��� �
retorno
�� 
.
�� "
listParLevel2Colleta
�� (
=
��) *
new
��+ .
List
��/ 3
<
��3 4
ParLevel2DTO
��4 @
>
��@ A
(
��A B
)
��B C
;
��C D
foreach
�� 
(
�� 
var
�� &
ParLevel3Level2Level1Dto
�� 1
in
��2 4
retorno
��5 <
.
��< =*
listParLevel3Level2Level1Dto
��= Y
.
��Y Z
Distinct
��Z b
(
��b c
)
��c d
)
��d e
{
�� 
if
�� 
(
�� 
!
�� 
retorno
�� 
.
�� "
listParLevel2Colleta
�� 1
.
��1 2
Any
��2 5
(
��5 6
r
��6 7
=>
��8 :
r
��; <
.
��< =
Id
��= ?
==
��@ B&
ParLevel3Level2Level1Dto
��C [
.
��[ \
ParLevel3Level2
��\ k
.
��k l
	ParLevel2
��l u
.
��u v
Id
��v x
)
��x y
)
��y z
{
�� &
ParLevel3Level2Level1Dto
�� ,
.
��, -
ParLevel3Level2
��- <
.
��< =
	ParLevel2
��= F
.
��F G"
listParCounterXLocal
��G [
=
��\ ]
Mapper
��^ d
.
��d e
Map
��e h
<
��h i
List
��i m
<
��m n"
ParCounterXLocalDTO��n �
>��� �
>��� �
(��� �%
_baseParCounterXLocal��� �
.��� �
GetAll��� �
(��� �
)��� �
.��� �
Where��� �
(��� �
r��� �
=>��� �
r��� �
.��� �
ParLevel2_Id��� �
==��� �(
ParLevel3Level2Level1Dto��� �
.��� �
ParLevel3Level2��� �
.��� �
	ParLevel2��� �
.��� �
Id��� �
&&��� �
r��� �
.��� �
IsActive��� �
==��� �
true��� �
)��� �
.��� �
ToList��� �
(��� �
)��� �
)��� �
;��� �&
ParLevel3Level2Level1Dto
�� ,
.
��, -
ParLevel3Level2
��- <
.
��< =
	ParLevel2
��= F
.
��F G
ParamEvaluation
��G V
=
��W X
Mapper
��Y _
.
��_ `
Map
��` c
<
��c d
ParEvaluationDTO
��d t
>
��t u
(
��u v!
_baseParEvaluation��v �
.��� �
GetAll��� �
(��� �
)��� �
.��� �
Where��� �
(��� �
r��� �
=>��� �
r��� �
.��� �
ParLevel2_Id��� �
==��� �(
ParLevel3Level2Level1Dto��� �
.��� �
ParLevel3Level2��� �
.��� �
	ParLevel2��� �
.��� �
Id��� �
)��� �
.��� �
FirstOrDefault��� �
(��� �
)��� �
)��� �
;��� �&
ParLevel3Level2Level1Dto
�� ,
.
��, -
ParLevel3Level2
��- <
.
��< =
	ParLevel2
��= F
.
��F G
ParamSample
��G R
=
��S T
Mapper
��U [
.
��[ \
Map
��\ _
<
��_ `
ParSampleDTO
��` l
>
��l m
(
��m n
_baseParSample
��n |
.
��| }
GetAll��} �
(��� �
)��� �
.��� �
Where��� �
(��� �
r��� �
=>��� �
r��� �
.��� �
ParLevel2_Id��� �
==��� �(
ParLevel3Level2Level1Dto��� �
.��� �
ParLevel3Level2��� �
.��� �
	ParLevel2��� �
.��� �
Id��� �
)��� �
.��� �
FirstOrDefault��� �
(��� �
)��� �
)��� �
;��� �
retorno
�� 
.
�� "
listParLevel2Colleta
�� 0
.
��0 1
Add
��1 4
(
��4 5&
ParLevel3Level2Level1Dto
��5 M
.
��M N
ParLevel3Level2
��N ]
.
��] ^
	ParLevel2
��^ g
)
��g h
;
��h i
}
�� 
var
�� %
parLevel3Level2DoLevel2
�� +
=
��, -
_repoParLevel3
��. <
.
��< =&
GetLevel3VinculadoLevel2
��= U
(
��U V
retorno
��V ]
.
��] ^
Id
��^ `
)
��` a
;
��a b
retorno
�� 
.
�� "
listParLevel2Colleta
�� ,
.
��, -
LastOrDefault
��- :
(
��: ;
)
��; <
.
��< =#
listaParLevel3Colleta
��= R
=
��S T
new
��U X
List
��Y ]
<
��] ^
ParLevel3DTO
��^ j
>
��j k
(
��k l
)
��l m
;
��m n
foreach
�� 
(
�� 
var
�� 
level3Level2
�� )
in
��* ,%
parLevel3Level2DoLevel2
��- D
.
��D E
Where
��E J
(
��J K
r
��K L
=>
��M O
r
��P Q
.
��Q R
ParLevel2_Id
��R ^
==
��_ a&
ParLevel3Level2Level1Dto
��b z
.
��z {
ParLevel3Level2��{ �
.��� �
	ParLevel2��� �
.��� �
Id��� �
)��� �
)��� �
{
�� 
if
�� 
(
�� 
!
�� 
retorno
��  
.
��  !"
listParLevel2Colleta
��! 5
.
��5 6
LastOrDefault
��6 C
(
��C D
)
��D E
.
��E F#
listaParLevel3Colleta
��F [
.
��[ \
Any
��\ _
(
��_ `
r
��` a
=>
��b d
r
��e f
.
��f g
Id
��g i
==
��j l
level3Level2
��m y
.
��y z
ParLevel3_Id��z �
)��� �
)��� �
retorno
�� 
.
��  "
listParLevel2Colleta
��  4
.
��4 5
LastOrDefault
��5 B
(
��B C
)
��C D
.
��D E#
listaParLevel3Colleta
��E Z
.
��Z [
Add
��[ ^
(
��^ _
Mapper
��_ e
.
��e f
Map
��f i
<
��i j
ParLevel3DTO
��j v
>
��v w
(
��w x!
_baseRepoParLevel3��x �
.��� �
GetById��� �
(��� �
level3Level2��� �
.��� �
ParLevel3_Id��� �
)��� �
)��� �
)��� �
;��� �
}
�� 
}
�� 
retorno
�� 
.
�� +
parNotConformityRuleXLevelDto
�� 1
=
��2 3
Mapper
��4 :
.
��: ;
Map
��; >
<
��> ?+
ParNotConformityRuleXLevelDTO
��? \
>
��\ ]
(
��] ^-
_baseParNotConformityRuleXLevel
��^ }
.
��} ~
GetAll��~ �
(��� �
)��� �
.��� �
FirstOrDefault��� �
(��� �
r��� �
=>��� �
r��� �
.��� �
ParLevel1_Id��� �
==��� �
retorno��� �
.��� �
Id��� �
)��� �
)��� �
??��� �
new��� �-
ParNotConformityRuleXLevelDTO��� �
(��� �
)��� �
;��� �
retorno
�� 
.
�� 
listParRelapseDto
�� %
=
��& '
Mapper
��( .
.
��. /
Map
��/ 2
<
��2 3
List
��3 7
<
��7 8
ParRelapseDTO
��8 E
>
��E F
>
��F G
(
��G H
_baseParRelapse
��H W
.
��W X
GetAll
��X ^
(
��^ _
)
��_ `
.
��` a
Where
��a f
(
��f g
r
��g h
=>
��i k
r
��l m
.
��m n
ParLevel1_Id
��n z
==
��{ }
retorno��~ �
.��� �
Id��� �
)��� �
)��� �
;��� �
return
�� 
retorno
�� 
;
�� 
}
�� 	
public
�� 
bool
�� (
SetRequiredCamposCabecalho
�� .
(
��. /
int
��/ 2
id
��3 5
,
��5 6
int
��7 :
required
��; C
)
��C D
{
�� 	
var
�� 
headerField
�� 
=
�� %
_baseRepoParHeaderField
�� 5
.
��5 6
GetById
��6 =
(
��= >
id
��> @
)
��@ A
;
��A B
if
�� 
(
�� 
required
�� 
==
�� 
$num
�� 
)
�� 
headerField
�� 
.
�� 

IsRequired
�� &
=
��' (
true
��) -
;
��- .
else
�� 
headerField
�� 
.
�� 

IsRequired
�� &
=
��' (
false
��) .
;
��. /%
_baseRepoParHeaderField
�� #
.
��# $
AddOrUpdate
��$ /
(
��/ 0
headerField
��0 ;
)
��; <
;
��< =
return
�� 
headerField
�� 
.
�� 

IsRequired
�� )
.
��) *
Value
��* /
;
��/ 0
}
�� 	
public
�� 
ParMultipleValues
��  '
SetDefaultMultiplaEscolha
��! :
(
��: ;
int
��; >
idHeader
��? G
,
��G H
int
��I L

idMultiple
��M W
)
��W X
{
�� 	
var
�� 
headerFieldList
�� 
=
��  !(
_baseRepoParMultipleValues
��" <
.
��< =
GetAll
��= C
(
��C D
)
��D E
.
��E F
Where
��F K
(
��K L
r
��L M
=>
��N P
r
��Q R
.
��R S
ParHeaderField_Id
��S d
==
��e g
idHeader
��h p
)
��p q
;
��q r
foreach
�� 
(
�� 
ParMultipleValues
�� &
m
��' (
in
��) +
headerFieldList
��, ;
)
��; <
{
�� 
m
�� 
.
�� 
IsDefaultOption
�� !
=
��" #
false
��$ )
;
��) *(
_baseRepoParMultipleValues
�� *
.
��* +
AddOrUpdate
��+ 6
(
��6 7
m
��7 8
)
��8 9
;
��9 :
}
�� 
var
�� 
multiple
�� 
=
�� 
new
�� 
ParMultipleValues
�� 0
(
��0 1
)
��1 2
;
��2 3
if
�� 
(
�� 

idMultiple
�� 
>
�� 
$num
�� 
)
�� 
{
�� 
multiple
�� 
=
�� (
_baseRepoParMultipleValues
�� 5
.
��5 6
GetById
��6 =
(
��= >

idMultiple
��> H
)
��H I
;
��I J
if
�� 
(
�� 
multiple
�� 
.
�� 
IsDefaultOption
�� ,
==
��- /
null
��0 4
||
��5 7
multiple
��8 @
.
��@ A
IsDefaultOption
��A P
==
��Q S
false
��T Y
)
��Y Z
multiple
�� 
.
�� 
IsDefaultOption
�� ,
=
��- .
true
��/ 3
;
��3 4
else
�� 
multiple
�� 
.
�� 
IsDefaultOption
�� ,
=
��- .
false
��/ 4
;
��4 5(
_baseRepoParMultipleValues
�� *
.
��* +
AddOrUpdate
��+ 6
(
��6 7
multiple
��7 ?
)
��? @
;
��@ A
}
�� 
return
�� 
multiple
�� 
;
�� 
}
�� 	
public
�� #
ParLevel2XHeaderField
�� $&
AddRemoveParHeaderLevel2
��% =
(
��= >#
ParLevel2XHeaderField
��> S#
parLevel2XHeaderField
��T i
)
��i j
{
�� 	#
parLevel2XHeaderField
�� !
.
��! "
AddDate
��" )
=
��* +
DateTime
��, 4
.
��4 5
Now
��5 8
;
��8 9#
parLevel2XHeaderField
�� !
.
��! "
IsActive
��" *
=
��+ ,
true
��- 1
;
��1 2#
parLevel2XHeaderField
�� !
=
��" #
_paramsRepo
��$ /
.
��/ 0!
SaveParHeaderLevel2
��0 C
(
��C D#
parLevel2XHeaderField
��D Y
)
��Y Z
;
��Z [
return
�� #
parLevel2XHeaderField
�� (
;
��( )
}
�� 	
}
�� 
}�� х
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
�� 
	resultSet
�� 
=
�� 
new
��  #&
ResultSetRelatorioColeta
��$ <
(
��< =
)
��= >
{
�� &
listCollectionLevel02DTO
�� ,
=
��- .
Mapper
��/ 5
.
��5 6
Map
��6 9
<
��9 :
List
��: >
<
��> ?"
CollectionLevel02DTO
��? S
>
��S T
>
��T U
(
��U V
result
��V \
)
��\ ]
}
�� 
;
�� 
return
�� 
new
�� 
GenericReturn
�� (
<
��( )&
ResultSetRelatorioColeta
��) A
>
��A B
(
��B C
	resultSet
��C L
)
��L M
;
��M N
}
�� 
catch
�� 
(
�� 
	Exception
�� 
e
�� 
)
�� 
{
�� 
return
�� 
new
�� 
GenericReturn
�� (
<
��( )&
ResultSetRelatorioColeta
��) A
>
��A B
(
��B C
e
��C D
,
��D E
$str
��F x
)
��x y
;
��y z
}
�� 
}
�� 	
public
�� 
GenericReturn
�� 
<
�� &
ResultSetRelatorioColeta
�� 5
>
��5 6

GetAllData
��7 A
(
��A B#
DataCarrierFormulario
��B W
form
��X \
)
��\ ]
{
�� 	
try
�� 
{
�� 
var
�� %
resultCollectionLevel02
�� +
=
��, -$
_repoCollectionLevel02
��. D
.
��D E
	GetByDate
��E N
(
��N O
form
��O S
)
��S T
.
��T U
ToList
��U [
(
��[ \
)
��\ ]
;
��] ^
var
�� %
resultCollectionLevel03
�� +
=
��, -$
_repoCollectionLevel03
��. D
.
��D E
	GetByDate
��E N
(
��N O
form
��O S
)
��S T
.
��T U
ToList
��U [
(
��[ \
)
��\ ]
;
��] ^
var
�� (
resultConsolidationLevel01
�� .
=
��/ 0'
_repoConsolidationLevel01
��1 J
.
��J K
	GetByDate
��K T
(
��T U
form
��U Y
)
��Y Z
.
��Z [
ToList
��[ a
(
��a b
)
��b c
;
��c d
var
�� (
resultConsolidationLevel02
�� .
=
��/ 0'
_repoConsolidationLevel02
��1 J
.
��J K
	GetByDate
��K T
(
��T U
form
��U Y
)
��Y Z
.
��Z [
ToList
��[ a
(
��a b
)
��b c
;
��c d
if
�� 
(
�� %
resultCollectionLevel03
�� +
.
��+ ,
IsNull
��, 2
(
��2 3
)
��3 4
&&
��5 7%
resultCollectionLevel02
��8 O
.
��O P
IsNull
��P V
(
��V W
)
��W X
&&
��Y [(
resultConsolidationLevel01
��\ v
.
��v w
IsNull
��w }
(
��} ~
)
��~ 
&&��� �*
resultConsolidationLevel02��� �
.��� �
IsNull��� �
(��� �
)��� �
)��� �
throw
�� 
new
�� 
ExceptionHelper
�� -
(
��- .
$str
��. ?
)
��? @
;
��@ A
var
�� 
	resultSet
�� 
=
�� 
new
��  #&
ResultSetRelatorioColeta
��$ <
(
��< =
)
��= >
{
�� &
listCollectionLevel02DTO
�� ,
=
��- .
Mapper
��/ 5
.
��5 6
Map
��6 9
<
��9 :
List
��: >
<
��> ?"
CollectionLevel02DTO
��? S
>
��S T
>
��T U
(
��U V%
resultCollectionLevel02
��V m
)
��m n
,
��n o&
listCollectionLevel03DTO
�� ,
=
��- .
Mapper
��/ 5
.
��5 6
Map
��6 9
<
��9 :
List
��: >
<
��> ?"
CollectionLevel03DTO
��? S
>
��S T
>
��T U
(
��U V%
resultCollectionLevel03
��V m
)
��m n
,
��n o&
listConsolidationLevel01
�� ,
=
��- .
Mapper
��/ 5
.
��5 6
Map
��6 9
<
��9 :
List
��: >
<
��> ?%
ConsolidationLevel01DTO
��? V
>
��V W
>
��W X
(
��X Y(
resultConsolidationLevel01
��Y s
)
��s t
,
��t u&
listConsolidationLevel02
�� ,
=
��- .
Mapper
��/ 5
.
��5 6
Map
��6 9
<
��9 :
List
��: >
<
��> ?%
ConsolidationLevel02DTO
��? V
>
��V W
>
��W X
(
��X Y(
resultConsolidationLevel02
��Y s
)
��s t
,
��t u
}
�� 
;
�� 
return
�� 
new
�� 
GenericReturn
�� (
<
��( )&
ResultSetRelatorioColeta
��) A
>
��A B
(
��B C
	resultSet
��C L
)
��L M
;
��M N
}
�� 
catch
�� 
(
�� 
	Exception
�� 
e
�� 
)
�� 
{
�� 
return
�� 
new
�� 
GenericReturn
�� (
<
��( )&
ResultSetRelatorioColeta
��) A
>
��A B
(
��B C
e
��C D
,
��D E
$str
��F x
)
��x y
;
��y z
}
�� 
}
�� 	
public
�� 
GenericReturn
�� 
<
�� 

GetSyncDTO
�� '
>
��' (
GetEntryByDate
��) 7
(
��7 8#
DataCarrierFormulario
��8 M
form
��N R
)
��R S
{
�� 	
try
�� 
{
�� 
var
�� $
consildatedLelve01List
�� *
=
��+ ,'
_repoConsolidationLevel01
��- F
.
��F G5
'GetEntryConsildatedLevel01ByDateAndUnit
��G n
(
��n o
form
��o s
)
��s t
.
��t u
ToList
��u {
(
��{ |
)
��| }
;
��} ~
var
�� (
consildatedLelve01ListDTO1
�� .
=
��/ 0
Mapper
��1 7
.
��7 8
Map
��8 ;
<
��; <
List
��< @
<
��@ A%
ConsolidationLevel01DTO
��A X
>
��X Y
>
��Y Z
(
��Z [$
consildatedLelve01List
��[ q
)
��q r
;
��r s
var
�� 
processador
�� 
=
��  !
new
��" %&
TableResultsForDataTable
��& >
(
��> ?
)
��? @
;
��@ A
var
�� 4
&resultadosProcessadosParaFormatoTabela
�� :
=
��; <
processador
��= H
.
��H I3
%DataCollectionReportsProcessedResults
��I n
(
��n o)
consildatedLelve01ListDTO1��o �
)��� �
;��� �
return
�� 
new
�� 
GenericReturn
�� (
<
��( )

GetSyncDTO
��) 3
>
��3 4
(
��4 5
new
��5 8

GetSyncDTO
��9 C
(
��C D
)
��D E
{
�� "
ConsolidationLevel01
�� (
=
��) *4
&resultadosProcessadosParaFormatoTabela
��+ Q
}
�� 
)
�� 
;
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
e
�� 
)
�� 
{
�� 
return
�� 
new
�� 
GenericReturn
�� (
<
��( )

GetSyncDTO
��) 3
>
��3 4
(
��4 5
e
��5 6
,
��6 7
$str
��8 J
)
��J K
;
��K L
}
�� 
}
�� 	
}
�� 
}�� �F
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
�� 
}�� ��
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
�� 
defaultCompany
�� "
=
��# $%
_baseParCompanyXUserSgq
��% <
.
��< =
GetAll
��= C
(
��C D
)
��D E
.
��E F
FirstOrDefault
��F T
(
��T U
r
�� 
=>
�� 
r
�� 
.
�� 

UserSgq_Id
�� %
==
��& (
isUser
��) /
.
��/ 0
Id
��0 2
&&
��3 5
r
��6 7
.
��7 8
ParCompany_Id
��8 E
==
��F H
isUser
��I O
.
��O P
ParCompany_Id
��P ]
)
��] ^
;
��^ _
if
�� 
(
�� 
defaultCompany
�� "
==
��# %
null
��& *
)
��* +
{
�� 
defaultCompany
�� "
=
��# $%
_baseParCompanyXUserSgq
��% <
.
��< =
GetAll
��= C
(
��C D
)
��D E
.
��E F
FirstOrDefault
��F T
(
��T U
r
�� 
=>
�� 
r
�� 
.
�� 

UserSgq_Id
�� %
==
��& (
isUser
��) /
.
��/ 0
Id
��0 2
)
��2 3
;
��3 4
using
�� 
(
�� 
var
�� 
db
�� !
=
��" #
new
��$ '
SgqDbDevEntities
��( 8
(
��8 9
)
��9 :
)
��: ;
{
�� 
var
�� 
atualizarUsuario
�� ,
=
��- .
db
��/ 1
.
��1 2
UserSgq
��2 9
.
��9 :
FirstOrDefault
��: H
(
��H I
r
��I J
=>
��J L
r
��M N
.
��N O
Id
��O Q
==
��R T
isUser
��U [
.
��[ \
Id
��\ ^
)
��^ _
;
��_ `
atualizarUsuario
�� (
.
��( )
ParCompany_Id
��) 6
=
��7 8
defaultCompany
��9 G
.
��G H
ParCompany_Id
��H U
;
��U V
db
�� 
.
�� 
UserSgq
�� "
.
��" #
Attach
��# )
(
��) *
atualizarUsuario
��* :
)
��: ;
;
��; <
db
�� 
.
�� 
Entry
��  
(
��  !
atualizarUsuario
��! 1
)
��1 2
.
��2 3
State
��3 8
=
��9 :
System
��; A
.
��A B
Data
��B F
.
��F G
Entity
��G M
.
��M N
EntityState
��N Y
.
��Y Z
Modified
��Z b
;
��b c
db
�� 
.
�� 
SaveChanges
�� &
(
��& '
)
��' (
;
��( )
}
�� 
}
�� 
return
�� 
new
�� 
GenericReturn
�� (
<
��( )
UserDTO
��) 0
>
��0 1
(
��1 2
Mapper
��2 8
.
��8 9
Map
��9 <
<
��< =
UserSgq
��= D
,
��D E
UserDTO
��F M
>
��M N
(
��N O
isUser
��O U
)
��U V
)
��V W
;
��W X
}
�� 
catch
�� 
(
�� 
	Exception
�� 
e
�� 
)
�� 
{
�� 
new
�� 
	CreateLog
�� 
(
�� 
e
�� 
)
��  
;
��  !
return
�� 
new
�� 
GenericReturn
�� (
<
��( )
UserDTO
��) 0
>
��0 1
(
��1 2
e
��2 3
,
��3 4
e
��5 6
.
��6 7
Message
��7 >
)
��> ?
;
��? @
}
�� 
}
�� 	
private
�� 
void
�� !
DescriptografaSenha
�� (
(
��( )
UserDTO
��) 0
userDto
��1 8
)
��8 9
{
�� 	
userDto
�� 
.
�� 
Password
�� 
=
�� 
Guard
��  %
.
��% &
DecryptStringAES
��& 6
(
��6 7
userDto
��7 >
.
��> ?
Password
��? G
)
��G H
;
��H I
}
�� 	
private
�� 
UserSgq
�� &
CheckUserAndPassDataBase
�� 0
(
��0 1
UserDTO
��1 8
userDto
��9 @
)
��@ A
{
�� 	
var
�� 
user
�� 
=
�� 
Mapper
�� 
.
�� 
Map
�� !
<
��! "
UserDTO
��" )
,
��) *
UserSgq
��+ 2
>
��2 3
(
��3 4
userDto
��4 ;
)
��; <
;
��< =
var
�� 
isUser
�� 
=
�� 
	_userRepo
�� "
.
��" #!
AuthenticationLogin
��# 6
(
��6 7
user
��7 ;
)
��; <
;
��< =
return
�� 
isUser
�� 
;
�� 
}
�� 	
public
�� 
List
�� 
<
�� 
UserDTO
�� 
>
�� 
GetAllUserByUnit
�� -
(
��- .
int
��. 1
	unidadeId
��2 ;
)
��; <
{
�� 	
return
�� 
Mapper
�� 
.
�� 
Map
�� 
<
�� 
List
�� "
<
��" #
UserDTO
��# *
>
��* +
>
��+ ,
(
��, -
	_userRepo
��- 6
.
��6 7
GetAllUserByUnit
��7 G
(
��G H
	unidadeId
��H Q
)
��Q R
)
��R S
;
��S T
}
�� 	
public
�� 
GenericReturn
�� 
<
�� 
UserDTO
�� $
>
��$ %
	GetByName
��& /
(
��/ 0
string
��0 6
username
��7 ?
)
��? @
{
�� 	
try
�� 
{
�� 
var
�� 
queryResult
�� 
=
��  !
	_userRepo
��" +
.
��+ ,
	GetByName
��, 5
(
��5 6
username
��6 >
)
��> ?
;
��? @
return
�� 
new
�� 
GenericReturn
�� (
<
��( )
UserDTO
��) 0
>
��0 1
(
��1 2
Mapper
��2 8
.
��8 9
Map
��9 <
<
��< =
UserSgq
��= D
,
��D E
UserDTO
��F M
>
��M N
(
��N O
queryResult
��O Z
)
��Z [
)
��[ \
;
��\ ]
}
�� 
catch
�� 
(
�� 
	Exception
�� 
e
�� 
)
�� 
{
�� 
return
�� 
new
�� 
GenericReturn
�� (
<
��( )
UserDTO
��) 0
>
��0 1
(
��1 2
e
��2 3
,
��3 4
$str
��5 O
)
��O P
;
��P Q
}
�� 
}
�� 	
public
�� 
GenericReturn
�� 
<
�� 

UserSgqDTO
�� '
>
��' (

GetByName2
��) 3
(
��3 4
string
��4 :
username
��; C
)
��C D
{
�� 	
try
�� 
{
�� 
var
�� 
queryResult
�� 
=
��  !
	_userRepo
��" +
.
��+ ,
	GetByName
��, 5
(
��5 6
username
��6 >
)
��> ?
;
��? @
return
�� 
new
�� 
GenericReturn
�� (
<
��( )

UserSgqDTO
��) 3
>
��3 4
(
��4 5
Mapper
��5 ;
.
��; <
Map
��< ?
<
��? @
UserSgq
��@ G
,
��G H

UserSgqDTO
��I S
>
��S T
(
��T U
queryResult
��U `
)
��` a
)
��a b
;
��b c
}
�� 
catch
�� 
(
�� 
	Exception
�� 
e
�� 
)
�� 
{
�� 
return
�� 
new
�� 
GenericReturn
�� (
<
��( )

UserSgqDTO
��) 3
>
��3 4
(
��4 5
e
��5 6
,
��6 7
$str
��8 R
)
��R S
;
��S T
}
�� 
}
�� 	
public
�� 
UserSgq
�� 
LoginSgq
�� 
(
��  
UserDTO
��  '
userDto
��( /
,
��/ 0
UserSgq
��1 8

userByName
��9 C
)
��C D
{
�� 	
return
�� &
CheckUserAndPassDataBase
�� +
(
��+ ,
userDto
��, 3
)
��3 4
;
��4 5
}
�� 	
private
�� 
UserSgq
�� 
LoginEUA
��  
(
��  !
UserDTO
��! (
userDto
��) 0
,
��0 1
UserSgq
��2 9

userByName
��: D
)
��D E
{
�� 	
if
�� 
(
�� 
GlobalConfig
�� 
.
�� 
mockLoginEUA
�� )
)
��) *
{
�� 
UserSgq
�� 
userDev
�� 
=
��  !&
CheckUserAndPassDataBase
��" :
(
��: ;
userDto
��; B
)
��B C
;
��C D
return
�� 
userDev
�� 
;
�� 
}
�� 
if
�� 
(
�� 

userByName
�� 
!=
�� 
null
�� "
)
��" #
{
�� 
var
�� 
IsActive
�� 
=
�� 
db
�� !
.
��! "
Database
��" *
.
��* +
SqlQuery
��+ 3
<
��3 4
bool
��4 8
>
��8 9
(
��9 :
$str
��: d
+
��e f

userByName
��g q
.
��q r
Id
��r t
)
��t u
.
��u v
FirstOrDefault��v �
(��� �
)��� �
;��� �
if
�� 
(
�� 
!
�� 
IsActive
�� 
)
�� 
throw
�� 
new
�� 
	Exception
�� '
(
��' (
$str
��( 8
)
��8 9
;
��9 :
}
�� 
if
�� 
(
�� 
CheckUserInAD
�� 
(
�� 
dominio
�� %
,
��% &
userDto
��' .
.
��. /
Name
��/ 3
,
��3 4
userDto
��5 <
.
��< =
Password
��= E
)
��E F
)
��F G
{
�� 
UserSgq
�� 
isUser
�� 
=
��  &
CheckUserAndPassDataBase
��! 9
(
��9 :
userDto
��: A
)
��A B
;
��B C
if
�� 
(
�� 

userByName
�� 
.
�� 
	IsNotNull
�� (
(
��( )
)
��) *
&&
��+ -
isUser
��. 4
.
��4 5
IsNull
��5 ;
(
��; <
)
��< =
)
��= >
{
�� 
isUser
�� 
=
�� %
AlteraSenhaAlteradaNoAd
�� 4
(
��4 5
userDto
��5 <
,
��< =

userByName
��> H
)
��H I
;
��I J
if
�� 
(
�� 
isUser
�� 
.
�� 
IsNull
�� %
(
��% &
)
��& '
)
��' (
throw
�� 
new
�� !
	Exception
��" +
(
��+ ,
$str
��, R
)
��R S
;
��S T
}
�� 
return
�� 
isUser
�� 
;
�� 
}
�� 
return
�� 
null
�� 
;
�� 
}
�� 	
private
�� 
UserSgq
�� %
AlteraSenhaAlteradaNoAd
�� /
(
��/ 0
UserDTO
��0 7
userDto
��8 ?
,
��? @
UserSgq
��A H

userByName
��I S
)
��S T
{
�� 	
UserSgq
�� 
isUser
�� 
;
�� 

userByName
�� 
.
�� 
Password
�� 
=
��  !
Guard
��" '
.
��' (
EncryptStringAES
��( 8
(
��8 9
userDto
��9 @
.
��@ A
Password
��A I
)
��I J
;
��J K
	_userRepo
�� 
.
�� 
Salvar
�� 
(
�� 

userByName
�� '
)
��' (
;
��( )
isUser
�� 
=
�� &
CheckUserAndPassDataBase
�� -
(
��- .
userDto
��. 5
)
��5 6
;
��6 7
return
�� 
isUser
�� 
;
�� 
}
�� 	
private
�� 
UserSgq
�� 
CreateUserFromAd
�� (
(
��( )
UserDTO
��) 0
userDto
��1 8
)
��8 9
{
�� 	
userDto
�� 
.
�� 
ParCompany_Id
�� !
=
��" #
_baseParCompany
��$ 3
.
��3 4
First
��4 9
(
��9 :
)
��: ;
.
��; <
Id
��< >
;
��> ?
userDto
�� 
.
�� 
FullName
�� 
=
�� 
userDto
�� &
.
��& '
Name
��' +
;
��+ ,
userDto
�� 
.
�� 
PasswordDate
��  
=
��! "
DateTime
��# +
.
��+ ,
Now
��, /
;
��/ 0
userDto
�� 
.
�� !
ValidaObjetoUserDTO
�� '
(
��' (
)
��( )
;
��) *
var
�� 
newUser
�� 
=
�� 
Mapper
��  
.
��  !
Map
��! $
<
��$ %
UserSgq
��% ,
>
��, -
(
��- .
userDto
��. 5
)
��5 6
;
��6 7
	_userRepo
�� 
.
�� 
Salvar
�� 
(
�� 
newUser
�� $
)
��$ %
;
��% &
return
�� 
newUser
�� 
;
�� 
}
�� 	
public
�� 
GenericReturn
�� 
<
�� 
List
�� !
<
��! "
UserDTO
��" )
>
��) *
>
��* +$
GetAllUserValidationAd
��, B
(
��B C
UserDTO
��C J
userDto
��K R
)
��R S
{
�� 	
try
�� 
{
�� !
AuthenticationLogin
�� #
(
��# $
userDto
��$ +
)
��+ ,
;
��, -
var
�� 
retorno
�� 
=
�� 
Mapper
�� $
.
��$ %
Map
��% (
<
��( )
List
��) -
<
��- .
UserSgq
��. 5
>
��5 6
,
��6 7
List
��8 <
<
��< =
UserDTO
��= D
>
��D E
>
��E F
(
��F G
	_userRepo
��G P
.
��P Q

GetAllUser
��Q [
(
��[ \
)
��\ ]
)
��] ^
;
��^ _
foreach
�� 
(
�� 
var
�� 
i
�� 
in
�� !
retorno
��" )
)
��) *
{
�� 
if
�� 
(
�� 
!
�� 
string
�� 
.
��  
IsNullOrEmpty
��  -
(
��- .
i
��. /
.
��/ 0
Password
��0 8
)
��8 9
)
��9 :
{
�� 
var
�� 
decript
�� #
=
��$ %
Guard
��& +
.
��+ ,
DecryptStringAES
��, <
(
��< =
i
��= >
.
��> ?
Password
��? G
)
��G H
;
��H I
if
�� 
(
�� 
i
�� 
.
�� 
Password
�� &
.
��& '
Equals
��' -
(
��- .
decript
��. 5
)
��5 6
)
��6 7
Guard
�� !
.
��! "
EncryptStringAES
��" 2
(
��2 3
i
��3 4
.
��4 5
Password
��5 =
)
��= >
;
��> ?
}
�� 
}
�� 
return
�� 
new
�� 
GenericReturn
�� (
<
��( )
List
��) -
<
��- .
UserDTO
��. 5
>
��5 6
>
��6 7
(
��7 8
retorno
��8 ?
)
��? @
;
��@ A
}
�� 
catch
�� 
(
�� 
	Exception
�� 
e
�� 
)
�� 
{
�� 
return
�� 
new
�� 
GenericReturn
�� (
<
��( )
List
��) -
<
��- .
UserDTO
��. 5
>
��5 6
>
��6 7
(
��7 8
e
��8 9
,
��9 :
	mensagens
��; D
.
��D E

falhaGeral
��E O
)
��O P
;
��P Q
}
�� 
}
�� 	
public
�� 
static
�� 
bool
�� 
CheckUserInAD
�� (
(
��( )
string
��) /
domain
��0 6
,
��6 7
string
��8 >
username
��? G
,
��G H
string
��I O
password
��P X
,
��X Y
string
��Z `
userVerific
��a l
)
��l m
{
�� 	
try
�� 
{
�� 
using
�� 
(
�� 
var
�� 
domainContext
�� (
=
��) *
new
��+ .
PrincipalContext
��/ ?
(
��? @
ContextType
��@ K
.
��K L
Domain
��L R
,
��R S
domain
��T Z
,
��Z [
username
��\ d
,
��d e
password
��f n
)
��n o
)
��o p
{
�� 
using
�� 
(
�� 
var
�� 
user
�� #
=
��$ %
new
��& )
UserPrincipal
��* 7
(
��7 8
domainContext
��8 E
)
��E F
)
��F G
{
�� 
user
�� 
.
�� 
SamAccountName
�� +
=
��, -
userVerific
��. 9
;
��9 :
using
�� 
(
�� 
var
�� "
pS
��# %
=
��& '
new
��( +
PrincipalSearcher
��, =
(
��= >
)
��> ?
)
��? @
{
�� 
pS
�� 
.
�� 
QueryFilter
�� *
=
��+ ,
user
��- 1
;
��1 2
using
�� !
(
��" ##
PrincipalSearchResult
��# 8
<
��8 9
	Principal
��9 B
>
��B C
results
��D K
=
��L M
pS
��N P
.
��P Q
FindAll
��Q X
(
��X Y
)
��Y Z
)
��Z [
{
�� 
if
��  "
(
��# $
results
��$ +
!=
��, .
null
��/ 3
&&
��4 6
results
��7 >
.
��> ?
Count
��? D
(
��D E
)
��E F
>
��G H
$num
��I J
)
��J K
{
��  !
return
��$ *
true
��+ /
;
��/ 0
}
��  !
}
�� 
}
�� 
}
�� 
}
�� 
return
�� 
false
�� 
;
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
)
�� 
{
�� 
return
�� 
false
�� 
;
�� 
}
�� 
}
�� 	
public
�� 
static
�� 
bool
�� 
CheckUserInAD
�� (
(
��( )
string
��) /
domain
��0 6
,
��6 7
string
��8 >
username
��? G
,
��G H
string
��I O
password
��P X
)
��X Y
{
�� 	
using
�� 
(
�� 
PrincipalContext
�� #
pc
��$ &
=
��' (
new
��) ,
PrincipalContext
��- =
(
��= >
ContextType
��> I
.
��I J
Domain
��J P
,
��P Q
domain
��R X
)
��X Y
)
��Y Z
{
�� 
var
�� 
	userValid
�� 
=
�� 
pc
��  "
.
��" #!
ValidateCredentials
��# 6
(
��6 7
username
��7 ?
,
��? @
password
��A I
)
��I J
;
��J K
return
�� 
	userValid
��  
;
��  !
}
�� 
}
�� 	
private
�� 
UserSgq
�� 
LoginBrasil
�� #
(
��# $
UserDTO
��$ +
userDto
��, 3
,
��3 4
UserSgq
��5 <

userByName
��= G
)
��G H
{
�� 	
var
�� 
isCreate
�� 
=
�� 
false
��  
;
��  !
try
�� 
{
�� 
if
�� 
(
�� 

userByName
�� 
==
�� !
null
��" &
)
��& '
{
�� &
CriaUSerSgqPeloUserSgqBR
�� ,
(
��, -
userDto
��- 4
)
��4 5
;
��5 6
isCreate
�� 
=
�� 
true
�� #
;
��# $
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
e
�� 
)
�� 
{
�� 
throw
�� 
new
�� 
	Exception
�� #
(
��# $
$str
��$ c
,
��c d
e
��e f
)
��f g
;
��g h
}
�� 
UserSgq
�� 
isUser
�� 
=
�� &
CheckUserAndPassDataBase
�� 5
(
��5 6
userDto
��6 =
)
��= >
;
��> ?
try
�� 
{
�� 
userDto
�� 
.
�� 
Id
�� 
=
�� 
isUser
�� #
.
��# $
Id
��$ &
;
��& '/
!AtualizaRolesSgqBrPelosDadosDoErp
�� 1
(
��1 2
userDto
��2 9
)
��9 :
;
��: ;
if
�� 
(
�� 
isCreate
�� 
&&
�� 
isUser
��  &
.
��& '
ParCompany_Id
��' 4
==
��5 7
null
��8 <
||
��= ?
!
��@ A
(
��A B
isUser
��B H
.
��H I
ParCompany_Id
��I V
>
��W X
$num
��Y Z
)
��Z [
)
��[ \
{
�� 
var
�� 
firstCompany
�� $
=
��% &%
_baseParCompanyXUserSgq
��' >
.
��> ?
GetAll
��? E
(
��E F
)
��F G
.
��G H
FirstOrDefault
��H V
(
��V W
r
��W X
=>
��Y [
r
��\ ]
.
��] ^

UserSgq_Id
��^ h
==
��i k
isUser
��l r
.
��r s
Id
��s u
)
��u v
;
��v w
isUser
�� 
.
�� 
ParCompany_Id
�� (
=
��) *
firstCompany
��+ 7
.
��7 8
ParCompany_Id
��8 E
;
��E F
	_userRepo
�� 
.
�� 
Salvar
�� $
(
��$ %
isUser
��% +
)
��+ ,
;
��, -
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
e
�� 
)
�� 
{
�� 
throw
�� 
new
�� 
	Exception
�� #
(
��# $
$str
��$ c
,
��c d
e
��e f
)
��f g
;
��g h
}
�� 
isUser
�� 
.
��  
ParCompanyXUserSgq
�� %
=
��& '%
_baseParCompanyXUserSgq
��( ?
.
��? @
GetAll
��@ F
(
��F G
)
��G H
.
��H I
Where
��I N
(
��N O
r
��O P
=>
��Q S
r
��T U
.
��U V

UserSgq_Id
��V `
==
��a c
isUser
��d j
.
��j k
Id
��k m
)
��m n
.
��n o
ToList
��o u
(
��u v
)
��v w
;
��w x
return
�� 
isUser
�� 
;
�� 
}
�� 	
private
�� 
void
�� /
!AtualizaRolesSgqBrPelosDadosDoErp
�� 6
(
��6 7
UserDTO
��7 >
userDto
��? F
)
��F G
{
�� 	
using
�� 
(
�� 
var
�� 
db
�� 
=
�� 
new
��  
SGQ_GlobalEntities
��  2
(
��2 3
)
��3 4
)
��4 5
{
�� 
Usuario
�� 
usuarioSgqBr
�� $
;
��$ %
try
�� 
{
�� 
usuarioSgqBr
��  
=
��! "
db
��# %
.
��% &
Usuario
��& -
.
��- .
AsNoTracking
��. :
(
��: ;
)
��; <
.
��< =
FirstOrDefault
��= K
(
��K L
r
��L M
=>
��N P
r
��Q R
.
��R S
cSigla
��S Y
.
��Y Z
ToLower
��Z a
(
��a b
)
��b c
==
��d f
userDto
��g n
.
��n o
Name
��o s
.
��s t
ToLower
��t {
(
��{ |
)
��| }
)
��} ~
;
��~ 
}
�� 
catch
�� 
(
�� 
	Exception
��  
e
��! "
)
��" #
{
�� 
throw
�� 
new
�� 
	Exception
�� '
(
��' (
$str
��( T
,
��T U
e
��V W
)
��W X
;
��X Y
}
�� 
if
�� 
(
�� 
usuarioSgqBr
��  
!=
��! #
null
��$ (
)
��( )
{
�� 
IEnumerable
�� 
<
��  "
UsuarioPerfilEmpresa
��  4
>
��4 5'
usuarioPerfilEmpresaSgqBr
��6 O
;
��O P
IEnumerable
�� 
<
��   
ParCompanyXUserSgq
��  2
>
��2 3
rolesSgqGlobal
��4 B
;
��B C
IEnumerable
�� 
<
��  

ParCompany
��  *
>
��* +!
allCompanySgqGlobal
��, ?
;
��? @
try
�� 
{
�� '
usuarioPerfilEmpresaSgqBr
�� 1
=
��2 3
db
��4 6
.
��6 7"
UsuarioPerfilEmpresa
��7 K
.
��K L
Where
��L Q
(
��Q R
r
��R S
=>
��T V
r
��W X
.
��X Y

nCdUsuario
��Y c
==
��d f
usuarioSgqBr
��g s
.
��s t

nCdUsuario
��t ~
)
��~ 
;�� �
rolesSgqGlobal
�� &
=
��' (%
_baseParCompanyXUserSgq
��) @
.
��@ A
GetAll
��A G
(
��G H
)
��H I
.
��I J
Where
��J O
(
��O P
r
��P Q
=>
��R T
r
��U V
.
��V W

UserSgq_Id
��W a
==
��b d
userDto
��e l
.
��l m
Id
��m o
)
��o p
;
��p q!
allCompanySgqGlobal
�� +
=
��, -
_baseParCompany
��. =
.
��= >
GetAll
��> D
(
��D E
)
��E F
;
��F G
}
�� 
catch
�� 
(
�� 
	Exception
�� $
e
��% &
)
��& '
{
�� 
throw
�� 
new
�� !
	Exception
��" +
(
��+ ,
$str
��, Y
,
��Y Z
e
��[ \
)
��\ ]
;
��] ^
}
�� 
foreach
�� 
(
�� 
var
��  
upe
��! $
in
��% ''
usuarioPerfilEmpresaSgqBr
��( A
)
��A B
{
�� 
var
�� 
perfilSgqBr
�� '
=
��( )
db
��* ,
.
��, -
Perfil
��- 3
.
��3 4
FirstOrDefault
��4 B
(
��B C
r
��C D
=>
��E G
r
��H I
.
��I J
	nCdPerfil
��J S
==
��T V
upe
��W Z
.
��Z [
	nCdPerfil
��[ d
)
��d e
.
��e f
	nCdPerfil
��f o
.
��o p
ToString
��p x
(
��x y
)
��y z
;
��z {
var
�� !
parCompanySgqGlobal
�� /
=
��0 1!
allCompanySgqGlobal
��2 E
.
��E F
FirstOrDefault
��F T
(
��T U
r
��U V
=>
��W Y
r
��Z [
.
��[ \
IntegrationId
��\ i
==
��j l
upe
��m p
.
��p q

nCdEmpresa
��q {
)
��{ |
;
��| }
if
�� 
(
�� !
parCompanySgqGlobal
�� /
!=
��0 2
null
��3 7
)
��7 8
{
�� 
if
�� 
(
��  
rolesSgqGlobal
��  .
.
��. /
Any
��/ 2
(
��2 3
r
��3 4
=>
��5 7
r
��8 9
.
��9 :
ParCompany_Id
��: G
==
��H J!
parCompanySgqGlobal
��K ^
.
��^ _
Id
��_ a
&&
��b d
r
��e f
.
��f g

UserSgq_Id
��g q
==
��r t
userDto
��u |
.
��| }
Id
��} 
&&��� �
r��� �
.��� �
Role��� �
==��� �
perfilSgqBr��� �
)��� �
)��� �
{
�� 
}
�� 
else
��  
if
��! #
(
��$ %
!
��% &
rolesSgqGlobal
��& 4
.
��4 5
Any
��5 8
(
��8 9
r
��9 :
=>
��; =
r
��> ?
.
��? @
ParCompany_Id
��@ M
==
��N P!
parCompanySgqGlobal
��Q d
.
��d e
Id
��e g
&&
��h j
r
��k l
.
��l m

UserSgq_Id
��m w
==
��x z
userDto��{ �
.��� �
Id��� �
)��� �
)��� �
{
�� 
var
��  # 
adicionaRoleGlobal
��$ 6
=
��7 8
new
��9 < 
ParCompanyXUserSgq
��= O
(
��O P
)
��P Q
{
��  !
ParCompany_Id
��$ 1
=
��2 3!
parCompanySgqGlobal
��4 G
.
��G H
Id
��H J
,
��J K

UserSgq_Id
��$ .
=
��/ 0
userDto
��1 8
.
��8 9
Id
��9 ;
,
��; <
Role
��$ (
=
��) *
perfilSgqBr
��+ 6
}
��  !
;
��! "%
_baseParCompanyXUserSgq
��  7
.
��7 8
AddOrUpdate
��8 C
(
��C D 
adicionaRoleGlobal
��D V
)
��V W
;
��W X
}
�� 
}
�� 
}
�� 
try
�� 
{
�� 
var
�� (
existentesSomenteSgqGlobal
�� 6
=
��7 8%
_baseParCompanyXUserSgq
��9 P
.
��P Q
GetAll
��Q W
(
��W X
)
��X Y
.
��Y Z
Where
��Z _
(
��_ `
r
��` a
=>
��b d
r
��e f
.
��f g

UserSgq_Id
��g q
==
��r t
userDto
��u |
.
��| }
Id
��} 
)�� �
;��� �
var
�� *
todosOsPerfisSgqBrAssociados
�� 8
=
��9 :
db
��; =
.
��= >
Perfil
��> D
.
��D E
Where
��E J
(
��J K
r
��K L
=>
��M O'
usuarioPerfilEmpresaSgqBr
��P i
.
��i j
Any
��j m
(
��m n
upe
��n q
=>
��r t
upe
��u x
.
��x y
	nCdPerfil��y �
==��� �
r��� �
.��� �
	nCdPerfil��� �
)��� �
)��� �
;��� �
if
�� 
(
�� *
todosOsPerfisSgqBrAssociados
�� 8
!=
��9 ;
null
��< @
)
��@ A
{
�� (
existentesSomenteSgqGlobal
�� 6
=
��7 8(
existentesSomenteSgqGlobal
��9 S
.
��S T
Where
��T Y
(
��Y Z
r
��Z [
=>
��\ ^*
todosOsPerfisSgqBrAssociados
��_ {
.
��{ |
Any
��| 
(�� �
t��� �
=>��� �
!��� �
(��� �
t��� �
.��� �
	nCdPerfil��� �
.��� �
ToString��� �
(��� �
)��� �
==��� �
r��� �
.��� �
Role��� �
)��� �
)��� �
)��� �
;��� �
foreach
�� #
(
��$ %
var
��% ($
removerPerfilSgqGlobal
��) ?
in
��@ B(
existentesSomenteSgqGlobal
��C ]
)
��] ^%
_baseParCompanyXUserSgq
��  7
.
��7 8
Remove
��8 >
(
��> ?$
removerPerfilSgqGlobal
��? U
)
��U V
;
��V W
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� $
e
��% &
)
��& '
{
�� 
throw
�� 
new
�� !
	Exception
��" +
(
��+ ,
$str
��, u
,
��u v
e
��w x
)
��x y
;
��y z
}
�� 
}
�� 
}
�� 
}
�� 	
private
�� 
void
�� &
CriaUSerSgqPeloUserSgqBR
�� -
(
��- .
UserDTO
��. 5
userDto
��6 =
)
��= >
{
�� 	
using
�� 
(
�� 
var
�� 
db
�� 
=
�� 
new
��  
SGQ_GlobalEntities
��  2
(
��2 3
)
��3 4
)
��4 5
{
�� 
try
�� 
{
�� 
var
�� !
existenteNoDbAntigo
�� +
=
��, -
db
��. 0
.
��0 1
Usuario
��1 8
.
��8 9
FirstOrDefault
��9 G
(
��G H
r
��H I
=>
��J L
r
��M N
.
��N O
cSigla
��O U
.
��U V
ToLower
��V ]
(
��] ^
)
��^ _
==
��` b
userDto
��c j
.
��j k
Name
��k o
.
��o p
ToLower
��p w
(
��w x
)
��x y
)
��y z
;
��z {
if
�� 
(
�� !
existenteNoDbAntigo
�� +
!=
��, .
null
��/ 3
)
��3 4
{
�� 
UserSgq
�� 

newUserSgq
��  *
;
��* +
try
�� 
{
�� 

newUserSgq
�� &
=
��' (
new
��) ,
UserSgq
��- 4
(
��4 5
)
��5 6
{
�� 
Name
��  $
=
��% &!
existenteNoDbAntigo
��' :
.
��: ;
cSigla
��; A
.
��A B
ToLower
��B I
(
��I J
)
��J K
,
��K L
FullName
��  (
=
��) *!
existenteNoDbAntigo
��+ >
.
��> ?

cNmUsuario
��? I
,
��I J
Password
��  (
=
��) *
Guard
��+ 0
.
��0 1
EncryptStringAES
��1 A
(
��A B
userDto
��B I
.
��I J
Password
��J R
)
��R S
}
�� 
;
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� (
e
��) *
)
��* +
{
�� 
throw
�� !
new
��" %
	Exception
��& /
(
��/ 0
$str
��0 R
,
��R S
e
��T U
)
��U V
;
��V W
}
�� 
try
�� 
{
�� 
	_userRepo
�� %
.
��% &
Salvar
��& ,
(
��, -

newUserSgq
��- 7
)
��7 8
;
��8 9
userDto
�� #
.
��# $
Id
��$ &
=
��' (

newUserSgq
��) 3
.
��3 4
Id
��4 6
;
��6 7
}
�� 
catch
�� 
(
�� 
	Exception
�� (
e
��) *
)
��* +
{
�� 
throw
�� !
new
��" %
	Exception
��& /
(
��/ 0
$str
��0 r
,
��r s
e
��t u
)
��u v
;
��v w
}
�� 
}
�� 
else
�� 
{
�� 
throw
�� 
new
�� !
	Exception
��" +
(
��+ ,
$str
��, b
)
��b c
;
��c d
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
��  
e
��! "
)
��" #
{
�� 
new
�� 
	CreateLog
�� !
(
��! "
new
��" %
	Exception
��& /
(
��/ 0
$str
��0 a
,
��a b
e
��c d
)
��d e
)
��e f
;
��f g
throw
�� 
e
�� 
;
�� 
}
�� 
}
�� 
}
�� 	
}
�� 
}�� 