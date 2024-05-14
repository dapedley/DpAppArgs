//************************************************************************************
// AppArgs.cs
//************************************************************************************

namespace DpUtilities;

//************************************************************************************
// Class DpAppArgs 
// The constructor accepts the argument array from the program and internally creates
// a structured argument list and an array of errors, if found.
// The success of construction can be tested via property 'HaveErrors'. If errors were
// found, an array of the error messages may be retrieved frpm 'ErrorList'.
// The structured argument list is available via property 'ArgList'. This is a list of
// type 'StringKvp', which are key-value pairs (with input sequence maintained).
// 
// The argument list is assumed to be optionally one or more string values without
// names, optionally followed by a series of name=value strings or flags preceded by a
// hyphen. If the value strings contain spaces, they must be enclosed in double quotes
// (which will be removed). Single quotes will simply be treated as any other character.
//
// In the case of the initial strings without names, the Key property consits of an
// empty string.
// An argument preceded by a hyphen is placed (sans hyphen) in Key and a hyphen in value.
// name=value arguments have the name placed in Key and value in Value.
// If errors are encountered, the corresponding argument's entry will have
// the character '>' in the Name and an error message in the Value. 
// Note that '=123' will produce an error (no name), whereas 'x=' is OK (empty value).
//
// 'HaveKey (key)' returns true if the given key was found in the argument list, in
// which case the property 'LastValue' is set to contain the corresponding Value, or
// null if the key is not found.
// 'GetValue (key)' directly returns the value corresponding to key, or null if not
// found.
//************************************************************************************
public class DpAppArgs
{
	public string? LastValue { get; private set; } = null;
	public string []? ErrorList { get; private set; } = null;
	public List<DpStringKvp>? ArgList { get; private set; } = null;
	public bool HaveErrors { get => (ErrorList?.Length ?? 99) > 0; }

	readonly Dictionary<string, string>? ArgDic = null;
	bool HaveError = false;

	//-----------------------------------------------------------------------------------
	// Constructor
	//-----------------------------------------------------------------------------------
	public DpAppArgs (string [] args)
	{
		ArgList = [];
		ArgDic = [];
		HaveError = false;

		//*** No parameters - Return an empty list
		int nParms = (args?.Length ?? 0);
		if (nParms == 0) { ErrorList = []; return; }

		//*** Iterate through args
		bool isStart = true;
		foreach (string argx in args!) {
			string arg = argx.Trim ();

			//*** Look for the '=' character (name/value separator)
			int eqIx = arg.IndexOf ('=');

			//*** If there is no '='
			if (eqIx < 0) {

				//*** Is the first char of the argument a hyphen?
				//*** If so, signal no more positional args allowed
				if (arg [0] == '-') { isStart = false; }

				//*** Otherwise, disallow positional argument after one with '=' or '-'
				else {
					if (!isStart) {
						AddError ($"Argument without a name - '{arg}'");
						continue;
					}
				}
				//*** Both positional and hyphen-prefixed arguments reach here
				//*** Add the hyphened argument in Key (without hyphen)
				if (arg [0] == '-') 
					{ AddEntry (arg [1..], "-"); }
				else {
					//*** OK - Add the positional argument value, with an empty Key
					AddEntry (String.Empty, 
						 arg.DpRemoveQuotes (DpQuoteType.Double));
				}
			}

			//*** Have an '=' character
			else {
				//*** Disallow further arguments without a name
				isStart = false;
				//*** Error if no name preceding the '='
				if (eqIx == 0) {
					AddError ($"Invalid argument - '{arg}'");
					continue;
				}
				//*** Otherwise, add the name and value
				else {
					string name = arg[..eqIx];
					string value = arg.Length > (eqIx + 1) ? arg[(eqIx + 1)..] : "";
					AddEntry (name, value.DpRemoveQuotes (DpQuoteType.Double));
				}
			}
		}
		//*** Compile the error list
		ErrorList = HaveError ? ArgList.Where (a => a.Key == ">")
																		.Select (a => a.Value)
																		.ToArray ()
														: [];
	}

	//------------------------------------------------------------------------------------
	// HaveKey - if an argument with the given key exists, returns true and sets the
	// property 'LastValue' to the corresponding value. Otherwise, or if GetArgs has not
	// been called, returns false and sets LastValue to null.
	//------------------------------------------------------------------------------------
	public bool HaveKey (string key)
	{
		if (ArgList == null) { return false; }
		bool haveKey = ArgDic!.TryGetValue (key.ToLower (), out string? value);
		LastValue = haveKey ? value : null;
		return haveKey;
	}

	//------------------------------------------------------------------------------------
	// GetValue - Returns the value if an argument with the given key exists, otherwise
	// null. 
	//------------------------------------------------------------------------------------
	public string? GetValue (string key)
	{
		string? value = null;
		bool haveKey = ArgDic?.TryGetValue (key.ToLower (), out value) ?? false;
		return haveKey ? value : null;
	}

	//====================================================================================
	// Private Helper Methods 
	//====================================================================================
	//------------------------------------------------------------------------------------
	// AddEntry - Adds an argument entry to the list, provided there have been no errors
	// to this point.
	//------------------------------------------------------------------------------------
	private void AddEntry (string name, string value)
	{
		string key = string.Empty;
		if (name.Length > 0) {
			key = name.ToLower();
			if (HaveKey (key)) { AddError ($"Duplicate Key '{key}'"); return; }
			ArgDic?.Add (key, value);
		}
		ArgList?.Add (new DpStringKvp (key, value));
	}

	//------------------------------------------------------------------------------------
	// AddError - If the first error, clears any argument entries from the list. Writes
	// the error nessage as the value, tith a name of '>' to signal an error entry. 
	//------------------------------------------------------------------------------------
	private void AddError (string msg)
	{
		ArgList?.Add (new DpStringKvp (">", msg));
		HaveError = true;
	}

}
