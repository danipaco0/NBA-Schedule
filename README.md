# NBA scheduler
The project, implemented using constraint programming techniques, aims to create a basketball match scheduling program 
over a specified period while adhering to several constraints. The application provides a complete schedule only if 
all the teams in the league have played all their matches within the predefined number of days.

<p align='center'>
<img src="https://github.com/danipaco0/NBA-Schedule/assets/7733838/a02c9aab-98a7-42cb-8aa8-0bd9dac6d87d" width=600>
</p>

## Technical Explanation

### Creating Variables
The only variables in this problem are the teams, and each of them has a domain D, defined as
_D = {1, 2, 3, ..., n}_, where n = the number of days.

The creation of variables begins with the instantiation of a Team object, which has three string parameters representing 
the team's name, the division to which it belongs, and the conference to which it is affiliated. It also includes three 
integer variables indicating the number of games to be played against teams from the same conference, the number of games 
against teams from the same division, and the number of games against teams from the other conference. 
These variables can be represented as _E = e1, e2, ..., e12_.

A league consists of two conferences, each containing three divisions, which, in turn, include two teams.

### Constraints
To ensure that the created program respects, more or less, the rules established by the NBA for schedule creation, 
a set of constraints has been defined. Some of the values used by the NBA have been adapted for the specific problem studied 
in this project, as the project's schedule is designed for only 12 teams, not the NBA's 30 teams. 
The applied constraints are as follows:

1. A team cannot play against itself.
$`C(e_i, e_j) = e_i ≠ e_j \quad \text{for all} \; i, j \in E`$

1. A team cannot play more than once per day.
$`C(e_{ijd}, e_{jid}) = e_{ijd} + e_{jid} ≤ 1 \quad \text{for all} \; i, j \in E,\; d \in D`$

1. There is a maximum of 4 games per day.
$`\sum_{i, j \in E} e_{ijd} \leq 4 \quad \text{for all} \; d \in D`$

1. Each team plays 3 games against teams from the same division (Ed represents the set of teams belonging to the same division).
$`\sum_{d \in D} e_{ijd} = 3 \quad \text{for all} \; i, j \in E_d`$

1. Each team plays 2 games against teams from the same conference (Ec represents the set of teams belonging to the same conference).
$`\sum_{d \in D} e_{ijd} = 2 \quad \text{for all} \; i, j \in E_c`$

1. Each team plays 1 game against teams from the other conference (Eo represents the set of teams belonging to the other conference).
$`\sum_{d \in D} e_{ijd} = 2 \quad \text{for all} \; i \in E_c,\; j \in E_o`$

1. A team cannot play for 3 consecutive days.
$`\sum_{m \in \{0,1,2\}} \sum_{j \in E} \sum_{k \in E} e_{ij(d+m)} + e_{ik(d+m)} < 3 \quad \text{for all} \; i \in E,\; d \in \{1, 2, \ldots, D - 2\}`$

1. If a team has played for 2 consecutive days, it must have a minimum of 2 days off.
$`\sum_{m \in \{0,1,2,3\}} \sum_{j \in E} \sum_{k \in E} e_{ij(d+m)} + e_{ik(d+m)} < 3 \quad \text{for all} \; i \in E,\; d \in \{1, 2, \ldots, D - 3\}`$

1. A team can play at most once on the first two days.
$`\sum_{j \in E} \sum_{k \in E} e_{ijd_1} + e_{ikd_2} \leq 1 \quad \text{for all} \; i \in E,\; d_1 = 1,\; d_2 = 2`$

### Constraints evaluation
For a match to be scheduled on a specific date, all the constraints must be met. Once created, the match is added to the schedule, 
but that alone is not sufficient for a positive final state of the search.

The "Status" checks, once the predefined deadline date is reached, whether all the teams have played all their matches. 
This verification is done to determine if the application can display the entire schedule.
