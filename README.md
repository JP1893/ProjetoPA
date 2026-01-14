# Student Performance and Engagement Dashboard 

## Syncfusion WPF SfChart

This high-performance [charting library](https://help.syncfusion.com/wpf/charts/getting-started) is designed for WPF applications and offers rich data visualization capabilities. It supports various chart types, making it ideal to creating a Health Tracker App . With built-in interactivity features such as zooming, panning, tooltips, and exporting, users can explore complex datasets with ease.

## Major Chart Types Used in This Dashboard

Below are the key SfChart types included in the application and why they are useful:

### 1. Column Chart

- Best for comparison across categories
- Clear representation of Pass/Fail/Not‑Attended results
- Useful for subject‑wise performance analytics
- Supports data labels, legends, and tooltip interaction

### 2. Spline Chart

- Smooth curve showing trends over time
- Helps visualize semester progression or yearly enrollment changes
- Great for spotting upward/downward performance patterns

### 3. Doughnut Chart

- Clean, modern representation of proportions
- Used for gender distribution and subject participation
- Provides a simple at‑a‑glance view
- Highly readable and customizable

## Dashboard Overview
This dashboard provides a complete visual summary of student strength, engagement, and academic performance using a modern Syncfusion‑based WPF UI.
Application Layout

The UI is built using a clean grid‑based layout consisting of:

### 1. Header Section

- Dashboard title
- Subject filter (ComboBoxAdv)
- Academic year filter (ComboBoxAdv)

### 2. KPI Tiles

- Quick, visual representation of key subject scores (Maths, Science, English, etc.)
- Highlights strengths and performance gaps

### 3. Main Visualization Area

- Multiple charts arranged in a responsive grid
- Each chart wrapped in a styled Border for consistency
- Smooth animations and tooltips using Syncfusion chart engine

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

Refer to the blog for step-by-step guidance on [Building a Student Performance and Engagement Dashboard]().
