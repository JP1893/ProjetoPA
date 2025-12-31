namespace StudentPerformanceDashboard
{
    public partial class StudentPerformanceViewModel
    {
        #region Methods

        private void SeedSampleData()
        {
            // Updated varied counts per year
            StudentsPerYear.Add(new StudentsPerYear { Year = 2021, Students = 650 });
            StudentsPerYear.Add(new StudentsPerYear { Year = 2022, Students = 690 });
            StudentsPerYear.Add(new StudentsPerYear { Year = 2023, Students = 730 });
            StudentsPerYear.Add(new StudentsPerYear { Year = 2024, Students = 715 });
            StudentsPerYear.Add(new StudentsPerYear { Year = 2025, Students = 700 });

            AverageSubjectScores.Add(new AverageSubjectScore
            {
                Year = 2021,
                Scores = new SubjectScores { PhysEd = 82.5, Arts = 88.7, English = 74.2, Maths = 79.1, Science = 76.8 }
            });
            AverageSubjectScores.Add(new AverageSubjectScore
            {
                Year = 2022,
                Scores = new SubjectScores { PhysEd = 84.1, Arts = 89.0, English = 75.5, Maths = 80.8, Science = 77.2 }
            });
            AverageSubjectScores.Add(new AverageSubjectScore
            {
                Year = 2023,
                Scores = new SubjectScores { PhysEd = 85.7, Arts = 87.1, English = 77.2, Maths = 82.5, Science = 77.8 }
            });
            AverageSubjectScores.Add(new AverageSubjectScore
            {
                Year = 2024,
                Scores = new SubjectScores { PhysEd = 84.9, Arts = 86.2, English = 76.1, Maths = 81.0, Science = 76.9 }
            });
            AverageSubjectScores.Add(new AverageSubjectScore
            {
                Year = 2025,
                Scores = new SubjectScores { PhysEd = 83.8, Arts = 85.0, English = 75.0, Maths = 79.6, Science = 75.5 }
            });

            // Year-wise gender totals (example realistic splits)
            _genderTotalsByYear[2021] = new StudentsByGradeAndGender { Male = 320, Female = 300, Others = 30 };
            _genderTotalsByYear[2022] = new StudentsByGradeAndGender { Male = 330, Female = 320, Others = 40 };
            _genderTotalsByYear[2023] = new StudentsByGradeAndGender { Male = 340, Female = 340, Others = 50 };
            _genderTotalsByYear[2024] = new StudentsByGradeAndGender { Male = 335, Female = 330, Others = 50 };
            _genderTotalsByYear[2025] = new StudentsByGradeAndGender { Male = 325, Female = 325, Others = 50 };
            // initialize current snapshot (will be refreshed per year in BuildProjections)
            StudentsByGradeAndGender = _genderTotalsByYear[2021];

            // Participation rates by subject per year
            StudentParticipationRateByBranch.Add(new StudentParticipationRateByBranch
            {
                Year = 2021,
                Rates = new BranchRates { PhysEd = 95.0, Arts = 81.0, English = 75.5, Maths = 83.5, Science = 86.0 }
            });
            StudentParticipationRateByBranch.Add(new StudentParticipationRateByBranch
            {
                Year = 2022,
                Rates = new BranchRates { PhysEd = 97.2, Arts = 83.0, English = 77.0, Maths = 85.2, Science = 87.5 }
            });
            StudentParticipationRateByBranch.Add(new StudentParticipationRateByBranch
            {
                Year = 2023,
                Rates = new BranchRates { PhysEd = 98.8, Arts = 84.0, English = 78.1, Maths = 86.4, Science = 88.1 }
            });
            StudentParticipationRateByBranch.Add(new StudentParticipationRateByBranch
            {
                Year = 2024,
                Rates = new BranchRates { PhysEd = 97.5, Arts = 83.2, English = 77.0, Maths = 85.0, Science = 86.8 }
            });
            StudentParticipationRateByBranch.Add(new StudentParticipationRateByBranch
            {
                Year = 2025,
                Rates = new BranchRates { PhysEd = 96.0, Arts = 82.0, English = 75.8, Maths = 83.6, Science = 85.2 }
            });

            // Derive examination results from participation rates and subject scores to keep data consistent
            ComputeExamResultsFromRatesAndScores();
        }

        private void InitializeSubjects()
        {
            Subjects.Clear();
            string[] names = new[] { "All", "PhysEd", "English", "Maths", "Science" };
            foreach (var name in names)
            {
                var subject = new Subject { Name = name };
                _subjectsByName[name] = subject;
                Subjects.Add(subject);
            }
        }

        private void UpdateFilteredData()
        {
            BuildProjections();

            // Participation (apply filter correctly and avoid unnecessary allocations)
            FilteredParticipationRates.Clear();
            if (SelectedSubject is not null && !IsAll(SelectedSubject))
            {
                var targetName = SelectedSubject.Name;
                for (int i = 0; i < ParticipationBySubject.Count; i++)
                {
                    var item = ParticipationBySubject[i];
                    if (item.Subject != null && string.Equals(item.Subject.Name, targetName, StringComparison.OrdinalIgnoreCase))
                    {
                        FilteredParticipationRates.Add(item);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < ParticipationBySubject.Count; i++)
                    FilteredParticipationRates.Add(ParticipationBySubject[i]);
            }

            // Semester trend (no filter currently; clone to filtered)
            FilteredSemesterTrend.Clear();
            foreach (var item in SemesterGradeTrend)
            {
                item.Value = Math.Round(item.Value, 1);
                FilteredSemesterTrend.Add(item);
            }

            // Select single item for gauge view
            SelectedParticipationRate = (FilteredParticipationRates.Count == 1) ? FilteredParticipationRates[0] : null;

            // Exam results (name-based filter to avoid object reference issues)
            FilteredExamResults.Clear();
            IEnumerable<SubjectExamResult> examData = ExamResultsBySubject;
            if (SelectedSubject is not null && !IsAll(SelectedSubject))
            {
                examData = examData.Where(e =>
                    e.Subject != null &&
                    string.Equals(e.Subject.Name, SelectedSubject.Name, StringComparison.OrdinalIgnoreCase));
            }
            foreach (var item in examData)
                FilteredExamResults.Add(item);
        }

        // Compute exam results for each year and subject so that:
        // - Attended = round(TotalStudents * ParticipationRate)
        // - NotAttended = TotalStudents - Attended
        // - Pass/Fail split of Attended is derived from the average subject score (higher score => higher pass rate)
        private void ComputeExamResultsFromRatesAndScores()
        {
            ExaminationResultsByBranch.Clear();

            foreach (var yearInfo in StudentsPerYear)
            {
                var year = yearInfo.Year;
                int total = yearInfo.Students;
                var rates = StudentParticipationRateByBranch.FirstOrDefault(r => r.Year == year)?.Rates;
                var scores = AverageSubjectScores.FirstOrDefault(s => s.Year == year)?.Scores;
                if (rates == null || scores == null) continue;

                var results = new Dictionary<string, ExaminationResult>();

                // helper local function
                void AddFor(string subjectName, double participationRatePercent, double avgScore)
                {
                    // Attended by participation rate
                    int attended = (int)Math.Round(total * participationRatePercent / 100.0, MidpointRounding.AwayFromZero);
                    int notAttended = Math.Max(0, total - attended);

                    // Map average score (0..100) to pass rate (0.4..0.98) to be realistic
                    double passRate = Math.Clamp(0.4 + (avgScore / 100.0) * 0.58, 0.4, 0.98);
                    int pass = (int)Math.Round(attended * passRate, MidpointRounding.AwayFromZero);
                    int fail = Math.Max(0, attended - pass);

                    results[subjectName] = new ExaminationResult { Pass = pass, Fail = fail, NotAttended = notAttended };
                }

                AddFor("PhysEd", rates.PhysEd, scores.PhysEd);
                AddFor("English", rates.English, scores.English);
                AddFor("Maths", rates.Maths, scores.Maths);
                AddFor("Science", rates.Science, scores.Science);

                ExaminationResultsByBranch.Add(new ExaminationResultsByBranch { Year = year, Results = results });
            }
        }

        private void BuildProjections()
        {
            // refresh current year gender snapshot for bindings that read it
            if (_genderTotalsByYear.TryGetValue(SelectedYear, out var yearGender))
                StudentsByGradeAndGender = yearGender;

            var avgScores = AverageSubjectScores.FirstOrDefault(s => s.Year == SelectedYear)?.Scores ?? new SubjectScores();

            PhysEdScore = avgScores.PhysEd;
            ArtsScore = avgScores.Arts;
            EnglishScore = avgScores.English;
            MathsScore = avgScores.Maths;
            ScienceScore = avgScores.Science;

            // Dynamic grade distribution based on the selected subject/overall score
            double scoreForGrades = (SelectedSubject is null || IsAll(SelectedSubject))
                ? (avgScores.PhysEd + avgScores.Arts + avgScores.English + avgScores.Maths + avgScores.Science) / 5.0
                : GetScoreBySubject(avgScores, SelectedSubject.Name);
            BuildGradeDistribution(scoreForGrades);

            // Gender participation pie (dynamic by year/subject)
            GenderParticipationPie.Clear();
            // pick rate: selected subject's participation rate; for "All" use the average across subjects
            double selectedRate = 0;
            var yearRates = StudentParticipationRateByBranch.FirstOrDefault(r => r.Year == SelectedYear)?.Rates;
            if (yearRates != null)
            {
                if (SelectedSubject == null || IsAll(SelectedSubject))
                {
                    selectedRate = new[] { yearRates.PhysEd, yearRates.Arts, yearRates.English, yearRates.Maths, yearRates.Science }.Average();
                }
                else
                {
                    selectedRate = GetRateBySubject(yearRates, SelectedSubject.Name);
                }
            }
            // convert rate to participation counts per gender using current gender totals
            double male = StudentsByGradeAndGender.Male * selectedRate / 100.0;
            double female = StudentsByGradeAndGender.Female * selectedRate / 100.0;
            double others = StudentsByGradeAndGender.Others * selectedRate / 100.0;
            GenderParticipationPie.Add(new LabelValue { Label = "Male", Value = male });
            GenderParticipationPie.Add(new LabelValue { Label = "Female", Value = female });
            GenderParticipationPie.Add(new LabelValue { Label = "Others", Value = others });

            // Participation by subject (reuse Subject instances)
            ParticipationBySubject.Clear();
            var rates = StudentParticipationRateByBranch.FirstOrDefault(r => r.Year == SelectedYear)?.Rates ?? new BranchRates();
            ParticipationBySubject.Add(new SubjectRate { Subject = _subjectsByName["English"], Rate = rates.English });
            ParticipationBySubject.Add(new SubjectRate { Subject = _subjectsByName["Maths"], Rate = rates.Maths });
            ParticipationBySubject.Add(new SubjectRate { Subject = _subjectsByName["PhysEd"], Rate = rates.PhysEd });
            ParticipationBySubject.Add(new SubjectRate { Subject = _subjectsByName["Science"], Rate = rates.Science });

            // Exam results (reuse Subject instances)
            ExamResultsBySubject.Clear();
            var exams = ExaminationResultsByBranch.FirstOrDefault(e => e.Year == SelectedYear)?.Results ?? new Dictionary<string, ExaminationResult>();
            foreach (var kvp in exams)
            {
                var subject = _subjectsByName.TryGetValue(kvp.Key, out var s) ? s : new Subject { Name = kvp.Key };
                if (!_subjectsByName.ContainsKey(kvp.Key)) _subjectsByName[kvp.Key] = subject;
                ExamResultsBySubject.Add(new SubjectExamResult
                {
                    Subject = subject,
                    Pass = kvp.Value.Pass,
                    Fail = kvp.Value.Fail,
                    NotAttended = kvp.Value.NotAttended
                });
            }

            // Scores over years (fallback to Maths when "All")
            SubjectScoresOverYears.Clear();
            var subjectForTrend = (SelectedSubject is null || IsAll(SelectedSubject)) ? "Maths" : SelectedSubject.Name;
            foreach (var score in AverageSubjectScores.OrderBy(s => s.Year))
            {
                double val = GetScoreBySubject(score.Scores, subjectForTrend);
                SubjectScoresOverYears.Add(new LabelValue { Label = score.Year.ToString(), Value = val });
            }

            // Semester trend - make it a slight downward trend with mild variation
            SemesterGradeTrend.Clear();
            double baseScore = GetScoreBySubject(avgScores, subjectForTrend);
            for (int i = 1; i <= 6; i++)
            {
                double decay = i * Math.Max(0.6, baseScore * 0.012); // scales with base score but trends down
                double variation = Math.Sin(i) * 0.8;                 // small up/down wiggle
                double value = Math.Max(0, baseScore - decay + variation);
                SemesterGradeTrend.Add(new LabelValue { Label = i.ToString(), Value = value });
            }
        }

        private static bool IsAll(Subject subject) =>
            subject != null && string.Equals(subject.Name, "All", StringComparison.OrdinalIgnoreCase);

        private static double GetRateBySubject(BranchRates rates, string subject) => subject switch
        {
            "PhysEd" => rates.PhysEd,
            "Arts" => rates.Arts,
            "English" => rates.English,
            "Maths" => rates.Maths,
            "Science" => rates.Science,
            _ => rates.Maths
        };

        private double GetScoreBySubject(SubjectScores scores, string subject) => subject switch
        {
            "PhysEd" => scores.PhysEd,
            "Arts" => scores.Arts,
            "English" => scores.English,
            "Maths" => scores.Maths,
            "Science" => scores.Science,
            _ => scores.Maths
        };

        private void InitializeCollections()
        {
            Years.Clear();
            // Show latest years first in the dropdown for quick access
            foreach (var year in StudentsPerYear.Select(y => y.Year).Distinct().OrderByDescending(y => y))
                Years.Add(year);
        }

        // Build grade distribution from the current average score using a normal model.
        // This ties grade buckets (A/B/C/D/F) to the mean score so it reacts to year/subject changes.
        private void BuildGradeDistribution(double average)
        {
            // Clamp
            average = Math.Max(0, Math.Min(100, average));

            // Assume scores follow N(average, sd^2). Use sd = 12 for reasonable spread.
            const double sd = 12.0;
            static double Phi(double x)
            {
                // Standard normal CDF approximation (error function based)
                // Abramowitz-Stegun approximation
                double t = 1.0 / (1.0 + 0.2316419 * Math.Abs(x));
                double d = Math.Exp(-x * x / 2.0) / Math.Sqrt(2.0 * Math.PI);
                double p = 1 - d * (0.319381530 * t - 0.356563782 * Math.Pow(t, 2) + 1.781477937 * Math.Pow(t, 3) - 1.821255978 * Math.Pow(t, 4) + 1.330274429 * Math.Pow(t, 5));
                return x >= 0 ? p : 1 - p;
            }

            double Cdf(double score) => Phi((score - average) / sd);

            // Bucket thresholds
            double pBelow60 = Cdf(60);
            double pBelow70 = Cdf(70);
            double pBelow80 = Cdf(80);
            double pBelow90 = Cdf(90);

            double pF = pBelow60;
            double pD = Math.Max(0, pBelow70 - pBelow60);
            double pC = Math.Max(0, pBelow80 - pBelow70);
            double pB = Math.Max(0, pBelow90 - pBelow80);
            double pA = Math.Max(0, 1 - pBelow90);

            // Convert to percentages and normalize rounding to 100
            int iA = (int)Math.Round(pA * 100);
            int iB = (int)Math.Round(pB * 100);
            int iC = (int)Math.Round(pC * 100);
            int iD = (int)Math.Round(pD * 100);
            int iF = 100 - (iA + iB + iC + iD);

            GradeDistributions.Clear();
            GradeDistributions.Add(new GradeDistribution { Grade = "A", Percentage = iA, Color = "#00C851" });
            GradeDistributions.Add(new GradeDistribution { Grade = "B", Percentage = iB, Color = "#2196F3" });
            GradeDistributions.Add(new GradeDistribution { Grade = "C", Percentage = iC, Color = "#FF9800" });
            GradeDistributions.Add(new GradeDistribution { Grade = "D", Percentage = iD, Color = "#F44336" });
            GradeDistributions.Add(new GradeDistribution { Grade = "F", Percentage = iF, Color = "#9C27B0" });
        }

        #endregion
    }
}