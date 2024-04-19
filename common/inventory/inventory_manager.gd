extends Node

var inv: Inventory = Inventory.new()

var uninitialized = true

enum SwordName {
	AMAZING_SWORD,
	
}

#var SwordMap = {
#	'Amazing Sword' = Sword.new("Rare Sword", 10.0, 5.0),
#	'Rare Sword' = Sword.new("Rare Sword", 5.0, 2.5),
#	'Epic Sword' = Sword.new("Rare Sword", 20.0, 7.5),
#	'Sword???' = Sword.new("Rare Sword", 2.5, 15.0)
#}

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
#	swapSword(SwordMap[swordName])
	inv.bigPassives = [BigPassive.new(bigPassiveName)]
	setAngelAbility(AngelAbility.new(angelAbilityName))
	

# Sword
func hasSword() -> bool:
	return inv.sword != null

func swapSword(newSwordName: String) -> Sword:
	var oldSword = inv.sword
#	inv.sword = SwordMap[newSwordName]
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
