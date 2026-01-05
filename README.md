# Student Performance Dashboard (WPF + Syncfusion Charts)

An interactive WPF dashboard that visualizes student performance and engagement using Syncfusion WPF controls (Charts, Gauges, and Editors). It demonstrates MVVM data binding, dynamic filtering (by Subject and Year), and multiple chart types in a cohesive layout.

## Features
- Subject and Year filters with responsive UI
- KPI tiles for quick subject averages
- Charts
  - Column: Examination results (Pass/Fail/Not Attended) by subject
  - Spline: Semester grade trends and Students per year
  - Doughnut: Gender participation and semi-doughnut sample
- Grade distribution panel with dynamic percentages
- MVVM-friendly, testable data generation and projections

## Prerequisites
- Windows 10/11
- .NET Desktop SDK (target matching the project’s csproj)
- Visual Studio 2022 (recommended) or VS Code with C# extensions
- Syncfusion WPF controls (community/commercial license). See “Syncfusion licensing”.

## Getting started

Using Visual Studio (recommended):
1. Open StudentDashBoard.slnx in Visual Studio 2022.
2. Set StudentDashBoard as the startup project.
3. Press F5 to build and run.

Using terminal (PowerShell):
- Build:
```powershell
dotnet build .\StudentDashBoard.csproj -c Release
```
- Run (Debug):
```powershell
dotnet run --project .\StudentDashBoard.csproj
```

## Project structure
```
StudentDashboard/
  App.xaml
  App.xaml.cs
  AssemblyInfo.cs
  Converters/
  MainWindow.xaml
  MainWindow.xaml.cs
  Models/
  README.md
  StudentDashBoard.csproj
  StudentDashBoard.slnx
  Style/
  ViewModels/
```

## Key bindings and data flow
- DataContext: MainWindow binds to ViewModels/StudentPerformanceViewModel.
- Subject and Year combos: ItemsSource bound to Subjects and Years; TwoWay to SelectedSubject and SelectedYear.
- Charts:
  - Examination Results (ColumnSeries): ItemsSource = FilteredExamResults; XBindingPath = Subject.Name; YBindingPath = Pass/Fail/NotAttended
  - Semester Trend (SplineSeries): ItemsSource = FilteredSemesterTrend; XBindingPath = Label; YBindingPath = Value
  - Students per Year (SplineSeries): ItemsSource = StudentsPerYear; XBindingPath = Year; YBindingPath = Students
  - Participation Rate (ColumnSeries): ItemsSource = FilteredParticipationRates; XBindingPath = Subject.Name; YBindingPath = Rate
  - Gender Participation (DoughnutSeries): ItemsSource = GenderParticipationPie; XBindingPath = Label; YBindingPath = Value
  - Sample Semi-Doughnut (DoughnutSeries): ItemsSource = SemiDoughnutData; XBindingPath = Name; YBindingPath = Value; StartAngle/EndAngle are bindable

### Doughnut data properties (ViewModel)
Ensure the ViewModel exposes these collections:
- GenderParticipationPie: ObservableCollection<LabelValue> with Label and Value
- SemiDoughnutData: ObservableCollection<LabelValue> with Name and Value

Both are populated during BuildProjections/SeedSampleData and refresh when filters change.

## Syncfusion licensing
This sample uses Syncfusion WPF controls. A valid license (community or commercial) is required. If prompted, register a license key as per Syncfusion’s documentation: https://help.syncfusion.com/common/essential-studio/licensing/license

## Troubleshooting
- NuGet restore errors: Right-click solution → Restore NuGet Packages, or run `dotnet restore`.
- Missing Syncfusion assemblies: Ensure packages are restored and license is configured.
- Startup issues: Confirm StudentDashBoard.csproj is the startup project.

## Contributing
Issues and PRs are welcome for improvements, fixes, and documentation.

## License
This project is for demo/educational use. Review and comply with Syncfusion license terms.
