extends Node

var inv: Inventory = Inventory.new()

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
	inv.bigPassives = [BigPassive.new(bigPassiveName)]
	setAngelAbility(AngelAbility.new(angelAbilityName))
	

# Sword
func hasSword() -> bool:
	return inv.sword != null

func swapSword(newSword: Sword) -> Sword:
	var oldSword = inv.sword
	inv.sword = newSword
	return oldSword
	
func getSword() -> Sword:
	return inv.sword

# Big Passives

func addBigPassive(bigPassive: BigPassive):
	inv.bigPassives.append(bigPassive)
	pass

func getBigPassives() -> Array[BigPassive]:
	return inv.bigPassives
	
# Angel Abilites

func setAngelAbility(newAngelAbility: AngelAbility):
	inv.angelAbility = newAngelAbility
	pass

func getAngelAbility() -> AngelAbility:
	return inv.angelAbility
