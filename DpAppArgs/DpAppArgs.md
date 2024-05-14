## DpAppArgs - Organises command-line arguments into key/value pairs

Namespace: DpUtilities

Nuget Package Name: DpAppArgs

Dependencies:  Package DpUtilities

### Introduction

Expects an argument list in the form

`[Pos1] [Pos2] ... [-xxx1 or aaa1=bbb1] ...`

Where `Pos1`, `Pos2`, etc are positional arguments (ie their function is inferred by their position). For instance, if a copy operation the first argument may be the path to copy from and the second the path to copy to.

`-xxx1` or `aaa1=bbb1` must follow the positional arguments but thereafter may be in any sequence. For example, `-overwrite convert=tolowercase`.

Any argument containing a space must be enclosed in double quotes (which will be removed).

The arguments are organised into a list of type `DpStringKvp` (`List<KeyValuePair<string, string>>`) (data properties `Key` and `Value`) and the sequence is maintained. 
- Positional arguments are placed in the `Value`, with an empty string as the `Key`. 
- Any argument preceded by a hyphen will be placed in `Key` (hyphen removed) with a `Value` of "-". 
- Arguments of the form name=value will have the name placed in `Key` and the value placed in `Value`. Note that `=value` is an error, whereas `name=` is OK (empty value).

Argument keys are case-insensitive and will be converted to lower-case. Keys must be unique within the list eg `-AKey` and `akey=xx` within the argument list would generate an error. There is no such restriction on values.

## DpAppArgs

`public class DpAppArgs`

### Constructor

`public DpAppArgs (string [] argList)`

`argList` is the argument list as supplied to the program's `Main ()` method.

The argument list is compiled into a Key/Value pair list, accessible as property ArgList.

If any errors were found, these are included in the list with a ">" in the `Key` and an error message in the `Value`. The errors are more conveniently accessed as an array of error messages via property ErrorList and flagged by property HaveErrors.

### Methods

#### HaveKey - Returns true if the given key exists

`public bool HaveKey (string key)`

where `key` is the key to be queried. 

Returns `true` if the given key (case-insensitive) was in the argument list as either `-key` or `key=xxx`, otherwise false.

If the key is in the argument list, the corresponding value is retained in property `LastValue`, which can be retrieved at any time before the next call to `HaveKey` (which will overwrite it).

#### GetValue - Returns the value corresponding to the given key

`public string? GetValue (string key)`

where `key` is the key to be queried (case-insensitive).

If `key` is in the argument list, returns the corresponding value. This may be an empty string, in the case of `key=` and will be a dash where the key was in the form `-key`. If the key is not in the argument list, null is returned.

### Properties

#### ArgList - List of all arguments

`public List<DpStringKvp>? ArgList {get;}`

Returns the list of (string Key/string Value) pairs corresponding to the arguments in their original sequence. 
- Positional arguments are placed in the `Value`, with an empty string as the `Key`. 
- Any argument preceded by a hyphen will be placed in `Key` (hyphen removed) with a `Value` of "-". 
- Arguments of the form name=value will have the name placed in `Key` and the value placed in `Value`. Note that `=value` is an error, whereas `name=` is OK (empty value).
- Errors will be recorded as a ">" in the `Key` and an error message in `Value`.

Check property `HaveErrors` before using the argument list.

#### ErrorList - List of any errors found

`public string []? ErrorList {get;}`

Returns a string array of error messages, or an empty array.

#### HaveErrors - True if any errors were found

`public bool HaveErrors {get}`

Returns true if any errors were found by the constructor, otherwise false.

#### LastValue - Value corresponding to key in last call to `HaveKey()`

`public string? LastValue {get;}`

If the last call to `HaveKey()` returned `true`, the value corresponding to that key is retained in property `LastValue`. If not found, the property is set to null.

`LastValue` is only available until the next call to `HaveKey()`, at which point it will be overwritten.