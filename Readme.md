## Things that are not currently supported by Json implementation
List<string>
- duplicated the entries in the list


## Things that are not currently supported by Toml implementation
Guid
- writes something like "Other.Id" to the file so the deserialized object will have a new Guid

DateTime
- loses some precision, so it won't be exact. Is accurate to at least the 1/100ths of a second


## Things that are not currently supported by Xml implementation
char
- writes characters out as their numeric equivalent

DateOnly
- didn't work at all

List<string>
- duplicated the entries in the list
