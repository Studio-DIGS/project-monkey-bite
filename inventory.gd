class_name Inventory
extends Node

# Sword

var sword: Sword

func hasSword():
	return sword != null
	
func swapSword(newSword: Sword):
	var oldSword = sword
	sword = newSword
	return oldSword

# Big Passives

var bigPassives: Array[BigPassive] = []

func addBigPassive(bigPassive: BigPassive):
	bigPassives.append(bigPassive)
	pass

func getBigPassives():
	return bigPassives
	
# Angel Abilites

var angelAbility: AngelAbility

func setAngelAbility(newAngelAbility: AngelAbility):
	angelAbility = newAngelAbility
	pass
	
func getAngelAbility():
	return angelAbility
