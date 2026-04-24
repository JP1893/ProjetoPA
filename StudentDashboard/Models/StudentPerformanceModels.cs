namespace StudentPerformanceDashboard
{
    public class StudentsPerYear
    {
        public int Year { get; set; }
        public int Students { get; set; }
    }

    public class ValorGrafico
    {
        public string ValorX { get; set; }
        public int ValorY { get; set; }
    }

    public class AverageSubjectScore
    {
        public int Year { get; set; }
        public SubjectScores Scores { get; set; } = new();
    }

    public class SubjectScores
    {
        public double PhysEd { get; set; }
        public double English { get; set; }
        public double Maths { get; set; }
        public double Science { get; set; }
    }

    public class StudentsByGradeAndGender
    {
        public double Male { get; set; }
        public double Female { get; set; }
        public double Others { get; set; }
    }

    public class StudentParticipationRateByBranch
    {
        public int Year { get; set; }
        public BranchRates Rates { get; set; } = new();
    }

    public class BranchRates
    {
        public double PhysEd { get; set; }
        public double Arts { get; set; }
        public double English { get; set; }
        public double Maths { get; set; }
        public double Science { get; set; }
    }

    public class ExaminationResultsByBranch
    {
        public int Year { get; set; }
        public Dictionary<string, ExaminationResult> Results { get; set; } = new();
    }

    public class ExaminationResult
    {
        public int Pass { get; set; }
        public int Fail { get; set; }
        public int NotAttended { get; set; }
    }

    public class LabelValue
    {
        public string Label { get; set; } = "";
        public double Value { get; set; }
        public double Percentage { get; set; }
    }

    public class SubjectRate
    {
        public Subject Subject { get; set; } = new();
        public double Rate { get; set; }
    }

    public class SubjectExamResult
    {
        public Subject Subject { get; set; } = new Subject();
        public int Pass { get; set; }
        public int Fail { get; set; }
        public int NotAttended { get; set; }
    }

    public class SubjectScoreTile
    {
        public string Subject { get; set; } = "";
        public double Score { get; set; }
    }

    public class GradeDistribution
    {
        public string Grade { get; set; } = "";
        public int Count { get; set; }
        public double Percentage { get; set; }
        public string Color { get; set; } = "";
    }

    public class Subject
    {
        public string Name { get; set; } = "All";
        public bool IsSelected { get; set; } // Optional if you want to track selection manually
    }
}