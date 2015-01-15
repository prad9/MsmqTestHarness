The new version of EventValidator.GetValidationResults uses the raw SQL with DataTable parameter when retrieving validation result for more than 10 events.

This means you will need to create a new user-defined table type in your service boundary. Alongside in this folder is a SQL script which you can use to create this user-defined table type.

You will also need to call the new two-parameter version of GetValidationResults, so existing call to:

	EventValidator.GetValidationResults(eventIds);

Should change to 

	EventValidator.GetValidationResults(eventIds, true);
