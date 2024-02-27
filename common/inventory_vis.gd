class_name InventoryVis
extends Node

@onready var swordTextLabel = $UICanvasGroup/SwordText
@onready var bigPassiveTextLabel = $UICanvasGroup/BigPassiveText
@onready var angelAbilityTextLabel = $UICanvasGroup/AngelAbilityText

func _ready():
	swordTextLabel.clear()
	bigPassiveTextLabel.clear()
	angelAbilityTextLabel.clear()
	updateAllText()
	
func updateAllText():
	updateAngelAbilityVis()
	updateBigPassiveVis()
	updateSwordVis()
	pass

# Sword

func updateSwordVis():
	swordTextLabel.clear()
	swordTextLabel.append_text(Inventory.getSword().name)

# Big Passives
	
func updateBigPassiveVis():
	if (Inventory.getBigPassives().size() <= 0):
		return
	bigPassiveTextLabel.clear()
	for bigPassive in Inventory.getBigPassives():
		bigPassiveTextLabel.append_text(bigPassive.name)

# Angel Abilites
	
func updateAngelAbilityVis():
	angelAbilityTextLabel.clear()
	angelAbilityTextLabel.append_text(Inventory.getAngelAbility().name)
