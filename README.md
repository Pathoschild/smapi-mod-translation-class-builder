**ModTranslationClassBuilder** autogenerates a strongly-typed class to access [`i18n`
translation files](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Translation)
from your [SMAPI](https://smapi.io/) mod code.

## Contents
* [Why does this exist?](#why-does-this-exist)
* [Usage](#usage)
  * [First-time setup](#first-time-setup)
  * [Conventions](#conventions)
* [Customization](#customization)
* [See also](#see-also)

## Why does this exist?
### Without the package
Mods use code like this to read their translations:
```cs
string text = helper.Translation.Get("range-value", new { min = 1, max = 5 });
```

Unfortunately there's no validation at this point; if the key is `range` (not `range-value`) or the
token name is `minimum` (not `min`), you won't know until you test that part of the mod in-game and
see an error message.

That also means that after changing the translation files, you need to manually search the code for
anywhere that referenced the translations to update them. That gets pretty tedious with larger
mods, which might have hundreds of translations used across dozens of files.

### With the package

This package lets you write code like this instead:
```cs
string text = I18n.RangeValue(min: 1, max: 5);
```

Since it's strongly typed, it's validated immediately as you type. For example, if you accidentally
typed `I18n.RangeValues` instead, you'll see an immediate error that `RangeValues` doesn't exist
without needing to test it in-game (or even compile the mod).

See the [test mod](TestMod) for an example of the generated class in an actual mod.

## Usage
### First-time setup
1. [Install the NuGet package](https://www.nuget.org/packages/Pathoschild.Stardew.ModTranslationClassBuilder).
2. In your mod's `Entry` method, add this line:
   ```cs
   I18n.Init(helper.Translation);
   ```

That's it! Now you can immediately use `I18n` anywhere in your mod code. The class will be updated
automatically whenever your `i18n/default.json` file changes.

### Conventions
* The class uses your project's root namespace by default (you can [change that](#customization)
  if needed).
* Translation keys are converted to CamelCase, with `.` changed to `_` to help group categories.

  For example:

  key in `i18n/default.json` | method
  -------------------------- | --------------------------
  `ready`                    | `I18n.Ready()`
  `ready-now`                | `I18n.ReadyNow()`
  `generic.ready-now`        | `I18n.Generic_ReadyNow()`

## Customization
You can configure the `I18n` class using a `<PropertyGroup>` section in your mod's `.csproj` file.
Each property must be prefixed with `TranslationClassBuilder_`. For example, this changes the class
name to `Translations`:

```xml
<PropertyGroup>
   <TranslationClassBuilder_ClassName>Translations</TranslationClassBuilder_ClassName>
</PropertyGroup>
```

Main options:

argument         | description | default value
---------------- | ----------- | ------------
`ClassName`      | The name of the generated class. | `I18n`
`Namespace`      | The namespace for the generated class. | _project's root namespace_

Advanced options:

argument         | description | default value
---------------- | ----------- | ------------
`ClassModifiers` | The [access modifiers](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers) to apply to the generated class (e.g. to make it public). | `internal static`
`CreateBackup`   | Whether to add a backup of the generated class to the project folder in a `Generated` subfolder. If it's disabled, the generated file will be hidden and excluded from source control. | `false`

## See also
* [Release notes](release-notes.md)
