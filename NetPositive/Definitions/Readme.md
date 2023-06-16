# Definitions

Definitions are a data based way to define signatures of method calls that the scanner should be looking for.
In the simplest form a signature is only dependent on things that happen within a single method.

## Definitions
A definition consists of one or multiple signatures
The order of signatures inside a definitions determine the order their markers are checked in.
A finding is **only generated for the first** signature that matches.

## Signatures
A signature consists of:
  * one or multiple markers
  * a risk rating
  * a description of the issue the signature looks for

If all markers of a signature apply then a finding is generated consisting of the scanned class/method, the risk rating and the description

## Markers
All markers can also be negated. ie Calls, Not(Calls)

If multiple markers exist in a signature they are implicitly connected through AND

#### SignatureMatches(nameRegEx)
True iff the full signature of the method being scanned matches nameRegEx.

#### Calls(nameRegEx)
True iff the method being scanned contains at least one call that matches nameRegEx.

#### ClassCalls(nameRegEx)
True iff at least one method in the class containing the method being scanned contains a call that matches nameRegEx

#### ~LocalClassCalls(nameRegEx)~
True iff at least one class of at least one local variable contains a method that contains a call that matches nameRegEx

#### Or(marker1, marker2)
True iff either marker1 or marker2 are true

#### And(marker1, marker2)
True iff both marker1 or marker2 are true

#### Not(marker)
True iff marker is false


## File Format
The types described above are stored in a json file.

### Base Format
The **root element** of the json file must be an **array**.

### Definitions
A definition must be either an **array** of signatures or a **string**.  

If a definition is of string type it **must** start with "Import:" and will cause all definitions of another file to be imported.

### Signatures
A signature must be an **object** and contain the following three elements

  * a marker **string**
  * a description **string**
  * an **array** of markers

### Markers
A marker must be a **string**. It will be parsed according to the description in the Markers section above.  

Here are some examples:

  * `Calls(.*BinaryFormatter::Deserialize.*)`
  * `SignatureMatches(.*::.*(Copy|Clone).*)`
  * `Not(ClassCalls(.*BinaryFormatter::set_Binder.*))`