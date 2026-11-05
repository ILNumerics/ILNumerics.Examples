# Fails when an example project drifts from the repository conventions:
#  - SDK-style project targeting the current .NET LTS (net10.0, or net10.0-windows for UI examples)
#  - every ILNumerics package reference uses a floating version (Version="*") so the newest release is used
#  - no project references into ILNumerics' internal source tree
# Each example stays self-contained (no shared Directory.Build.props), so it can still be copied out on its own.
param(
    [string]$Root = (Join-Path $PSScriptRoot '..'),
    [string]$Tfm = 'net10.0'
)
$errors = @()
# Legacy .NET Framework variant, kept until it is removed or converted.
$skip = @('STL_Viewer_NET_FW.csproj')

Get-ChildItem -Path $Root -Recurse -Include *.csproj, *.vbproj |
    Where-Object { $_.FullName -notmatch '[\\/](bin|obj)[\\/]' -and $skip -notcontains $_.Name } |
    ForEach-Object {
        $rel = [IO.Path]::GetRelativePath($Root, $_.FullName)
        [xml]$xml = Get-Content -LiteralPath $_.FullName -Raw
        if (-not $xml.Project.Sdk) { $errors += "${rel}: not an SDK-style project"; return }
        $tfms = @($xml.SelectNodes('//TargetFramework') | ForEach-Object { $_.InnerText.Trim() })
        foreach ($t in $tfms) {
            if ($t -ne $Tfm -and $t -ne "$Tfm-windows") { $errors += "${rel}: TargetFramework '$t' (expected $Tfm or $Tfm-windows)" }
        }
        if ($xml.SelectNodes('//TargetFrameworks').Count -gt 0) { $errors += "${rel}: uses TargetFrameworks, expected a single TargetFramework" }
        foreach ($pr in $xml.SelectNodes('//PackageReference')) {
            if ($pr.Include -like 'ILNumerics.*' -and $pr.Version -ne '*') { $errors += "${rel}: $($pr.Include) Version='$($pr.Version)' (expected '*')" }
        }
        foreach ($pr in $xml.SelectNodes('//ProjectReference')) {
            if ($pr.Include -match 'Version6|ILNumerics\.(Core|Drawing|Computing|Toolboxes)' -and -not $pr.Condition -and -not $pr.ParentNode.Condition) {
                $errors += "${rel}: unconditional reference into ILNumerics sources: $($pr.Include)"
            }
        }
    }

if ($errors.Count) {
    $errors | ForEach-Object { Write-Host "::error::$_" }
    exit 1
}
Write-Host "All projects follow the conventions."
