extends Node


var sword: Sword = Sword.new("null")
var bigPassives: Array[BigPassive] = []
var angelAbility: AngelAbility = AngelAbility.new("null")

var uninitialized = true

const SWORD_NAME_LIST: Array = [
	'Rare Sword',
	'Epic Sword',
	'Sword???'
]

const PASSIVE_NAME_LIST: Array = [
	'Double Jump',
	'Def +50%',
	'Dash Speed +100%'
]

const ANGEL_NAME_LIST: Array = [
	'Enable Flying',
	'Always Crit',
	'+500% Defence'
]

func randomInitAll():
	randomize()
	var swordName = SWORD_NAME_LIST[randi() % len(SWORD_NAME_LIST)] as String
	var bigPassiveName = PASSIVE_NAME_LIST[randi() % len(PASSIVE_NAME_LIST)] as String
	var angelAbilityName = ANGEL_NAME_LIST[randi() % len(ANGEL_NAME_LIST)] as String
	swapSword(Sword.new(swordName))
	bigPassives = [BigPassive.new(bigPassiveName)]
	setAngelAbility(AngelAbility.new(angelAbilityName))
	

# Sword
func hasSword() -> bool:
	return sword != null

func swapSword(newSword: Sword) -> Sword:
	var oldSword = sword
	sword = newSword
	return oldSword
	
func getSword() -> Sword:
	return sword

# Big Passives

func addBigPassive(bigPassive: BigPassive):
	bigPassives.append(bigPassive)
	pass

func getBigPassives() -> Array[BigPassive]:
	return bigPassives
	
# Angel Abilites

func setAngelAbility(newAngelAbility: AngelAbility):
	angelAbility = newAngelAbility
	pass

func getAngelAbility() -> AngelAbility:
	return angelAbility
