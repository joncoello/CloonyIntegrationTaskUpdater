Create Procedure spGetTaskCloonyData

	@activityid int

as

select 
	cs.ClientCode,
	assign.NameRight Assignment,
	act.Subject TaskName
from 
	Activity act
	inner join ActivityAssignment aa on aa.ActivityID = act.ActivityID
	inner join Assignment assign on assign.AssignmentID = aa.AssignmentID
	inner join ClientSupplier cs on cs.ClientID = assign.ClientID
where 
	act.activityid = @activityid