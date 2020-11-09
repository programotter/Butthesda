Scriptname jp_buttplug_alias extends ReferenceAlias  

jp_buttplug_init Property QuestScript Auto


event OnPlayerLoadGame()
	QuestScript.Maintenance()
	QuestScript.sendInfo("player loaded game")
endEvent

Event OnHit(ObjectReference akAggressor, Form akSource, Projectile akProjectile, bool abPowerAttack, bool abSneakAttack, bool abBashAttack, bool abHitBlocked)
	QuestScript.SendMessage("'mod':'game','event':'damage','source':'"+akSource.getName()+"','projectile':'"+akProjectile+"','powerAttack':"+abPowerAttack+",'blocked':"+abHitBlocked)
endEvent