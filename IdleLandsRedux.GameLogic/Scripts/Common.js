/* global _HasPersonalitySet */
/* global _HasClassSet */
/* global _GetDefaultBonusObject */
/* global _GetDefaultMultiplyBonusObject */
/* global System */
// pure JS stuff

//Internal function exposure

function HasPersonalitySet(personalityName) {
  	return _HasPersonalitySet(personalityName);
}

function HasClassSet(className) {
 	return _HasClassSet(className);
}

function GetDefaultBonusObject() {
	return _GetDefaultBonusObject();
}

function GetDefaultMultiplyBonusObject() {
	return _GetDefaultMultiplyBonusObject();
}

var log = System.Console.WriteLine;