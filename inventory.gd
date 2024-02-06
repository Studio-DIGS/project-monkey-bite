class_name Inventory
extends Node

@onready var swordTextLabel = $UICanvasGroup/SwordText
@onready var bigPassiveTextLabel = $UICanvasGroup/BigPassiveText
@onready var angelAbilityTextLabel = $UICanvasGroup/AngelAbilityText

@onready var player : Player = $"../Player"

func _ready():
	swapSword(player.sword)
	bigPassiveTextLabel.clear()
	for bigPassive in player.bigPassives:
		addBigPassive(bigPassive)
	setAngelAbility(player.angelAbility)

# Sword

var sword: Sword

func hasSword():
	return sword != null
	
func swapSword(newSword: Sword):
	var oldSword = sword
	sword = newSword
	swordTextLabel.clear()
	swordTextLabel.append_text(sword.name)
	return oldSword

# Big Passives

var bigPassives: Array[BigPassive] = []

func addBigPassive(bigPassive: BigPassive):
	bigPassives.append(bigPassive)
	bigPassiveTextLabel.append_text("\n" + bigPassive.name)
	pass

func getBigPassives():
	return bigPassives
	
# Angel Abilites

var angelAbility: AngelAbility

func setAngelAbility(newAngelAbility: AngelAbility):
	angelAbility = newAngelAbility
	angelAbilityTextLabel.clear()
	angelAbilityTextLabel.append_text(angelAbility.name)
	pass
	
func getAngelAbility():
	return angelAbility
