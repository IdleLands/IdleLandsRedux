// pure JS stuff

function GetStaticBonusObject() {
	return GetDefaultBonusObject();
}

function GetDefaultMultiplyOtherScriptBonusObject() {
	return GetDefaultMultiplyBonusObject();
}

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