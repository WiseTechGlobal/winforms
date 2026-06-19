// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;

[assembly: System.Runtime.InteropServices.ComVisible(false)]

[assembly: InternalsVisibleTo($"System.Windows.Forms.Tests, PublicKey={PublicKeys.MicrosoftShared}")]
[assembly: InternalsVisibleTo($"System.Windows.Forms.TestUtilities, PublicKey={PublicKeys.MicrosoftShared}")]
[assembly: InternalsVisibleTo($"System.Windows.Forms.Primitives.TestUtilities, PublicKey={PublicKeys.Ecma}")]
[assembly: InternalsVisibleTo($"System.Windows.Forms.Interop.Tests, PublicKey={PublicKeys.MicrosoftShared}")]
[assembly: InternalsVisibleTo($"System.Windows.Forms.UI.IntegrationTests, PublicKey={PublicKeys.MicrosoftShared}")]
[assembly: InternalsVisibleTo($"ScratchProjectWithInternals, PublicKey={PublicKeys.Ecma}")]
[assembly: InternalsVisibleTo($"ComDisabled.Tests, PublicKey={PublicKeys.MicrosoftShared}")]

// This is needed in order to Moq internal interfaces for testing
[assembly: InternalsVisibleTo($"DynamicProxyGenAssembly2, PublicKey={PublicKeys.Moq}")]

// TODO: Confirm the actual WiseTech CargoWise assembly name and strong-name public key before shipping.
//       Replace "WiseTech.CargoWise" with the real assembly name and add a "PublicKey={PublicKeys.*}"
//       token (adding a new PublicKeys constant if needed) once the key is confirmed.
//       This entry grants CargoWise access to RelatedCurrencyManager.UseParentCurrentChanged,
//       the supported .NET 10 replacement for the ZBindingContext reflection hack.
[assembly: InternalsVisibleTo("WiseTech.CargoWise")]
