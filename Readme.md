## Things that are not currently supported by Json implementation
```csharp
// Serialized properly, deserialization duplicated the entries in the list
List<string>
```

## Things that are not currently supported by Toml implementation
```csharp
// Writes something like "Other.Id" to the file so the deserialized object will have a new Guid
Guid

// Loses some precision, so it won't be exact. Is accurate to at least the 1/100ths of a second
DateTime

// This didn't serialize and as a result, stored nothing
// See https://github.com/xoofx/Tomlyn/issues/55
enum

// Completely unsupported
// See https://github.com/xoofx/Tomlyn/issues/43
char
```

## Things that are not currently supported by Xml implementation
```csharp
// Writes characters out as their numeric equivalent
char

// Serialized properly, deserialization duplicated the entries in the list
List<string>

// Serialized properly, deserialized as the minimum date
DateOnly
```