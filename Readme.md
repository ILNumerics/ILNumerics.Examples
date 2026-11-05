# ILNumerics Examples

[![Build and test examples](https://github.com/ILNumerics/ILNumerics.Examples/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/ILNumerics/ILNumerics.Examples/actions/workflows/build-and-test.yml)

Welcome to the **ILNumerics Examples** repository.  
This project contains practical examples to help you learn, test, and apply [ILNumerics](https://ilnumerics.net) in real-world .NET workflows.

---

## Intro

**ILNumerics** is a high-performance numerical / C# math library for .NET, focused on technical computing, visualization, and array-based algorithm development.

This repository is intended to:

- provide easy-to-run sample projects,
- demonstrate common ILNumerics workflows,
- offer reusable code snippets for your own applications.

Whether you are getting started or looking for specific implementation patterns, these examples are designed to be concise and practical.

---

## HowTo

### 1) Clone the repository

```bash
git clone https://github.com/ILNumerics/ILNumerics.Examples
cd ilnumerics.examples
```

### 2) Select an example 
Pick an example from its folder or use 'Folder View' in Visual Studio Solution Explorer to browse all examples. Load the example project file (note: project files have either the *.csproj or *.vbproj extension). Alternatively, just navigate into the example's folder to run the project from the dotnet CLI: 

```bash
cd Interpolation\BasicKrigingExample
```

### 3) Run the project
In recent versions of Visual Studio is it sufficient to just run the loaded project with or without debugging (or just hit F5). All package dependency resolution will be done automatically for you. 

Command line alternative: open the .NET CLI (or the Visual Studio developer prompt) and navigate into the example directory. Here, build and start the example project: 
```bash
dotnet run
```

## Requirements

All examples target **.NET 10** (`net10.0`, or `net10.0-windows` for WinForms/WPF examples) and reference the ILNumerics NuGet packages with a floating version (`Version="*"`), so a restore always picks up the newest ILNumerics release. No license key is required: the examples run with the free ILNumerics Starter Edition.

`ILNumerics.Examples.slnx` in the repository root contains all examples, if you prefer to browse them in one solution.

## Testing

The [Build and test examples](.github/workflows/build-and-test.yml) workflow runs on every push and nightly, so a new ILNumerics release is tested automatically. It

1. checks that every project follows the conventions above (`tests/check-projects.ps1`),
2. builds all examples against the newest ILNumerics packages,
3. starts every example without a license key (`tests/SmokeTest`). A startup hook, injected without changing the examples, fails the run on unhandled exceptions, saves a screenshot of the main window and renders the ILNumerics scene offscreen with the software renderer. Hangs, errors and blank renders fail the run. Screenshots and renders are uploaded as the `smoke-results` artifact.

To run the smoke test locally on Windows:

```bash
dotnet build ILNumerics.Examples.slnx -c Release
dotnet run --project tests/SmokeTest/SmokeRunner -c Release --no-build -- --root . --out smoke-results --hook tests/SmokeTest/SmokeHook/bin/Release/net10.0-windows/SmokeHook.dll
```

Add `--only <part of the path>` to run a single example. Examples listed in `tests/SmokeTest/known-failures.txt` are reported but do not fail the build.

## Drawing test app

`TestApps/ILNumerics.Drawing.TestApp` is the visual test tool for the ILNumerics Visualization Engine (formerly the separate ILNumerics.Drawing.TestApp repository). It renders test scenes with the software and OpenGL renderers in WinForms and WPF side by side.

## Getting Help
Find the ILNumerics [getting started guides](https://ilnumerics.net/Getting-Started-with-ILNumerics.html) on the ILNumerics website. 

[ILNumerics documentation section](https://ilnumerics.net/docs.html)
