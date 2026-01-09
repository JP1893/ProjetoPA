# Student Performance and Engagement Dashboard 

## Overview
The application’s shell is structured using a clean WPF grid-based layout containing:

### 1. Header Section

Dashboard title
Subject and Year filter dropdowns (ComboBoxAdv)

### 2. KPI Score Tiles

Visual highlights of key subject scores (Maths, Science, English, etc.)

### 3. Main Content Area

Grid-based chart containers
Each chart wrapped with styled Border elements for visual consistency


## Dashboard Modules

### 1. Examination Results Module
**Purpose:**
Provide a clear comparison of Pass, Fail, and Not Attended results across subjects in the selected academic year.

**Components:**
Column Chart: Examination Results by Subject

Multi-series Column chart
Each column represents a result category
Helps identify subjects with performance gaps or low engagement

**Visual Behavior:**

Interactive tooltips
Legend for Pass/Fail categories
Smooth column animations


### 2. Semester Grade Trends Module
**Purpose:**
Track how students' average grades change over the semester.

**Components:**
Spline Chart: Semester Grade Trends

Smooth curve showing grade progression
Highlights fluctuations, improvements, or drops
Enables early interventions


### 3. Gender Participation Module
**Purpose:**
Show the gender distribution of participating students for a selected year and subject.

**Components:**
Doughnut Chart: Gender Participation

Male / Female / Others distribution
Clear proportional visualization
Modern and easy-to-read chart style


### 4. Students Per Year Trend Module
**Purpose:**
Visualize enrollment patterns over different academic years.

**Components:**
Spline Chart: Students per Year Trend

Year-over-year student count
Identifies growth or decline in enrollment
Supports long-term academic planning


### 5. Participation Rate by Subject Module

**Purpose:**
Display student engagement levels across subjects with dynamic visualization.

**Components:**
Dynamic Chart Rendering: Column or Doughnut


* If “All Subjects” selected → Column Chart

    * Comparative view of participation rates



* If a single subject selected → Doughnut Chart

    * Focused breakdown of participation for that subject

![Student Performance and Engagement Dashboard](https://github.com/user-attachments/assets/92391775-9300-4b25-a5eb-1d2bae682acd)

## Troubleshooting

### Path Too Long Exception

If you are facing a **path too long** exception when building this example project, close Visual Studio and rename the repository to a shorter name before building the project.**Path too long** exception when building this example project, close Visual Studio and rename the repository to a shorter name before building the project.

### Phase 1: Calories and Steps Tracking

Refer to the blog for step-by-step guidance on [visualizing calories burned and steps taken in the Health Tracker dashboard](https://www.syncfusion.com/blogs/post/wpf-health-tracker-chart-calories-steps).

### Phase 2: Water and Sleep Tracking

Refer to the blog for step-by-step guidance on [Building a Student Performance and Engagement Dashboard]().