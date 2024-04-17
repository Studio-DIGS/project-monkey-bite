class_name InventoryVis
extends Node

@onready var swordTextLabel = $UICanvasGroup/SwordText
@onready var bigPassiveTextLabel = $UICanvasGroup/BigPassiveText
@onready var angelAbilityTextLabel = $UICanvasGroup/AngelAbilityText

func _ready():
#	swordTextLabel.clear()
#	bigPassiveTextLabel.clear()
#	angelAbilityTextLabel.clear()
#	updateAllText()
	pass
	
func updateAllText():
	updateAngelAbilityVis()
	updateBigPassiveVis()
	updateSwordVis()

# Sword

func updateSwordVis():
	swordTextLabel.clear()
	swordTextLabel.append_text(InventoryManager.getSword().name)

# Big Passives
	
func updateBigPassiveVis():
	if (InventoryManager.getBigPassives().size() <= 0):
		return
	bigPassiveTextLabel.clear()
	for bigPassive in InventoryManager.getBigPassives():
		bigPassiveTextLabel.append_text(bigPassive.name)

# Angel Abilites
	
func updateAngelAbilityVis():
	angelAbilityTextLabel.clear()
	angelAbilityTextLabel.append_text(InventoryManager.getAngelAbility().name)
