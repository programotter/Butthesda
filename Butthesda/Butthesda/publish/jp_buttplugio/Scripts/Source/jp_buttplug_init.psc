Scriptname jp_buttplug_init extends Quest  
Actor Property PlayerREF Auto
SexLabFramework property SexLab auto

zadLibs zad_libs
slaUtilScr sla_util
;dcc_sgo_QuestController SGO_Controller

;running mods
bool DD_Running = false
bool SGO_Running = false
bool BF_Running = false
bool MME_Running = false
bool SLA_Running = false

;DD
keyword DeviousPlugVaginal 
keyword DeviousPlugAnal  
keyword DeviousPiercingsVaginal 
keyword DeviousPlug
keyword DeviousPiercingsNipple
Actor PlayerHorse



event OnInit()
	Maintenance()
endEvent


Function Maintenance()
	CheckLoadMods()
	RegModEvents()
	SendMessage("")
	SendMessage("'mod':'game','event':'loading save','DD_Running':"+DD_Running+",'SGO_Running':"+SGO_Running+",'BF_Running':"+BF_Running+",'MME_Running':"+MME_Running+",'SLA_Running':"+SLA_Running)
	
	SetVariables()
	
	SendMessage("'mod':'game','event':'loading save done'")
endFunction

string link_file = "data/FunScripts/link.txt"
Function sendMessage(string msg)
	MiscUtil.WriteToFile(link_file,"{"+msg+"}\n",true,true)
EndFunction

Function sendInfo(string msg)
	MiscUtil.WriteToFile(link_file,msg+"\n",true,true)
EndFunction



Function CheckLoadMods()
	if  Game.GetModByName("Devious Devices - Integration.esm")!=255
		DD_Running = true
	endIf
	if  Game.GetModByName("dcc-soulgem-oven-000.esp")!=255
		SGO_Running = true
	endIf
	if Game.GetModByName("BeeingFemale.esm")!=255
		BF_Running = true
	endIf
	if Game.GetModByName("MilkModNEW.esp")!=255
		MME_Running = true
	endIf
	if Game.GetModByName("SexLabAroused.esm")!=255
		SLA_Running = true
	endIf
EndFunction

Function RegModEvents()
	;Cleanup
		UnregisterForAllModEvents()		

	;General
		RegisterForMenu("Cursor Menu")

	;SEXLAB
		RegisterForModEvent("PlayerAnimationStart", "SL_AnimationStart")
		RegisterForModEvent("PlayerAnimationEnding", "SL_AnimationEnding")
		RegisterForModEvent("PlayerAnimationChange", "SL_AnimationChange")
		RegisterForModEvent("PlayerPositionChange", "SL_PositionChange")
		;..
		RegisterForModEvent("PlayerStageStart", "SL_StageStart")
		RegisterForModEvent("PlayerStageEnd", "SL_StageEnd")
		;..
		RegisterForModEvent("PlayerOrgasmStart", "SL_OrgasmStart")
		RegisterForModEvent("PlayerOrgasmEnd", "SL_OrgasmEnd")

	if SLA_Running
		RegisterForModEvent("sla_UpdateComplete", "sla_Update")
	endif

	if DD_Running
		RegisterForModEvent("DDI_DeviceRemoved", "DD_DeviceRemovedEvent")
		RegisterForModEvent("DDI_DeviceEquipped", "DD_DeviceEquippedEvent")
	
		RegisterForModEvent("DeviceVibrateEffectStart", "DD_VibrateEffectStart")
		RegisterForModEvent("DeviceVibrateEffectStop", "DD_VibrateEffectStop")
		
		RegisterForModEvent("DeviceActorOrgasm", "DD_Orgasm")
		RegisterForModEvent("DeviceEdgedActor", "DD_Edged")
		

		RegisterForSingleUpdate(3.0) ;wait for dd to register its events
		
		
		;We dont need RegisterForAnimationEvent we hook in to the game and read it there
		;animations
		;RegisterForAnimationEvent(playerref,"IdleStop");to stop horny and yokeStruggle
		;walking
		;RegisterForAnimationEvent(playerref,"FootLeft")
		;RegisterForAnimationEvent(playerref,"FootRight")
		;RegisterForAnimationEvent(playerref,"FootSprintRight")
		;RegisterForAnimationEvent(playerref,"FootSprintLeft")
		;RegisterForAnimationEvent(playerref,"tailMTIdle")
		;horse
		;RegisterForAnimationEvent(playerref,"SoundPlay.NPCHorseMount")
		;RegisterForAnimationEvent(playerref,"SoundPlay.NPCHorseDismount")
		;sitting
		;RegisterForAnimationEvent(playerref,"IdleChairSitting")
		;RegisterForAnimationEvent(playerref,"idleChairGetUp")
		;jumping
		;RegisterForAnimationEvent(playerref,"JumpUp")
		;RegisterForAnimationEvent(playerref,"JumpLandEnd")
	endIf

	if MME_Running
		RegisterForModEvent("MilkQuest.StartMilkingMachine", "MME_StartMilkingMachine")
		RegisterForModEvent("MilkQuest.StopMilkingMachine",  "MME_StopMilkingMachine")
		
		RegisterForModEvent("MilkQuest.FeedingStage", "MME_FeedingStage")
		RegisterForModEvent("MilkQuest.MilkingStage",  "MME_MilkingStage")
		RegisterForModEvent("MilkQuest.FuckMachineStage", "MME_FuckMachineStage")
	endif

	If SGO_Running
		RegisterForModEvent("SGO.OnBirthing", "SGO_SoulGemBirthing")
		RegisterForModEvent("SGO.OnBirthed",  "SGO_SoulGem")

		RegisterForModEvent("SGO.OnMilking", "SGO_Milking")
		
		RegisterForModEvent("SGO.OnInseminating", "SGO_Inseminating")
		RegisterForModEvent("SGO.OnInseminated", "SGO_Inseminated")
		
		RegisterForModEvent("SGO.OnInserting", "SGO_Inserting")
		RegisterForModEvent("SGO.OnInserted", "SGO_Inserted")
	endIf

	if BF_Running
		;We dont need RegisterForAnimationEvent we hook in to the game and read it there
		;RegisterForAnimationEvent(playerref, "Birth_S1")
		;RegisterForAnimationEvent(playerref, "Birth_S2")
		;RegisterForAnimationEvent(playerref, "Birth_S3")
	endIf

EndFunction

Event OnUpdate()
	sendInfo("OnUpdate")
	int i = 0
	while i < zad_libs.EventSlots.Slotted
		string name = zad_libs.EventSlots.Slots[i].Name
		sendInfo("register: "+name)
		RegisterForModEvent("DeviousEvent"+name, "DD_GenericEvent")
		i += 1
	EndWhile
endevent



Function SetVariables()
	sla_util = none
	if SLA_Running
		sla_util = Quest.GetQuest("sla_Framework") as slaUtilScr
		SendMessage("'mod':'sla','arousal':"+sla_util.GetActorArousal(PlayerRef))
	endIf

	zad_libs = none
	if DD_Running
		zad_libs = Quest.GetQuest("zadQuest") as zadLibs

		DeviousPlug  = zad_libs.zad_DeviousPlug
		DeviousPlugVaginal  = zad_libs.zad_DeviousPlugVaginal
		DeviousPlugAnal  = zad_libs.zad_DeviousPlugAnal
		DeviousPiercingsVaginal  = zad_libs.zad_DeviousPiercingsVaginal
		DeviousPiercingsNipple = zad_libs.zad_DeviousPiercingsNipple

		DD_CheckEquipPlugs()
	endIf

	;SGO_Controller = none
	;If SGO_Running
	;	SGO_Controller = Quest.GetQuest("dcc_sgo_QuestController") as dcc_sgo_QuestController
	;endIf

EndFunction


;this code pauses the vibrator when a menu is open.
bool menuOpen = false
Event OnMenuOpen(String MenuName)
	if MenuName == "Cursor Menu"
		if !menuOpen
			menuOpen = true
			SendMessage("'mod':'game','event':'menu opened'")
		endif
	endif
endEvent
Event OnMenuClose(String MenuName)
	if MenuName == "Cursor Menu"
		if menuOpen
			menuOpen = false
			SendMessage("'mod':'game','event':'menu closed'")
		endif
	endif
EndEvent


;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;DEVIUOS DEVICES
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

function DD_CheckEquipPlugs()
	string[] EquippedItems = new string[4] ;0-vagina 1-anal 2-vaginalPiecing 3-nipplePiercing (pump,magic,normal)

	if playerref.WornHasKeyword(DeviousPlugVaginal)
		Armor EquipPlugVaginal = zad_libs.GetWornDevice(playerref, DeviousPlugVaginal)
		if zad_libs.HasTag(EquipPlugVaginal,"pump")||zad_libs.HasTag(EquipPlugVaginal,"inflatable")
			EquippedItems[0] = "pump"
		elseIf zad_libs.HasTag(EquipPlugVaginal,"magic")||zad_libs.HasTag(EquipPlugVaginal,"soulgem")
			EquippedItems[0] = "magic"
		else
			EquippedItems[0] = "normal"
		endIf
	else
		EquippedItems[0] = "none"
	endIf
	
	if playerref.WornHasKeyword(DeviousPlugAnal) 
		Armor EquipPlugAnal = zad_libs.GetWornDevice(playerref, DeviousPlugAnal)
		if zad_libs.HasTag(EquipPlugAnal,"pump")||zad_libs.HasTag(EquipPlugAnal,"inflatable")
			EquippedItems[1] = "pump"
		elseIf zad_libs.HasTag(EquipPlugAnal,"magic")||zad_libs.HasTag(EquipPlugAnal,"soulgem")
			EquippedItems[1] = "magic"
		else
			EquippedItems[1] = "normal"
		endIf
	else
		EquippedItems[1] = "none"
	endIf
	
	if playerref.WornHasKeyword(DeviousPiercingsVaginal)
		Armor EquipPiercingsVaginal = zad_libs.GetWornDevice(playerref, DeviousPiercingsVaginal)
		If zad_libs.HasTag(EquipPiercingsVaginal,"magic")||zad_libs.HasTag(EquipPiercingsVaginal,"soulgem")
			EquippedItems[2] = "magic"
		else
			EquippedItems[2] = "normal"
		endIf
	else
		EquippedItems[2] = "none"
	endIf
	
	if playerref.WornHasKeyword(DeviousPiercingsNipple)
		Armor EquipPiercingsNipple = zad_libs.GetWornDevice(playerref, DeviousPiercingsNipple)
		If zad_libs.HasTag(EquipPiercingsNipple,"magic")||zad_libs.HasTag(EquipPiercingsNipple,"soulgem")
			EquippedItems[3] = "magic"
		else
			EquippedItems[3] = "normal"
		endIf
	else
		EquippedItems[3] = "none"
	endIf
	
	sendMessage("'mod':'dd','event':'(de)equiped','vaginal':'"+EquippedItems[0]+"','anal':'"+EquippedItems[1]+"','vaginalPiecing':'"+EquippedItems[2]+"','nipplePiercing':'"+EquippedItems[3]+"'")
endFunction
		
Event DD_VibrateEffectStart(string eventName, string argString, float argNum, form sender)
	SendInfo("DD_VibrateEffectStart")
	if(argString == playerref.GetBaseObject().GetName());
		sendMessage("'mod':'dd','event':'Vibrate Effect Start','arg':"+argNum)
	endIf
endEvent

Event DD_VibrateEffectStop(string eventName, string argString, float argNum, form sender)
	SendInfo("DD_VibrateEffectStop")
	if(argString == playerref.GetBaseObject().GetName());
		sendMessage("'mod':'dd','event':'Vibrate Effect Stop','arg':"+argNum)
	endIf
endEvent

Event DD_Orgasm(string eventName, string argString, float argNum, form sender)
	SendInfo("DD_Orgasm")
	if(argString == playerref.GetBaseObject().GetName());
		sendMessage("'mod':'dd','event':'orgasm','arg':"+argNum)
	endIf
endEvent

Event DD_Edged(string eventName, string argString, float argNum, form sender)
	SendInfo("DD_Edged")
	if(argString == playerref.GetBaseObject().GetName());
		sendMessage("'mod':'dd','event':'edged','arg':"+argNum)
	endIf
endEvent

Event DD_GenericEvent(string eventName, string argString, float argNum, form sender)
	if(argString == playerref.GetBaseObject().GetName());
		sendMessage("'mod':'dd','event':'device event', 'type':'"+StringUtil.Substring(eventName, 12) +"'")
	endif
endEvent

Event DD_DeviceEquippedEvent(form inventoryDevice, form deviceKeyword, form akActor)
	sendInfo("DD_DeviceEquippedEvent")
	if(akActor as actor == playerref)
		Utility.WaitMenuMode(0.5)
		DD_CheckEquipPlugs()
	endIf
endEvent

Event DD_DeviceRemovedEvent(form inventoryDevice, form deviceKeyword, form akActor)
	sendInfo("DD_DeviceRemovedEvent")
	if(akActor as actor == playerref)
		Utility.WaitMenuMode(0.5)
		DD_CheckEquipPlugs()
	endIf
endEvent


;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;SEXLAB AROUSAL
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
event sla_Update(string eventName, string argString, float argNum, form sender)
	sendInfo("sla_Update")
	if(argNum > 0)     ;Do my thing because there is at least one actor to process
		SendMessage("'mod':'sla','arousal':"+sla_util.GetActorArousal(PlayerRef))
	endif
endevent

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;SEXLAB
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

event SL_AnimationStart(string eventName, string argString, float argNum, form sender)
	SL_AnimationFunction("animation started", argString)
endEvent

event SL_AnimationChange(string eventName, string argString, float argNum, form sender)
	SL_AnimationFunction("animation changed", argString)
endEvent

event SL_AnimationEnding(string eventName, string argString, float argNum, form sender)
	SendMessage("'mod':'sexlab','event':'animation ended'")
endEvent

event SL_PositionChange(string eventName, string argString, float argNum, form sender)
	SL_AnimationFunction("position changed", argString)
endEvent

event SL_StageStart(string eventName, string argString, float argNum, form sender)
	SL_AnimationFunction("stage started", argString)
endEvent

event SL_StageEnd(string eventName, string argString, float argNum, form sender)
	SendMessage("'mod':'sexlab','event':'stage ended'")
endEvent

event SL_OrgasmStart(string eventName, string argString, float argNum, form sender)
	SendMessage("'mod':'sexlab','event':'orgasm started'")
endEvent

event SL_OrgasmEnd(string eventName, string argString, float argNum, form sender)
	SendMessage("'mod':'sexlab','event':'orgasm ended'")
endEvent


function SL_AnimationFunction(String eventName, String argString)
	sslBaseAnimation animation = SexLab.HookAnimation(argString)
	sslThreadController controller = SexLab.HookController(argString)	
	
	String name = animation.name
	int stage = SexLab.HookStage(argString)
	int pos = controller.getPlayerPosition()
	
	Utility.wait(0.1)
	bool usingStrappon = SexLab.HasStrapon(PlayerREF);
	
	SendMessage("'mod':'sexlab','event':'"+eventName+"','name':'"+name+"','stage':"+stage+",'pos':"+pos+",'usingStrappon':"+usingStrappon)
endFunction


;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;Milk Mod Economy
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
event MME_StartMilkingMachine(Form Who, int mpas, int MilkingType)
	if Who == playerref
		SendMessage("'mod':'MME','event':'StartMilkingMachine','mpas':"+mpas+",'MilkingType':"+MilkingType)
	endIf
endEvent

event MME_StopMilkingMachine(Form Who, int mpas, int MilkingType)
	if Who == playerref
		SendMessage("'mod':'MME','event':'StopMilkingMachine','mpas':"+mpas+",'MilkingType':"+MilkingType)
	endIf
endEvent

event MME_FeedingStage(Form Who, int mpas, int MilkingType)
	if Who == playerref
		SendMessage("'mod':'MME','event':'FeedingStage','mpas':"+mpas+",'MilkingType':"+MilkingType)
	endIf
endEvent

event MME_MilkingStage(Form Who, int mpas, int MilkingType)
	if Who == playerref
		SendMessage("'mod':'MME','event':'MilkingStage','mpas':"+mpas+",'MilkingType':"+MilkingType)
	endIf
endEvent

event MME_FuckMachineStage(Form Who, int mpas, int MilkingType)
	if Who == playerref
		SendMessage("'mod':'MME','event':'FuckMachineStage','mpas':"+mpas+",'MilkingType':"+MilkingType)
	endIf
endEvent

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;Soul Gem Oven
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
event SGO_Milking(Form Who)
	if Who == playerref
		sendMessage("SGO:milking")
	endIf
endEvent
event SGO_SoulGemBirthing(Form Who)
	if Who == playerref
		sendMessage("SGO:SoulGemBirthing")
	endIf
endEvent
event SGO_SoulGem(Form Who, Form What)
	if Who == playerref
		;sendMessage("SGO:SoulGem:"+SGO_Controller.GetGemValue(What))
	endIf
endEvent
event SGO_Inseminating(Form Who, Form What)
	if Who == playerref
		sendMessage("SGO:Inseminating")
	endIf
endevent 
event SGO_Inseminated(Form Who, Form What)
	if Who == playerref
		sendMessage("SGO:Inseminated")
	endIf
endevent 
event SGO_Inserting(Form Who, Form What)
	if Who == playerref
		sendMessage("SGO:Inserting")
	endIf
endevent 
event SGO_Inserted(Form Who, Form What)
	if Who == playerref
		;sendMessage("SGO:Inserted"+SGO_Controller.GetGemValue(What))
	endIf
endevent