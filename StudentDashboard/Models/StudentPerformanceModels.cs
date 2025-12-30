using System.Collections.Generic;

namespace StudentPerformanceDashboard
{
    // Base data models
    public sealed class StudentsPerYear
    {
        public int Year { get; set; }
        public int Students { get; set; }
    }

    public sealed class AverageSubjectScore
    {
        public int Year { get; set; }
        public SubjectScores Scores { get; set; } = new();
    }

    public sealed class SubjectScores
    {
        public double PhysEd { get; set; }
        public double Arts { get; set; }
        public double English { get; set; }
        public double Maths { get; set; }
        public double Science { get; set; }
    }

    public sealed class StudentsByGradeAndGender
    {
        public double Male { get; set; }
        public double Female { get; set; }
        public double Others { get; set; }
    }

    public sealed class StudentParticipationRateByBranch
    {
        public int Year { get; set; }
        public BranchRates Rates { get; set; } = new();
    }

    public sealed class BranchRates
    {
        public double PhysEd { get; set; }
        public double Arts { get; set; }
        public double English { get; set; }
        public double Maths { get; set; }
        public double Science { get; set; }
    }

    public sealed class ExaminationResultsByBranch
    {
        public int Year { get; set; }
        public Dictionary<string, ExaminationResult> Results { get; set; } = new();
    }

    public sealed class ExaminationResult
    {
        public int Pass { get; set; }
        public int Fail { get; set; }
        public int NotAttended { get; set; }
    }

    // Chart-friendly DTOs with enhanced properties
    public sealed class LabelValue
    {
        public string Label { get; set; } = "";
        public double Value { get; set; }
        public double Percentage { get; set; }
    }

    public sealed class SubjectRate
    {
        public Subject Subject { get; set; } = new ();
        public double Rate { get; set; }
    }

    public sealed class SubjectExamResult
    {
        public Subject Subject { get; set; } = new Subject();
        public int Pass { get; set; }
        public int Fail { get; set; }
        public int NotAttended { get; set; }
    }

    public sealed class SubjectScoreTile
    {
        public string Subject { get; set; } = "";
        public double Score { get; set; }
    }

    // Additional enhanced models for dashboard features
    public sealed class GradeDistribution
    {
        public string Grade { get; set; } = "";
        public int Count { get; set; }
        public double Percentage { get; set; }
        public string Color { get; set; } = "";
    }

    public sealed class TrendPoint
    {
        public string Period { get; set; } = "";
        public double Value { get; set; }
        public string Subject { get; set; } = "";
    }

    public sealed class PerformanceSummary
    {
        public string Subject { get; set; } = "";
        public double CurrentScore { get; set; }
        public double PreviousScore { get; set; }
        public double Change => CurrentScore - PreviousScore;
        public string TrendDirection => Change > 0 ? "Up" : Change < 0 ? "Down" : "Stable";
    }

    public class Subject
    {
        public string Name { get; set; } = "All";
        public bool IsSelected { get; set; } // Optional if you want to track selection manually
    }

    // Tooltip data models for enhanced chart interactivity
    public sealed class TooltipData
    {
        public string Title { get; set; } = "";
        public string Value { get; set; } = "";
        public string Description { get; set; } = "";
        public string Color { get; set; } = "";
    }
}