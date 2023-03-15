## Things that are not currently supported by Json implementation
```csharp
// Duplicated the entries in the list
List<string>
```

## Things that are not currently supported by Toml implementation
```csharp
// Writes something like "Other.Id" to the file so the deserialized object will have a new Guid
Guid

// Loses some precision, so it won't be exact. Is accurate to at least the 1/100ths of a second
DateTime
```

## Things that are not currently supported by Xml implementation
```csharp
// Writes characters out as their numeric equivalent
char

// Duplicated the entries in the list
List<string>

// Didn't work at all
DateOnly
```