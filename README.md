# WTG Notes

Migrating obsolete winforms controls to compile and run in Net 8 for CW1's ZArchitecture.

# WTG ChangeLog

| Version | What's changed |
|------------------|-------------|
| 0.0.6-dev.final | initial version for net7 and net8 |
| 0.0.7-dev.final | System.Windows.Forms.Design.dll added to package |
| 0.0.8-dev.final | net8.0: changed version of System.Drawing.Common.dll (v4.0.0.0 -> v8.0.0.0) to fix Dev build |
| 0.0.9-dev.final | net8.0: changed versions of all libraries in package to v8.0.0.0 |
| 0.0.10-dev.final | skipped due to error in publishing |
| 0.0.11-dev.final | net8.0: remove warning WFDEV001 |
| 0.0.12-dev.final | avoid type issue when instanciated from ZBindingContext (WI00826420) |
| 0.0.13-dev.final | comment out Debug.Fail in PaintEventArgs (WI00857973) |
| 0.0.14-dev.final | switched to using Release configuration assemblies instead of Debug, to avoid Microsoft's plentiful Debug.Assert checks (WI00876922) |
| 0.0.15-dev.final | removed need for forked System.Drawing.Common. We can use the public package listed on nuget.org now. |
| 0.0.16-dev.final | fixed issue with keyboard shortcuts associated with menus not being captured (WI00895180) |
| 0.0.17-dev.final | temporarily fixed WebBrowser memory leak (WI00938771) |
| 0.0.18-dev.final | fixed menu popup events not firing (WI00949199) |
| 0.0.19-dev.final | fixed menu bar size not taken into account in form sizing (WI00949199) |
| 0.0.20-dev.final | Menu property change should trigger size update (WI00949199) |
| 0.0.21-dev.final | fixed ToolBar tooltip moving the position of the toolbar (WI00951596) |

# WTG How to publish new version

* Do changes in code you want to be published
* Increase version in $(LIB_ROOT)\eng\Versions.props
* CH0 on DAT
* Go to DAT deployments for the CH0 build and click deploy

# End of WTG part, Microsoft part below

# Windows Forms

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/dotnet/winforms/blob/main/LICENSE.TXT)

Windows Forms (WinForms) is a UI framework for building Windows desktop applications. It is a .NET wrapper over Windows user interface libraries, such as User32 and GDI+. It also offers controls and other functionality that is unique to Windows Forms.

Windows Forms also provides one of the most productive ways to create desktop applications based on the visual designer provided in Visual Studio. It enables drag-and-drop of visual controls and other similar functionality that make it easy to build desktop applications.

### Windows Forms Designer
For more information about the designer, please see the [Windows Forms Designer Documentation](docs/designer/readme.md).<br />

### Relationship to .NET Framework

This codebase is a fork of the Windows Forms code in the .NET Framework 4.8. 
In Windows Forms .NET Core 3.0, we've strived to bring the two runtimes to a parity. However since then, we've done a number of changes, including [breaking changes](https://docs.microsoft.com/dotnet/core/compatibility/winforms), which diverged the two.
For more information about breaking changes, see the [Porting guide][porting-guidelines].


## Please note

:warning: This repository contains only implementations for Windows Forms for [.NET platform](https://github.com/dotnet/core).<br />
It does not contain either:
* The .NET Framework variant of Windows Forms. Issues with .NET Framework, including Windows Forms, should be filed on the [Developer Community](https://developercommunity.visualstudio.com/spaces/61/index.html) or [Product Support](https://support.microsoft.com/en-us/contactus?ws=support) websites. They should not be filed on this repository.
* The Windows Forms Designer implementations. Issues with the Designer should be filed via VS Feedback tool (top right-hand side icon in Visual Studio).


# How can I contribute?

We welcome contributions! Many people all over the world have helped make this project better.

* [Contributing][contributing] explains what kinds of changes we welcome
* [Developer Guide][developer-guide] explains how to build and test
* [Get Up and Running with Windows Forms .NET][getting-started] explains how to get started building Windows Forms applications.


## How to Engage, Contribute, and Provide Feedback

Some of the best ways to contribute are to try things out, file bugs, join in design conversations, and fix issues.

* The [contributing guidelines][contributing] and the more general [.NET contributing guide][net-contributing] define contributing rules.
* The [Developer Guide][developer-guide] defines the setup and workflow for working on this repository.
* If you have a question or have found a bug, [file an issue](https://github.com/dotnet/winforms/issues/new?template=bug_report.md).
* Use [daily builds][developer-guide] if you want to contribute and stay up to date with the team.

## Reporting security issues

Security issues and bugs should be reported privately via email to the Microsoft Security Response Center (MSRC) <secure@microsoft.com>. You should receive a response within 24 hours. If for some reason you do not, please follow up via email to ensure we received your original message. Further information, including the MSRC PGP key, can be found in the [Security TechCenter](https://www.microsoft.com/msrc/faqs-report-an-issue). Also see info about related [Microsoft .NET Core and ASP.NET Core Bug Bounty Program](https://www.microsoft.com/msrc/bounty-dot-net-core).

## Code of Conduct

This project uses the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct) to define expected conduct in our community. Instances of abusive, harassing, or otherwise unacceptable behavior may be reported by contacting a project maintainer at conduct@dotnetfoundation.org.

## License

.NET (including the Windows Forms repository) is licensed under the [MIT license](LICENSE.TXT).

## .NET Foundation

.NET Windows Forms is a [.NET Foundation](https://www.dotnetfoundation.org/projects) project.<br />
See the [.NET home repository](https://github.com/Microsoft/dotnet) to find other .NET-related projects.

[contributing]: CONTRIBUTING.md
[developer-guide]: docs/developer-guide.md
[getting-started]: docs/getting-started.md
[net-contributing]: https://github.com/dotnet/runtime/blob/master/CONTRIBUTING.md
[porting-guidelines]: docs/porting-guidelines.md

